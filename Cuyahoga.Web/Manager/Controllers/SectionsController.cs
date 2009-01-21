using System;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Validation;
using Cuyahoga.Core.Validation.ModelValidators;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc.Filters;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManagePages)]
	public class SectionsController : SecureController
	{
		private const string settingsFormElementPrefix = "settings_";
		private readonly ISectionService _sectionService;
		private readonly INodeService _nodeService;
		private ModuleLoader _moduleLoader;

		public SectionsController(ISectionService sectionService, INodeService nodeService, ModuleLoader moduleLoader, SectionModelValidator modelValidator)
		{
			_sectionService = sectionService;
			_nodeService = nodeService;
			_moduleLoader = moduleLoader;
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

		public ActionResult SectionProperties(int id)
		{
			Section section = this._sectionService.GetSectionById(id);
			return View(section);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult AddSectionToPage([Bind(Include = "PlaceHolderId, Title, ShowTitle, CacheDuration")]Section section, int moduleTypeId, int nodeId)
		{
			section.ModuleType = this._sectionService.GetModuleTypeById(moduleTypeId);
			section.Node = this._nodeService.GetNodeById(nodeId);
			section.Node.AddSection(section);
			UpdateSectionSettingsFromForm(section, settingsFormElementPrefix);
			try
			{
				if (ValidateModel(section, new [] { "Title", "ModuleType", "Settings" }, "section"))
				{
					this._sectionService.SaveSection(section);
					ShowMessage(String.Format(GlobalResources.SectionCreatedMessage, section.Title));
					ViewData["CanCloseDialog"] = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while adding section to page.", ex);
				ShowException(ex);
			}
			ViewData["NodeId"] = nodeId;
			return View("NewSectionDialog", section);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UpdateSection(int id)
		{
			Section section = this._sectionService.GetSectionById(id);
			UpdateSectionSettingsFromForm(section, settingsFormElementPrefix);
			try
			{
				if (TryUpdateModel(section, "section", new[] { "Title", "ShowTitle", "CacheDuration" })
					&& ValidateModel(section, new [] { "Title", "Settings" }, "section"))
				{
					this._sectionService.UpdateSection(section);
					ShowMessage(GlobalResources.SectionPropertiesUpdatedMessage, true);
					return RedirectToAction("SectionProperties", new { id = section.Id });
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while updating section.", ex);
				ShowException(ex);
			}
			return View("SectionProperties", section);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DeleteSectionFromPage(int nodeId, int sectionIdToDelete)
		{
			Section sectionToDelete = this._sectionService.GetSectionById(sectionIdToDelete);
			try
			{
				this._sectionService.DeleteSection(sectionToDelete, nodeId, this._moduleLoader.GetModuleFromSection(sectionToDelete));
				ShowMessage(String.Format(GlobalResources.SectionDeletedMessage, sectionToDelete.Title), true);
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while deleting section.", ex);
				ShowException(ex, true);
			}
			return RedirectToAction("Design", "Pages", new { id = nodeId });
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DetachSectionFromPage(int nodeId, int sectionIdToDelete)
		{
			Section sectionToDetach = this._sectionService.GetSectionById(sectionIdToDelete);
			try
			{
				this._sectionService.DetachSection(sectionToDetach, nodeId);
				ShowMessage(String.Format(GlobalResources.SectionDetachedMessage, sectionToDetach.Title), true);
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while detaching section.", ex);
				ShowException(ex, true);
			}
			return RedirectToAction("Design", "Pages", new { id = nodeId });
		}

		#region Ajax actions

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ArrangeSections(string placeholder, int[] orderedSectionIds)
		{
			AjaxMessageViewData result = new AjaxMessageViewData();
			try
			{
				if (orderedSectionIds != null)
				{
					this._sectionService.ArrangeSections(placeholder, orderedSectionIds);
					result.Message = String.Format(GlobalResources.SectionsArrangedMessage, placeholder);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while arranging sections.", ex);
				result.Error = ex.Message;
			}

			return Json(result);
		}

		#endregion

		private void UpdateSectionSettingsFromForm(Section section, string prefix)
		{
			// Perhaps we should create a custom model binder?
			foreach (ModuleSetting moduleSetting in section.ModuleType.ModuleSettings)
			{
				string formElementName = prefix + moduleSetting.Name;
				// Special treatment for boolean (checkbox) settings because these aren't passed when the checkbox is not checked.
				if (moduleSetting.GetRealType() == typeof(System.Boolean))
				{
					section.Settings[moduleSetting.Name] = (Request.Params[formElementName] != null).ToString();
				}
				else if (Request.Params[formElementName] != null)
				{
					section.Settings[moduleSetting.Name] = Request.Params[formElementName];
				}
			}
		}
	}
}
