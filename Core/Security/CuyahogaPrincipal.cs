using System;
using System.Security;
using System.Security.Principal;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Security
{
	/// <summary>
	/// Summary description for CuyahogaPrincipal.
	/// </summary>
	public class CuyahogaPrincipal : IPrincipal
	{
		private User _user;

		/// <summary>
		/// 
		/// </summary>
		public IIdentity Identity
		{
			get { return this._user; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool IsInRole(string role)
		{
			foreach (Role roleObject in this._user.Roles)
			{
				if (roleObject.Name.Equals(role))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Default constructor. An instance of an authenticated user is required when creating this principal.
		/// </summary>
		/// <param name="user"></param>
		public CuyahogaPrincipal(User user)
		{
			if (user != null && user.IsAuthenticated)
			{
				this._user = user;
			}
			else
			{
				throw new SecurityException("Cannot create a principal without u valid user");
			}
		}
	}
}
