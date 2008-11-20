using System;
using System.Collections;
using System.Collections.Generic;

using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using Cuyahoga.Core.Util;
using NHibernate;
using NHibernate.Expression;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// Provides data access for user-related components.
	/// </summary>
	[Transactional]
	public class UserDao : IUserDao
	{
		private ISessionManager _sessionManager;
		private ICommonDao _commonDao;

		/// <summary>
		/// Default constructor;
		/// </summary>
		/// <param name="sessionManager"></param>
		public UserDao(ISessionManager sessionManager, ICommonDao commonDao)
		{
			this._sessionManager = sessionManager;
			this._commonDao = commonDao;
		}

		#region IUserDao Members

		public User GetUserByUsernameAndPassword(string username, string password)
		{
			ISession session = this._sessionManager.OpenSession();

			ICriteria crit = session.CreateCriteria(typeof(User));
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

		public User GetUserByUsernameAndEmail(string username, string email)
		{
			ISession session = this._sessionManager.OpenSession();

			ICriteria crit = session.CreateCriteria(typeof(User));
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

		public IList FindUsersByUsername(string searchString)
		{
			if (searchString.Length > 0)
			{
				ISession session = this._sessionManager.OpenSession();

				string hql = "from User u where u.UserName like ? order by u.UserName ";
				return session.Find(hql, searchString + "%", NHibernateUtil.String);
			}
			else
			{
				return this._commonDao.GetAll(typeof(User), "UserName");
			}
		}

		public IList<User> FindUsers(string username, int? roleId, bool? isActive, int? siteId, int pageSize, int pageNumber, out int totalCount)
		{
			ISession session = this._sessionManager.OpenSession();
			ICriteria userCriteria = session.CreateCriteria(typeof(User));
			ICriteria countCriteria = session.CreateCriteria(typeof(User), "userCount");

			if (!String.IsNullOrEmpty(username))
			{
				userCriteria.Add(Expression.InsensitiveLike("UserName", username, MatchMode.Start));
				countCriteria.Add(Expression.InsensitiveLike("UserName", username, MatchMode.Start));
			}
			if (roleId.HasValue)
			{
				userCriteria.CreateCriteria("Roles", "r1").Add(Expression.Eq("r1.Id", roleId));
				countCriteria.CreateCriteria("Roles", "r1").Add(Expression.Eq("r1.Id", roleId));
			}
			if (isActive.HasValue)
			{
				userCriteria.Add(Expression.Eq("IsActive", isActive));
				countCriteria.Add(Expression.Eq("IsActive", isActive));
			}
			// Filter users that are related to the given site. Don't do this when already filtering on a role
			if (siteId.HasValue && ! roleId.HasValue)
			{
				// We need two subqueries to traverse two many-many relations. Directly creating 
				// criteria on the collection properties results in a cartesian product.
				DetachedCriteria roleIdsForSite = DetachedCriteria.For(typeof (Role))
					.SetProjection(Projections.Property("Id"))
					.CreateCriteria("Sites", "site")
					.Add(Expression.Eq("site.Id", siteId.Value));
				DetachedCriteria userIdsForRoles = DetachedCriteria.For(typeof(User))
					.SetProjection(Projections.Distinct(Projections.Property("Id")))
					.CreateCriteria("Roles")
						.Add(Subqueries.PropertyIn("Id", roleIdsForSite));
				userCriteria.Add(Subqueries.PropertyIn("Id", userIdsForRoles));
				countCriteria.Add(Subqueries.PropertyIn("Id", userIdsForRoles));		
			}

			userCriteria.SetFirstResult((pageNumber - 1) * pageSize);
			userCriteria.SetMaxResults(pageSize);

			countCriteria.SetProjection(Projections.RowCount());
			totalCount = (int) countCriteria.UniqueResult();

			return userCriteria.List<User>();
		}


		/// <summary>
		/// Find users and return paged results.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="roleId"></param>
		/// <param name="isActive"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageNumber"></param>
		/// <returns></returns>
		public PagedResultSet<User> FindUsers(string username, int? roleId, bool? isActive, int pageSize, int pageNumber)
		{
			ISession session = this._sessionManager.OpenSession();
			ICriteria userCriteria = session.CreateCriteria(typeof (User));
			ICriteria countCriteria = session.CreateCriteria(typeof (User));

			if (! String.IsNullOrEmpty(username))
			{
				userCriteria.Add(Expression.InsensitiveLike("UserName", username, MatchMode.Start));
				countCriteria.Add(Expression.InsensitiveLike("UserName", username, MatchMode.Start));
			}
			if (roleId.HasValue)
			{
				userCriteria.CreateCriteria("Roles").Add(Expression.Eq("Id", roleId));
				countCriteria.CreateCriteria("Roles").Add(Expression.Eq("Id", roleId));
			}
			if (isActive.HasValue)
			{
				userCriteria.Add(Expression.Eq("IsActive", isActive));
				countCriteria.Add(Expression.Eq("IsActive", isActive));
			}
			userCriteria.SetFirstResult((pageNumber - 1) * pageSize);
			userCriteria.SetMaxResults(pageSize);
			countCriteria.SetProjection(Projections.RowCount());

			return new PagedResultSet<User>(userCriteria.List<User>(), (int)countCriteria.UniqueResult());
		}

		public IList<Section> GetViewableSectionsByUser(User user)
        {

            string hql = "select s from User u join u.Roles as r, Section s join s.SectionPermissions sp " +
                        "where u.Id = :userId and r.Id = sp.Role.Id and sp.ViewAllowed = 1";
            IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
            q.SetInt32("userId", user.Id);
            return q.List<Section>();
        }

        //TODO: check why this throws an ADO Exeption (NHibernate bug?)
        //public IList<Section> GetViewableSectionsByRoles(IList<Role> roles)
        //{
        //    string hql = "select s from Section s join s.SectionPermissions as sp where sp.Role in :roles and sp.ViewAllowed = 1";
        //    IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
        //    q.SetParameterList("roles", roles);
        //    return q.List<Section>();
        //}

        public IList<Section> GetViewableSectionsByAccessLevel(AccessLevel accessLevel)
        {
            int permission = (int)accessLevel;

            string hql = "select s from Section s join s.SectionPermissions sp, Role r "+
                "where r.PermissionLevel = :permission and r.Id = sp.Role.Id and sp.ViewAllowed = 1";
            IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
            q.SetInt32("permission", permission);
            return q.List<Section>();
        }

        public IList<Role> GetRolesByAccessLevel(AccessLevel accessLevel)
        {
            int permission = (int)accessLevel;
            string hql = "select r from Role r where r.PermissionLevel = :permission";
            IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
            q.SetInt32("permission", permission);
            return q.List<Role>();
        }

		public IList<Role> GetAllRolesBySite(Site site)
		{
			ISession session = this._sessionManager.OpenSession();
			ICriteria crit = session.CreateCriteria(typeof (Role))
				.AddOrder(Order.Asc("Name"))
				.CreateCriteria("Sites")
				.Add(Expression.Eq("Id", site.Id));
			return crit.List<Role>();
		}

		[Transaction(TransactionMode.Requires)]
		public void SaveOrUpdateUser(User user)
		{
			ISession session = this._sessionManager.OpenSession();
			session.SaveOrUpdate(user);
		}

		[Transaction(TransactionMode.Requires)]
		public void DeleteUser(User user)
		{
			ISession session = this._sessionManager.OpenSession();
			session.Delete(user);
		}

		#endregion
	}
}
