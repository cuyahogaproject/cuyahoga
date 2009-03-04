using System.Web.Mvc;
using Cuyahoga.Web.Mvc.Controllers;

namespace Cuyahoga.Modules.StaticHtml.Controllers
{
	public class ManageContentController : ModuleAdminController
	{
		public ActionResult Edit()
		{
			return View();
		}

	}
}
