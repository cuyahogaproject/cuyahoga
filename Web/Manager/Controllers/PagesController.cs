using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Util;
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
				// There is an active node. Add node to ViewData.
				Node activeNode = this._nodeService.GetNodeById(id.Value);
				ViewData["ActiveNode"] = activeNode;
				ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", activeNode.Culture);
			}
			return View(CuyahogaContext.CurrentSite.RootNodes);
		}

		public ActionResult GetPageListItem(int nodeId)
		{
			Node node = this._nodeService.GetNodeById(nodeId);
			return PartialView("PageListItem", node);
		}

		public ActionResult GetChildPageListItems(int nodeId)
		{
			Node node = this._nodeService.GetNodeById(nodeId);
			return PartialView("PageListItems", node.ChildNodes);
		}

		public ActionResult SelectPage(int nodeId)
		{
			Node node = this._nodeService.GetNodeById(nodeId);
			ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", node.Culture);
			return PartialView("SelectedPage", node);
		}
	}
}
