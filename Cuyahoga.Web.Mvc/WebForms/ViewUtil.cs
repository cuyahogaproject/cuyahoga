using System;
using System.Collections.Generic;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Cuyahoga.Web.Mvc.WebForms
{
	/// <summary>
	/// Utilities for webforms management.
	/// </summary>
	public static class ViewUtil
	{
		/// <summary>
		/// Load a webforms view (Page, UserControl, MasterPage) from the given virtual path.
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static Control LoadControlFromVirtualPath(string virtualPath)
		{
			// We're using LoadControl of an arbitrary UserControl to load the view. We could also use the BuildManager, but
			// in that case, the control hierarchy appears to be empty and thus, pretty useless.
			UserControl controlLoader = new UserControl();
			return controlLoader.LoadControl(virtualPath);
		}

		/// <summary>
		/// Get all placeholder controls from the webforms view from the given virtual path.
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static IDictionary<string, PlaceHolder> GetPlaceholdersFromVirtualPath(string virtualPath)
		{
			IDictionary<string, PlaceHolder> placeholders = new Dictionary<string, PlaceHolder>();
			Control control = LoadControlFromVirtualPath(virtualPath);
			HtmlForm form = FindForm(control);
			if (form != null)
			{
				foreach (Control childControl in form.Controls)
				{
					if (childControl is PlaceHolder)
					{
						placeholders.Add(childControl.ID, (PlaceHolder) childControl);
					}
					// Also check for user controls with content placeholders.
					else if (childControl is UserControl)
					{
						foreach (Control userControlChildControl in childControl.Controls)
						{
							if (userControlChildControl is PlaceHolder)
							{
								placeholders.Add(userControlChildControl.ID, (PlaceHolder) userControlChildControl);
							}
						}
					}
				}
			}
			return placeholders;
		}

		private static HtmlForm FindForm(Control control)
		{
			foreach (Control ctrl in control.Controls)
			{
				if (ctrl is HtmlForm)
				{
					return ctrl as HtmlForm;
				}
			}
			return null;
		}
	}
}