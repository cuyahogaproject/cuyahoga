using System.Security.Principal;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Mvc.Helpers
{
	public static class PermissionExtensions
	{
		public static bool HasRight(this HtmlHelper htmlHelper, IPrincipal user, string right)
		{
			User cuyahogaUser = user as User;
			if (cuyahogaUser != null && cuyahogaUser.HasRight(right))
			{
				return true;
			}
			return false; 
		}
	}
}
