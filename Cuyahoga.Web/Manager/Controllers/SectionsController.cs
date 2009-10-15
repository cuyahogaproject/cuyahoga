using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Validation.ModelValidators;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Manager.Filters;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc.Filters;
using Cuyahoga.Web.Mvc.WebForms;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManagePages)]
	public class SectionsController : ManagerController
	{
		private const string settingsFormElementPrefix = "section.Settings_";
		private readonly ISectionService _sectionService;
		private readonly INodeService _nodeService;
		private readonly ITemplateService _templateService;
		private readonly ModuleLoader _moduleLoader;

		public SectionsController(ISectionService sectionService, INodeService nodeService, ModuleLoader moduleLoader, SectionModelValidator modelValidator, ITemplateService templateService)
		{
			_sectionService = sectionService;
			_templateService = templateService;
			_nodeService = nodeService;
			_moduleLoader = moduleLoader;
			ModelValidator = modelValidator;
		}

		public ActionResult Index(int? id)
		{
			if (id.HasValue)
			{
				ViewData["ActiveSection"] = this._sectionService.GetSectionById(id.Value);
			}
			var sharedSections = this._sectionService.GetUnconnectedSections(this.CuyahogaContext.CurrentSite);
			var model = new List<SharedSectionViewData>();
			foreach (var sharedSection in sharedSections)
			{
				var sharedSectionViewData = new SharedSectionViewData(sharedSection);
				sharedSectionViewData.AttachedToTemplates = String.Join(", ",
				                                                        this._sectionService.GetTemplatesBySection(sharedSection).
				                                                        	Select(t => t.Name).ToArray());
				model.Add(sharedSectionViewData);
			}
			return View(model);
		}

		public ActionResult NewSectionDialog(int moduleTypeId, int nodeId, string placeholder)
		{
			ViewData["NodeId"] = nodeId;
			Section newSection = new Section();
			newSection.ModuleType = this._sectionService.GetModuleTypeById(moduleTypeId);
			newSection.PlaceholderId = placeholder;
			return View("NewSectionDialog", newSection);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DeleteSectionFromPage(int nodeId, int sectionIdToDelete)
		{
			Section sectionToDelete = this._sectionService.GetSectionById(sectionIdToDelete);
			try
			{
				this._sectionService.DeleteSection(sectionToDelete, nodeId, this._moduleLoader.GetModuleFromSection(sectionToDelete));
				Messages.AddFlashMessageWithParams("SectionDeletedMessage", sectionToDelete.Title);
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while deleting section.", ex);
				Messages.AddFlashException(ex);
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
				Messages.AddFlashMessageWithParams("SectionDetachedMessage", sectionToDetach.Title);
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while detaching section.", ex);
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Design", "Pages", new { id = nodeId });
		}

		public ActionResult NewShared()
		{
			var moduleTypes = this._sectionService.GetSortedActiveModuleTypes();
			var selectedModuleType = moduleTypes[0];
			ViewData["ModuleTypes"] = new SelectList(moduleTypes, "ModuleTypeId", "Name", selectedModuleType.ModuleTypeId);
			var newSection = new Section();
			newSection.ModuleType = selectedModuleType;
			return View(newSection);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CreateSharedSection([Bind(Include = "PlaceHolderId, Title, ShowTitle, CacheDuration")]Section section, int moduleTypeId)
		{
			section.ModuleType = this._sectionService.GetModuleTypeById(moduleTypeId);
			section.Site = CuyahogaContext.CurrentSite;
			UpdateSectionSettingsFromForm(section, settingsFormElementPrefix);
			try
			{
				if (ModelState.IsValid && ValidateModel(section, new[] { "Title", "ModuleType", "Settings" }, "section"))
				{
					this._sectionService.SaveSection(section);
					Messages.AddFlashMessageWithParams("SectionCreatedMessage", section.Title);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while creating a shared section to page.", ex);
				Messages.AddException(ex);
			}
			var moduleTypes = this._sectionService.GetSortedActiveModuleTypes();
			ViewData["ModuleTypes"] = new SelectList(moduleTypes, "ModuleTypeId", "Name", section.ModuleType.ModuleTypeId);
			return View("NewShared", section);
		}

		public ActionResult AttachSectionToTemplate(int id)
		{
			var model = new AttachSectionTemplateViewData();
			model.Section = this._sectionService.GetSectionById(id);
			model.Templates = this._templateService.GetAllTemplatesBySite(CuyahogaContext.CurrentSite);
			foreach (var template in model.Templates)
			{
				// read placeholders
				string virtualTemplatePath = VirtualPathUtility.Combine(CuyahogaContext.CurrentSite.SiteDataDirectory, template.Path);
				model.PlaceHoldersByTemplate[template] = ViewUtil.GetPlaceholdersFromVirtualPath(virtualTemplatePath).Select(p => p.Key).ToArray();
				// add sectiontemplate viewdata
				var sectionTemplate = new SectionTemplateViewData();
				sectionTemplate.TemplateId = template.Id;
				if (template.Sections.Any(s => s.Value == model.Section))
				{
					KeyValuePair<string, Section> templateSection = template.Sections.Where(s => s.Value == model.Section).Single();
					sectionTemplate.Placeholder = templateSection.Key;
					sectionTemplate.IsAttached = true;
				}
				model.SectionTemplates.Add(template.Id, sectionTemplate);
			}
			return View(model);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UpdateSectionTemplateAttachment(int sectionId, IList<SectionTemplateViewData> sectionTemplates)
		{
			var section = this._sectionService.GetSectionById(sectionId);
			try
			{
				foreach (var sectionTemplate in sectionTemplates)
				{
					var template = this._templateService.GetTemplateById(sectionTemplate.TemplateId);
					if (sectionTemplate.IsAttached)
					{
						this._templateService.AttachSectionToTemplate(section, template, sectionTemplate.Placeholder);
					}
					else
					{
						this._templateService.RemoveSectionFromTemplate(section, template);
					}
				}
				Messages.AddFlashMessageWithParams("SectionAttachedMessage", section.Title);
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while attaching section to templates.", ex);
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("AttachSectionToTemplate", "Sections", new { id = sectionId });
		}

		#region Ajax actions

		[RolesFilter]
		public ActionResult SelectSection(int sectionId)
		{
			Section section = this._sectionService.GetSectionById(sectionId);
			return PartialView("SelectedSection", section);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ArrangeSections(string placeholder, int[] orderedSectionIds)
		{
			AjaxMessageViewData result = new AjaxMessageViewData();
			try
			{
				if (orderedSectionIds != null)
				{
					this._sectionService.ArrangeSections(placeholder, orderedSectionIds);
					result.Message = String.Format(GetText("SectionsArrangedMessage"), placeholder);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while arranging sections.", ex);
				result.Error = ex.Message;
			}

			return Json(result);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[PartialMessagesFilter]
		public ActionResult AddSectionToPage([Bind(Include = "PlaceHolderId, Title, ShowTitle, CacheDuration")]Section section, int moduleTypeId, int nodeId)
		{
			section.ModuleType = this._sectionService.GetModuleTypeById(moduleTypeId);
			section.Site = CuyahogaContext.CurrentSite;
			section.Node = this._nodeService.GetNodeById(nodeId);
			section.Node.AddSection(section);
			UpdateSectionSettingsFromForm(section, settingsFormElementPrefix);
			try
			{
				if (ModelState.IsValid && ValidateModel(section, new [] { "Title", "ModuleType", "Settings" }, "section"))
				{
					this._sectionService.SaveSection(section);
					Messages.AddMessageWithParams("SectionCreatedMessage", section.Title);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while adding section to page.", ex);
				Messages.AddException(ex);
			}
			ViewData["NodeId"] = nodeId;
			return View("NewSectionDialog", section);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[RolesFilter]
		[PartialMessagesFilter]
		public ActionResult UpdateSection(int id)
		{
			Section section = this._sectionService.GetSectionById(id);
			UpdateSectionSettingsFromForm(section, settingsFormElementPrefix);
			try
			{
				if (TryUpdateModel(section, "section", new[] { "Title", "ShowTitle", "CacheDuration" })
					&& ValidateModel(section, new[] { "Title", "Settings" }, "section"))
				{
					this._sectionService.UpdateSection(section);
					Messages.AddMessage("SectionPropertiesUpdatedMessage");
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while updating section.", ex);
				Messages.AddException(ex);
			}
			if (Request.IsAjaxRequest())
			{
				return PartialView("SelectedSection", section);
			}
			else
			{
				throw new NotImplementedException("UpdateSection not yet implemented for non-AJAX scenario's");
			}
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[RolesFilter]
		[PartialMessagesFilter]
		public ActionResult SetSectionPermissions(int id, int[] viewRoleIds, int[] editRoleIds)
		{
			Section section = this._sectionService.GetSectionById(id);
			try
			{
				this._sectionService.SetSectionPermissions(section, viewRoleIds, editRoleIds);
				Messages.AddMessageWithParams("PermissionsUpdatedMessage", section.Title);

			}
			catch (Exception ex)
			{
				Messages.AddException(ex);
			}
			if (Request.IsAjaxRequest())
			{
				return PartialView("SelectedSection", section);
			}
			else
			{
				throw new NotImplementedException("UpdateSection not yet implemented for non-AJAX scenario's");
			}
		}

		public ActionResult GetSectionElements(int moduleTypeId)
		{
			var moduleType = this._sectionService.GetModuleTypeById(moduleTypeId);
			var section = new Section();
			section.ModuleType = moduleType;
			return PartialView("SharedSectionElements", section);
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
				// If a property doesn't exist in ModelState (such as with dynamic dictionary's), add a dummy value
				// , otherwise rendering of Html elements crashes.
				if (!ViewData.ModelState.ContainsKey(formElementName))
				{
					string value = Request.Params[formElementName];
					ViewData.ModelState.Add(new KeyValuePair<string, ModelState>(formElementName, new ModelState() { Value = new ValueProviderResult(value, value, CultureInfo.CurrentUICulture) }));
				}
			}
		}
	}
}
