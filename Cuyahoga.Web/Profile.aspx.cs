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

using Cuyahoga.Web.UI;

namespace Cuyahoga.Web
{
	/// <summary>
	/// Summary description for Profile.
	/// </summary>
	public class Profile : GeneralPage
	{
		protected System.Web.UI.WebControls.Panel pnlControl;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		private void InitPage()
		{
			// Load the right user control based on the pathinfo.
			string pathInfo = HttpContext.Current.Request.PathInfo;
			string[] parameters = Util.UrlHelper.GetParamsFromPathInfo(pathInfo);
			if (parameters != null)
			{
				Control ctrl = null;
				string cmd = parameters[0];
				switch (cmd)
				{
					case "view":
						ctrl = this.LoadControl("~/Controls/ViewProfile.ascx");
						base.Title = "View profile";
						if (parameters.Length == 2)
						{
							// The second pathinfo parameter should be the UserId.
							int userId = Int32.Parse(parameters[1]);
							(ctrl as Cuyahoga.Web.Controls.ViewProfile).UserId = userId;
						}
						break;
					case "edit":
						ctrl = this.LoadControl("~/Controls/EditProfile.ascx");
						base.Title = "Edit profile";
						break;
					case "register":
						ctrl = this.LoadControl("~/Controls/Register.ascx");
						base.Title = "Register";
						break;
					case "reset":
						ctrl = this.LoadControl("~/Controls/ResetPassword.ascx");
						base.Title = "Reset password";
						break;
					default:
						throw new Exception("Invalid command found in pathinfo.");
				}

				this.pnlControl.Controls.Add(ctrl);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitPage();
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
