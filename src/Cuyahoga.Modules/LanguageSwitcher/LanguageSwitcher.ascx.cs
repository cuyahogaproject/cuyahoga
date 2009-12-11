namespace Cuyahoga.Modules.LanguageSwitcher
{
	using System;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections.Generic;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using System.Collections.Specialized;
	using Cuyahoga.Core.Util;

	/// <summary>
	///		Summary description for LanguageSwitcher.
	/// </summary>
	public partial class LanguageSwitcher : BaseModuleControl
	{
		private LanguageSwitcherModule _module;
		private Dictionary<string, string> _homePagesForLanguages = new Dictionary<string, string>();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as LanguageSwitcherModule;

			this.imbGo.ImageUrl = this.TemplateSourceDirectory + "/Images/go.gif";
			if (! this.IsPostBack)
			{
				HandleDisplayMode();
				BindLanguageOptions();
				if (this._module.RedirectToUserLanguage)
				{
					SetPreferedLanguage();
				}
			}
		}

		private void HandleDisplayMode()
		{
			this.pnlLinks.Visible = this._module.DisplayMode == DisplayMode.Text || this._module.DisplayMode == DisplayMode.Flag;
			this.pnlDropDown.Visible = this._module.DisplayMode == DisplayMode.DropDown;
		}

		private void BindLanguageOptions()
		{
			Dictionary<string, Node> cultureNodes = this._module.GetCultureRootNodesBySite(base.PageEngine.CurrentSite);
			HtmlGenericControl listControl = new HtmlGenericControl("ul");
			listControl.EnableViewState = false;

			foreach (KeyValuePair<string, Node> cultureNode in cultureNodes)
			{
				string languageAsText = Globalization.GetNativeLanguageTextFromCulture(cultureNode.Key);
				switch (this._module.DisplayMode)
				{
					case DisplayMode.Text:
						AddLanguageLink(cultureNode.Key, cultureNode.Value, listControl, languageAsText, false);
						break;
					case DisplayMode.Flag:
						AddLanguageLink(cultureNode.Key, cultureNode.Value, listControl, languageAsText, true);
						break;
					case DisplayMode.DropDown:
						AddDropDownOption(cultureNode.Key, languageAsText);
						break;
				}

				// Also add the language and root url to the list of possible redirectable urls. We can use this 
				// later to redirect to the root node that corresponds with the browser language if there is no
				// specific page requested.
				if (cultureNode.Key != base.PageEngine.RootNode.Culture)
				{
					this._homePagesForLanguages.Add(Globalization.GetLanguageFromCulture(cultureNode.Key), UrlUtil.GetUrlFromNode(cultureNode.Value));
				}
			}
			if (this._module.DisplayMode == DisplayMode.Text || this._module.DisplayMode == DisplayMode.Flag)
			{
				this.plhLanguageLinks.Controls.Add(listControl);
			}
		}

		private void SetPreferedLanguage()
		{
			NameValueCollection vars = this.Request.ServerVariables;
			if (vars.Get("HTTP_ACCEPT_LANGUAGE") != null)
			{
				string preferedLanguage = vars.Get("HTTP_ACCEPT_LANGUAGE").Substring(0, 2);
				string url = System.Web.HttpContext.Current.Items["VirtualUrl"].ToString();
				bool isStartPage = url.ToLower().EndsWith("/default.aspx");

				// Only redirect when there is no specific url requested and there is a root page for the 
				// user language.
				if (isStartPage && this._homePagesForLanguages.ContainsKey(preferedLanguage))
				{
					Context.Response.Redirect(this._homePagesForLanguages[preferedLanguage].ToString());
				}
			}
		}

		private void AddLanguageLink(string culture, Node node, HtmlGenericControl listControl, string languageAsText, bool showImage)
		{
			if (node.Culture != base.PageEngine.RootNode.Culture)
			{
				HtmlGenericControl listItem = new HtmlGenericControl("li");
				HyperLink hpl = new HyperLink();
				hpl.NavigateUrl = UrlUtil.GetUrlFromNode(node);
				hpl.Text = languageAsText;
				if (showImage)
				{
					string countryCode = Globalization.GetCountryFromCulture(culture).ToLower();
					string imageUrl = this.TemplateSourceDirectory + String.Format("/Images/flags/{0}.png", countryCode);
					Image image = new Image();
					image.ImageUrl = imageUrl;
					image.AlternateText = languageAsText;
					hpl.ToolTip = languageAsText;
					hpl.Controls.Add(image);
				}
				listItem.Controls.Add(hpl);
				listControl.Controls.Add(listItem);
			}
		}

		private void AddDropDownOption(string culture, string languageAsText)
		{
			ListItem item = new ListItem(languageAsText, culture);
			if (base.PageEngine.ActiveNode.Culture == culture)
			{
				item.Selected = true;
			}
			this.ddlLanguage.Items.Add(item);
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.imbGo.Click += new System.Web.UI.ImageClickEventHandler(this.imbGo_Click);

		}
		#endregion

		private void imbGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			string selectedCulture = this.ddlLanguage.SelectedValue;
			// Get the root node for the selected culture from the cache and build an url from
			// it where the user will be redirected to.
			Node rootNodeForSelectedCulture = this._module.GetRootNodeByCultureAndSite(selectedCulture, base.PageEngine.CurrentSite);
			if (rootNodeForSelectedCulture != null)
			{
				// Set cookie for the selected culture. In the future we might enable persisting
				// this cookie to remember the prefered language of the user.
				HttpContext.Current.Response.Cookies.Add(new HttpCookie("CuyahogaCulture", selectedCulture));
				HttpContext.Current.Response.Redirect(UrlUtil.GetUrlFromNode(rootNodeForSelectedCulture));
			}
		}
	}
}
