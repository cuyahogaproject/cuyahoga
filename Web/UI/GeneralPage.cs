using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Service;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// The GeneralPage serves as a base page for pages that are not related to a specific Node.
	/// It uses the default template and placeholder from the current Site to inject the
	/// content of inherited pages.
	/// </summary>
	public class GeneralPage : PageEngine
	{
		private BaseTemplate _templateControl;
		private PlaceHolder _contentPlaceHolder;
		private Site _currentSite;
		private string _title;

		/// <summary>
		/// The GeneralPage only utilizes one placeholder. This property exposes that property to inherited pages.
		/// </summary>
		protected PlaceHolder ContentPlaceHolder
		{
			get { return this._contentPlaceHolder; }
		}

		/// <summary>
		/// The page title.
		/// </summary>
		public string Title
		{
			get { return this._title; }
			set 
			{ 
				this._title = value; 
				if (this._templateControl != null)
				{
					this._templateControl.Title = value;
				}
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public GeneralPage()
		{
		}

		protected override void OnInit(EventArgs e)
		{
			// The GeneralPage loads it's own content. No need for the PageEngine to do that.
			base.ShouldLoadContent = false;
			// Init the PageEngine.
			base.OnInit (e);
			// Build page.
			ControlCollection col = this.Controls; 

			this._currentSite = base.RootNode.Site;
			if (this._currentSite.DefaultTemplate != null 
				&& this._currentSite.DefaultPlaceholder != null 
				&& this._currentSite.DefaultPlaceholder != String.Empty)
			{
				// Load the template
				this._templateControl = (BaseTemplate)this.LoadControl(UrlHelper.GetApplicationPath() 
					+ this._currentSite.DefaultTemplate.Path);
				this._templateControl.Css = UrlHelper.GetApplicationPath() + Config.GetConfiguration()["CssDir"] + this._currentSite.DefaultTemplate.Css;
				if (this._title != null)
				{
					this._templateControl.Title = this._title;
				}

				// Add the pagecontrol on top of the control collection of the page
				this._templateControl.ID = "p";
				col.AddAt(0, this._templateControl);

				// Get the Content placeholder
				this._contentPlaceHolder = this._templateControl.FindControl(this._currentSite.DefaultPlaceholder) as PlaceHolder;
				if (this._contentPlaceHolder != null)
				{
					// Iterate through the controls in the page to find the form control.
					foreach (Control control in col)
					{
						if (control is HtmlForm)
						{
							// We've found the form control. Now move all child controls into the placeholder.
							HtmlForm formControl = (HtmlForm)control;
							while (formControl.Controls.Count > 0)
							{
								this._contentPlaceHolder.Controls.Add(formControl.Controls[0]);						
							}
						}
					}
					// throw away all controls in the page, except the page control 
					while (col.Count > 1)
					{
						col.Remove(col[1]);				
					}
				}
			}
			else
			{
				// The default template and placeholders are not correctly configured.
				throw new Exception("Unable to display page because the default template is not configured.");
			}
		}

		/// <summary>
		/// Use a custom HtmlTextWriter to render the page if the url is rewritten, to correct the form action.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (Context.Items["VirtualUrl"] != null)
			{
				writer = new FormFixerHtmlTextWriter(writer.InnerWriter, "", Context.Items["VirtualUrl"].ToString());
			}
			base.Render (writer);
		}
	}
}
