using System;
using System.Collections;

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
			if (this._activeSession != null && this._activeSession.IsOpen)
			{
				this._activeSession.Close();
			}
		}

		#region Generic methods

		/// <summary>
		/// Generic method for retrieving single objects by primary key.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="id"></param>
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
		/// Attach potentially stale objects to the current NHibernate session. This is required
		/// when objects are cached in the ASP.NET cache and they contain lazy loaded members.
		/// </summary>
		/// <param name="obj"></param>
		public void AttachObjectToCurrentSession(object obj)
		{
			if (this._activeSession != null)
			{
				if (this._activeSession.IsOpen)
				{
					this._activeSession.Update(obj);
				}
				else
				{
					throw new InvalidOperationException("The current NHibernate session is not open, so no objects can be attached.");
				}
			}
			else
			{
				throw new NullReferenceException("No active NHibernate session available to attach the object to.");
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

		#region Site specific

		public Site GetSiteBySiteUrl(string applicationPath)
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(Site));
			crit.Add(Expression.Eq("SiteUrl", applicationPath.ToLower()));
			IList results = crit.List();
			if (results.Count == 1)
			{
				return (Site)results[0];
			}
			else if (results.Count > 1)
			{
				throw new Exception(String.Format("Multiple sites found for ApplicationPath {0}. The ApplicationPath should be unique.", applicationPath));
			}
			else
			{
				return null;
			}
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

		/// <summary>
		/// Retrieve a node by short description (friendly url).
		/// </summary>
		/// <param name="shortDescription"></param>
		/// <returns></returns>
		public Node GetNodeByShortDescription(string shortDescription)
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(Node));
			crit.Add(Expression.Eq("ShortDescription", shortDescription));
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
		/// Attach a node to the current session (we need this for cached nodes that lose their sessions).
		/// </summary>
		/// <param name="node"></param>
		public void AttachNodeToCurrentSession(Node node)
		{
			if (this._activeSession != null)
			{
				if (this._activeSession.IsOpen)
				{
					this._activeSession.Update(node);
					// Also re-attach the sections. Updating the node doesn't automatically re-attach the sections.
					foreach (Section section in node.Sections)
					{
						this._activeSession.Update(section);
					}
				}
				else
				{
					throw new InvalidOperationException("The current NHibernate session is not open, so no nodes can be attached.");
				}
			}
			else
			{
				throw new NullReferenceException("No active NHibernate session available to attach the node to.");
			}
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
			return this._activeSession.Find(hql, node.Id, NHibernate.Type.TypeFactory.GetInt32Type());
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
				return this._activeSession.Find(hql, searchString + "%", NHibernate.Type.TypeFactory.GetStringType());
			}
			else
			{
				return GetAll(typeof(User), "UserName");
			}
		}

		#endregion
	}
}
