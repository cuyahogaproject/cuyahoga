namespace Cuyahoga.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Core.Service;
	using Cuyahoga.Web.UI;

	/// <summary>
	///		Summary description for ResetPassword.
	/// </summary>
	public class ResetPassword : LocalizedUserControl
	{
		private GeneralPage _page;

		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvUsername;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator revEmail;
		protected System.Web.UI.WebControls.Button btnReset;
		protected System.Web.UI.WebControls.Panel pnlReset;
		protected System.Web.UI.WebControls.Label lblConfirmation;
		protected System.Web.UI.WebControls.Panel pnlConfirmation;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (! this.IsPostBack)
			{
				// Databind is required to bind the localized resources.
				this.DataBind();
			}
			if (this.Page is GeneralPage)
			{
				this._page = (GeneralPage)this.Page;
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
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid)
			{
				// Check if the username and email combination exists.
				User user = this._page.CoreRepository.GetUserByUsernameAndEmail(this.txtUsername.Text, this.txtEmail.Text);
				if (user == null)
				{
					this.lblError.Text = GetText("RESETUSERERROR");
					this.lblError.Visible = true;
				}
				else
				{
					Site site = this._page.ActiveNode.Site;
					// OK, reset password
					string prevPassword = user.Password;
					string newPassword = user.GeneratePassword();
					this._page.CoreRepository.SaveObject(user);
					
					// Send email
					string subject = GetText("RESETEMAILSUBJECT").Replace("{site}", site.Name);
					string body = GetText("RESETEMAILBODY");
					body = body.Replace("{username}", user.UserName);
					body = body.Replace("{password}", newPassword);
					try
					{
						Util.Email.Send(user.Email, site.WebmasterEmail, subject, body);
					}
					catch
					{
						this.lblError.Text = GetText("RESETEMAILERROR");
						this.lblError.Visible = true;
					}

					this.pnlReset.Visible = false;
					this.pnlConfirmation.Visible = true;
					this.lblConfirmation.Text = String.Format(GetText("RESETCONFIRMATION"), user.Email);
				}
			}
		}
	}
}
