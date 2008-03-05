using Cuyahoga.Core.Service.Membership;

namespace Cuyahoga.Web.Manager.Controllers
{
	[CuyahogaPermission(RequiredRights = Rights.ManageFiles)]
	public class FilesController : SecureController
	{
		public void Index()
		{
		}
	}
}
