using System;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Admin.UI;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for Roles.
	/// </summary>
	public class Roles : AdminBasePage
	{
		protected Button btnNew;
		protected Repeater rptRoles;
	
		private void Page_Load(object sender, EventArgs e)
		{
			this.Title = "Roles";
			if (! this.IsPostBack)
			{
				BindRoles();
			}
		}

		private void BindRoles()
		{
			this.rptRoles.DataSource = base.CoreRepository.GetAll(typeof(Role), "PermissionLevel");
			this.rptRoles.DataBind();
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
			this.rptRoles.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptRoles_ItemDataBound);
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Role role = e.Item.DataItem as Role;
			if (role != null)
			{
                HyperLink hplEdit = (HyperLink)e.Item.FindControl("hplEdit");
                Image imgRole = (Image)e.Item.FindControl("imgRole");
                if (role.Name == "Administrator" || role.Name == "Anonymous user")
                {
                    imgRole.ImageUrl = "~/Admin/Images/lock.png";
                    hplEdit.Visible = false;
                }
                else
                {
                    imgRole.Visible = false;
                    // HACK: as long as ~/ doesn't work properly in mono we have to use a relative path from the Controls
                    // directory due to the template construction.
                    hplEdit.NavigateUrl = String.Format("~/Admin/RoleEdit.aspx?RoleId={0}", role.Id);
                }
                    // Permissions
                    Label lblRights = (Label)e.Item.FindControl("lblRights");
                    lblRights.Text = role.RightsString;
                    // Last update
                    Label lblLastUpdate = (Label)e.Item.FindControl("lblLastUpdate");
                    lblLastUpdate.Text = role.UpdateTimestamp.ToString();
			}
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			Context.Response.Redirect("RoleEdit.aspx?RoleId=-1");
		}
	}
}
