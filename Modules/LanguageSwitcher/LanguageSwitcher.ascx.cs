namespace Cuyahoga.Modules.LanguageSwitcher
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Globalization;
	using System.Collections;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;

	/// <summary>
	///		Summary description for LanguageSwitcher.
	/// </summary>
	public class LanguageSwitcher : BaseModuleControl
	{
		private PageEngine _page;

		protected System.Web.UI.WebControls.ImageButton imbGo;
		protected System.Web.UI.WebControls.DropDownList ddlLanguage;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._page = this.Page as PageEngine;
			this.imbGo.ImageUrl = this.TemplateSourceDirectory + "/Images/go.gif";
			if (! this.IsPostBack)
			{
				BindLanguageOptions();
			}
		}

		private void BindLanguageOptions()
		{
			if (this._page != null)
			{
				IList rootNodes = this._page.CoreRepository.GetRootNodes(this._page.ActiveNode.Site);
				foreach (Node node in rootNodes)
				{
					CultureInfo ci = new CultureInfo(node.Culture);
					string languageAsText = ci.NativeName.Substring(0, ci.NativeName.IndexOf("(") - 1);
					ListItem item = new ListItem(languageAsText, node.Culture);
					if (this._page.ActiveNode.Culture == ci.Name)
					{
						item.Selected = true;
					}
					this.ddlLanguage.Items.Add(item);
				}
			}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void imbGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			string selectedCulture = this.ddlLanguage.SelectedValue;
			// Get the root node for the selected culture from the cache and build an url from
			// it where the user will be redirected to.
			Node rootNodeForSelectedCulture = this._page.CoreRepository.GetRootNodeByCultureAndSite(selectedCulture, this._page.CurrentSite);
			if (rootNodeForSelectedCulture != null)
			{
				// Set cookie for the selected culture. In the future we might enable persisting
				// this cookie to remember the prefered language of the user.
				HttpContext.Current.Response.Cookies.Add(new HttpCookie("CuyahogaCulture", selectedCulture));
				HttpContext.Current.Response.Redirect(UrlHelper.GetUrlFromNode(rootNodeForSelectedCulture));
			}
		}
	}
}
