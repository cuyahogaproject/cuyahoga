using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using Castle.Services.Transaction;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Util;
using NHibernate.Expression;

namespace Cuyahoga.Core.Service.Membership
{
	/// <summary>
	/// Provides functionality for user management based on Cuyahoga's internal database.
	/// </summary>
	[Transactional]
	public class DefaultUserService : IUserService
	{
		private IUserDao _userDao;
		private ICommonDao _commonDao;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="userDao"></param>
		/// <param name="commonDao"></param>
		public DefaultUserService(IUserDao userDao, ICommonDao commonDao)
		{
			this._userDao = userDao;
			this._commonDao = commonDao;
		}

		#region IUserService Members

		public IList FindUsersByUsername(string searchString)
		{
			return this._userDao.FindUsersByUsername(searchString);
		}

		public IList<User> FindUsers(string username, int? roleId, bool? isActive, Site site, int pageSize, int? pageNumber, out int totalCount)
		{ 
			int? siteId = null;
			// When site is null, the user needs to have permissions to perform a global search across all sites.
			if (site == null)
			{
				User currentUser = Thread.CurrentPrincipal as User;
				if (currentUser == null || ! currentUser.HasRight(Rights.GlobalPermissions))
				{
					throw new SecurityException("ActionNotAllowedException");
				}
			}
			else
			{
				siteId = site.Id;
			}

			if (!pageNumber.HasValue)
			{
				pageNumber = 1;
			}
			return this._userDao.FindUsers(username, roleId, isActive, siteId, pageSize, pageNumber.Value, out totalCount);
		}

		public User GetUserById(int userId)
		{
			return this._commonDao.GetObjectById(typeof(User), userId, true) as User;
		}

		public User GetUserByUserName(string userName)
		{
			return this._commonDao.GetObjectByDescription(typeof(User), "UserName", userName) as User;
		}

		public User GetUserByUsernameAndEmail(string username, string email)
		{
			return this._userDao.GetUserByUsernameAndEmail(username, email);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public string CreateUser(string username, string email, Site currentSite)
		{
			User user = new User();
			user.UserName = username;
			user.Email = email;
			user.IsActive = true;
			string newPassword = user.GeneratePassword();
			// Add the default role from the current site.
			user.Roles.Add(currentSite.DefaultRole);
			this._commonDao.SaveOrUpdateObject(user);

			return newPassword;
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void CreateUser(User user)
		{
			this._commonDao.SaveObject(user);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void UpdateUser(User user)
		{
			this._commonDao.SaveOrUpdateObject(user);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void DeleteUser(User user)
		{
			User currentUser = Thread.CurrentPrincipal as User;
			if (currentUser != null && currentUser.Id == user.Id)
			{
				throw new DeleteForbiddenException("DeleteYourselfNotAllowedException");
			}
			this._commonDao.DeleteObject(user);
		}

		public string ResetPassword(string username, string email)
		{
			User user = this._userDao.GetUserByUsernameAndEmail(username, email);
			if (user == null)
			{
				throw new NullReferenceException("No user found with the given username and email");
			}
			string newPassword = user.GeneratePassword();
			this._userDao.SaveOrUpdateUser(user);
			return newPassword;
		}

		public IList GetAllRoles()
		{
			return this._commonDao.GetAll(typeof(Role));
		}

		public IList<Role> GetAllRolesBySite(Site site)
		{
			return this._userDao.GetAllRolesBySite(site);
		}

		public Role GetRoleById(int roleId)
		{
			return (Role)this._commonDao.GetObjectById(typeof(Role), roleId);
		}

		public IList<Role> GetRolesByIds(int[] roleIds)
		{
			return this._commonDao.GetByIds<Role>(roleIds);
		}

		public IList<Role> GetAllGlobalRoles()
		{
			DetachedCriteria crit = DetachedCriteria.For(typeof(Role))
				.Add(Expression.Eq("IsGlobal", true))
				.AddOrder(Order.Asc("Name"));
			return this._commonDao.GetAllByCriteria<Role>(crit);
		}

		/// <summary>
		/// Get all available rights.
		/// </summary>
		/// <returns></returns>
		public IList<Right> GetAllRights()
		{
			return this._commonDao.GetAll<Right>("Name");
		}

		public Right GetRightById(int rightId)
		{
			return (Right)this._commonDao.GetObjectById(typeof (Right), rightId);
		}

		public IList<Right> GetRightsByIds(int[] rightIds)
		{
			return this._commonDao.GetByIds<Right>(rightIds);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void CreateRole(Role role, Site currentSite)
		{
			ConnectRoleToSites(role, currentSite);
			CheckRightsForRoleAndSite(role, currentSite);
			this._commonDao.SaveObject(role);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void UpdateRole(Role role, Site currentSite)
		{
			ConnectRoleToSites(role, currentSite);
			this._commonDao.SaveOrUpdateObject(role);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void DeleteRole(Role role)
		{
			this._commonDao.DeleteObject(role);
		}

		#endregion

		private void ConnectRoleToSites(Role role, Site currentSite)
		{
			role.Sites.Clear();
			// If role is global, it has to be connected to all sites
			if (role.IsGlobal)
			{
				// First check if the user is allowed to connect to all sites.
				CheckGlobalRole(role);
			
				IList<Site> allSites = this._commonDao.GetAll<Site>();
				foreach (Site site in allSites)
				{
					role.Sites.Add(site);
				}
			}
			else
			{
				role.Sites.Add(currentSite);
			}
		}

		private void CheckGlobalRole(Role role)
		{
			User currentUser = Thread.CurrentPrincipal as User;
			if (currentUser == null || (!currentUser.HasRight(Rights.GlobalPermissions) && role.IsGlobal))
			{
				throw new SecurityException("Tried to set a role to global without enough permissions.");
			}
		}

		private void CheckRightsForRoleAndSite(Role role, Site currentSite)
		{
			// Make sure that the role hasn't any rights that the user doesn't have for the current site.
			User currentUser = (User)Thread.CurrentPrincipal;
			foreach (Right right in role.Rights)
			{
				if (! currentUser.HasRight(right.Name, currentSite))
				{
					throw new SecurityException("You can not assign rights to a role that you don't have yourself.");
				}
			}
		}
	}
}
