using System;
using System.Web.Mvc;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Mvc.Partials;
using log4net;

namespace Cuyahoga.Web.Mvc.Filters
{
	public class SiteFilter : ActionFilterAttribute
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (SiteFilter));
		private readonly ISiteService _siteService;
		private readonly ICuyahogaContext _cuyahogaContext;

		/// <summary>
		/// Creates a new instance of the <see cref="SiteFilter"></see> class.
		/// </summary>
		/// <remarks>
		/// Lookup components via the static IoC resolver. It would be great if we could do this via dependency injection
		/// but filters cannot be managed via the container in the current version of ASP.NET MVC. 
		/// </remarks>
		public SiteFilter() : this(IoC.Resolve<ISiteService>(), IoC.Resolve<ICuyahogaContext>())
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="SiteFilter"></see> class.
		/// </summary>
		/// <param name="siteService"></param>
		/// <param name="cuyahogaContext"></param>
		public SiteFilter(ISiteService siteService, ICuyahogaContext cuyahogaContext)
		{
			_siteService = siteService;
			_cuyahogaContext = cuyahogaContext;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			SetCurrentSite(filterContext);
		}

		private void SetCurrentSite(ActionExecutingContext filterContext)
		{
			if (this._siteService == null)
			{
				throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");
			}
			try
			{
				Site currentSite = this._siteService.GetSiteBySiteUrl(UrlUtil.GetSiteUrl());
				if (currentSite != null) // null check, so the installer won't choke when there isn't any site yet.
				{
					this._cuyahogaContext.SetSite(currentSite);
					this._cuyahogaContext.PhysicalSiteDataDirectory =
						filterContext.HttpContext.Server.MapPath(currentSite.SiteDataDirectory);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Exception occured while getting the current site. This may be caused by the installer, so that's why we won't throw the exception.", ex);
			}
		}
	}
}