namespace Cuyahoga.Modules.User
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core;
	using Cuyahoga.Web.UI;

	/// <summary>
	///		Summary description for ResetPassword.
	/// </summary>
	public partial class ResetPassword : BaseModuleControl
	{
		private ProfileModule _module;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as ProfileModule;

			if (! this.IsPostBack)
			{
				// Databind is required to bind the localized resources.
				this.DataBind();
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

		}
		#endregion

		protected void btnReset_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid)
			{
				try
				{
					this._module.ResetPassword(this.txtUsername.Text, this.txtEmail.Text);
					this.pnlReset.Visible = false;
					this.pnlConfirmation.Visible = true;
					this.lblConfirmation.Text = String.Format(GetText("RESETCONFIRMATION"), this.txtEmail.Text);
				}
				catch (EmailException)
				{
					this.lblError.Text = GetText("RESETEMAILERROR");
					this.lblError.Visible = true;
				}
				catch (Exception ex)
				{
					this.lblError.Text = ex.Message;
					this.lblError.Visible = true;
				}
			}
		}
	}
}
