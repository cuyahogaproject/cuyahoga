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
using System.Text;

using Cuyahoga.Web.UI;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for Users.
	/// </summary>
	public class Users : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.Panel pnlResults;
		protected System.Web.UI.WebControls.Repeater rptUsers;
		protected Cuyahoga.ServerControls.Pager pgrUsers;
		protected System.Web.UI.WebControls.Button btnNew;
		protected System.Web.UI.WebControls.Button btnFind;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Users";
			this.pgrUsers.CacheVaryByParams = new string[1] {this.txtUsername.UniqueID};
		}

		private void BindUsers()
		{
			GetUserData();
			this.rptUsers.DataBind();
		}

		private void GetUserData()
		{
			this.rptUsers.DataSource = base.CoreRepository.FindUsersByUsername(this.txtUsername.Text);
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
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			this.rptUsers.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptUsers_ItemDataBound);
			this.pgrUsers.PageChanged += new Cuyahoga.ServerControls.PageChangedEventHandler(this.pgrUsers_PageChanged);
			this.pgrUsers.CacheEmpty += new System.EventHandler(this.pgrUsers_CacheEmpty);
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnFind_Click(object sender, System.EventArgs e)
		{
			BindUsers();	
		}

		private void pgrUsers_PageChanged(object sender, Cuyahoga.ServerControls.PageChangedEventArgs e)
		{
			//this.pgrUsers.CurrentPageIndex = e.CurrentPage;
			//BindUsers();
			this.rptUsers.DataBind();
		}

		private void pgrUsers_CacheEmpty(object sender, System.EventArgs e)
		{
			// The CacheEmpty event is raised when the pager can't find any cached data so 
			// the data has to be retrieved again and set as DataSource of the control that is
			// being paged.
			GetUserData();
		}

		private void rptUsers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			User user = e.Item.DataItem as User;

			if (user != null)
			{
				Label lblLastLogin = e.Item.FindControl("lblLastLogin") as Label;
				if (user.LastLogin != null)
				{
					lblLastLogin.Text = TimeZoneUtil.AdjustDateToUserTimeZone(user.LastLogin.Value, this.Page.User.Identity).ToString();
				}

				HyperLink hplEdit = (HyperLink)e.Item.FindControl("hplEdit");

				// HACK: as long as ~/ doesn't work properly in mono we have to use a relative path from the Controls
				// directory due to the template construction.
				hplEdit.NavigateUrl = String.Format("~/Admin/UserEdit.aspx?UserId={0}", user.Id);
			}
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("UserEdit.aspx?UserId=-1");
		}
	}
}
