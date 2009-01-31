using System.Collections.Generic;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class MenuViewData
	{
		private IList<MenuItemData> _standardMainMenuItems = new List<MenuItemData>();
		private IList<MenuItemData> _optionalMainMenuItems = new List<MenuItemData>();
		private IList<MenuItemData> _subMenuItems = new List<MenuItemData>();

		public IList<MenuItemData> StandardMainMenuItems
		{
			get { return _standardMainMenuItems; }
		}

		public IList<MenuItemData> OptionalMainMenuItems
		{
			get { return _optionalMainMenuItems; }
		}

		public IList<MenuItemData> SubMenuItems
		{
			get { return _subMenuItems; }
		}

		public void AddStandardMenuItem(MenuItemData menuItemData)
		{
			_standardMainMenuItems.Add(menuItemData);
		}

		public void AddOptionalMenuItem(MenuItemData menuItemData)
		{
			_optionalMainMenuItems.Add(menuItemData);
		}

		public void AddSubMenuItem(MenuItemData menuItemData)
		{
			_subMenuItems.Add(menuItemData);
		}
	}

	public class MenuItemData
	{
		private string _url;
		private string _text;
		private bool _isSelected;
		private string _iconUrl;

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

		public MenuItemData(string url, string text, bool isSelected, string iconUrl)
		{
			this._url = url;
			this._text = text;
			this._isSelected = isSelected;
			this._iconUrl = iconUrl;
		}
	}
}


