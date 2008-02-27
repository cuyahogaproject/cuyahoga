using System;
using System.Security.Authentication;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Membership
{
	/// <summary>
	/// Provides authentication functionality based on Cuyahoga's internal database.
	/// </summary>
	public class DefaultAuthenticationService : IAuthenticationService
	{
		private IUserDao _userDao;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="userDao"></param>
		public DefaultAuthenticationService(IUserDao userDao)
		{
			this._userDao = userDao;
		}

		#region IAuthenticationService Members

		public User AuthenticateUser(string username, string password, string ipAddress)
		{
			if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
			{
				throw new AuthenticationException("EmptyUsernameOrPassword");
			}
			try
			{
				string hashedPassword = User.HashPassword(password);
				User user = this._userDao.GetUserByUsernameAndPassword(username, hashedPassword);
				if (user == null)
				{
					throw new AuthenticationException("InvalidUsernamePassword");
				}
				user.LastIp = ipAddress;
				user.LastLogin = DateTime.Now;
				this._userDao.SaveOrUpdateUser(user);
				user.IsAuthenticated = true;
				return user;
			}
			catch (Exception ex)
			{
				throw new AuthenticationException("AuthenticationException", ex);
			}
		}

		#endregion
	}
}
