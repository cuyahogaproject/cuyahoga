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
		private string _baseName;
		private ResourceManager _resMan;

		public LocalizedUserControl()
		{
			// Base name of the resources consists of Namespace.Resources.Strings
			this._baseName = this.GetType().BaseType.Namespace + ".Resources.Strings";
			this._resMan = new ResourceManager(this._baseName, this.GetType().BaseType.Assembly);
		}

		/// <summary>
		/// Get a localized text string for a given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected virtual string GetText(string key)
		{
			string localizedText;
			CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;

			// Try to get text from assembly
			try
			{
				localizedText = this._resMan.GetString(key, currentUICulture);
			}
			catch (MissingManifestResourceException)
			{
				localizedText = key + " [error retrieving value]";
			}

			
			return localizedText;
		}

		/// <summary>
		/// Get a localized text string from a resource file (in App_GlobalResources) for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected virtual string GetTextFromFile(string key)
		{
			string localizedText;
			CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
			if (HttpContext.Current != null)
			{
				try
				{
					object resource = HttpContext.GetGlobalResourceObject(this._baseName, key, currentUICulture);
					localizedText = (string) resource;
				}
				catch (MissingManifestResourceException)
				{
					localizedText = key + " [error retrieving value]";
				}
			}
			else
			{
				throw new Exception("Unable to get text from resource file because there is no HttpContext available");
			}
			return localizedText;
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
		/// The resource keys should be the same as the ID of the control that should be translated.
		/// You can override this behavior by adding a prefix with the UserControl name and a 
		/// semicolon, for example: "Articles:lblTitle".
		/// </summary>
		/// <param name="control"></param>
		protected virtual void LocalizeControls(Control control)
		{
			foreach (Control childControl in control.Controls)
			{
				// First try to find a string for this specific user control.
				// Use the name of BaseType because we need the code-behind class.
				string resourceKey = this.GetType().BaseType.Name + ":" + childControl.ID;
				string localizedText = GetText(resourceKey);

				if (localizedText == null)
				{
					// No translation found with the user control prefix. Try to find a translation that
					// only has the control ID as key
					if (childControl.ID != null)
					{
						localizedText = GetText(childControl.ID);
					}
				}

				if (!String.IsNullOrEmpty(localizedText))
				{
					if ((childControl is Label) && !(childControl is BaseValidator))
					{
						Label label = (Label)childControl;
						label.Text = localizedText;
					}
					if (childControl is Button)
					{
						Button button = (Button)childControl;
						button.Text = localizedText;
					}
					if (childControl is LinkButton)
					{
						LinkButton linkButton = (LinkButton)childControl;
						linkButton.Text = localizedText;
					}
					if (childControl is HyperLink)
					{
						HyperLink hyperLink = (HyperLink)childControl;
						hyperLink.Text = localizedText;
					}
					if (childControl is RadioButton)
					{
						RadioButton radioButton = (RadioButton)childControl;
						radioButton.Text = localizedText;
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
