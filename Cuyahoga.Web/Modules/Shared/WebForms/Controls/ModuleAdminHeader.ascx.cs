namespace Cuyahoga.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;

	/// <summary>
	///		Summary description for ModuleAdminHeader.
	/// </summary>
	public class ModuleAdminHeader : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label lblNode;
		protected System.Web.UI.WebControls.Label lblSection;
		protected System.Web.UI.WebControls.HyperLink hplBack;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Page is ModuleAdminBasePage)
			{
				ModuleAdminBasePage adminPage = (ModuleAdminBasePage)this.Page;
				this.lblNode.Text = adminPage.Node.Title;
				this.lblSection.Text = adminPage.Section.Title;
				this.hplBack.NavigateUrl = UrlHelper.GetUrlFromNode(adminPage.Node);
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
