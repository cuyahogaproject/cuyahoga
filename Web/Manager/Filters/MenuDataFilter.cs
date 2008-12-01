using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc.Sitemap;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Filters
{
	public class MenuDataFilter : ActionFilterAttribute
	{
		private readonly ICuyahogaContext _cuyahogaContext;
		private readonly IMvcSitemapProvider _sitemapProvider;

		/// <summary>
		/// Creates a new instance of the <see cref="MenuDataFilter"></see> class.
		/// </summary>
		/// <remarks>
		/// Lookup components via the static IoC resolver. It would be great if we could do this via dependency injection
		/// but filters cannot be managed via the container in the current version of ASP.NET MVC. 
		/// </remarks>
		public MenuDataFilter() : this(IoC.Resolve<ICuyahogaContext>(), IoC.Resolve<IMvcSitemapProvider>())
		{}

		/// <summary>
		/// Creates a new instance of the <see cref="MenuDataFilter"></see> class.
		/// </summary>
		/// <param name="cuyahogaContext"></param>
		/// <param name="sitemapProvider"></param>
		public MenuDataFilter(ICuyahogaContext cuyahogaContext, IMvcSitemapProvider sitemapProvider)
		{
			_cuyahogaContext = cuyahogaContext;
			_sitemapProvider = sitemapProvider;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			InitMenuViewData(filterContext);
		}

		private void InitMenuViewData(ActionExecutingContext filterContext)
		{
			MainMenuViewData mainMenuViewData = new MainMenuViewData();
			User user = this._cuyahogaContext.CurrentUser;
			if (user != null && user.IsAuthenticated)
			{
				var nodes = this._sitemapProvider.GetMvcChildNodes(this._sitemapProvider.RootNode);
				var currentNode = this._sitemapProvider.CurrentNode;

				foreach (MvcSiteMapNode node in nodes)
				{
					if (this._sitemapProvider.IsAccessibleToUser(filterContext.HttpContext, node, this._cuyahogaContext.CurrentSite))
					{
						MenuItem menuItem = new MenuItem(VirtualPathUtility.ToAbsolute(node.Url)
							, GlobalResources.ResourceManager.GetString(node.ResourceKey, Thread.CurrentThread.CurrentUICulture)
							, CheckInPath(node, currentNode, filterContext.RouteData));
						bool isSystem = Convert.ToBoolean(node["system"]);
						if (isSystem)
						{
							mainMenuViewData.AddOptionalMenuItem(menuItem);
						}
						else
						{
							mainMenuViewData.AddStandardMenuItem(menuItem);
						}
					}
				}
			}
			filterContext.Controller.ViewData.Add("MainMenuViewData", mainMenuViewData);
		}

		private bool CheckInPath(MvcSiteMapNode node, SiteMapNode currentNode, RouteData currentRouteData)
		{
			MvcSiteMapNode currentMvcNode = currentNode as MvcSiteMapNode;
			if (currentMvcNode != null)
			{
				return currentMvcNode.Key == node.Key || currentMvcNode.IsDescendantOf(node);
			}
			if (currentRouteData != null)
			{
				return node.Controller == currentRouteData.Values["controller"].ToString();
			}
			return false;
		}
	}
}
