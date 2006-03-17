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

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="userDao"></param>
		public DefaultUserService(IUserDao userDao)
		{
			this._userDao = userDao;
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

		#endregion
	}
}
