using System;
using System.Collections;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

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

		public Role GetRoleById(int roleId)
		{
			return (Role)this._commonDao.GetObjectById(typeof(Role), roleId);
		}

		#endregion
	}
}
