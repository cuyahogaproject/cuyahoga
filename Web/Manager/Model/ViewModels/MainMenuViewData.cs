using System.Collections.Generic;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class MainMenuViewData
	{
		private IList<MenuItem> _standardMainMenuItems = new List<MenuItem>();
		private IList<MenuItem> _optionalMainMenuItems = new List<MenuItem>();

		public IList<MenuItem> StandardMainMenuItems
		{
			get { return _standardMainMenuItems; }
		}

		public IList<MenuItem> OptionalMainMenuItems
		{
			get { return _optionalMainMenuItems; }
		}

		public void AddStandardMenuItem(MenuItem menuItem)
		{
			_standardMainMenuItems.Add(menuItem);
		}

		public void AddOptionalMenuItem(MenuItem menuItem)
		{
			_optionalMainMenuItems.Add(menuItem);
		}
	}

	public class MenuItem
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

		public MenuItem(string url, string text, bool isSelected)
		{
			this._url = url;
			this._text = text;
			this._isSelected = isSelected;
		}
	}
}


