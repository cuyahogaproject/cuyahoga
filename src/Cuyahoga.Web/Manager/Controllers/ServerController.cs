using System.Web.Mvc;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Mvc.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageServer)]
	public class ServerController : ManagerController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
