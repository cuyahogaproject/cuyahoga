using System;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Caching;
using System.Collections;

using Cuyahoga.Core;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Collections;

using Npgsql;

namespace Cuyahoga.Core.DAL
{
	/// <summary>
	/// The PostgreSQL implementation of the ICmsDataProvider interface.
	/// See <see cref="ICmsDataProvider"/> for the interface descriptions.
	/// </summary>
	public class PgSqlDataProvider : ICmsDataProvider
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PgSqlDataProvider()
		{
		}

		#region nodes

		public void GetNodeById(int id, Node node)
		{
			string sql = @"	SELECT n.nodeid, n.parentnodeid, n.title, n.shortdescription, n.position, t.templateid, t.name, t.path
							FROM cuyahoga_node n
								LEFT OUTER JOIN cuyahoga_template t ON t.templateid = n.templateid
							WHERE n.nodeid = :nodeid";
			Debug.WriteLine("GetNodeById(" + node.Id.ToString() + ")\n");
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			NpgsqlParameter prmNodeId = new NpgsqlParameter(":nodeid", DbType.Int32);
			prmNodeId.Value = id;
			cmd.Parameters.Add(prmNodeId);
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
					FillNodeFromDataReader(dr, node);
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading node data", ex);
			}
			finally
			{
				con.Close();
			}
		}

		public void GetNodesByParent(Node parentNode, NodeCollection nodes)
		{
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand();
			string sql = "";
			// HACK: Don't use a parameter if parent = null. Hardcode 'is null' instead
			if (parentNode == null)
			{
				sql =	@"	SELECT n.nodeid, n.parentnodeid, n.templateid, n.title, n.shortdescription, n.position, t.templateid, t.name, t.path
							FROM cuyahoga_node n
								LEFT OUTER JOIN cuyahoga_template t ON t.templateid = n.templateid
							WHERE parentnodeid IS NULL ORDER BY position";
			}
			else
			{
				sql =	@"	SELECT n.nodeid, n.parentnodeid, n.templateid, n.title, n.shortdescription, n.position, t.templateid, t.name, t.path
							FROM cuyahoga_node n
								LEFT OUTER JOIN cuyahoga_template t ON t.templateid = n.templateid
							WHERE parentnodeid = :parentnodeid ORDER BY position";
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":parentnodeid", DbType.Int32, 4, parentNode.Id));
			}
			if (parentNode != null)
				Debug.WriteLine("GetNodesByParent(" + parentNode.Id.ToString() + ")\n");
			else
				Debug.WriteLine("GetNodesByParent(null)\n");
			cmd.CommandText = sql;
			cmd.Connection = con;
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Node node = new Node();
					FillNodeFromDataReader(dr, node);
					if (parentNode != null)
					{
						node.ParentNode = parentNode;
						node.ParentId = parentNode.Id;
					}
					nodes.Add(node);
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading nodes", ex);
			}
			finally
			{
				con.Close();
			}			
		}

		public void GetNodeByParentIdAndPosition(int parentId, int position, Node node)
		{
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.Connection = con;
			string sql = @"	SELECT n.nodeid, n.parentnodeid, n.title, n.shortdescription, n.position, t.templateid, t.name, t.path
							FROM cuyahoga_node n
								LEFT OUTER JOIN cuyahoga_template t ON t.templateid = n.templateid
							WHERE n.position = :position";
			if (parentId > 0)
			{
				sql += " AND parentnodeid = :parentnodeid";
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam("parentnodeid", DbType.Int32, 4, parentId));
			}
			else
				sql += " AND parentnodeid IS NULL";
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":position", DbType.Int32, 4, position));
			cmd.CommandText = sql;
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
					FillNodeFromDataReader(dr, node);
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading node data", ex);
			}
			finally
			{
				con.Close();
			}

		}

		public void UpdateVerticalNodePosition(Node node, NodePositionMovement movement)
		{
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			// change position of the node that has to make room the moved node
			string sql1 = "";
			NpgsqlCommand cmd1 = new NpgsqlCommand();
			cmd1.Connection = con;
			switch (movement)
			{
				case NodePositionMovement.Up:
					sql1 = "UPDATE cuyahoga_node SET position = position + 1 WHERE position = :position";
					break;
				case NodePositionMovement.Down:
					sql1 = "UPDATE cuyahoga_node SET position = position - 1 WHERE position = :position";
					break;
			}
			if (node.ParentNode == null)
				sql1 += " AND parentnodeid IS NULL";
			else
			{
				sql1 += " AND parentnodeid = :parentnodeid";
				NpgsqlParameter prmParentNodeId = new NpgsqlParameter(":parentnodeid", DbType.Int32);
				prmParentNodeId.Value = node.ParentId;
				cmd1.Parameters.Add(prmParentNodeId);
			}
			NpgsqlParameter prmPosition = new NpgsqlParameter(":position", DbType.Int32);
			prmPosition.Value = node.Position;
			cmd1.Parameters.Add(prmPosition);
			cmd1.CommandText = sql1;

			// move the node itself
			string sql2 = "UPDATE cuyahoga_node SET position = :position WHERE nodeid = :nodeid";
			NpgsqlCommand cmd2 = new NpgsqlCommand(sql2, con);
			NpgsqlParameter prmNodeId = new NpgsqlParameter(":nodeid", DbType.Int32);
			prmNodeId.Value = node.Id;
			cmd2.Parameters.Add(prmNodeId);
			cmd2.Parameters.Add(prmPosition);

			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd1.Transaction = trn;
				cmd2.Transaction = trn;
				cmd1.ExecuteNonQuery();
				cmd2.ExecuteNonQuery();
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error moving node position", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void UpdateNodePositions(Node parentNode, int positionFrom, int amount)
		{
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.Connection = con;
			string sql = @"	UPDATE cuyahoga_node SET position = position + :amount WHERE position >= :positionFrom";
			if (parentNode == null)
				sql += " AND parentnodeid is null";
			else
			{
				sql += " AND parentnodeid = :parentnodeid";
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":parentnodeid", DbType.Int32, 4, parentNode.Id));
			}			
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":positionFrom", DbType.Int32, 4, positionFrom));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":amount", DbType.Int32, 4, amount));
			cmd.CommandText = sql;

			con.Open();
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error moving node positions", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void InsertNode(Node node)
		{
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.Connection = con;
			string sql = @"	INSERT INTO cuyahoga_node (parentnodeid, templateid, title, shortdescription, position)
							VALUES (:parentnodeid, :templateid, :title, :shortdescription, :position)";
			if (node.ParentNode != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":parentnodeid", node.ParentId));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":parentnodeid", DBNull.Value));
			if (node.Template != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":templateid", DbType.Int32, 4, node.Template.Id));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":templateid", DbType.Int32, 4, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":title", DbType.String, 255, node.Title));
			if (node.ShortDescription != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":shortdescription", DbType.String, 100, node.ShortDescription));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":shortdescription", DbType.String, 100, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":position", DbType.Int32, 4, node.Position));
			cmd.CommandText = sql;

			string sqlId = "SELECT CURRVAL('cuyahoga_node_nodeid_seq')";
			NpgsqlCommand cmdId = new NpgsqlCommand(sqlId, con);
			
			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				cmdId.Transaction = trn;
				// Insert node
				cmd.ExecuteNonQuery();
				node.Id = Convert.ToInt32(cmdId.ExecuteScalar());
				// Insert roles
				InsertRoles(node, trn);
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error inserting node", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void UpdateNode(Node node)
		{
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.Connection = con;
			string sql = @"	UPDATE cuyahoga_node 
							SET parentnodeid = :parentnodeid, 
								templateid = :templateid,
								title = :title,
								shortdescription = :shortdescription,
								position = :position,
								updatetimestamp = current_timestamp
							WHERE nodeid = :nodeid";
			if (node.ParentNode != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":parentnodeid", node.ParentId));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":parentnodeid", DBNull.Value));
			if (node.Template != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":templateid", DbType.Int32, 4, node.Template.Id));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":templateid", DbType.Int32, 4, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":title", DbType.String, 255, node.Title));
			if (node.ShortDescription != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":shortdescription", DbType.String, 100, node.ShortDescription));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":shortdescription", DbType.String, 100, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":position", DbType.Int32, 4, node.Position));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":nodeid", DbType.Int32, 4, node.Id));
			cmd.CommandText = sql;

			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				// first delete old roles
				DeleteRoles(node, trn);
				// update node
				cmd.ExecuteNonQuery();
				// insert new roles
				InsertRoles(node, trn);
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error updating node", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void DeleteNode(Node node)
		{
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand();
			cmd.Connection = con;
			string sql = @"	DELETE FROM cuyahoga_node 
							WHERE nodeid = :nodeid";
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":nodeid", DbType.Int32, 4, node.Id));
			cmd.CommandText = sql;

			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				// First delete roles
				DeleteRoles(node, trn);
				// Delete node
				cmd.ExecuteNonQuery();
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error deleting node", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public int GetMaxNodePositionAtRootLevel()
		{
			int position = -1;

			string sql = "SELECT MAX(position) FROM cuyahoga_node WHERE parentnodeid IS NULL";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);

			con.Open();
			try
			{
				position = Convert.ToInt32(cmd.ExecuteScalar());
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error fetching max position at root level", ex);
			}
			catch (InvalidCastException ex)
			{
				// apparently there is no max position, return -1
				Trace.Write(ex.Message);
				position = -1;
			}
			finally
			{
				con.Close();
			}
			return position;
		}


		private void FillNodeFromDataReader(NpgsqlDataReader dr, Node node)
		{
			if (dr["parentnodeid"] != DBNull.Value)
				node.ParentId = Convert.ToInt32(dr["parentnodeid"]);
			node.Id = Convert.ToInt32(dr["nodeid"]);
			node.Title = Convert.ToString(dr["title"]);
			node.ShortDescription = Convert.ToString(dr["shortdescription"]);
			node.Position = Convert.ToInt32(dr["position"]);
			if (dr["templateid"] != DBNull.Value && dr["templateid"] != null)
			{
				node.Template = new Template();
				node.Template.Id = Convert.ToInt32(dr["templateid"]);
				node.Template.Name = Convert.ToString(dr["name"]);
				node.Template.Path = Convert.ToString(dr["path"]);
			}
		}

		#endregion

		#region templates

		public void GetAllTemplates(TemplateCollection templates)
		{
			string sql = "SELECT templateid, name, path FROM cuyahoga_template ORDER BY name";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Template template = new Template();
					template.Id = Convert.ToInt32(dr["templateid"]);
					template.Name = Convert.ToString(dr["name"]);
					template.Path = Convert.ToString(dr["path"]);
					templates.Add(template);					
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading templates", ex);
			}
			finally
			{
				con.Close();
			}
		}

		#endregion

		#region modules

		/// <summary>
		/// Reads modules from database and stores them in the cache.
		/// </summary>
		public void ReadAndCacheAllModules()
		{
			// Modules are not in the cache, get from database.
			Hashtable modules = new Hashtable();
			string sql = @"	SELECT moduleid, name, assemblyname, classname, path, editpath 
							FROM cuyahoga_module
							ORDER BY name";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					// Get module instance from factory and add to cache
					Module module = ModuleFactory.GetInstance((string)dr["assemblyname"], (string)dr["classname"]);
					module.ModuleId = Convert.ToInt32(dr["moduleid"]);
					module.Name = Convert.ToString(dr["name"]);
					module.Path = Convert.ToString(dr["path"]);
					module.EditPath = Convert.ToString(dr["editpath"]);
					modules.Add((string)dr["classname"], module);
				}
				dr.Close();
				HttpContext.Current.Cache.Insert("Modules", modules);
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading modules", ex);
			}
			catch (Exception ex)
			{
				throw new Exception("Error retrieving modules for cache", ex);
			}
			finally
			{
				con.Close();
			}
		}

		public Hashtable GetAllModules()
		{
			if (HttpContext.Current.Cache["Modules"] == null)
			{
				ReadAndCacheAllModules();
			}
			return (Hashtable)HttpContext.Current.Cache["Modules"];
		}

		#endregion

		#region sections

		public void GetSectionById(int id, Section section)
		{
			string sql = @"	SELECT s.sectionid, s.moduleid, s.nodeid, s.title, s.showtitle, s.placeholder, 
								s.cacheduration, s.position, m.name, m.assemblyname, m.classname, m.path, m.editpath								
							FROM cuyahoga_section s
								INNER JOIN cuyahoga_module m ON m.moduleid = s.moduleid
							WHERE s.sectionid = :sectionid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":sectionid", DbType.Int32, 4, id));
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					FillSectionFromDataReader(dr, section);
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading section data", ex);
			}
			finally
			{
				con.Close();
			}
		}

		public void GetSectionsByNode(Node node, SectionCollection sections)
		{
			string sql = @"	SELECT s.sectionid, s.moduleid, s.nodeid, s.title, s.showtitle, s.placeholder, 
								s.cacheduration, s.position, m.name, m.assemblyname, m.classname, m.path, m.editpath								
							FROM cuyahoga_section s
								INNER JOIN cuyahoga_module m ON m.moduleid = s.moduleid
							WHERE s.nodeid = :nodeid ORDER BY placeholder, position";
			Debug.WriteLine("GetSectionsByNode(" + node.Id.ToString() + ")\n");
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":nodeid", DbType.Int32, 4, node.Id));
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Section section = new Section();
					FillSectionFromDataReader(dr, section);
					section.Node = node;
					sections.Add(section);
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading section data", ex);
			}
			finally
			{
				con.Close();
			}            
		}

		public void InsertSection(Section section)
		{
			string sql = @"	INSERT INTO cuyahoga_section (moduleid, nodeid, title, showtitle, placeholder, cacheduration, position)
							VALUES (:moduleid, :nodeid, :title, :showtitle, :placeholder, :cacheduration, :position)";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":moduleid", DbType.Int32, 4, section.Module.ModuleId));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":nodeid", DbType.Int32, 4, section.Node.Id));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":title", DbType.String, 255, section.Title));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":showtitle", DbType.Boolean, 1, section.ShowTitle));
			if (section.PlaceholderId != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":placeholder", DbType.String, 100, section.PlaceholderId));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":placeholder", DbType.String, 100, DBNull.Value));
			if (section.CacheDuration >= 0)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":cacheduration", DbType.Int32, 4, section.CacheDuration));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":cacheduration", DbType.Int32, 4, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":position", DbType.Int32, 4, section.Position));

			string sqlId = "SELECT CURRVAL('cuyahoga_section_sectionid_seq')";
			NpgsqlCommand cmdId = new NpgsqlCommand(sqlId, con);
			
			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				cmdId.Transaction = trn;
				cmd.ExecuteNonQuery();
				section.Id = Convert.ToInt32(cmdId.ExecuteScalar());
				// Insert roles
				InsertRoles(section, trn);
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error inserting section", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void UpdateSection(Section section)
		{
			string sql = @"	UPDATE cuyahoga_section 
							SET moduleid = :moduleid,
								nodeid = :nodeid, 
								title = :title,
								showtitle = :showtitle,
								placeholder = :placeholder,
								cacheduration = :cacheduration,
								position = :position,
								updatetimestamp = current_timestamp
							WHERE sectionid = :sectionid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":moduleid", DbType.Int32, 4, section.Module.ModuleId));
			if (section.Node != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":nodeid", DbType.Int32, 4, section.Node.Id));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":nodeid", DbType.Int32, 4, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":title", DbType.String, 255, section.Title));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":showtitle", DbType.Boolean, 1, section.ShowTitle));
			if (section.PlaceholderId != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":placeholder", DbType.String, 100, section.PlaceholderId));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":placeholder", DbType.String, 100, DBNull.Value));
			if (section.CacheDuration >= 0)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":cacheduration", DbType.Int32, 4, section.CacheDuration));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":cacheduration", DbType.Int32, 4, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":position", DbType.Int32, 4, section.Position));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":sectionid", DbType.Int32, 4, section.Id));

			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				// first delete old roles
				DeleteRoles(section, trn);
				// update section
				cmd.ExecuteNonQuery();
				// insert refreshed roles
				InsertRoles(section, trn);
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error updating section", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void DeleteSection(Section section)
		{
			string sql = @"	DELETE FROM cuyahoga_section 
							WHERE sectionid = :sectionid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":sectionid", DbType.Int32, 4, section.Id));

			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				// first delete attached roles
				DeleteRoles(section, trn);
				// delete section
				cmd.ExecuteNonQuery();
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error deleting section", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		private void FillSectionFromDataReader(NpgsqlDataReader dr, Section section)
		{
			section.Id = Convert.ToInt32(dr["sectionid"]);
			if (dr["nodeid"] != DBNull.Value)
				section.NodeId = Convert.ToInt32(dr["nodeid"]);
			if (dr["title"] != DBNull.Value)
				section.Title = Convert.ToString(dr["title"]);
			section.ShowTitle = Convert.ToBoolean(dr["showtitle"]);
			if (dr["placeholder"] != DBNull.Value)
				section.PlaceholderId = Convert.ToString(dr["placeholder"]);
			section.Position = Convert.ToInt32(dr["position"]);
			if (dr["cacheduration"] != DBNull.Value)
				section.CacheDuration = Convert.ToInt32(dr["cacheduration"]);
			// Add module (create a new instance with the same type as a cached one)
			Module module = ModuleFactory.GetNewInstanceFromCache(Convert.ToString(dr["classname"]));
			if (module != null)
			{
				module.ModuleId = Convert.ToInt32(dr["moduleid"]);
				module.Name = Convert.ToString(dr["name"]);
				module.Path = Convert.ToString(dr["path"]);
				module.EditPath = Convert.ToString(dr["editpath"]);
				module.Section = section;
				section.Module = module;
			}
		}

		private void InsertRoles(IPersonalizable personalizableObject, NpgsqlTransaction trn)
		{
			string sql = null;
			if (personalizableObject is Node)
			{
				sql = @"INSERT INTO cuyahoga_noderole(roleid, nodeid, viewallowed, editallowed)
						VALUES(:roleid, :objectid, :viewallowed, :editallowed)";
			}
			if (personalizableObject is Section)
			{
				sql = @"INSERT INTO cuyahoga_sectionrole(roleid, sectionid, viewallowed, editallowed)
						VALUES(:roleid, :objectid, :viewallowed, :editallowed)";
			}
			NpgsqlCommand cmd = new NpgsqlCommand(sql, trn.Connection, trn);
			
			foreach (Role role in personalizableObject.ViewRoles)
			{
				cmd.Parameters.Clear();
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":objectid", DbType.Int32, 4, personalizableObject.Id));
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":roleid", DbType.Int32, 4, role.Id));
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":viewallowed", DbType.Boolean, 1, true));
				// Check if the role is also in the EditRoles. If so create a parameter with the value true.
				if (personalizableObject.EditRoles.Contains(role))
					cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":editallowed", DbType.Boolean, 1, true));
				else
					cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":editallowed", DbType.Boolean, 1, false));
				cmd.ExecuteNonQuery();
			}
			foreach (Role role in personalizableObject.EditRoles)
			{
				cmd.Parameters.Clear();
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":sectionid", DbType.Int32, 4, personalizableObject.Id));
				// Check if the role is not in the ViewRoles. If so, the role is already stored and we don't
				// want to store it twice.
				if (! personalizableObject.ViewRoles.Contains(role))
				{
					cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":roleid", DbType.Int32, 4, role.Id));
					cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":editallowed", DbType.Boolean, 1, true));
					cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":viewallowed", DbType.Boolean, 1, false));
					cmd.ExecuteNonQuery();
				}
			}				
		}

		private void DeleteRoles(IPersonalizable personalizableObject, NpgsqlTransaction trn)
		{
			string sql = null;
			if (personalizableObject is Node)
			{
				sql = @"DELETE FROM cuyahoga_noderole
						WHERE nodeid = :objectid";
			}
			if (personalizableObject is Section)
			{
				sql = @"DELETE FROM cuyahoga_sectionrole
						WHERE sectionid = :objectid";
			}
			NpgsqlCommand cmd = new NpgsqlCommand(sql, trn.Connection, trn);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":objectid", DbType.Int32, 4, personalizableObject.Id));
			cmd.ExecuteNonQuery();
		}

		#endregion

		#region users and roles

		public void GetUserByUsernameAndPassword(string username, string password, User user)
		{			
			string sql = @"	SELECT userid, firstname, lastname, email 
							FROM cuyahoga_user 
							WHERE username=:username AND password=:password";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":username", DbType.String, 50, username));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":password", DbType.String, 100, password));
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					user.Id = Convert.ToInt32(dr["userid"]);
					user.FirstName = Convert.ToString(dr["firstname"]);
					user.LastName = Convert.ToString(dr["lastname"]);
					user.Email = Convert.ToString(dr["email"]);
					// Now update the lastlogindate and lastip. These properties are already filled.
					string sqlUpd = @"	UPDATE cuyahoga_user 
										SET	lastlogin = :lastlogin, 
											lastip = :lastip,
											updatetimestamp = current_timestamp
										WHERE userid = :userid";
					NpgsqlCommand cmdUpd = new NpgsqlCommand(sqlUpd, con);
					cmdUpd.Parameters.Add(PgSqlDataHelper.MakeInParam(":lastlogin", DbType.DateTime, 8, user.LastLogin));
					cmdUpd.Parameters.Add(PgSqlDataHelper.MakeInParam(":lastip", DbType.String, 40, user.LastIp));
					cmdUpd.Parameters.Add(PgSqlDataHelper.MakeInParam(":userid", DbType.Int32, 4, user.Id));
					cmdUpd.ExecuteNonQuery();
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading user data", ex);
			}
			finally
			{
				con.Close();
			}
		}

		public void GetUserById(int userId, User user)
		{
			string sql = @"	SELECT username, firstname, lastname, email, lastlogin, lastip
							FROM cuyahoga_user 
							WHERE userid = :userid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":userid", DbType.Int32, 4, userId));
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					user.Id = userId;
					FillUserFromDataReader(dr, user);
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading user data", ex);
			}
			finally
			{
				con.Close();
			}			
		}

		public void FindUsersByName(string userName, UserCollection users)
		{
			string sql = @"	SELECT userid, username, firstname, lastname, email, lastlogin, lastip
							FROM cuyahoga_user ";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			if (userName.Trim().Length > 0)
			{
				cmd.CommandText += "WHERE username LIKE :username ";
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":username", DbType.String, 255, userName + "%"));
			}
			cmd.CommandText += "ORDER BY username ";
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					User user = new User();
					user.Id = Convert.ToInt32(dr["userid"]);
					FillUserFromDataReader(dr, user);
					users.Add(user);
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading users", ex);
			}
			finally
			{
				con.Close();
			}			
		}

		public void InsertUser(User user)
		{
			string sql = @"	INSERT INTO cuyahoga_user (username, password, firstname, lastname, email)
							VALUES (:username, :password, :firstname, :lastname, :email) ";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":username", DbType.String, 50, user.UserName));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":password", DbType.String, 100, user.Password));
			if (user.FirstName != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":firstname", DbType.String, 100, user.FirstName));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":firstname", DbType.String, 100, DBNull.Value));
			if (user.LastName != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":lastname", DbType.String, 100, user.LastName));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":lastname", DbType.String, 100, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":email", DbType.String, 100, user.Email));

			string sqlId = "SELECT CURRVAL('cuyahoga_user_userid_seq')";
			NpgsqlCommand cmdId = new NpgsqlCommand(sqlId, con);
			
			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				cmdId.Transaction = trn;
				cmd.ExecuteNonQuery();
				user.Id = Convert.ToInt32(cmdId.ExecuteScalar());
				InsertUserRoles(user, trn);
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error inserting user", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void UpdateUser(User user)
		{
			string sql = @"	UPDATE cuyahoga_user
							SET	username = :username,
								firstname = :firstname,
								lastname = :lastname,
								email = :email,
								updatetimestamp = current_timestamp ";
			if (user.Password != null)
				sql += ", password = :password ";
			sql += "WHERE userid = :userid ";

			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":username", DbType.String, 50, user.UserName));
			if (user.Password != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":password", DbType.String, 100, user.Password));
			if (user.FirstName != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":firstname", DbType.String, 100, user.FirstName));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":firstname", DbType.String, 100, DBNull.Value));
			if (user.LastName != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":lastname", DbType.String, 100, user.LastName));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":lastname", DbType.String, 100, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":email", DbType.String, 100, user.Email));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":userid", DbType.Int32, 4, user.Id));
		
			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				DeleteUserRoles(user, trn);
				cmd.ExecuteNonQuery();
				InsertUserRoles(user, trn);
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error updating user", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void DeleteUser(User user)
		{
			string sql = @"	DELETE FROM cuyahoga_user
							WHERE userid = :userid ";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":userid", DbType.Int32, 4, user.Id));
		
			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				DeleteUserRoles(user, trn);
				cmd.ExecuteNonQuery();
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error deleting user", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void GetAllRoles(RoleCollection roles)
		{
			string sql = @"	SELECT r.roleid, r.name, r.permissionlevel
							FROM cuyahoga_role r ORDER BY r.permissionlevel";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Role role = new Role();
					role.Id = Convert.ToInt32(dr["roleid"]);
					role.Name = Convert.ToString(dr["name"]);
					role.PermissionLevel = Convert.ToInt32(dr["permissionlevel"]);
					roles.Add(role);
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading roles", ex);
			}
			finally
			{
				con.Close();
			}		
		}

		public void GetRolesByUser(User user)
		{
			string sql = @"	SELECT r.roleid, r.name, r.permissionlevel
							FROM cuyahoga_role r
								INNER JOIN cuyahoga_userrole cu ON cu.roleid = r.roleid
							WHERE cu.userid = :userid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":userid", DbType.Int32, 4, user.Id));
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Role role = new Role();
					role.Id = Convert.ToInt32(dr["roleid"]);
					role.Name = Convert.ToString(dr["name"]);
					role.PermissionLevel = Convert.ToInt32(dr["permissionlevel"]);
					user.Roles.Add(role);
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error reading user roles", ex);
			}
			finally
			{
				con.Close();
			}		
		}

		public void GetRolesByNode(Node node)
		{
			string sql = @" SELECT nr.viewallowed, nr.editallowed, r.roleid, r.name, r.permissionlevel
							FROM cuyahoga_noderole nr
								INNER JOIN cuyahoga_role r ON r.roleid = nr.roleid
							WHERE nr.nodeid = :nodeid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":nodeid", DbType.Int32, 4, node.Id));

			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Role role = new Role();
					role.Id = Convert.ToInt32(dr["roleid"]);
					role.Name = Convert.ToString(dr["name"]);
					role.PermissionLevel = Convert.ToInt32(dr["permissionlevel"]);
					if (Convert.ToBoolean(dr["viewallowed"]))
						node.ViewRoles.Add(role);
					if (Convert.ToBoolean(dr["editallowed"]))
						node.EditRoles.Add(role);
				}
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error getting roles for node", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void GetRolesBySection(Section section)
		{
			string sql = @" SELECT sr.viewallowed, sr.editallowed, r.roleid, r.name, r.permissionlevel
							FROM cuyahoga_sectionrole sr
								INNER JOIN cuyahoga_role r ON r.roleid = sr.roleid
							WHERE sr.sectionid = :sectionid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":sectionid", DbType.Int32, 4, section.Id));

			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Role role = new Role();
					role.Id = Convert.ToInt32(dr["roleid"]);
					role.Name = Convert.ToString(dr["name"]);
					role.PermissionLevel = Convert.ToInt32(dr["permissionlevel"]);
					if (Convert.ToBoolean(dr["viewallowed"]))
						section.ViewRoles.Add(role);
					if (Convert.ToBoolean(dr["editallowed"]))
						section.EditRoles.Add(role);
				}
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error getting roles for section", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void GetRoleById(int roleId, Role role)
		{
			string sql = @" SELECT name, permissionlevel
							FROM cuyahoga_role
							WHERE roleid = :roleid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":roleid", DbType.Int32, 4, roleId));

			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					role.Id = roleId;
					role.Name = Convert.ToString(dr["name"]);
					role.PermissionLevel = Convert.ToInt32(dr["permissionlevel"]);
				}
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error getting role", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void InsertRole(Role role)
		{
			string sql = @"	INSERT INTO cuyahoga_role (name, permissionlevel)
							VALUES (:name, :permissionlevel) ";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":name", DbType.String, 50, role.Name));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":permissionlevel", DbType.Int32, 4, role.PermissionLevel));

			string sqlId = "SELECT CURRVAL('cuyahoga_role_roleid_seq')";
			NpgsqlCommand cmdId = new NpgsqlCommand(sqlId, con);
			
			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				cmdId.Transaction = trn;
				cmd.ExecuteNonQuery();
				role.Id = Convert.ToInt32(cmdId.ExecuteScalar());
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error inserting role", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void UpdateRole(Role role)
		{
			string sql = @"	UPDATE cuyahoga_role
							SET name = :name,
								permissionlevel = :permissionlevel,
								updatetimestamp = current_timestamp
							WHERE roleid = :roleid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":name", DbType.String, 50, role.Name));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":permissionlevel", DbType.Int32, 4, role.PermissionLevel));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":roleid", DbType.Int32, 4, role.Id));
	
			con.Open();
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error updating role", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void DeleteRole(Role role)
		{
			string sql = @"	DELETE FROM cuyahoga_role
							WHERE roleid = :roleid ";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":roleid", DbType.Int32, 4, role.Id));
	
			con.Open();
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error deleting role", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		private void FillUserFromDataReader(NpgsqlDataReader dr, User user)
		{
			user.UserName = Convert.ToString(dr["username"]);
			user.FirstName = Convert.ToString(dr["firstname"]);
			user.LastName = Convert.ToString(dr["lastname"]);
			user.Email = Convert.ToString(dr["email"]);
			if (dr["lastlogin"] != DBNull.Value)
				user.LastLogin = Convert.ToDateTime(dr["lastlogin"]);
			if (dr["lastip"] != DBNull.Value)
				user.LastIp = Convert.ToString(dr["lastip"]);
		}

		private void InsertUserRoles(User user, NpgsqlTransaction trn)
		{
			string sql = @"	INSERT INTO cuyahoga_userrole(roleid, userid)
							VALUES(:roleid, :userid)";
			NpgsqlCommand cmd = new NpgsqlCommand(sql, trn.Connection, trn);
			
			foreach (Role role in user.Roles)
			{
				cmd.Parameters.Clear();
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":userid", DbType.Int32, 4, user.Id));
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":roleid", DbType.Int32, 4, role.Id));
				cmd.ExecuteNonQuery();
			}
		}

		private void DeleteUserRoles(User user, NpgsqlTransaction trn)
		{
			string sql = @"	DELETE FROM cuyahoga_userrole
							WHERE userid = :userid";
			NpgsqlCommand cmd = new NpgsqlCommand(sql, trn.Connection, trn);
			
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":userid", DbType.Int32, 4, user.Id));
			cmd.ExecuteNonQuery();
		}
		
		#endregion

	}
}
