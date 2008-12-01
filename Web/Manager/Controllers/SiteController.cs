using System.Web.Mvc;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Mvc.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageSite)]
	public class SiteController : SecureController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
