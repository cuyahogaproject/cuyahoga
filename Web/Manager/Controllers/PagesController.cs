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
		private INodeService _nodeService;

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
					ShowMessage("Page properties are updated sucessfully.", true);
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
					ShowMessage("Link properties are updated sucessfully.", true);
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
		public ActionResult MovePage(int nodeIdToMove, int nodeIdToMoveTo)
		{
			this._nodeService.MoveNode(nodeIdToMove, nodeIdToMoveTo);
			return RedirectToAction("Index", new { id = nodeIdToMove });
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
				result.Message = "Order of pages was updated successfully.";
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
