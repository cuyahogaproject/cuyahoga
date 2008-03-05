using System;
using Castle.MonoRail.Framework;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Manager.Controllers
{
	[Layout("Default"), Rescue("GenericError")]
	[Resource("globaltext", "Cuyahoga.Web.Manager.GlobalResources")]
	[ControllerDetails(Area = "Manager")]
	public abstract class BaseController : SmartDispatcherController
	{
		private ICuyahogaContext _cuyahogaContext;
		private ISiteService _siteService;

		/// <summary>
		/// Get or sets the Cuyahoga context.
		/// </summary>
		public ICuyahogaContext CuyahogaContext
		{
			get { return this._cuyahogaContext; }
			set { this._cuyahogaContext = value; }
		}

		/// <summary>
		/// Sets the SiteService.
		/// </summary>
		public ISiteService SiteService
		{
			set { this._siteService = value; }
		}

		protected override void Initialize()
		{
			SetCurrentSite();
			base.Initialize();
		}

		private void SetCurrentSite()
		{
			if (this._siteService == null)
			{
				throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");
			}
			this._cuyahogaContext.SetSite(this._siteService.GetSiteBySiteUrl(UrlHelper.GetSiteUrl()));

		}
	}
}
