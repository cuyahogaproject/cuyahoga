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

using log4net;

using Cuyahoga.Core;

namespace Cuyahoga.Web
{
	/// <summary>
	/// Summary description for Error.
	/// </summary>
	public class Error : System.Web.UI.Page
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Error));

		protected System.Web.UI.WebControls.Label lblTitle;
		protected System.Web.UI.WebControls.Label lblError;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			Exception ex = Server.GetLastError();
			if (ex != null)
			{
				log.Error("An unexpected error occured", ex);
				Exception innerException = ex.InnerException;
				if (innerException != null)
				{
					if (innerException is SiteNullException)
					{
						this.lblTitle.Text = innerException.Message;
						this.lblError.Text = "The url you entered is invalid or the site is down for maintenance.";
					}
					else
					{
						this.lblTitle.Text = "An error occured:";
						this.lblError.Text = innerException.Message;
					}
				}
				else
				{
					this.lblTitle.Text = "An error occured:";
					this.lblError.Text = ex.Message;
				}
			}
			else
			{
				this.lblTitle.Text = "Something strange happened...";
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
