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
		protected System.Web.UI.WebControls.ImageButton imbGo;
		protected System.Web.UI.WebControls.DropDownList ddlLanguage;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.imbGo.ImageUrl = this.TemplateSourceDirectory + "/Images/go.gif";
			if (! this.IsPostBack)
			{
				BindLanguageOptions();
			}
		}

		private void BindLanguageOptions()
		{
			PageEngine page = this.Page as PageEngine;
			if (page != null)
			{
				IList rootNodes = page.CoreRepository.GetRootNodes(page.ActiveNode.Site);
				foreach (Node node in rootNodes)
				{
					CultureInfo ci = new CultureInfo(node.Culture);
					string languageAsText = ci.NativeName.Substring(0, ci.NativeName.IndexOf("(") - 1);
					ListItem item = new ListItem(languageAsText, node.Id.ToString());
					if (page.ActiveNode.Culture == ci.Name)
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
			int rootNodeId = Int32.Parse(this.ddlLanguage.SelectedValue);
			string url = UrlHelper.GetUrlFromNodeId(rootNodeId);
			HttpContext.Current.Response.Redirect(url);
		}
	}
}
