using System;
using System.Collections;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;

namespace Cuyahoga.Web.Manager.Controllers
{
	/// <summary>
	/// Controller for the Cuyahoga Manager start page.
	/// </summary>
	[CuyahogaPermission(RequiredRights = Rights.AccessAdmin)]
	public class DashboardController : SecureController
	{
		private ISiteService _siteService;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="siteService"></param>
		public DashboardController(ISiteService siteService)
		{
			this._siteService = siteService;
		}

		public void Index()
		{
		}

		/// <summary>
		/// Jump to the administration site of the site with the given id. 
		/// </summary>
		/// <param name="siteId"></param>
		public void SetSite(int siteId)
		{
			Site site = this._siteService.GetSiteById(siteId);
			string dashBoardUrl = UrlBuilder.BuildUrl(Context.UrlInfo, "manager", "dashboard", "index");
			dashBoardUrl = dashBoardUrl.Replace(Context.UrlInfo.AppVirtualDir + "/", String.Empty);
			Redirect(site.SiteUrl + dashBoardUrl);
		}
	}
}
