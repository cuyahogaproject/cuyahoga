using System.Web.Routing;

namespace Cuyahoga.Web.Mvc.Areas
{
	public static class RouteCollectionExtensions
	{
		/// <summary>
		/// Create a specific area to partition the MVC application.
		/// </summary>
		/// <param name="routes"></param>
		/// <param name="areaName"></param>
		/// <param name="controllersNamespace"></param>
		/// <param name="routeEntries"></param>
		/// <remarks>Thanks to Steve Sanderson, http://blog.codeville.net/2008/11/05/app-areas-in-aspnet-mvc-take-2/</remarks>
		public static void CreateArea(this RouteCollection routes, string areaName, string controllersNamespace, params Route[] routeEntries)
		{
			foreach (var route in routeEntries)
			{
				if (route.Constraints == null) route.Constraints = new RouteValueDictionary();
				if (route.Defaults == null) route.Defaults = new RouteValueDictionary();
				if (route.DataTokens == null) route.DataTokens = new RouteValueDictionary();

				route.Constraints.Add("area", areaName);
				route.Defaults.Add("area", areaName);
				route.DataTokens.Add("namespaces", new string[] { controllersNamespace });

				if (!routes.Contains(route)) // To support "new Route()" in addition to "routes.MapRoute()"
				{
					routes.Add(route);
				}
			}
		}
	}
}