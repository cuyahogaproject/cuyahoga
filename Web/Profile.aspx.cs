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
			// Put user code to initialize the page here
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
						break;
					case "edit":
						ctrl = this.LoadControl("~/Controls/EditProfile.ascx");
						break;
					case "register":
						ctrl = this.LoadControl("~/Controls/Register.ascx");
						break;
					case "reset":
						ctrl = this.LoadControl("~/Controls/ResetPassword.ascx");
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
