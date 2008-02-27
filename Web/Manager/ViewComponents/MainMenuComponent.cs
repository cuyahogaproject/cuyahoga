using System.Collections.Generic;
using Castle.MonoRail.Framework;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Components;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.ViewComponents
{
	public class MainMenuComponent : ViewComponent
	{
		private ICuyahogaContext _cuyahogaContext;

		public MainMenuComponent(ICuyahogaContext cuyahogaContext)
		{
			this._cuyahogaContext = cuyahogaContext;
		}
		public override void Initialize()
		{
			IUrlBuilder urlBuilder = RailsContext.GetService<IUrlBuilder>();
			 
			List<MainMenuItem> standardMenuItems = new List<MainMenuItem>();
			List<MainMenuItem> optionalMenuItems = new List<MainMenuItem>();
			User user = this._cuyahogaContext.CurrentUser;
			if (user != null && user.IsAuthenticated)
			{
				standardMenuItems.Add(
					new MainMenuItem(urlBuilder.BuildUrl(RailsContext.UrlInfo, "dashboard", "index")
					, GlobalResources.ManagerMenuDashboard, CheckInPath("dashboard")));
				standardMenuItems.Add(
					new MainMenuItem(urlBuilder.BuildUrl(RailsContext.UrlInfo, "pages", "index")
					, GlobalResources.ManagerMenuPages, CheckInPath("pages")));
				standardMenuItems.Add(
					new MainMenuItem(urlBuilder.BuildUrl(RailsContext.UrlInfo, "files", "index")
					, GlobalResources.ManagerMenuFiles, CheckInPath("files")));
				if (user.HasRight("Manage Users"))
				{
					optionalMenuItems.Add(
						new MainMenuItem(urlBuilder.BuildUrl(RailsContext.UrlInfo, "users", "index")
						, GlobalResources.ManagerMenuUsers, CheckInPath("users")));
				}
				if (user.HasRight("Manage Site"))
				{
					optionalMenuItems.Add(
						new MainMenuItem(urlBuilder.BuildUrl(RailsContext.UrlInfo, "site", "index")
						, GlobalResources.ManagerMenuSite, CheckInPath("site")));
				}
				if (user.HasRight("Manage Server"))
				{
					optionalMenuItems.Add(
						new MainMenuItem(urlBuilder.BuildUrl(RailsContext.UrlInfo, "server", "index")
						, GlobalResources.ManagerMenuServer, CheckInPath("site")));
				}
			}
			PropertyBag["standardmenuitems"] = standardMenuItems;
			PropertyBag["optionalmenuitems"] = optionalMenuItems;
			base.Initialize();
		}

		private bool CheckInPath(string areaOrControllerName)
		{
			return
				RailsContext.CurrentController.AreaName.ToLower().Contains(areaOrControllerName) ||
				RailsContext.CurrentController.Name.ToLower().Contains(areaOrControllerName);
		}

		public override void Render()
		{
			RenderView("MainMenu");
		}

		private class MainMenuItem
		{
			private string _url;
			private string _text;
			private bool _isSelected;

			public string Url
			{
				get { return _url; }
			}

			public string Text
			{
				get { return _text; }
			}

			public bool IsSelected
			{
				get { return _isSelected; }
			}

			public MainMenuItem(string url, string text, bool isSelected)
			{
				this._url = url;
				this._text = text;
				this._isSelected = isSelected;
			}
		}
	}
}
