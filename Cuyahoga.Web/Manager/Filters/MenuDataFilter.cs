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
			MenuViewData menuViewData = new MenuViewData();
			User user = this._cuyahogaContext.CurrentUser;
			if (user != null && user.IsAuthenticated)
			{
				var nodes = this._sitemapProvider.GetMvcChildNodes(this._sitemapProvider.RootNode);
				var currentNode = this._sitemapProvider.CurrentNode;
				UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);

				// mainmenu
				foreach (MvcSiteMapNode node in nodes)
				{
					if (this._sitemapProvider.IsAccessibleToUser(filterContext.HttpContext, node, this._cuyahogaContext.CurrentSite))
					{
						MenuItemData menuItemData = new MenuItemData(VirtualPathUtility.ToAbsolute(node.Url)
							, GlobalResources.ResourceManager.GetString(node.ResourceKey, Thread.CurrentThread.CurrentUICulture)
							, CheckInPath(node, currentNode, filterContext.RouteData)
							, node.Icon != null ? urlHelper.Content("~/manager/Content/images/" + node.Icon) : null);
						bool isSystem = Convert.ToBoolean(node["system"]);
						if (isSystem)
						{
							menuViewData.AddOptionalMenuItem(menuItemData);
						}
						else
						{
							menuViewData.AddStandardMenuItem(menuItemData);
						}
					}
					// submenu
					if (node.HasChildNodes && CheckInPath(node, currentNode, filterContext.RouteData))
					{
						foreach (SiteMapNode childNode in node.ChildNodes)
						{
							MvcSiteMapNode mvcChildNode = childNode as MvcSiteMapNode;
							if (mvcChildNode != null)
							{
								if (this._sitemapProvider.IsAccessibleToUser(filterContext.HttpContext, mvcChildNode, this._cuyahogaContext.CurrentSite))
								{
									MenuItemData menuItemData = new MenuItemData(VirtualPathUtility.ToAbsolute(mvcChildNode.Url)
										, GlobalResources.ResourceManager.GetString(mvcChildNode.ResourceKey, Thread.CurrentThread.CurrentUICulture)
										, CheckInPath(mvcChildNode, currentNode, filterContext.RouteData)
										, mvcChildNode.Icon != null ? urlHelper.Content("~/manager/Content/images/" + mvcChildNode.Icon) : null);
									menuViewData.AddSubMenuItem(menuItemData);
								}
							}
						}
					}
				}
			}
			filterContext.Controller.ViewData.Add("MenuViewData", menuViewData);
		}

		private bool CheckInPath(MvcSiteMapNode node, SiteMapNode currentNode, RouteData currentRouteData)
		{
			MvcSiteMapNode currentMvcNode = currentNode as MvcSiteMapNode;
			if (currentMvcNode != null)
			{
				return currentMvcNode.Key == node.Key || currentMvcNode.IsDescendantOf(node);
			}
			// We might have some unmapped actions. Check routedata if the node is in the path. Only check top-level nodes.
			if (currentRouteData != null)
			{
				return node.Controller == currentRouteData.Values["controller"].ToString()
					&& node.ParentNode == this._sitemapProvider.RootNode;
			}
			return false;
		}
	}
}
