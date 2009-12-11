using System.Web.Security;
using Cuyahoga.Core;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;
using System;
using System.Web;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Core.Domain;
using log4net;
using CuyahogaUser = Cuyahoga.Core.Domain.User;


namespace Cuyahoga.Modules.User
{
	/// <summary>
	///	Module to enable authentication and user sign-up etc.
	/// </summary>
	public partial class User : BaseModuleControl
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(User));

		private bool _showRegister = true;
		private bool _showResetPassword = true;
		private bool _showEditProfile = true;
		private UserModule _module;
		private IAuthenticationService _authenticationService;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as UserModule;
			this._authenticationService = IoC.Resolve<IAuthenticationService>();

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
			if (this.txtUsername.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
			{
				try
				{
					if (AuthenticateUser(this.txtUsername.Text, this.txtPassword.Text, this.chkPersistLogin.Checked))
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
			if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
			{
				FormsAuthentication.SignOut();
			}
			this.pnlLogin.Visible = true;
			this.pnlUserInfo.Visible = false;
			// Redirect to self to refresh rendering of the page because this event happens after 
			// everything is already constructed.
			Context.Response.Redirect(Context.Request.RawUrl);
		}

		private bool AuthenticateUser(string username, string password, bool persistLogin)
		{
			try
			{
				CuyahogaUser user =
					this._authenticationService.AuthenticateUser(username, password, HttpContext.Current.Request.UserHostAddress);
				if (user != null)
				{
					if (!user.IsActive)
					{
						log.Warn(String.Format("Inactive user {0} tried to login.", user.UserName));
						throw new AccessForbiddenException("The account is disabled.");
					}
					// Create the authentication ticket
					HttpContext.Current.User = user;
					FormsAuthentication.SetAuthCookie(user.Name, persistLogin);

					return true;
				}
				else
				{
					log.Warn(String.Format("Invalid username-password combination: {0}:{1}.", username, password));
					return false;
				}
			}
			catch (Exception ex)
			{
				log.Error(String.Format("An error occured while logging in user {0}.", username));
				throw new Exception(String.Format("Unable to log in user '{0}': " + ex.Message, username), ex);
			}
			
		}
	}
}
