namespace Cuyahoga.Modules.User
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Threading;
	using System.Globalization;

	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Core;

	/// <summary>
	///	Module to enable authentication and user sign-up etc.
	/// </summary>
	public class User : BaseModuleControl
	{
		protected System.Web.UI.WebControls.Panel pnlLogin;
		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.Button btnLogin;
		protected System.Web.UI.WebControls.Label lblLoginError;
		protected System.Web.UI.WebControls.Label lblUsername;
		protected System.Web.UI.WebControls.Button btnLogout;
		protected System.Web.UI.WebControls.Label lblLoggedInText;
		protected System.Web.UI.WebControls.Label lblLoggedInUser;
		protected System.Web.UI.WebControls.Label lblPassword;
		protected System.Web.UI.WebControls.Panel pnlUserInfo;

		private void Page_Load(object sender, System.EventArgs e)
		{
			bool isAuthenticated = false;
			if (Context.User != null)
			{
				isAuthenticated = Context.User.Identity.IsAuthenticated;
			}
			if (! this.IsPostBack)
			{
				if (isAuthenticated)
				{
					this.lblLoggedInUser.Text = ((Cuyahoga.Core.Domain.User)Context.User.Identity).UserName;
				}

				this.pnlLogin.Visible = ! isAuthenticated;
				this.pnlUserInfo.Visible = isAuthenticated;
				Translate();
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
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Translate()
		{
			this.lblUsername.Text = base.GetText("USERNAME");
			this.lblPassword.Text = base.GetText("PASSWORD");
			this.lblLoggedInText.Text = base.GetText("LOGGEDINTEXT");
			this.btnLogin.Text = base.GetText("LOGIN");
			this.btnLogout.Text = base.GetText("LOGOUT");			
		}

		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			AuthenticationModule am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
			if (this.txtUsername.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
			{
				try
				{
					if (am.AuthenticateUser(this.txtUsername.Text, this.txtPassword.Text))
					{
						this.lblLoggedInUser.Text = this.txtUsername.Text;
						this.pnlUserInfo.Visible = true;
						this.pnlLogin.Visible = false;
					}
					else
					{
						this.lblLoginError.Text = base.GetText("USERNAMEPASSWORDERROR");
					}
				}
				catch (Exception ex)
				{
					this.lblLoginError.Text = base.GetText("LOGINERROR") + " " + ex.Message;
				}
			}
			else
			{
				this.lblLoginError.Text = base.GetText("USERNAMEPASSWORDMISSING");
			}

			if (this.lblLoginError.Text.Length > 0)
			{
				this.lblLoginError.Visible = true;
			}
			else
			{
				// Redirect to self to refresh rendering of the page because this event happens after 
				// everything is already constructed.
				Context.Response.Redirect(Context.Request.RawUrl);
			}
		}

		private void btnLogout_Click(object sender, System.EventArgs e)
		{
			// Log out
			AuthenticationModule am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
            am.Logout();
			this.pnlLogin.Visible = true;
			this.pnlUserInfo.Visible = false;
			// Redirect to self to refresh rendering of the page because this event happens after 
			// everything is already constructed.
			Context.Response.Redirect(Context.Request.RawUrl);
		}
	}
}
