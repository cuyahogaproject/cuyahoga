using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Web.Manager.Filters;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.AccessAdmin)]
	public class DashboardController : SecureController
	{
		private readonly ISiteService _siteService;

		public DashboardController(ISiteService siteService)
		{
			_siteService = siteService;
		}

		public ActionResult Index()
		{
			ViewData["Title"] = GlobalResources.DashboardPageTitle;
			return View();
		}

		/// <summary>
		/// Render a partial view for choosing the site.
		/// </summary>
		/// <returns></returns>
		public ActionResult SiteChooser()
		{
			List<Site> availableSites = new List<Site>();
			IList allSites = this._siteService.GetAllSites();
			foreach (Site site in allSites)
			{
				if (this.CuyahogaContext.CurrentUser.HasRight(Rights.AccessAdmin, site))
				{
					availableSites.Add(site);
				}
			}
			ViewData["SiteId"] = new SelectList(availableSites, "Id", "Name", this.CuyahogaContext.CurrentSite.Id);
			return PartialView();
		}

		/// <summary>
		/// Jump to the administration site of the site with the given id. 
		/// </summary>
		/// <param name="siteId"></param>
		public ActionResult SetSite(int siteId)
		{
			Site site = this._siteService.GetSiteById(siteId);
			string dashBoardUrl = Url.Action("Index", "Dashboard");
			if (HttpContext.Request.ApplicationPath.Length > 1)
			{
				dashBoardUrl = dashBoardUrl.Replace(HttpContext.Request.ApplicationPath, String.Empty);
			}
			return Redirect(site.SiteUrl + dashBoardUrl);
		}
	}
}