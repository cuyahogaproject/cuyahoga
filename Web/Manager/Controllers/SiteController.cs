using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Mvc.Filters;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageSite)]
	public class SiteController : SecureController
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
			ViewData["Title"] = GlobalResources.ManageSitePageTitle;
			return View("EditSite", CuyahogaContext.CurrentSite);
		}

		public ActionResult New()
		{
			ViewData["Title"] = GlobalResources.NewSitePageTitle;
			ViewData["Roles"] = new SelectList(this._userService.GetAllGlobalRoles(), "Id", "Name");
			ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value");
			ViewData["Templates"] = this._templateService.GetAllSystemTemplates();
			return View("NewSite", new Site());
		}

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

					ShowMessage("Site created", true);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			ViewData["Title"] = GlobalResources.NewSitePageTitle;
			ViewData["Roles"] = new SelectList(this._userService.GetAllGlobalRoles(), "Id", "Name", site.DefaultRole.Id);
			ViewData["Cultures"] = new SelectList(Globalization.GetOrderedCultures(), "Key", "Value", site.DefaultCulture);
			ViewData["Templates"] = this._templateService.GetAllSystemTemplates();
			ViewData["TemplateIds"] = templateIds;
			return View("NewSite", site);
		}
	}
}