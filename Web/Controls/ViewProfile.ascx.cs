namespace Cuyahoga.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Core.Service;
	using Cuyahoga.Web.UI;

	/// <summary>
	///		Summary description for Profile.
	/// </summary>
	public class ViewProfile : LocalizedUserControl
	{
		private int _userId;

		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.WebControls.Label lblUsername;
		protected System.Web.UI.WebControls.Label lblRegisteredOn;
		protected System.Web.UI.WebControls.Label lblFirstname;
		protected System.Web.UI.WebControls.Label lblLastLogin;
		protected System.Web.UI.WebControls.HyperLink hplWebsite;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label lblLastname;

		/// <summary>
		/// Property UserId (int)
		/// </summary>
		public int UserId
		{
			set { this._userId = value; }
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Page is GeneralPage && this._userId > 0)
			{
				GeneralPage page = (GeneralPage)this.Page;
				try
				{
					User user = (User)page.CoreRepository.GetObjectById(typeof(User), this._userId);
					this.litTitle.Text = String.Format(GetText("VIEWPROFILETITLE"), user.UserName);
					BindUser(user);
					if (! this.IsPostBack)
					{
						// Databind is required to bind the localized resources.
						this.DataBind();
					}
				}
				catch
				{
					this.lblError.Text = String.Format(GetText("USERNOTFOUND"), this._userId.ToString());
					this.lblError.Visible = true;
				}
			}
		}

		private void BindUser(User user)
		{
			this.lblUsername.Text = user.UserName;
			this.lblFirstname.Text = user.FirstName;
			this.lblLastname.Text = user.LastName;
			if (user.Website != null && user.Website != string.Empty)
			{
				this.hplWebsite.NavigateUrl = user.Website;
				this.hplWebsite.Text = user.Website;
			}
			this.lblRegisteredOn.Text = user.InsertTimestamp.ToShortDateString();
			if (user.LastLogin != null)
			{
				this.lblLastLogin.Text = user.LastLogin.ToString();
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
