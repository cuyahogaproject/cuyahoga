using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for UserEdit.
	/// </summary>
	public class UserEdit : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		private User _activeUser;

		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.TextBox txtLastname;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.TextBox txtPassword1;
		protected System.Web.UI.WebControls.TextBox txtPassword2;
		protected System.Web.UI.WebControls.Repeater rptRoles;
		protected System.Web.UI.WebControls.RegularExpressionValidator revEmail;
		protected System.Web.UI.WebControls.CompareValidator covPassword;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnCancel;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.WebControls.Label lblUsername;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvUsername;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
		protected System.Web.UI.WebControls.CheckBox chkActive;
		protected System.Web.UI.WebControls.DropDownList ddlTimeZone;
		protected System.Web.UI.WebControls.TextBox txtWebsite;
		protected System.Web.UI.WebControls.TextBox txtFirstname;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Edit user";

			if (Context.Request.QueryString["UserId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["UserId"]) == -1)
				{
					// Create a new user instance
					this._activeUser = new User();
				}
				else
				{
					// Get user data
					this._activeUser = (Cuyahoga.Core.Domain.User)base.CoreRepository.GetObjectById(typeof(Cuyahoga.Core.Domain.User)
						, Int32.Parse(Context.Request.QueryString["UserId"]));
				}

				if (! this.IsPostBack)
				{
					BindTimeZones();
					BindUserControls();
					BindRoles();
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

		private void BindUserControls()
		{
			if (this._activeUser.Id > 0)
			{
				this.txtUsername.Visible = false;
				this.lblUsername.Text = this._activeUser.UserName;
				this.lblUsername.Visible = true;
				this.rfvUsername.Enabled = false;
			}
			else
			{
				this.txtUsername.Text = this._activeUser.UserName;
				this.txtUsername.Visible = true;
				this.lblUsername.Visible = false;
				this.rfvUsername.Enabled = true;
			}
			this.txtFirstname.Text = this._activeUser.FirstName;
			this.txtLastname.Text = this._activeUser.LastName;
			this.txtEmail.Text = this._activeUser.Email;
			this.txtWebsite.Text = this._activeUser.Website;
			this.ddlTimeZone.Items.FindByValue(this._activeUser.TimeZone.ToString()).Selected = true;
			this.chkActive.Checked = this._activeUser.IsActive;
			this.btnDelete.Visible = (this._activeUser.Id > 0);
			this.btnDelete.Attributes.Add("onclick", "return confirmDeleteUser();");
		}

		private void BindRoles()
		{
			IList roles = base.CoreRepository.GetAll(typeof(Role), "PermissionLevel");
			FilterAnonymousRoles(roles);
			this.rptRoles.ItemDataBound += new RepeaterItemEventHandler(rptRoles_ItemDataBound);
			this.rptRoles.DataSource = roles;
			this.rptRoles.DataBind();
		}

		/// <summary>
		/// Filter the anonymous roles from the list.
		/// </summary>
		private void FilterAnonymousRoles(IList roles)
		{
			int roleCount = roles.Count;
			for (int i = roleCount -1; i >= 0; i--)
			{
				Role role = (Role)roles[i];
				if (role.PermissionLevel == (int)AccessLevel.Anonymous)
				{
					roles.Remove(role);
				}
			}
		}

		private void SetRoles()
		{
			this._activeUser.Roles.Clear();

			foreach (RepeaterItem ri in rptRoles.Items)
			{	
				// HACK: RoleId is stored in the ViewState because the repeater doesn't have a DataKeys property.
				// Another HACK: we're only using the role id's to save database roundtrips.
				CheckBox chkRole = (CheckBox)ri.FindControl("chkRole");
				if (chkRole.Checked)
				{
					int roleId = (int)this.ViewState[ri.ClientID];
					Role role = (Role)base.CoreRepository.GetObjectById(typeof(Role), roleId);
					this._activeUser.Roles.Add(role);
				}
			}

			// Check if the Adminstrator role is not accidently deleted for the superuser.
			string adminRole = Config.GetConfiguration()["AdministratorRole"];
			if (this._activeUser.UserName == Config.GetConfiguration()["SuperUser"]
				&& ! this._activeUser.IsInRole(adminRole))
			{
				throw new Exception(String.Format("The user '{0}' has to have the '{1}' role."
					, this._activeUser.UserName, adminRole));
			}
		}

		private void SaveUser()
		{
			try
			{
				SetRoles();
				if (this._activeUser.Id == -1)
				{
					base.CoreRepository.SaveObject(this._activeUser);
					Context.Response.Redirect("UserEdit.aspx?UserId=" + this._activeUser.Id + "&message=User created successfully");
				}
				else
				{
					base.CoreRepository.UpdateObject(this._activeUser);
					ShowMessage("User saved");
				}
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("Users.aspx");
		}

		private void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Role role = e.Item.DataItem as Role;
			if (role != null)
			{
				CheckBox chkRole = (CheckBox)e.Item.FindControl("chkRole");
				chkRole.Checked = this._activeUser.IsInRole(role);
				// Add RoleId to the ViewState with the ClientID of the repeateritem as key.
				this.ViewState[e.Item.ClientID] = role.Id;
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				if (this._activeUser.Id == -1)
				{
					this._activeUser.UserName = this.txtUsername.Text;
				}
				else
				{
					this._activeUser.UserName = this.lblUsername.Text;
				}
				if (this.txtFirstname.Text.Length > 0)
				{
					this._activeUser.FirstName = this.txtFirstname.Text;
				}
				if (this.txtLastname.Text.Length > 0)
				{
					this._activeUser.LastName = this.txtLastname.Text;
				}
				this._activeUser.Email = this.txtEmail.Text;
				this._activeUser.Website = this.txtWebsite.Text;
				this._activeUser.IsActive = this.chkActive.Checked;
				this._activeUser.TimeZone = Int32.Parse(this.ddlTimeZone.SelectedValue);
				
				if (this.txtPassword1.Text.Length > 0 && this.txtPassword2.Text.Length > 0)
				{
					try
					{
						this._activeUser.Password = Core.Domain.User.HashPassword(this.txtPassword1.Text);
					}
					catch (Exception ex)
					{
						ShowError(ex.Message);
					}
				}

				if (this._activeUser.Id == -1 && this._activeUser.Password == null)
				{
					ShowError("Password is required");
				}
				else
				{
					SaveUser();
				}
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._activeUser.Id > 0)
			{
				if (this._activeUser.Id != ((Cuyahoga.Core.Domain.User)this.Page.User.Identity).Id)
				{
					if (this._activeUser.UserName != Config.GetConfiguration()["SuperUser"])
					{
						try
						{
							base.CoreRepository.DeleteObject(this._activeUser);
							Context.Response.Redirect("Users.aspx");
						}
						catch (Exception ex)
						{
							ShowError(ex.Message);
						}
					}
					else
					{
						ShowError("You can't delete the superuser.");
					}
				}
				else
				{
					ShowError("You can't delete yourself.");
				}
			}
		}
	}
}
