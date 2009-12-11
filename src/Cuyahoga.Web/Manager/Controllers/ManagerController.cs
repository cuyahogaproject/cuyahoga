using Cuyahoga.Web.Manager.Filters;
using Cuyahoga.Web.Mvc.Controllers;

namespace Cuyahoga.Web.Manager.Controllers
{
	[MenuDataFilter]
	[SiteChooserFilter]
    public class ManagerController : SecureController
    {
    }
}
