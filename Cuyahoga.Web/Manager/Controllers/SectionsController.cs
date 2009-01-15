using System;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Mvc.Filters;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManagePages)]
	public class SectionsController : SecureController
	{
		private readonly ISectionService _sectionService;
		private readonly INodeService _nodeService;

		public SectionsController(ISectionService sectionService, INodeService nodeService, IModelValidator<Section> modelValidator)
		{
			_sectionService = sectionService;
			_nodeService = nodeService;
			ModelValidator = modelValidator;
		}

		public ActionResult Index()
		{
			// Add action logic here
			throw new NotImplementedException();
		}

		public ActionResult NewSectionDialog(int moduleTypeId, int nodeId, string placeholder)
		{
			ViewData["NodeId"] = nodeId;
			Section newSection = new Section();
			newSection.ModuleType = this._sectionService.GetModuleTypeById(moduleTypeId);
			newSection.PlaceholderId = placeholder;
			return View("NewSectionDialog", newSection);
		}

		public ActionResult AddSectionToPage([Bind(Include = "PlaceHolderId, Title, ShowTitle, CacheDuration")]Section section, int moduleTypeId, int nodeId)
		{
			section.ModuleType = this._sectionService.GetModuleTypeById(moduleTypeId);
			section.Node = this._nodeService.GetNodeById(nodeId);
			section.Node.AddSection(section);

			try
			{
				if (ValidateModel(section, new [] { "Title", "ModuleType" }, "section"))
				{
					this._sectionService.SaveSection(section);
					ShowMessage(String.Format(GlobalResources.SectionCreatedMessage, section.Title));
					ViewData["CanCloseDialog"] = true;
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			ViewData["NodeId"] = nodeId;
			return View("NewSectionDialog", section);
		}
	}
}
