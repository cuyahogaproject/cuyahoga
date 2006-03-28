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

		public User GetUserByUsernameAndEmail(string username, string email)
		{
			return this._userDao.GetUserByUsernameAndEmail(username, email);
		}

		public void CreateUser(User user)
		{
			throw new NotImplementedException();
		}

		public void ResetPassword(string username, string email)
		{
			throw new NotImplementedException();
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
