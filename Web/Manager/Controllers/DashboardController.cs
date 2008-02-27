using Castle.MonoRail.Framework;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Manager.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	/// <summary>
	/// Controller for the Cuyahoga Manager start page.
	/// </summary>
	[CuyahogaPermission(RequiredRights = "Administrator")]
	public class DashboardController : SecureController
	{	
		public void Index()
		{
		}
	}
}
