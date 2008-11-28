using System.Web.Mvc;

namespace Cuyahoga.Web.Manager.Controllers
{
	public class TemplateController : SecureController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}