namespace Cuyahoga.Modules.User
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Core.Service;
	using Cuyahoga.Core.Util;
	using Cuyahoga.Web.UI;

	/// <summary>
	///		Summary description for EditProfile.
	/// </summary>
	public class EditProfile : BaseModuleControl
	{
		private const string USER_CACHE_PREFIX = "User_";

		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label lblUsername;
		protected System.Web.UI.WebControls.TextBox txtFirstname;
		protected System.Web.UI.WebControls.TextBox txtLastname;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.TextBox txtWebsite;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Panel pnlEdit;
		protected System.Web.UI.WebControls.Label lblConfirmation;
		protected System.Web.UI.WebControls.TextBox txtNewPassword;
		protected System.Web.UI.WebControls.TextBox txtNewPasswordConfirmation;
		protected System.Web.UI.WebControls.TextBox txtCurrentPassword;
		protected System.Web.UI.WebControls.Button btnSavePassword;
		protected System.Web.UI.WebControls.Label lblInfo;
		protected System.Web.UI.WebControls.Panel pnlInfo;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator revEmail;
		protected System.Web.UI.WebControls.DropDownList ddlTimeZone;
		protected System.Web.UI.WebControls.Panel pnlConfirmation;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (! Context.User.Identity.IsAuthenticated)
			{
				ShowMessage(base.GetText("NOTAUTHENTICATED"));
			}
			else
			{
				if (! this.IsPostBack)
				{
					BindTimeZones();
					BindUser();
					// Databind is required to bind the localized resources.
					base.BindResources();
				}
			}
		}

		private void BindTimeZones()
		{
			this.ddlTimeZone.DataSource = TimeZoneUtil.GetTimeZones();
			this.ddlTimeZone.DataValueField = "Key";
			this.ddlTimeZone.DataTextField = "Value";
			this.ddlTimeZone.DataBind();
		}
		private void BindUser()
		{
			Cuyahoga.Core.Domain.User currentUser = Context.User.Identity as Cuyahoga.Core.Domain.User;
			this.lblUsername.Text = currentUser.UserName;
			this.txtFirstname.Text = currentUser.FirstName;
			this.txtLastname.Text = currentUser.LastName;
			this.txtEmail.Text = currentUser.Email;
			this.txtWebsite.Text = currentUser.Website;
			this.ddlTimeZone.Items.FindByValue(currentUser.TimeZone.ToString()).Selected = true;
		}

		private void ShowError(string msg)
		{
			this.lblError.Text = msg;
			this.lblError.Visible = true;
		}

		private void ShowMessage(string msg)
		{
			this.pnlEdit.Visible = false;
			this.pnlInfo.Visible = true;
			this.lblInfo.Text = msg;
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnSavePassword.Click += new System.EventHandler(this.btnSavePassword_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid)
			{
				Cuyahoga.Core.Domain.User currentUser = Context.User.Identity as Cuyahoga.Core.Domain.User;
				currentUser.FirstName = this.txtFirstname.Text;
				currentUser.LastName = this.txtLastname.Text;
				currentUser.Email = this.txtEmail.Text;
				currentUser.Website = this.txtWebsite.Text;
				currentUser.TimeZone = Int32.Parse(this.ddlTimeZone.SelectedValue);

				try
				{
					// Save user
					base.PageEngine.CoreRepository.UpdateObject(currentUser);
					// Remove old user from the cache
					HttpContext.Current.Cache.Remove(USER_CACHE_PREFIX + currentUser.Id.ToString());
					ShowMessage(GetText("EDITPROFILECONFIRMATION"));
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
		}

		private void btnSavePassword_Click(object sender, System.EventArgs e)
		{
			Cuyahoga.Core.Domain.User currentUser = Context.User.Identity as Cuyahoga.Core.Domain.User;
			try
			{
				// Validate passwords
				if (! Cuyahoga.Core.Domain.User.ValidatePassword(this.txtNewPassword.Text) 
					|| ! Cuyahoga.Core.Domain.User.ValidatePassword(this.txtCurrentPassword.Text))
				{
					ShowError(GetText("INVALIDPASSWORD"));
					return;
				}
				// Check current password.
				if (currentUser.Password != Cuyahoga.Core.Domain.User.HashPassword(this.txtCurrentPassword.Text))
				{
					ShowError(GetText("EDITPASSWORDCURRENTERROR"));
					return;
				}
				// Check if confirmation password is the same as the new password.
				if (this.txtNewPassword.Text != this.txtNewPasswordConfirmation.Text)
				{
					ShowError(GetText("EDITPASSWORDCONFIRMERROR"));
					return;
				}
				currentUser.Password = Cuyahoga.Core.Domain.User.HashPassword(this.txtNewPassword.Text);
				// Save user
				base.PageEngine.CoreRepository.UpdateObject(currentUser);
				// Remove old user from the cache
				HttpContext.Current.Cache.Remove(USER_CACHE_PREFIX + currentUser.Id.ToString());
				ShowMessage(GetText("EDITPASSWORDCONFIRMATION"));
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}
	}
}
