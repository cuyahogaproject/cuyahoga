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

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DAL;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for RoleEdit.
	/// </summary>
	public class RoleEdit : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		private Role _activeRole;

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.CheckBoxList cblRoles;
		protected System.Web.UI.WebControls.Button btnCancel;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.WebControls.Button btnSave;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Edit role";

			if (Context.Request.QueryString["RoleId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["RoleId"]) == -1)
				{
					// Create a new user instance
					this._activeRole = new Role();
				}
				else
				{
					// Get user data
					this._activeRole = new Role(Int32.Parse(Context.Request.QueryString["RoleId"]));			
				}

				if (! this.IsPostBack)
				{
					BindRoleControls();
					BindPermissions();
				}
			}	
		}

		private void BindRoleControls()
		{
			this.txtName.Text = this._activeRole.Name;
			this.btnDelete.Visible = (this._activeRole.Id > 0);
			this.btnDelete.Attributes.Add("onClick", "return confirmDeleteRole();");
		}

		private void BindPermissions()
		{
			this.cblRoles.DataSource = Enum.GetValues(typeof(AccessLevel));
			this.cblRoles.DataBind();
			if (this._activeRole.Permissions != null)
			{
				foreach (AccessLevel accessLevel in this._activeRole.Permissions)
				{
					ListItem li = cblRoles.Items.FindByText(accessLevel.ToString());
					li.Selected = true;
				}
			}
		}

		private void SetPermissions()
		{
			int tmpLevel = 0;
			foreach (ListItem li in this.cblRoles.Items)
			{
				if (li.Selected)
				{
					tmpLevel += (int)(AccessLevel)Enum.Parse(typeof(AccessLevel), li.Text, true);
				}
			}
			if (tmpLevel > 0)
			{
				this._activeRole.PermissionLevel = (int)tmpLevel;
			}
		}

		private void SaveRole()
		{
			ICmsDataProvider dp = CmsDataFactory.GetInstance();
			try
			{
				if (this._activeRole.Id == -1)
				{
					dp.InsertRole(this._activeRole);
					Context.Response.Redirect("Roles.aspx");
				}
				else
				{
					dp.UpdateRole(this._activeRole);
					ShowMessage("Role saved");
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
			Context.Response.Redirect("Roles.aspx");
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				this._activeRole.Name = txtName.Text;
				SetPermissions();
				if (this._activeRole.PermissionLevel == -1)
				{
					ShowError("Please select one or more Permission(s)");
				}
				else
				{
					SaveRole();
				}
			}	
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._activeRole.Id > 0)
			{
				try
				{
					CmsDataFactory.GetInstance().DeleteRole(this._activeRole);
					Context.Response.Redirect("Roles.aspx");
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
		}

	}
}
