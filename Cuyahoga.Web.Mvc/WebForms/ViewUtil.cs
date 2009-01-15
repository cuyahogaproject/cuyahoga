using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
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
			Control control = LoadControlFromVirtualPath(virtualPath);
			IDictionary<string, PlaceHolder> placeholders = ExtractPlaceholdersFromControl(control);
			return placeholders;
		}

		public static string RenderTemplateHtml(string virtualPath)
		{
			Page pageHolder = new Page();
			UserControl viewControl = (UserControl)pageHolder.LoadControl(virtualPath);

			// Insert placeholder div's into the placholder.
			foreach (KeyValuePair<string, PlaceHolder> placeHolder in ExtractPlaceholdersFromControl(viewControl))
			{
				string placeHolderDiv = String.Format("<div id=\"{0}\" class=\"{1}\"><div class=\"placeholdertitle\">{2}</div><ul class=\"sectionlist\"></ul></div>"
					, "plh-" + placeHolder.Key, "contentplaceholder", placeHolder.Key);
				Literal placeHolderContentControl = new Literal();
				placeHolderContentControl.Text = placeHolderDiv;
				placeHolder.Value.Controls.Add(placeHolderContentControl);
			}

			// Only render inner contents of the form
			HtmlForm theForm = FindForm(viewControl);
			while (theForm.Controls.Count > 0)
			{
				pageHolder.Controls.Add(theForm.Controls[0]);
			}
			StringWriter output = new StringWriter();
			HttpContext.Current.Server.Execute(pageHolder, output, false);

			return output.ToString();
		}

		private static IDictionary<string, PlaceHolder> ExtractPlaceholdersFromControl(Control control)
		{
			HtmlForm form = FindForm(control);
			IDictionary<string, PlaceHolder> placeholders = new Dictionary<string, PlaceHolder>();
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