using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// This is the base class for every admin template usercontrol.
	/// </summary>
	public abstract class BasePageControl : System.Web.UI.UserControl
	{
		/// <summary>
		/// Template controls that inherit from BasePageControl must have a Literal control with id="PageTitle"
		/// </summary>
		protected Literal PageTitle;
		/// <summary>
		/// Template controls that inherit from BasePageControl must have a HtmlControl control id="CssStyleSheet"
		/// </summary>
		protected HtmlControl CssStyleSheet;
		/// <summary>
		/// Template controls that inherit from BasePageControl must have a PlaceHolder control id="PageContent"
		/// </summary>
		protected PlaceHolder PageContent;


		/// <summary>
		/// Template controls that inherit from BasePageControl must have a PlaceHolder control id="AddedCssPlaceHolder"
		/// </summary>
		protected PlaceHolder AddedCssPlaceHolder;
		/// <summary>
		/// Template controls that inherit from BasePageControl must have a PlaceHolder control id="AddedJavaScriptPlaceHolder"
		/// </summary>
		protected PlaceHolder AddedJavaScriptPlaceHolder;

		private OrderedDictionary _stylesheets;
		private OrderedDictionary _javascripts;

		public BasePageControl()
		{
			this._stylesheets = new OrderedDictionary();
			this._javascripts = new OrderedDictionary();
		}

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

		#region Register Javascript and CSS
		/// <summary>
		/// Register stylesheets.
		/// </summary>
		/// <param name="key">The unique key for the stylesheet. Note that Cuyahoga already uses 'maincss' as key.</param>
		/// <param name="absoluteCssPath">The path to the css file from the application root (starting with /).</param>
		public void RegisterStylesheet(string key, string absoluteCssPath)
		{
			if (this._stylesheets[key] == null)
			{
				this._stylesheets.Add(key, absoluteCssPath);
			}
		}

		/// <summary>
		/// Register javascripts.
		/// </summary>
		/// <param name="key">The unique key for the stylesheet. Note that Cuyahoga already uses 'maincss' as key.</param>
		/// <param name="absoluteJavascriptPath">The path to the css file from the application root (starting with /).</param>
		public void RegisterJavascript(string key, string absoluteJavascriptPath)
		{
			if (this._javascripts[key] == null)
			{
				this._javascripts.Add(key, absoluteJavascriptPath);
			}
		}

		public void InsertStylesheets()
		{
			string[] stylesheetlinks = new string[this._stylesheets.Count];
			int i = 0;
			foreach (string stylesheet in this._stylesheets.Values)
			{
				stylesheetlinks[i] = stylesheet;
				i++;
			}
			this.RenderCssLinks(stylesheetlinks);
		}

		public void InsertJavascripts()
		{
			string[] javascriptlinks = new string[this._javascripts.Count];
			int i = 0;
			foreach (string javascript in this._javascripts.Values)
			{
				javascriptlinks[i] = javascript;
				i++;
			}
			this.RenderJavascriptLinks(javascriptlinks);
		}

		/// <summary>
		/// Converts the list of css links to stylesheet tags and inserts these in the appropriate place.
		/// </summary>
		/// <param name="stylesheets"></param>
		public void RenderCssLinks(string[] stylesheets)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string stylesheet in stylesheets)
			{
				HtmlLink css = new HtmlLink();
				css.Href = stylesheet;
				css.Attributes["rel"] = "stylesheet";
				css.Attributes["type"] = "text/css";
				css.Attributes["media"] = "all";
				AddedCssPlaceHolder.Controls.Add(css);
			}
		}

		/// <summary>
		/// Converts the list of javascript links to stylesheet tags and inserts these in the appropriate place.
		/// </summary>
		/// <param name="javascripts"></param>
		public void RenderJavascriptLinks(string[] javascripts)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string javascript in javascripts)
			{
				HtmlGenericControl js = new HtmlGenericControl();
				js.TagName = "script";
				js.Attributes["src"] = javascript;
				js.Attributes["type"] = "text/javascript";
				AddedJavaScriptPlaceHolder.Controls.Add(js);
			}
		}


		#endregion Register Javascript and CSS

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