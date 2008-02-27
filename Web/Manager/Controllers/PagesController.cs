using Castle.MonoRail.Framework;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Manager.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[CuyahogaPermission(RequiredRights = "Administrator")]
	public class PagesController : SecureController
	{
		public void Index()
		{
		}
	}
}
