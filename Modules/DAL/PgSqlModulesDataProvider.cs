using System;
using System.Data;

using Npgsql;

using Cuyahoga.Core.Util;
using Cuyahoga.Core.DAL; // for the PgSqlDataHelper
using Cuyahoga.Modules.StaticHtml;

namespace Cuyahoga.Modules.DAL
{
	/// <summary>
	/// The PostgreSQL implementation of the IModulesDataProvider interface.
	/// See <see cref="IModulesDataProvider"/> for the interface descriptions.
	/// </summary>
	public class PgSqlModulesDataProvider : IModulesDataProvider
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PgSqlModulesDataProvider()
		{
		}

		#region StaticHtml

		public void GetStaticHtmlContentBySectionId(int sectionId, StaticHtmlContent staticHtmlContent)
		{
			string sql = @"	SELECT s.statichtmlid, s.title, s.content, s.createdby, s.modifiedby
							FROM cm_statichtml s
							WHERE s.sectionid = :sectionid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":sectionid", DbType.Int32, 4, sectionId));
			con.Open();
			try
			{
				NpgsqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					staticHtmlContent.Id = Convert.ToInt32(dr["statichtmlid"]);
					if (dr["title"] != null)
						staticHtmlContent.Title = Convert.ToString(dr["title"]);
					staticHtmlContent.Content = Convert.ToString(dr["content"]);
				}
				dr.Close();
			}
			catch (NpgsqlException ex)
			{
				throw new Exception("Error reading StaticHtml data", ex);
			}
			finally
			{
				con.Close();
			}
		}

		public void InsertStaticHtmlContent(int sectionId, int userId, StaticHtmlContent staticHtmlContent)
		{
			string sql = @"	INSERT INTO cm_statichtml(sectionid, title, content, createdby)
							VALUES(:sectionid, :title, :content, :createdby)";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":sectionid", DbType.Int32, 4, sectionId));
			if (staticHtmlContent.Title != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":title", DbType.String, 255, staticHtmlContent.Title));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":title", DbType.String, 255, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":content", DbType.String, 8000, staticHtmlContent.Content));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":createdby", DbType.Int32, 4, userId));

			string sqlId = "SELECT CURRVAL('cm_statichtml_statichtmlid_seq')";
			NpgsqlCommand cmdId = new NpgsqlCommand(sqlId, con);
			
			con.Open();
			NpgsqlTransaction trn = con.BeginTransaction();
			try
			{
				cmd.Transaction = trn;
				cmdId.Transaction = trn;
				cmd.ExecuteNonQuery();
				staticHtmlContent.Id = Convert.ToInt32(cmdId.ExecuteScalar());
				trn.Commit();
			}
			catch (NpgsqlException ex)
			{
				trn.Rollback();
				throw new CmsDataException("Error inserting staticHtmlContent", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void UpdateStaticHtmlContent(int sectionId, int userId, StaticHtmlContent staticHtmlContent)
		{
			string sql = @"	UPDATE cm_statichtml
							SET sectionid = :sectionid,
								title = :title,
								content = :content,
								modifiedby = :modifiedby,
								updatetimestamp = current_timestamp
							WHERE statichtmlid = :statichtmlid";
			NpgsqlConnection con = new NpgsqlConnection(Config.GetConfiguration()["ConnectionString"]);
			NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":sectionid", DbType.Int32, 4, sectionId));
			if (staticHtmlContent.Title != null)
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":title", DbType.String, 255, staticHtmlContent.Title));
			else
				cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":title", DbType.String, 255, DBNull.Value));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":content", DbType.String, 8000, staticHtmlContent.Content));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":modifiedby", DbType.Int32, 4, userId));
			cmd.Parameters.Add(PgSqlDataHelper.MakeInParam(":statichtmlid", DbType.Int32, 4, staticHtmlContent.Id));

			con.Open();
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (NpgsqlException ex)
			{
				throw new CmsDataException("Error updating staticHtmlContent", ex);
			}
			finally 
			{
				con.Close();
			}
		}

		public void DeleteStaticHtmlContent(int sectionId)
		{
			// TODO:  Add PgSqlModulesDataProvider.DeleteStaticHtmlContent implementation
		}

		#endregion
	}
}
