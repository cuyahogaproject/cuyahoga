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
using Cuyahoga.Core;

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
			// Generate dummy data and set it as the DataSource of the repeater.
			ArrayList userList = new ArrayList();
			int number = Encoding.ASCII.GetBytes(this.txtUsername.Text)[0];
			for (int i = 0; i < number; i++)
			{
				User user = new User();
				user.UserName = "User" + i.ToString();
				user.FirstName = "Firstname " + i.ToString();
				user.LastName = "Lastname " + i.ToString();
				user.Email = "User" + i.ToString() + "@cuyahoga.org";
				userList.Add(user);
			}
			this.pnlResults.Visible = true;
			this.rptUsers.DataSource = userList;
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
			this.pgrUsers.PageChanged += new Cuyahoga.ServerControls.PageChangedEventHandler(this.pgrUsers_PageChanged);
			this.pgrUsers.CacheEmpty += new System.EventHandler(this.pgrUsers_CacheEmpty);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnFind_Click(object sender, System.EventArgs e)
		{
			if (this.txtUsername.Text.Length > 0)
			{
				BindUsers();	
			}
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
	}
}
