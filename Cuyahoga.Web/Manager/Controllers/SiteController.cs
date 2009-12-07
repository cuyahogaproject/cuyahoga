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
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Mvc.Filters;
using Cuyahoga.Web.Mvc.WebForms;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageSite)]
	public class SiteController : ManagerController
	{
		private readonly ISiteService _siteService;
		private readonly IUserService _userService;
		private readonly ITemplateService _templateService;
		private readonly INodeService _nodeService;
		private readonly IModelValidator<SiteAlias> _siteAliasValidator;

		public SiteController(ISiteService siteservice, IUserService userService, ITemplateService templateService, INodeService nodeService, IModelValidator<Site> siteValidator, IModelValidator<SiteAlias> siteAliasValidator)
		{
			_siteService = siteservice;
			_userService = userService;
			_templateService = templateService;
			_nodeService = nodeService;
			_siteAliasValidator = siteAliasValidator;
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
					this._siteService.CreateSite(site, Server.MapPath("~/SiteData"), templates, systemTemplateDir);

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
			Site newSite = this._siteService.GetSiteById(siteId);
			return View("NewSiteSuccess", newSite);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Update(int id, int defaultRoleId, int defaultTemplateId)
		{
			Site site = this._siteService.GetSiteById(id);
			try
			{
				UpdateModel(site, new [] {"Name", "SiteUrl", "WebmasterEmail", "UserFriendlyUrls", "DefaultCulture", "DefaultPlaceholder", "MetaDescription", "MetaKeywords"});
				site.DefaultRole = this._userService.GetRoleById(defaultRoleId);
				site.DefaultTemplate = this._templateService.GetTemplateById(defaultTemplateId);
				if (ValidateModel(site))
				{
					this._siteService.SaveSite(site);
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

		public ActionResult Aliases()
		{
			Site currentSite = CuyahogaContext.CurrentSite;
			IList<SiteAlias> aliases = this._siteService.GetSiteAliasesBySite(currentSite);
			ViewData["Site"] = currentSite;
			return View(aliases);
		}

		public ActionResult NewAlias()
		{
			Site currentSite = CuyahogaContext.CurrentSite;
			ViewData["Site"] = currentSite;
			ViewData["EntryNodes"] = new SelectList(GetDisplayRootNodes(currentSite), "Key", "Value");
			return View(new SiteAlias());
		}

		public ActionResult EditAlias(int id)
		{
			var currentSite = CuyahogaContext.CurrentSite;
			var siteAlias = this._siteService.GetSiteAliasById(id);
			ViewData["Site"] = currentSite;
			ViewData["EntryNodes"] = new SelectList(GetDisplayRootNodes(currentSite), "Key", "Value", siteAlias.EntryNode != null ? siteAlias.EntryNode.Id : -1);
			return View(siteAlias);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CreateAlias([Bind(Exclude="Id")]SiteAlias siteAlias, int? entryNodeId)
		{
			Site currentSite = CuyahogaContext.CurrentSite;
			try
			{
				siteAlias.Site = currentSite;
				if (entryNodeId.HasValue)
				{
					siteAlias.EntryNode = this._nodeService.GetNodeById(entryNodeId.Value);
				}
				if (ValidateModel(siteAlias, this._siteAliasValidator, new[] { "Site", "Url" }))
				{
					this._siteService.SaveSiteAlias(siteAlias);
					Messages.AddFlashMessageWithParams("SiteAliasCreatedMessage", siteAlias.Url);
					return RedirectToAction("Aliases");
				}
			}
			catch (Exception ex)
			{
				Messages.AddException(ex);
			}
			ViewData["Site"] = currentSite;
			ViewData["EntryNodes"] = new SelectList(GetDisplayRootNodes(currentSite), "Key", "Value", entryNodeId.HasValue ? entryNodeId.Value : -1);
			return View("NewAlias", siteAlias);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UpdateAlias(int id, int? entryNodeId)
		{
			Site currentSite = CuyahogaContext.CurrentSite;
			SiteAlias siteAlias = this._siteService.GetSiteAliasById(id);
			try
			{
				if (entryNodeId.HasValue)
				{
					siteAlias.EntryNode = this._nodeService.GetNodeById(entryNodeId.Value);
				}
				else
				{
					siteAlias.EntryNode = null;
				}
				if (TryUpdateModel(siteAlias, new [] { "Url" }) && ValidateModel(siteAlias, this._siteAliasValidator, new[] { "Site", "Url" }))
				{
					this._siteService.SaveSiteAlias(siteAlias);
					Messages.AddFlashMessageWithParams("SiteAliasUpdatedMessage", siteAlias.Url);
					return RedirectToAction("Aliases");
				}
			}
			catch (Exception ex)
			{
				Messages.AddException(ex);
			}
			ViewData["Site"] = currentSite;
			ViewData["EntryNodes"] = new SelectList(GetDisplayRootNodes(currentSite), "Key", "Value", siteAlias.EntryNode != null ? siteAlias.EntryNode.Id : -1);
			return View("NewAlias", currentSite);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DeleteAlias(int id)
		{
			var siteAlias = this._siteService.GetSiteAliasById(id);
			try
			{
				this._siteService.DeleteSiteAlias(siteAlias);
				Messages.AddFlashMessageWithParams("SiteAliasDeletedMessage", siteAlias.Url);
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Aliases");
		}

		private IDictionary<int, string> GetDisplayRootNodes(Site currentSite)
		{
			return this._nodeService.GetRootNodes(currentSite)
				.ToDictionary(n => n.Id, n => string.Format("{0} ({1})", n.Title, n.Culture));
		}
	}
}