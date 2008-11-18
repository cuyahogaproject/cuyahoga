using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Core.Service.Membership
{
	/// <summary>
	/// Provides functionality for user management based on Cuyahoga's internal database.
	/// </summary>
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

		public User GetUserByUsernameAndEmail(string username, string email)
		{
			return this._userDao.GetUserByUsernameAndEmail(username, email);
		}

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

		public void UpdateUser(User user)
		{
			this._commonDao.SaveOrUpdateObject(user);
		}

		public void DeleteUser(User user)
		{
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

		#endregion
	}
}
