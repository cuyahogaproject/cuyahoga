using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Files;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc.Filters;
using Cuyahoga.Web.Mvc.WebForms;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	public class TemplatesController : SecureController
	{
		private readonly ITemplateService _templateService;
		private readonly IFileService _fileService;

		public TemplatesController(ITemplateService templateService, IFileService fileService, IModelValidator<Template> modelValidator)
		{
			_templateService = templateService;
			_fileService = fileService;
			ModelValidator = modelValidator;
		}

		[PermissionFilter(RequiredRights = Rights.ManageTemplates)]
		public ActionResult Index()
		{
			ViewData["Title"] = GlobalResources.ManageTemplatesPageTitle;
			IList<Template> templates = this._templateService.GetAllTemplatesBySite(CuyahogaContext.CurrentSite);
			return View(templates);
		}

		public ActionResult View(int id)
		{
			Template template = this._templateService.GetTemplateById(id);
			ViewData["Title"] = GlobalResources.ViewTemplatePageTitle;
			string siteDataDir = CuyahogaContext.CurrentSite.SiteDataDirectory;
			string absoluteBasePath = VirtualPathUtility.Combine(siteDataDir, template.BasePath) + "/";
			string htmlContent = ViewUtil.RenderTemplateHtml(VirtualPathUtility.Combine(absoluteBasePath, template.TemplateControl));
			string cssContent = GetCssContent(absoluteBasePath + "Css/" + template.Css);
			TemplateViewData templateViewData = new TemplateViewData(template, htmlContent, cssContent);
			templateViewData.PrepareTemplateDataForEmbedding(Url.Content(CuyahogaContext.CurrentSite.SiteDataDirectory));
			return View("ViewTemplate", templateViewData);
		}

		[PermissionFilter(RequiredRights = Rights.ManageTemplates)]
		public ActionResult New()
		{
			Template template = new Template();
			return RenderNewTemplateView(template);
		}

		[PermissionFilter(RequiredRights = Rights.ManageTemplates)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create()
		{
			Template template = new Template();
			try
			{
				template.Site = CuyahogaContext.CurrentSite;
				if (TryUpdateModel(template, new[] { "Name", "BasePath", "TemplateControl", "Css" }) && ValidateModel(template))
				{
					this._templateService.SaveTemplate(template);
					ShowMessage(String.Format(GlobalResources.TemplateCreatedMessage, template.Name), true);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			return RenderNewTemplateView(template);
		}

		private ActionResult RenderNewTemplateView(Template template)
		{
			ViewData["Title"] = GlobalResources.RegisterTemplatePageTitle;
			string siteDataDir = CuyahogaContext.CurrentSite.SiteDataDirectory;
			var basePaths = (	from directory in this._fileService.GetSubDirectories(Server.MapPath(VirtualPathUtility.Combine(siteDataDir, "Templates")))
			                 	select "Templates/" + directory
			                ).ToArray();
			if (basePaths.Length == 0)
			{
				throw new Exception("Can't register a new template when there are no template directories on the server.");
			}
			string relativeBasePath = template.BasePath ?? basePaths[0];
			ViewData["BasePaths"] = new SelectList(basePaths, relativeBasePath);

			SetupTemplateControlAndCssLists(template, relativeBasePath);

			return View("NewTemplate", template);
		}

		[PermissionFilter(RequiredRights = Rights.ManageTemplates)]
		public ActionResult Edit(int id)
		{
			ViewData["Title"] = GlobalResources.EditTemplatePageTitle;
			Template template = this._templateService.GetTemplateById(id);
			SetupTemplateControlAndCssLists(template, template.BasePath);
			return View("EditTemplate", template);
		}

		[PermissionFilter(RequiredRights = Rights.ManageTemplates)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Update(int id)
		{
			Template template = this._templateService.GetTemplateById(id);
			try
			{
				if (TryUpdateModel(template, new[] { "Name", "TemplateControl", "Css" }) && ValidateModel(template))
				{
					this._templateService.SaveTemplate(template);
					ShowMessage(String.Format(GlobalResources.TemplateUpdatedMessage, template.Name), true);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			ViewData["Title"] = GlobalResources.EditTemplatePageTitle;
			SetupTemplateControlAndCssLists(template, template.BasePath);
			return View("EditTemplate", template);
		}

		[PermissionFilter(RequiredRights = Rights.ManageTemplates)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(int id)
		{
			Template template = this._templateService.GetTemplateById(id);
			try
			{
				this._templateService.DeleteTemplate(template);
				ShowMessage(String.Format(GlobalResources.TemplateDeletedMessage, template.Name), true);
			}
			catch (Exception ex)
			{
				ShowException(ex, true);
			}
			return RedirectToAction("Index");
		}

		#region AJAX actions

		public ActionResult GetPlaceholdersByTemplateId(int templateId)
		{
			Template template = this._templateService.GetTemplateById(templateId);
			string virtualTemplatePath = VirtualPathUtility.Combine(CuyahogaContext.CurrentSite.SiteDataDirectory, template.Path);
			var placeholders = ViewUtil.GetPlaceholdersFromVirtualPath(virtualTemplatePath);
			var jsonValues = from key in placeholders.Keys
							 select new
							 {
								 Placeholder = key
							 };
			return Json(jsonValues);
		}

		public ActionResult GetTemplateControlsForBasePath(string basePath)
		{
			string virtualTemplatePath = VirtualPathUtility.Combine(CuyahogaContext.CurrentSite.SiteDataDirectory, basePath);
			string[] templateControls = this._fileService.GetFiles(Server.MapPath(virtualTemplatePath));
			var jsonValues = from templateControl in templateControls
							 select new
							 {
								 TemplateControl = templateControl
							 };
			return Json(jsonValues);
		}

		public ActionResult GetCssFilesForBasePath(string basePath)
		{
			string virtualTemplatePath = VirtualPathUtility.Combine(CuyahogaContext.CurrentSite.SiteDataDirectory, basePath);
			string absoluteCssPath = virtualTemplatePath + "/Css";
			string[] cssFiles = this._fileService.GetFiles(Server.MapPath(absoluteCssPath));
			var jsonValues = from cssFile in cssFiles
							 select new
							 {
								 CssFile = cssFile
							 };
			return Json(jsonValues);
		}

		public ActionResult UploadTemplates()
		{
			string message = String.Empty;
			string error = String.Empty;
			try
			{
				if (Request.Files.Count > 0)
				{
					HttpPostedFileBase theFile = Request.Files[0];
					if (! theFile.FileName.EndsWith(".zip"))
					{
						throw new Exception(GlobalResources.InvalidZipFileMessage);
					}
					string templatesRoot = VirtualPathUtility.Combine(CuyahogaContext.CurrentSite.SiteDataDirectory, "Templates");
					string filePath = Path.Combine(Server.MapPath(templatesRoot), theFile.FileName);
					this._templateService.ExtractTemplatePackage(filePath, theFile.InputStream);
					message = GlobalResources.TemplatesUploadedMessage;
				}
				else
				{
					error = GlobalResources.NoFileUploadedMessage;
				}
			}
			catch (InvalidPackageException ex)
			{
				Logger.Error(ex.Message, ex);
				error = TranslateMessage(ex.Message);
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while uploading templates.", ex);
				error = ex.Message;
			}

			var result = Json( new { Message = message, Error = error });
			result.ContentType = "text/html"; // otherwise the ajax form doesn't handle the callback
			return result;
		}

		#endregion

		#region private methods

		private void SetupTemplateControlAndCssLists(Template template, string relativeBasePath)
		{
			string siteDataDir = CuyahogaContext.CurrentSite.SiteDataDirectory;
			string absoluteBasePath = VirtualPathUtility.Combine(siteDataDir, relativeBasePath); 
			string[] templateControls = this._fileService.GetFiles(Server.MapPath(absoluteBasePath));
			ViewData["TemplateControls"] = new SelectList(templateControls, template.TemplateControl);
			string absoluteCssPath = absoluteBasePath + "/Css";
			string[] cssFiles = this._fileService.GetFiles(Server.MapPath(absoluteCssPath));
			ViewData["CssFiles"] = new SelectList(cssFiles, template.Css);
		}

		private string GetCssContent(string virtualCssFilePath)
		{
			string physicalCssFilePath = Server.MapPath(virtualCssFilePath);

			using (Stream fileStream = this._fileService.ReadFile(physicalCssFilePath))
			using (StreamReader sr = new StreamReader(fileStream))
			{
				return sr.ReadToEnd();
			}
		}

		#endregion
	}
}