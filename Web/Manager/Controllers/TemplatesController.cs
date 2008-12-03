using System.Web.Mvc;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Mvc.Filters;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageTemplates)]
	public class TemplatesController : SecureController
	{
		public ActionResult Index()
		{
			ViewData["Title"] = GlobalResources.ManageTemplatesPageTitle;
			return View();
		}
	}
}