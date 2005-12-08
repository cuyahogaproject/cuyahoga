using System;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Summary description for LocalizedUserControl.
	/// </summary>
	public class LocalizedUserControl : UserControl
	{
		private ResourceManager _resMan;
		private CultureInfo _currentUICulture;

		public LocalizedUserControl()
		{
			// Base name of the resources consists of Namespace.Resources.Strings
			string baseName = this.GetType().BaseType.Namespace + ".Resources.Strings";
			this._resMan = new ResourceManager(baseName, this.GetType().BaseType.Assembly);
			this._currentUICulture = Thread.CurrentThread.CurrentUICulture;
		}

		/// <summary>
		/// Get a localized text string for a given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected virtual string GetText(string key)
		{
			return this._resMan.GetString(key, this._currentUICulture);
		}

		/// <summary>
		/// Try to localize the controls. 
		/// The resource keys should consist of the name of the User Control class
		/// with the ID of the control added. For example "Articles.lblTitle".
		/// </summary>
		protected virtual void LocalizeControls()
		{
			LocalizeControls(this);
		}

		/// <summary>
		/// Try to localize the controls. 
		/// The resource keys should consist of the name of the User Control code-behind  class
		/// with the ID of the control added, separated with a colon. 
		/// Example: "Articles:lblTitle".
		/// </summary>
		/// <param name="control"></param>
		protected virtual void LocalizeControls(Control control)
		{
			foreach (Control childControl in control.Controls)
			{
				// Use the name of BaseType because we need the code-behind class.
				string resourceKey = this.GetType().BaseType.Name + ":" + childControl.ID;
				string localizedText = GetText(resourceKey);

				if (localizedText != null && localizedText != String.Empty)
				{
					if ((childControl is Label) && ! (childControl is BaseValidator))
					{
						Label label = (Label)childControl;
						label.Text = localizedText;
					}
					if (childControl is Button)
					{
						Button button = (Button)childControl;
						button.Text = localizedText;
					}
					else if (childControl is BaseValidator)
					{
						BaseValidator validator = (BaseValidator)childControl;
						validator.ErrorMessage = localizedText;
					}
				}
				// Recursive translate childcontrols
				LocalizeControls(childControl);
			}
		}

		/// <summary>
		/// Recursively databind controls that might have localized texts.
		/// </summary>
		protected virtual void BindResources()
		{
			BindResources(this);
		}

		private void BindResources(Control control)
		{
			foreach (Control childControl in control.Controls)
			{
				if (childControl is Label || childControl is Button || childControl is BaseValidator)
				{
					childControl.DataBind();
				}
				BindResources(childControl);
			}
		}
	}
}
