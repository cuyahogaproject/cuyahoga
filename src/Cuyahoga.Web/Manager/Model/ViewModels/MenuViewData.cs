using System.Collections.Generic;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class MenuViewData
	{
		private readonly IList<MenuItemData> _standardMainMenuItems = new List<MenuItemData>();
		private readonly IList<MenuItemData> _optionalMainMenuItems = new List<MenuItemData>();

		public IList<MenuItemData> StandardMainMenuItems
		{
			get { return _standardMainMenuItems; }
		}

		public IList<MenuItemData> OptionalMainMenuItems
		{
			get { return _optionalMainMenuItems; }
		}

		public void AddStandardMenuItem(MenuItemData menuItemData)
		{
			_standardMainMenuItems.Add(menuItemData);
		}

		public void AddOptionalMenuItem(MenuItemData menuItemData)
		{
			_optionalMainMenuItems.Add(menuItemData);
		}
	}

	public class MenuItemData
	{
		private string _url;
		private string _text;
		private bool _isSelected;
		private string _iconUrl;
		private IList<MenuItemData> _childMenuItems;

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

		public string IconUrl
		{
			get { return _iconUrl; }
		}

		public IList<MenuItemData> ChildMenuItems
		{
			get { return _childMenuItems; }
		}

		public MenuItemData(string url, string text, bool isSelected, string iconUrl)
		{
			this._childMenuItems = new List<MenuItemData>();
			this._url = url;
			this._text = text;
			this._isSelected = isSelected;
			this._iconUrl = iconUrl;
		}

		public void AddChildMenuItem(MenuItemData childMenuItem)
		{
			this._childMenuItems.Add(childMenuItem);
		}
	}
}


