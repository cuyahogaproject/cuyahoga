using System;
using System.Security;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Mvc.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class PermissionFilterAttribute : FilterAttribute, IAuthorizationFilter
	{
		private string _rights;
		private string[] _rightsArray;

		/// <summary>
		/// The required rights as a comma-separated string.
		/// </summary>
		public string RequiredRights
		{
			get { return _rights; }
			set { SetRights(value); }
		}

		/// <summary>
		/// Array of required rights.
		/// </summary>
		public string[] RightsArray
		{
			get { return this._rightsArray; }
		}

		private void SetRights(string rightsAsString)
		{
			this._rights = rightsAsString;
			this._rightsArray = rightsAsString.Split(new char[1] { ',' });
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			// Only check authorization when rights are defined and the user is authenticated.
			if (this._rightsArray.Length > 0 && filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				// Get the current user from the request. It would be nice if we could inject ICuyahogaContext
				// but that's not possible with the current version.
				User cuyahogaUser = filterContext.HttpContext.User as User;

				if (cuyahogaUser == null)
				{
					throw new SecurityException("UserNullException");
				}
				foreach (string right in RightsArray)
				{
					if (!cuyahogaUser.HasRight(right))
					{
						throw new SecurityException("ActionNotAllowedException");
					}
				}
			}
		}
	}
}