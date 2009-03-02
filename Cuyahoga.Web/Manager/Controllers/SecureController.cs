using System.Web.Mvc;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Controllers
{
	[Authorize]
	public abstract class SecureController : BaseController
	{
		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			// Add Cuyahoga user to the ViewData, so the views don't have to call dirty static 
			// things like CuyahogaContext.Current
			if (filterContext.HttpContext.User is User)
			{
				ViewData["CuyahogaUser"] = filterContext.HttpContext.User;
			}
			base.OnAuthorization(filterContext);
		}
	}
}