using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cuyahoga.Web.Mvc.Partials
{
	/// <summary>
	/// Store these in ViewData to invoke partial requests.
	/// </summary>
	/// <remarks>
	/// Thanks to Steve Sanderson: http://blog.codeville.net/2008/10/14/partial-requests-in-aspnet-mvc/
	/// </remarks>
	public class PartialRequest
	{
		public RouteValueDictionary RouteValues { get; private set; }

		public PartialRequest(object routeValues)
		{
			RouteValues = new RouteValueDictionary(routeValues);
		}

		public void Invoke(ControllerContext context)
		{
			RouteData rd = new RouteData(context.RouteData.Route, context.RouteData.RouteHandler);
			foreach (var pair in RouteValues)
			{
				rd.Values.Add(pair.Key, pair.Value);
			}
			IHttpHandler handler = new MvcHandler(new RequestContext(context.HttpContext, rd));
			handler.ProcessRequest(System.Web.HttpContext.Current);
		}
	}
}
