using Cuyahoga.Core.Service.Membership;

namespace Cuyahoga.Web.Manager.Controllers
{
	[CuyahogaPermission(RequiredRights = Rights.ManagePages)]
	public class PagesController : SecureController
	{
		public void Index()
		{
		}
	}
}
