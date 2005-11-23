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
	///		Summary description for Register.
	/// </summary>
	public class Register : LocalizedUserControl
	{
		private GeneralPage _page;

		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.Panel pnlRegister;
		protected System.Web.UI.WebControls.Button btnRegister;
		protected System.Web.UI.WebControls.Label lblConfirmation;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvUsername;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator revEmail;
		protected System.Web.UI.WebControls.Label lblError;
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
			this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnRegister_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid)
			{
				// Check if username already exists.
				if (this._page.CoreRepository.FindUsersByUsername(this.txtUsername.Text).Count > 0)
				{
					this.lblError.Text = String.Format(GetText("USEREXISTS"), this.txtUsername.Text);
					this.lblError.Visible = true;
				}
				else
				{
					Site site = this._page.ActiveNode.Site;
					// OK, create new user.
					User user = new User();
					user.UserName = txtUsername.Text;
					user.Email = txtEmail.Text;
					user.IsActive = true;
					string newPassword = user.GeneratePassword();
					// Add the default role from the current site.
					user.Roles.Add(site.DefaultRole);
					this._page.CoreRepository.SaveObject(user);
					
					// Send email
					string subject = GetText("REGISTEREMAILSUBJECT").Replace("{site}", site.Name);
					string body = GetText("REGISTEREMAILBODY");
					body = body.Replace("{site}", site.Name + " (" + site.SiteUrl + ")");
					body = body.Replace("{username}", user.UserName);
					body = body.Replace("{password}", newPassword);
					try
					{
						Util.Email.Send(user.Email, site.WebmasterEmail, subject, body);
						this.pnlConfirmation.Visible = true;
						this.lblConfirmation.Text = String.Format(GetText("REGISTERCONFIRMATION"), user.Email);
					}
					catch
					{
						// delete user when sending email fails.
						this._page.CoreRepository.DeleteObject(user);
						this.lblError.Text = GetText("REGISTEREMAILERROR");
						this.lblError.Visible = true;
					}					
					this.pnlRegister.Visible = false;
				}
			}
		}
	}
}
