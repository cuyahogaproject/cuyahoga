using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Core.Communication;
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
		private const string SettingsFormElementPrefix = "section.Settings_";
		private readonly ISectionService _sectionService;
		private readonly INodeService _nodeService;
		private readonly ITemplateService _templateService;
		private readonly IModuleTypeService _moduleTypeService;
		private readonly ModuleLoader _moduleLoader;

		public SectionsController(ISectionService sectionService, INodeService nodeService, ModuleLoader moduleLoader, 
			SectionModelValidator modelValidator, ITemplateService templateService, IModuleTypeService moduleTypeService)
		{
			_sectionService = sectionService;
			_templateService = templateService;
			_moduleTypeService = moduleTypeService;
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

		public ActionResult SectionProperties(int id)
		{
			Section section = this._sectionService.GetSectionById(id);
			return View(new SectionViewData(section));
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
			UpdateSectionSettingsFromForm(section, SettingsFormElementPrefix);
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
			var sectionViewData = BuildSectionViewData(this._sectionService.GetSectionById(sectionId));
			return PartialView("SelectedSection", sectionViewData);
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
			UpdateSectionSettingsFromForm(section, SettingsFormElementPrefix);
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
			UpdateSectionSettingsFromForm(section, SettingsFormElementPrefix);
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
			var sectionViewData = BuildSectionViewData(section);
			if (Request.IsAjaxRequest())
			{
				return PartialView("SelectedSection", sectionViewData);
			}
			else
			{
				return View("SectionProperties", sectionViewData);
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
				var sectionViewData = BuildSectionViewData(section);
				return PartialView("SelectedSection", sectionViewData);
			}
			else
			{
				throw new NotImplementedException("SetSectionPermissions not yet implemented for non-AJAX scenario's");
			}
		}

		public ActionResult GetSectionElements(int moduleTypeId)
		{
			var moduleType = this._sectionService.GetModuleTypeById(moduleTypeId);
			var section = new Section();
			section.ModuleType = moduleType;
			return PartialView("SharedSectionElements", section);
		}

		public ActionResult GetAvailableConnectionsForSectionAndAction(int sectionId, string actionname)
		{
			// NOTE: it would be better to have this kind of logic in SectionService, but we don't have access to 
			// ModuleLoader over there.
			Section section = this._sectionService.GetSectionById(sectionId);
			var outboundAction = ((IActionProvider) _moduleLoader.GetModuleFromSection(section)).GetOutboundActions().FindByName(actionname);

			var compatibleSections = new List<Section>();

			var allActiveModuleTypes = this._moduleTypeService.GetAllModuleTypesInUse();
			// Load modules and check if these can accept the outbound action.
			foreach (var moduleType in allActiveModuleTypes)
			{
				var module = this._moduleLoader.GetModuleFromType(moduleType);
				if (module is IActionConsumer)
				{
					var actionConsumer = (IActionConsumer) module;
					var inboundActions = actionConsumer.GetInboundActions();
					var matchingInboundAction = inboundActions.OfType<ModuleAction>().Where(ma => ma.Equals(outboundAction)).SingleOrDefault();
					if (matchingInboundAction != null)
					{
						// Compatible module. Find all sections with this module and add to the list.
						compatibleSections.AddRange(this._sectionService.GetSectionsByModuleTypeAndSite(moduleType, CuyahogaContext.CurrentSite));
					}
				}
			}
			var jsonValues = from s in compatibleSections
							 select new
							 {
								 SectionId = s.Id,
								 PageName = s.Node != null ? s.Node.Title : String.Empty,
								 SectionName = s.Title
							 };
			return Json(jsonValues);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[RolesFilter]
		[PartialMessagesFilter]
		public ActionResult AddConnection(int sectionId, int connectToId, string actionName)
		{
			Section section = this._sectionService.GetSectionById(sectionId);
			Section sectionToConnectTo = this._sectionService.GetSectionById(connectToId);

			try
			{
				section.Connections.Add(actionName, sectionToConnectTo);
				this._sectionService.UpdateSection(section);
				Messages.AddMessageWithParams("ConnectionAddedMessage", actionName);
			}
			catch (Exception ex)
			{
				Messages.AddException(ex);
			}
			if (Request.IsAjaxRequest())
			{
				var sectionViewData = BuildSectionViewData(section);
				sectionViewData.ExpandConnections = true;
				return PartialView("SelectedSection", sectionViewData);
			}
			else
			{
				throw new NotImplementedException("AddConnection not yet implemented for non-AJAX scenario's");
			}
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[RolesFilter]
		[PartialMessagesFilter]
		public ActionResult DeleteConnection(int sectionId, string actionName)
		{
			Section section = this._sectionService.GetSectionById(sectionId);
			try
			{
				section.Connections.Remove(actionName);
				this._sectionService.UpdateSection(section);
				Messages.AddMessageWithParams("ConnectionDeletedMessage", actionName);
			}
			catch (Exception ex)
			{
				Messages.AddException(ex);
			}
			if (Request.IsAjaxRequest())
			{
				var sectionViewData = BuildSectionViewData(section);
				sectionViewData.ExpandConnections = true;
				return PartialView("SelectedSection", sectionViewData);
			}
			else
			{
				throw new NotImplementedException("DeleteConnection not yet implemented for non-AJAX scenario's");
			}
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

		private SectionViewData BuildSectionViewData(Section section)
		{
			var sectionViewData = new SectionViewData(section);
			var module = _moduleLoader.GetModuleFromSection(section);
			if (module is IActionProvider)
			{
				sectionViewData.OutboundActions = ((IActionProvider)module).GetOutboundActions();
			}
			if (module is IActionConsumer)
			{
				sectionViewData.InboundActions = ((IActionConsumer)module).GetInboundActions();
			}
			return sectionViewData;
		}
	}
}
