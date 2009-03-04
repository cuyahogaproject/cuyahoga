using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Mvc.Filters;
using Cuyahoga.Web.Mvc.WebForms;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageSite)]
	public class SiteController : ManagerController
	{
		private readonly ISiteService _siteservice;
		private readonly IUserService _userService;
		private readonly ITemplateService _templateService;

		public SiteController(ISiteService siteservice, IUserService userService, ITemplateService templateService, IModelValidator<Site> siteValidator)
		{
			_siteservice = siteservice;
			_userService = userService;
			_templateService = templateService;
			ModelValidator = siteValidator;
		}

		public ActionResult Index()
		{
			Site currentSite = CuyahogaContext.CurrentSite;
			ViewData["Roles"] = new SelectList(this._userService.GetAllGlobalRoles(), "Id", "Name", currentSite.DefaultRole.Id);
			ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", currentSite.DefaultCulture);
			ViewData["Templates"] = new SelectList(currentSite.Templates, "Id", "Name", currentSite.DefaultTemplate != null ? currentSite.DefaultTemplate.Id : 0);
			if (currentSite.DefaultTemplate != null)
			{
				string virtualTemplatePath = VirtualPathUtility.Combine(currentSite.SiteDataDirectory, currentSite.DefaultTemplate.Path);
				ViewData["PlaceHolders"] = new SelectList(ViewUtil.GetPlaceholdersFromVirtualPath(virtualTemplatePath).Keys, currentSite.DefaultPlaceholder);
			}
			return View("EditSite", currentSite);
		}

		[PermissionFilter(RequiredRights = Rights.CreateSite)]
		public ActionResult New()
		{
			ViewData["Roles"] = new SelectList(this._userService.GetAllGlobalRoles(), "Id", "Name");
			ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value");
			ViewData["Templates"] = this._templateService.GetAllSystemTemplates();
			return View("NewSite", new Site());
		}

		[PermissionFilter(RequiredRights = Rights.CreateSite)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(int defaultRoleId, int[] templateIds)
		{
			Site site = new Site();
			try
			{
				UpdateModel(site, new [] { "Name", "SiteUrl", "WebmasterEmail", "UserFriendlyUrls", "DefaultCulture"});
				site.DefaultRole = this._userService.GetRoleById(defaultRoleId);
				if (ValidateModel(site))
				{
					IList<Template> templates = new List<Template>();
					if (templateIds.Length > 0)
					{
						templates = this._templateService.GetAllSystemTemplates().Where(t => templateIds.Contains(t.Id)).ToList();
					}
					string systemTemplateDir = Server.MapPath(Config.GetConfiguration()["TemplateDir"]);
					this._siteservice.CreateSite(site, Server.MapPath("~/SiteData"), templates, systemTemplateDir);

					return RedirectToAction("CreateSuccess", new { siteId = site.Id });
				}
			}
			catch (Exception ex)
			{
				Messages.AddException(ex);
			}
			ViewData["Roles"] = new SelectList(this._userService.GetAllGlobalRoles(), "Id", "Name", site.DefaultRole.Id);
			ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", site.DefaultCulture);
			ViewData["Templates"] = this._templateService.GetAllSystemTemplates();
			return View("NewSite", site);
		}

		[PermissionFilter(RequiredRights = Rights.CreateSite)]
		public ActionResult CreateSuccess(int siteId)
		{
			Site newSite = this._siteservice.GetSiteById(siteId);
			return View("NewSiteSuccess", newSite);
		}

		public ActionResult Update(int id, int defaultRoleId, int defaultTemplateId)
		{
			Site site = this._siteservice.GetSiteById(id);
			try
			{
				UpdateModel(site, new [] {"Name", "SiteUrl", "WebmasterEmail", "UserFriendlyUrls", "DefaultCulture", "DefaultPlaceholder", "MetaDescription", "MetaKeywords"});
				site.DefaultRole = this._userService.GetRoleById(defaultRoleId);
				site.DefaultTemplate = this._templateService.GetTemplateById(defaultTemplateId);
				if (ValidateModel(site))
				{
					this._siteservice.SaveSite(site);
					Messages.AddMessage("SiteUpdatedMessage");
					RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				Messages.AddException(ex);
			}
			ViewData["Roles"] = new SelectList(this._userService.GetAllGlobalRoles(), "Id", "Name", site.DefaultRole.Id);
			ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", site.DefaultCulture);
			ViewData["Templates"] = new SelectList(site.Templates, "Id", "Name", site.DefaultTemplate != null ? site.DefaultTemplate.Id : 0);
			if (site.DefaultTemplate != null)
			{
				string virtualTemplatePath = VirtualPathUtility.Combine(site.SiteDataDirectory, site.DefaultTemplate.Path);
				ViewData["PlaceHolders"] = new SelectList(ViewUtil.GetPlaceholdersFromVirtualPath(virtualTemplatePath).Keys, site.DefaultPlaceholder);
			}
			return View("EditSite", site);
		}
	}
}