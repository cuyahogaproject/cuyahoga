using System.Web.Routing;

namespace Cuyahoga.Web.Mvc
{
    public interface IMvcModule
    {
    	void RegisterRoutes(RouteCollection routes);
    }
}
