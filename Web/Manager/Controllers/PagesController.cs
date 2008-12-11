using System.Web.Mvc;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Web.Mvc.Filters;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManagePages)]
	public class PagesController : SecureController
	{
		private INodeService _nodeService;

		public PagesController(INodeService nodeService)
		{
			_nodeService = nodeService;
		}

		public ActionResult Index(int? id)
		{
			ViewData["Title"] = GlobalResources.ManagePagesPageTitle;
			// The given id is of the active node.
			if (id.HasValue)
			{
				ViewData["ActiveNode"] = this._nodeService.GetNodeById(id.Value);
			}
			return View(CuyahogaContext.CurrentSite.RootNodes);
		}
	}
}
