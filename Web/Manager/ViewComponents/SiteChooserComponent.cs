using System.Collections;
using System.Collections.Generic;
using Castle.MonoRail.Framework;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Web.Components;

namespace Cuyahoga.Web.Manager.ViewComponents
{
	public class SiteChooserComponent : ViewComponent
	{
		private ISiteService _siteService;
		private ICuyahogaContext _cuyahogaContext;

		public SiteChooserComponent(ISiteService siteService, ICuyahogaContext cuyahogaContext)
		{
			this._siteService = siteService;
			this._cuyahogaContext = cuyahogaContext;
		}

		public override void Initialize()
		{
			List <Site> availableSites = new List<Site>();
			IList allSites = this._siteService.GetAllSites();
			foreach (Site site in allSites)
			{
				if (this._cuyahogaContext.CurrentUser.HasRight(Rights.AccessAdmin, site))
				{
					availableSites.Add(site);
				}
			}
			PropertyBag["availablesites"] = availableSites;
			base.Initialize();
		}

		public override void Render()
		{
			RenderView("SiteChooser");
		}
	}
}
