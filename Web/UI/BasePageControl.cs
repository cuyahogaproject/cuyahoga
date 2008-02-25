using System;
using System.Web.UI.WebControls;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// This is the base class for every admin template usercontrol.
	/// </summary>
	public abstract class BasePageControl: System.Web.UI.UserControl
	{
		/// <summary>
		/// Template controls that inherit from BasePageControl must have a Literal control with id="PageTitle"
		/// </summary>
		protected System.Web.UI.WebControls.Literal PageTitle;
		/// <summary>
		/// Template controls that inherit from BasePageControl must have a HtmlControl control id="CssStyleSheet"
		/// </summary>
		protected System.Web.UI.HtmlControls.HtmlControl CssStyleSheet;
		/// <summary>
		/// Template controls that inherit from BasePageControl must have a PlaceHolder control id="PageContent"
		/// </summary>
		protected System.Web.UI.WebControls.PlaceHolder PageContent;

		/// <summary>
		/// The page title as shown in the title bar of the browser
		/// </summary>
		public string Title
		{
			get
			{
				return this.PageTitle.Text;
			}
			set
			{
				this.PageTitle.Text = value;
			}
		}

		/// <summary>
		/// Path to external stylesheet file, relative from the application root.
		/// </summary>
		public string Css
		{
			get
			{
				return this.CssStyleSheet.Attributes["href"];
			}
			set 
			{
				this.CssStyleSheet.Attributes.Add("href", this.ResolveUrl(value));
			}
		}

		/// <summary>
		/// Placeholder for the actual page content.
		/// </summary>
		public PlaceHolder Content
		{
			get
			{
				return this.PageContent;
			}
			set
			{
                this.PageContent = value;
			}
		}
	}
}
