using System.Web.Mvc;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Manager.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageServer)]
	public class ServerController : SecureController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
