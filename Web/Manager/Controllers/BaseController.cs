using Castle.MonoRail.Framework;

namespace Cuyahoga.Web.Manager.Controllers
{
	[Layout("Default"), Rescue("GenericError")]
	[Resource("globaltext", "Cuyahoga.Web.Manager.GlobalResources")]
	[ControllerDetails(Area = "Manager")]
	public abstract class BaseController : SmartDispatcherController
	{
	}
}
