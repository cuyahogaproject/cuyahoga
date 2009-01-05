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
	using Cuyahoga.Web.HttpModules;
	using Cuyahoga.Core.Domain;

	/// <summary>
	///	Module to enable authentication and user sign-up etc.
	/// </summary>
	public partial class User : BaseModuleControl
	{
		private bool _showRegister = true;
		private bool _showResetPassword = true;
		private bool _showEditProfile = true;
		private UserModule _module;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as UserModule;
			if (this._module.Section.Settings["SHOW_REGISTER"] != null)
			{
				this._showRegister = Convert.ToBoolean(this._module.Section.Settings["SHOW_REGISTER"]);
			}
			if (this._module.Section.Settings["SHOW_RESET_PASSWORD"] != null)
			{
				this._showResetPassword = Convert.ToBoolean(this._module.Section.Settings["SHOW_RESET_PASSWORD"]);
			}
			if (this._module.Section.Settings["SHOW_EDIT_PROFILE"] != null)
			{
				this._showEditProfile = Convert.ToBoolean(this._module.Section.Settings["SHOW_EDIT_PROFILE"]);
			}

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
				this.hplRegister.Visible = this._showRegister;
				this.hplResetPassword.Visible = this._showResetPassword;
				this.hplEdit.Visible = this._showEditProfile;
				BindLinks();
				Translate();
			}

			// Register default buttons.
			DefaultButton.SetDefault(this.Page, this.txtUsername, this.btnLogin);
			DefaultButton.SetDefault(this.Page, this.txtPassword, this.btnLogin);			
		}

		private void BindLinks()
		{
			// Keep static links to Profile.aspx in case there are no connections for
			// backward compatibility.
			Section sectionTo = this._module.Section.Connections["Register"] as Section;
			if (sectionTo != null)
			{
				this.hplRegister.NavigateUrl = UrlHelper.GetUrlFromSection(sectionTo) + "/Register";
			}
			else
			{
				this.hplRegister.NavigateUrl = this.Page.ResolveUrl("~/Profile.aspx/register");
			}
			sectionTo = this._module.Section.Connections["ResetPassword"] as Section;
			if (sectionTo != null)
			{
				this.hplResetPassword.NavigateUrl = UrlHelper.GetUrlFromSection(sectionTo) + "/ResetPassword";
			}
			else
			{
				this.hplResetPassword.NavigateUrl = this.Page.ResolveUrl("~/Profile.aspx/reset");
			}
			sectionTo = this._module.Section.Connections["EditProfile"] as Section;
			if (sectionTo != null)
			{
				this.hplEdit.NavigateUrl = UrlHelper.GetUrlFromSection(sectionTo) + "/EditProfile";
			}
			else
			{
				this.hplEdit.NavigateUrl = this.Page.ResolveUrl("~/Profile.aspx/edit");
			}
		}

		private void Translate()
		{
			this.lblUsername.Text = base.GetText("USERNAME");
			this.lblPassword.Text = base.GetText("PASSWORD");
			this.lblLoggedInText.Text = base.GetText("LOGGEDINTEXT");
			this.btnLogin.Text = base.GetText("LOGIN");
			this.btnLogout.Text = base.GetText("LOGOUT");	
			this.hplRegister.Text = base.GetText("REGISTER");
			this.hplResetPassword.Text = base.GetText("RESET");
			this.hplEdit.Text = base.GetText("EDITACCOUNT");
			this.chkPersistLogin.Text = base.GetText("PERSISTLOGIN");
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

		

		protected void btnLogin_Click(object sender, System.EventArgs e)
		{
			AuthenticationModule am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
			if (this.txtUsername.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
			{
				try
				{
					if (am.AuthenticateUser(this.txtUsername.Text, this.txtPassword.Text, this.chkPersistLogin.Checked))
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

		protected void btnLogout_Click(object sender, System.EventArgs e)
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
