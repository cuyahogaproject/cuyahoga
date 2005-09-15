using System;
using System.Collections;
using System.Reflection;

using log4net;
using NHibernate;
using NHibernate.Expression;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// This is the repository for persistance of the Cuyahoga core classes. Maybe it 
	/// should be split up into several classes, but we'll start with one
	/// repository for all core classes.
	/// </summary>
	public class CoreRepository
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(CoreRepository));

		private ISessionFactory _factory;
		private ISession _activeSession;

		/// <summary>
		/// Get the active NHibernate session.
		/// </summary>
		public ISession ActiveSession
		{
			get { return this._activeSession; }
		}
		/// <summary>
		/// Create a repository for core objects.
		/// </summary>
		public CoreRepository() : this(false)
		{
		}

		/// <summary>
		/// Create a repository for core objects.
		/// </summary>
		/// <param name="openSession">Indicate if the CoreRepository should open a session and keep it in memory.</param>
		public CoreRepository(bool openSession)
		{
			this._factory = SessionFactory.GetInstance().GetNHibernateFactory();
			if (openSession)
			{
				this._activeSession = this._factory.OpenSession();
			}
		}

		/// <summary>
		/// Open a NHibernate session.
		/// </summary>
		public void OpenSession()
		{
			if (this._activeSession == null || ! this._activeSession.IsOpen)
			{
				this._activeSession = this._factory.OpenSession();
			}
			else
			{
				throw new InvalidOperationException("The repository already has an open session");
			}
		}

		/// <summary>
		/// Flushes the current active NHibernate session.
		/// </summary>
		public void FlushSession()
		{
			if (this._activeSession != null && this._activeSession.IsOpen)
			{
				this._activeSession.Flush();
			}
		}

		/// <summary>
		/// Close the active NHibernate session
		/// </summary>
		public void CloseSession()
		{
			if (this._activeSession != null)
			{
				if (this._activeSession.IsOpen)
				{
					this._activeSession.Close();
				}
				this._activeSession.Dispose();
			}
		}

		#region Generic methods

		/// <summary>
		/// Generic method for retrieving single objects by primary key.
		/// </summary>
		/// <param name="type">The type of the object to fetch.</param>
		/// <param name="id">The identifier of the object.</param>
		/// <returns></returns>
		public object GetObjectById(Type type, int id)
		{
			if (this._activeSession != null)
			{
				return this._activeSession.Load(type, id);
			}
			else
			{
				throw new NullReferenceException("The repository doesn't have an active session");
			}
		}

		/// <summary>
		/// Generic method for retrieving single objects by primary key.
		/// </summary>
		/// <param name="type">The type of the object to fetch.</param>
		/// <param name="id">The identifier of the object.</param>
		/// <param name="allowNull">Allow null as return value.</param>
		/// <returns></returns>
		public object GetObjectById(Type type, int id, bool allowNull)
		{
			if (allowNull)
			{
				if (this._activeSession != null)
				{
					return this._activeSession.Get(type, id);
				}
				else
				{
					throw new NullReferenceException("The repository doesn't have an active session");
				}
			}
			else
			{
				return GetObjectById(type, id);
			}
		}

		/// <summary>
		/// Get all objects of a given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IList GetAll(Type type)
		{
			return GetAll(type, null);
		}

		/// <summary>
		/// Get all objects of a given type and add one or more names of properties to sort on.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="sortProperties"></param>
		/// <remarks>Sorting is Ascending order. Construct a specific query/method when the sort order
		/// should be different.</remarks>
		/// <returns></returns>
		public IList GetAll(Type type, params string[] sortProperties)
		{
			ICriteria crit = this._activeSession.CreateCriteria(type);
			if (sortProperties != null)
			{
				foreach (string sortProperty in sortProperties)
				{
					crit.AddOrder(Order.Asc(sortProperty));
				}
			}
			return crit.List();
		}

		/// <summary>
		/// Generic method to insert an object.
		/// </summary>
		/// <param name="obj"></param>
		public void SaveObject(object obj)
		{
			ITransaction trn = this._activeSession.BeginTransaction();
			try
			{
				// Try to find a UpdateTimestamp property and when found, set it to the current date/time.
				PropertyInfo pi = obj.GetType().GetProperty("UpdateTimestamp");
				if (pi != null)
				{
					pi.SetValue(obj, DateTime.Now, null);
				}
				this._activeSession.Save(obj);
				trn.Commit();
			}
			catch (Exception ex)
			{
				trn.Rollback();
				throw ex;
			}
		}

		/// <summary>
		/// Generic method to update an object.
		/// </summary>
		/// <param name="obj"></param>
		public void UpdateObject(object obj)
		{
			ITransaction trn = this._activeSession.BeginTransaction();
			try
			{
				this._activeSession.Update(obj);
				trn.Commit();
			}
			catch (Exception ex)
			{
				trn.Rollback();
				throw ex;
			}
		}

		/// <summary>
		/// Delete a specific object. Settings in the mapping file determine if this cascades
		/// to related objects.
		/// </summary>
		/// <param name="obj"></param>
		public void DeleteObject(object obj)
		{
			ITransaction trn = this._activeSession.BeginTransaction();
			try
			{
				this._activeSession.Delete(obj);
				trn.Commit();
			}
			catch (Exception ex)
			{
				trn.Rollback();
				throw ex;
			}
		}

		/// <summary>
		/// Mark an object for deletion. Commit the deletion with Session.Flush.
		/// </summary>
		/// <param name="obj"></param>
		public void MarkForDeletion(object obj)
		{
			this._activeSession.Delete(obj);
		}

		#endregion

		#region Site / SiteAlias specific

		/// <summary>
		/// 
		/// </summary>
		/// <param name="siteUrl"></param>
		/// <returns></returns>
		public Site GetSiteBySiteUrl(string siteUrl)
		{
			// The query is case insensitive.
			string hql = "from Site s where lower(s.SiteUrl) = :siteUrl1 or lower(s.SiteUrl) = :siteUrl2";
			IQuery q = this._activeSession.CreateQuery(hql);
			q.SetString("siteUrl1", siteUrl.ToLower());
			q.SetString("siteUrl2", siteUrl.ToLower() + "/"); // Also allow trailing slashes
			IList results = q.List();
			if (results.Count == 1)
			{
				return (Site)results[0];
			}
			else if (results.Count > 1)
			{
				throw new Exception(String.Format("Multiple sites found for SiteUrl {0}. The SiteUrl should be unique.", siteUrl));
			}
			else
			{
				return null;
			}
		}

		public SiteAlias GetSiteAliasByUrl(string url)
		{
			// The query is case insensitive.
			string hql = "from SiteAlias sa where lower(sa.Url) = :url1 or lower(sa.Url) = :url2";
			IQuery q = this._activeSession.CreateQuery(hql);
			q.SetString("url1", url.ToLower());
			q.SetString("url2", url.ToLower() + "/"); // Also allow trailing slashes
			IList results = q.List();
			if (results.Count == 1)
			{
				return (SiteAlias)results[0];
			}
			else if (results.Count > 1)
			{
				throw new Exception(String.Format("Multiple site aliases found for Url {0}. The Url should be unique.", url));
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Return all aliases belonging to a specific site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		public IList GetSiteAliasesBySite(Site site)
		{
			string hql = "from SiteAlias sa where sa.Site.Id = :siteId ";
			IQuery query = this._activeSession.CreateQuery(hql);
			query.SetInt32("siteId", site.Id);
			return query.List();
		}

		#endregion

		#region Node specific

		/// <summary>
		/// Retrieve the root nodes for a given site.
		/// </summary>
		/// <returns></returns>
		public IList GetRootNodes(Site site)
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(Node));
			crit.Add(Expression.IsNull("ParentNode"));
			crit.Add(Expression.Eq("Site", site));
			crit.AddOrder(Order.Asc("Position"));
			return crit.List();
		}

		public Node GetRootNodeByCultureAndSite(string culture, Site site)
		{
			string hql = "from Node n where n.ParentNode is null and n.Culture = :culture and n.Site.Id = :siteId";
			IQuery q = this._activeSession.CreateQuery(hql);
			q.SetString("culture", culture);
			q.SetInt32("siteId", site.Id);
			IList results = q.List();
			if (results.Count == 1)
			{
				return results[0] as Node;
			}
			else if (results.Count == 0)
			{
				throw new NodeNullException(String.Format("No root node found for culture {0} and site {1}.", culture, site.Id));
			}
			else
			{
				throw new Exception(String.Format("Multiple root nodes found for culture {0} and site {1}.", culture, site.Id));
			}
		}

		/// <summary>
		/// Retrieve a node by short description (friendly url).
		/// </summary>
		/// <param name="shortDescription"></param>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public Node GetNodeByShortDescriptionAndSite(string shortDescription, Site site)
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(Node));
			crit.Add(Expression.Eq("ShortDescription", shortDescription));
			crit.Add(Expression.Eq("Site.Id", site.Id));
			IList results = crit.List();
			if (results.Count == 1)
			{
				return (Node)results[0];
			}
			else if (results.Count > 1)
			{
				throw new Exception(String.Format("Multiple nodes found for ShortDescription {0}. The ShortDescription should be unique.", shortDescription));
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Retrieve a list of Nodes that are connected to a given template.
		/// </summary>
		/// <param name="template"></param>
		/// <returns></returns>
		public IList GetNodesByTemplate(Template template)
		{
			string hql = "from Node n where n.Template.Id = ? ";
			return this._activeSession.Find(hql, template.Id, NHibernateUtil.Int32);
		}

		/// <summary>
		/// Update a node.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="propagatePermissionsToChildNodes"></param>
		/// <param name="propagatePermissionsToChildNodes"></param>
		public void UpdateNode(Node node, bool propagatePermissionsToChildNodes, bool propagatePermissionsToSections)
		{
			UpdateObject(node);
			if (propagatePermissionsToChildNodes)
			{
				PropagatePermissionsToChildNodes(node, propagatePermissionsToSections);
			}
			if (propagatePermissionsToSections)
			{
				PropagatePermissionsToSections(node);
			}
		}

		/// <summary>
		/// Delete a node. Also clean up any references in custom menu's first.
		/// </summary>
		/// <param name="node"></param>
		public void DeleteNode(Node node)
		{
			string hql = "select m from Menu m join m.Nodes n where n.Id = :nodeId";
			IQuery q = this._activeSession.CreateQuery(hql);
			q.SetInt32("nodeId", node.Id);
			IList menus = q.List();
			foreach (Menu menu in menus)
			{
				// HACK: due to a bug with proxies IList.Remove(object) always removes the first object in
				// the list. Also IList.IndexOf always returns 0. Therefore, we'll loop through the collection
				// and find the right index. Btw, when turning off proxies everything works fine.
				int positionFound = -1;
				for (int i = 0; i < menu.Nodes.Count; i++)
				{
					if (((Node)menu.Nodes[i]).Id == node.Id)
					{
						positionFound = i;
						break;
					}
				}
				if (positionFound > -1)
				{
					menu.Nodes.RemoveAt(positionFound);
				}
				UpdateObject(menu);
			}
			DeleteObject(node);
		}

		private void PropagatePermissionsToChildNodes(Node parentNode, bool propagateToSections)
		{
			foreach (Node childNode in parentNode.ChildNodes)
			{
				childNode.NodePermissions.Clear();
				foreach (NodePermission pnp in parentNode.NodePermissions)
				{
					NodePermission childNodePermission = new NodePermission();
					childNodePermission.Node = childNode;
					childNodePermission.Role = pnp.Role;
					childNodePermission.ViewAllowed = pnp.ViewAllowed;
					childNodePermission.EditAllowed = pnp.EditAllowed;
					childNode.NodePermissions.Add(childNodePermission);
				}
				if (propagateToSections)
				{
					PropagatePermissionsToSections(childNode);
				}
				PropagatePermissionsToChildNodes(childNode, propagateToSections);
				UpdateObject(childNode);
			}
		}

		private void PropagatePermissionsToSections(Node node)
		{
			foreach (Section section in node.Sections)
			{
				section.SectionPermissions.Clear();
				foreach (NodePermission np in node.NodePermissions)
				{
					SectionPermission sp = new SectionPermission();
					sp.Section = section;
					sp.Role = np.Role;
					sp.ViewAllowed = np.ViewAllowed;
					sp.EditAllowed = np.EditAllowed;
					section.SectionPermissions.Add(sp);
				}
			}
			UpdateObject(node);
		}

		#endregion

		#region Menu specific

		/// <summary>
		/// Get all available menus for a given root node.
		/// </summary>
		/// <param name="rootNode"></param>
		/// <returns></returns>
		public IList GetMenusByRootNode(Node rootNode)
		{
			string hql = "from Menu m where m.RootNode.Id = :rootNodeId";
			IQuery q = this._activeSession.CreateQuery(hql);
			q.SetInt32("rootNodeId", rootNode.Id);
			return q.List();
		}

		#endregion

		#region Section specific
		
		/// <summary>
		/// Retrieve the sections belonging to a given node sorted by PlaceholderId and Position.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public IList GetSortedSectionsByNode(Node node)
		{
			string hql = "from Section s where s.Node.Id = ? order by s.PlaceholderId, s.Position ";
			return this._activeSession.Find(hql, node.Id, NHibernateUtil.Int32);
		}

		#endregion

		#region User specific

		/// <summary>
		/// Get a User by username and password. Use for login.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public User GetUserByUsernameAndPassword(string username, string password)
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(User));
			crit.Add(Expression.Eq("UserName", username));
			crit.Add(Expression.Eq("Password", password));
			IList results = crit.List();
			if (results.Count == 1)
			{
				return (User)results[0];
			}
			else if (results.Count > 1)
			{
				throw new Exception(String.Format("Multiple users found with the give username and password. Something is pretty wrong here"));
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Get a User by username and email.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		public User GetUserByUsernameAndEmail(string username, string email)
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(User));
			crit.Add(Expression.Eq("UserName", username));
			crit.Add(Expression.Eq("Email", email));
			IList results = crit.List();
			if (results.Count == 1)
			{
				return (User)results[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Find users with a given name or with a name that starts with the given search string.
		/// When the search string is empty, all users will be fetched.
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		public IList FindUsersByUsername(string searchString)
		{
			string hql;
			if (searchString.Length > 0)
			{
				hql = "from User u where u.UserName like ? order by u.UserName ";
				return this._activeSession.Find(hql, searchString + "%", NHibernateUtil.String);
			}
			else
			{
				return GetAll(typeof(User), "UserName");
			}
		}
		#endregion
	}
}
