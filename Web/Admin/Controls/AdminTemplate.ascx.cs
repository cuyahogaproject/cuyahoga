namespace Cuyahoga.Web.Admin.Controls
{
	using System;
	// using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Admin.UI;

	/// <summary>
	///		Summary description for AdminTemplate.
	/// </summary>
	public class AdminTemplate : BasePageControl
	{
		protected Literal PageTitleLabel;
		protected HtmlGenericControl MessageBox;

		private void Page_Load(object sender, System.EventArgs e)
		{
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
			this.PreRender += new EventHandler(AdminTemplate_PreRender);
		}
		#endregion

		private void AdminTemplate_PreRender(object sender, EventArgs e)
		{
			if (this.Page is AdminBasePage)
				this.PageTitleLabel.Text = this.PageTitle.Text;
		}
	}
}
