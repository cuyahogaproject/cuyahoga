namespace Cuyahoga.Modules.User
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Web.UI;

	/// <summary>
	///		Summary description for Profile.
	/// </summary>
	public partial class ViewProfile : BaseModuleControl
	{
		private ProfileModule _module;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as ProfileModule;
			if (this._module.CurrentUserId > 0)
			{
				Cuyahoga.Core.Domain.User user = this._module.GetUserById(this._module.CurrentUserId);
				if (user != null)
				{
					this.litTitle.Text = String.Format(GetText("VIEWPROFILETITLE"), user.UserName);
					BindUser(user);
					if (!this.IsPostBack)
					{
						// Databind is required to bind the localized resources.
						this.DataBind();
					}
				}
				else
				{
					this.lblError.Text = String.Format(GetText("USERNOTFOUND"), this._module.CurrentUserId.ToString());
					this.lblError.Visible = true;
				}
			}
		}

		private void BindUser(Cuyahoga.Core.Domain.User user)
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

		}
		#endregion
	}
}
