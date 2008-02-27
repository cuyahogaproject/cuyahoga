using System;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Admin.UI;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for RoleEdit.
	/// </summary>
	public class RoleEdit : AdminBasePage
	{
		private Role _activeRole;
		private IUserService _userService;

		protected TextBox txtName;
		protected RequiredFieldValidator rfvName;
		protected CheckBoxList cblRights;
		protected Button btnCancel;
		protected Button btnDelete;
		protected Button btnSave;

		private void Page_Load(object sender, EventArgs e)
		{
			this._userService = IoC.Resolve<IUserService>();

			this.Title = "Edit role";

			if (Context.Request.QueryString["RoleId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["RoleId"]) == -1)
				{
					this._activeRole = new Role();
				}
				else
				{
					this._activeRole = (Role)base.CoreRepository.GetObjectById(typeof(Role)
						, Int32.Parse(Context.Request.QueryString["RoleId"]));			
				}

				if (! this.IsPostBack)
				{
					BindRoleControls();
					BindRights();
				}
			}	
		}

		private void BindRoleControls()
		{
			this.txtName.Text = this._activeRole.Name;
			this.btnDelete.Visible = (this._activeRole.Id > 0);
			this.btnDelete.Attributes.Add("onclick", "return confirm(\"Ary you sure?\")");
		}

		private void BindRights()
		{
			this.cblRights.DataSource = this._userService.GetAllRights();
			this.cblRights.DataValueField = "Id";
			this.cblRights.DataTextField = "Name";
			this.cblRights.DataBind();
			foreach (Right right in this._activeRole.Rights)
			{
				ListItem li = cblRights.Items.FindByValue(right.Id.ToString());
				li.Selected = true;
			}
		}

		private void SetPermissions()
		{
			this._activeRole.Rights.Clear();
			foreach (ListItem listItem in this.cblRights.Items)
			{
				if (listItem.Selected)
				{
					int rightId = Int32.Parse(listItem.Value);
					this._activeRole.Rights.Add(this._userService.GetRightById(rightId));
				}
			}
		}

		private void SaveRole()
		{
			try
			{
				if (this._activeRole.Id == -1)
				{
					base.CoreRepository.SaveObject(this._activeRole);
					Context.Response.Redirect("Roles.aspx");
				}
				else
				{
					base.CoreRepository.UpdateObject(this._activeRole);
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

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Context.Response.Redirect("Roles.aspx");
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (this.IsValid)
			{
				this._activeRole.Name = txtName.Text;
				SetPermissions();
				if (this._activeRole.Rights.Count == 0)
				{
					ShowError("Please select one or more Right(s)");
				}
				else
				{
					SaveRole();
				}
			}	
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this._activeRole.Id > 0)
			{
				// Can't delete the Administrator role and the Anonymous role (which has a 
				// PermissionLevel of 1).
				// TODO: add an extra flag to determine if a role is a system role.
				if (this._activeRole.Name == Config.GetConfiguration()["AdministratorRole"]
					|| this._activeRole.PermissionLevel == 1)
				{
					ShowError("You can't delete the Administrator Role or the Anonymous Role.");
				}
				else
				{
					try
					{
						base.CoreRepository.DeleteObject(this._activeRole);
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
}
