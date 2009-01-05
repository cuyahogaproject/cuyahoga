using System;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Manager.Helpers;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc.Filters;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManagePages)]
	public class PagesController : SecureController
	{
		private readonly INodeService _nodeService;

		public PagesController(INodeService nodeService, IModelValidator<Node> modelValidator)
		{
			_nodeService = nodeService;
			this.ModelValidator = modelValidator;
		}

		public ActionResult Index(int? id)
		{
			ViewData["Title"] = GlobalResources.ManagePagesPageTitle;
			// The given id is of the active node.
			if (id.HasValue)
			{
				// There is an active node. Add node to ViewData.
				Node activeNode = this._nodeService.GetNodeById(id.Value);
				if (activeNode.IsExternalLink)
				{
					ViewData["LinkTargets"] = new SelectList(EnumHelper.GetValuesWithDescription<LinkTarget>(), "Key", "Value", activeNode.LinkTarget);
				}
				else
				{
					ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", activeNode.Culture);
				}
				ViewData["ActiveNode"] = activeNode;
			}
			return View("Index", CuyahogaContext.CurrentSite.RootNodes);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult SavePageProperties(int id)
		{
			Node node = this._nodeService.GetNodeById(id);
			var includeProperties = new[] { "Title", "ShortDescription", "Culture", "ShowInNavigation" };
			try
			{
				if (TryUpdateModel(node, includeProperties) && ValidateModel(node, includeProperties))
				{
					this._nodeService.SaveNode(node);
					ShowMessage(GlobalResources.PagePropertiesUpdatedMessage, true);
					return RedirectToAction("Index", new { id = node.Id });
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			return Index(node.Id);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult SaveLinkProperties(int id)
		{
			Node node = this._nodeService.GetNodeById(id);
			var includeProperties = new[] { "Title", "LinkUrl", "LinkTarget" };
			try
			{
				if (TryUpdateModel(node, includeProperties) && ValidateModel(node, includeProperties))
				{
					this._nodeService.SaveNode(node);
					ShowMessage(GlobalResources.LinkPropertiesUpdatedMessage, true);
					return RedirectToAction("Index", new { id = node.Id });
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			return Index(node.Id);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult MovePage(int nodeId, int newParentNodeId)
		{
			this._nodeService.MoveNode(nodeId, newParentNodeId);
			ShowMessage(GlobalResources.PageMovedMessage, true);
			return RedirectToAction("Index", new { id = nodeId });
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CopyPage(int nodeId, int newParentNodeId)
		{
			Node newNode = this._nodeService.CopyNode(nodeId, newParentNodeId);
			ShowMessage(GlobalResources.PageCopiedMessage, true);
			return RedirectToAction("Index", new { id = newNode.Id });
		}

		#region AJAX actions

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
			if (node.IsExternalLink)
			{
				ViewData["LinkTargets"] = new SelectList(EnumHelper.GetValuesWithDescription<LinkTarget>(), "Key", "Value", node.LinkTarget);
				return PartialView("SelectedLink", node);
			}
			else
			{
				ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", node.Culture);
				return PartialView("SelectedPage", node);
			}
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult SortPages(int parentNodeId, int[] orderedChildNodeIds)
		{
			AjaxMessageViewData result = new AjaxMessageViewData();
			try
			{
				this._nodeService.SortNodes(parentNodeId, orderedChildNodeIds);
				result.Message = GlobalResources.PageOrderUpdatedMessage;
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while sorting pages.", ex);
				result.Error = ex.Message;
			}

			return Json(result);
		}

		#endregion
	}
}
