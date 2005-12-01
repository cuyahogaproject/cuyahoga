namespace Cuyahoga.Web.Admin.Controls
{
	using System;
	// using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Web.Security;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.Admin.UI;
	using Cuyahoga.Web.Util;

	/// <summary>
	///		Summary description for Header.
	/// </summary>
	public class Header : System.Web.UI.UserControl
	{
		private AdminBasePage _page;
		protected System.Web.UI.WebControls.HyperLink hplSite;

		protected System.Web.UI.WebControls.LinkButton lbtLogout;

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				this._page = (AdminBasePage)this.Page;		
			}
			catch (InvalidCastException ex)
			{
				throw new Exception("This control requires a Page of the type AdminBasePage.", ex);
			}
			if (this._page.ActiveSection != null)
			{
				this.hplSite.NavigateUrl = Util.UrlHelper.GetFullUrlFromSectionViaSite(this._page.ActiveSection);
			}
			else if (this._page.ActiveNode != null)
			{
				this.hplSite.NavigateUrl = Util.UrlHelper.GetFullUrlFromNodeViaSite(this._page.ActiveNode);
			}
			else if (this._page.ActiveSite != null)
			{
				this.hplSite.NavigateUrl = this._page.ActiveSite.SiteUrl;
			}
			else
			{
				this.hplSite.Visible = false;
			}
			this.lbtLogout.Visible = this.Page.User.Identity.IsAuthenticated;
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
			this.lbtLogout.Click += new System.EventHandler(this.lbtLogout_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lbtLogout_Click(object sender, System.EventArgs e)
		{
            FormsAuthentication.SignOut();	
			Context.Response.Redirect(Context.Request.RawUrl);
		}
	}
}
