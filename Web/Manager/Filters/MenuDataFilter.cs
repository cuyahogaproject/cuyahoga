using System.Web.Mvc;
using System.Web.Routing;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Filters
{
	public class MenuDataFilter : ActionFilterAttribute
	{
		private readonly ICuyahogaContext _cuyahogaContext;

		/// <summary>
		/// Creates a new instance of the <see cref="MenuDataFilter"></see> class.
		/// </summary>
		/// <remarks>
		/// Lookup components via the static IoC resolver. It would be great if we could do this via dependency injection
		/// but filters cannot be managed via the container in the current version of ASP.NET MVC. 
		/// </remarks>
		public MenuDataFilter() : this(IoC.Resolve<ICuyahogaContext>())
		{}

		/// <summary>
		/// Creates a new instance of the <see cref="MenuDataFilter"></see> class.
		/// </summary>
		/// <param name="cuyahogaContext"></param>
		public MenuDataFilter(ICuyahogaContext cuyahogaContext)
		{
			_cuyahogaContext = cuyahogaContext;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			InitMenuViewData(filterContext);
		}

		private void InitMenuViewData(ActionExecutingContext filterContext)
		{
			UrlHelper urlHelper = new UrlHelper(filterContext);

			MainMenuViewData mainMenuViewData = new MainMenuViewData();
			User user = this._cuyahogaContext.CurrentUser;
			if (user != null && user.IsAuthenticated)
			{
				mainMenuViewData.AddStandardMenuItem(
					new MenuItem(urlHelper.Action("Index", "Dashboard")
					, GlobalResources.ManagerMenuDashboard, CheckInPath(filterContext.RouteData, "Dashboard")));
				if (user.HasRight(Rights.ManagePages, this._cuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddStandardMenuItem(
						new MenuItem(urlHelper.Action("Index", "Pages")
						, GlobalResources.ManagerMenuPages, CheckInPath(filterContext.RouteData, "Pages")));
				}
				if (user.HasRight(Rights.ManageFiles, this._cuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddStandardMenuItem(
						new MenuItem(urlHelper.Action("Index", "Files")
						, GlobalResources.ManagerMenuFiles, CheckInPath(filterContext.RouteData, "Files")));
				}
				if (user.HasRight(Rights.ManageUsers, this._cuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddOptionalMenuItem(
						new MenuItem(urlHelper.Action("Index", "Users")
						, GlobalResources.ManagerMenuUsers, CheckInPath(filterContext.RouteData, "Users")));
				}
				if (user.HasRight(Rights.ManageSite, this._cuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddOptionalMenuItem(
						new MenuItem(urlHelper.Action("Index", "Site")
						, GlobalResources.ManagerMenuSite, CheckInPath(filterContext.RouteData, "Site")));
				}
				if (user.HasRight(Rights.ManageServer, this._cuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddOptionalMenuItem(
						new MenuItem(urlHelper.Action("Index", "Server")
						, GlobalResources.ManagerMenuServer, CheckInPath(filterContext.RouteData, "Server")));
				}
			}
			filterContext.Controller.ViewData.Add("MainMenuViewData", mainMenuViewData);
		}

		private bool CheckInPath(RouteData routeData, string controllerName)
		{
			bool isInPath = routeData.Values["controller"].ToString().ToLower().Contains(controllerName.ToLower());
			if (! isInPath)
			{
				// also check if we have a subarea with the controller name
				if (routeData.Values.ContainsKey("subarea"))
				{
					isInPath = routeData.Values["subarea"].ToString().ToLower().Contains(controllerName.ToLower());
				}
			}
			return isInPath;
		}
	}
}
