using System.Web.Mvc;
using Cuyahoga.Web.Mvc.Partials;

namespace Cuyahoga.Web.Manager.Filters
{
	public class SiteChooserFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			// No need to do this for ajax requests.
			if (!filterContext.HttpContext.Request.IsAjaxRequest())
			{
				// Register partial request for the site chooser component.
				filterContext.Controller.ViewData["SiteChooser"] = new PartialRequest(new
				                                                                      	{
				                                                                      		area = "Manager",
				                                                                      		controller = "Dashboard",
				                                                                      		action = "SiteChooser"
				                                                                      	});
			}
		}
	}
}
