using System.Web.Mvc;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	public class TemplatesController : SecureController
	{
		public ActionResult Index()
		{
			ViewData["Title"] = GlobalResources.ManageTemplatesPageTitle;
			return View();
		}
	}
}