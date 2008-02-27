using Castle.MonoRail.Framework;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Manager.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	public class FilesController : SecureController
	{
		[CuyahogaPermission(RequiredRights = "SomeNonExistingRight")]
		public void Index()
		{
		}
	}
}
