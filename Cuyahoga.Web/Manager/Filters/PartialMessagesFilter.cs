using System.Web.Mvc;

namespace Cuyahoga.Web.Manager.Filters
{
	/// <summary>
	/// While rendering partial views, we have no possiblity to show messages in the standard Cuyahoga way.
	/// This filter adds a little bit of javascript that displays the messages. So when you want to use the standard
	/// ShowMessage() etc stuff with partial views, decorate the action methods with this attribute.
	/// </summary>
	public class PartialMessagesFilter : ActionFilterAttribute
	{
		public override void  OnResultExecuting(ResultExecutingContext filterContext)
		{
			PartialViewResult result = new PartialViewResult();
			result.ViewName = "PartialMessages";
			result.ViewData.Model = filterContext.Controller.ViewData["Messages"];
			result.ExecuteResult(filterContext.Controller.ControllerContext);
 			base.OnResultExecuting(filterContext);
		}
	}
}
