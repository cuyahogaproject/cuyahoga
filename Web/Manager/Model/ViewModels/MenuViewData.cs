using System.Collections.Generic;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class MenuViewData
	{
		private IList<MenuItem> _standardMainMenuItems = new List<MenuItem>();
		private IList<MenuItem> _optionalMainMenuItems = new List<MenuItem>();
		private IList<MenuItem> _subMenuItems = new List<MenuItem>();

		public IList<MenuItem> StandardMainMenuItems
		{
			get { return _standardMainMenuItems; }
		}

		public IList<MenuItem> OptionalMainMenuItems
		{
			get { return _optionalMainMenuItems; }
		}

		public IList<MenuItem> SubMenuItems
		{
			get { return _subMenuItems; }
		}

		public void AddStandardMenuItem(MenuItem menuItem)
		{
			_standardMainMenuItems.Add(menuItem);
		}

		public void AddOptionalMenuItem(MenuItem menuItem)
		{
			_optionalMainMenuItems.Add(menuItem);
		}

		public void AddSubMenuItem(MenuItem menuItem)
		{
			_subMenuItems.Add(menuItem);
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


