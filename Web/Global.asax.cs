using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Reflection;

using log4net;

using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web
{
	public class Global : System.Web.HttpApplication
	{
		private static readonly string ERROR_PAGE_LOCATION = "~/Error.aspx";

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			// Check version and redirect to install pages if neccessary.
			DatabaseInstaller dbInstaller = new DatabaseInstaller(HttpContext.Current.Server.MapPath("~/Install/Core"), Assembly.Load("Cuyahoga.Core"));
			if (dbInstaller.TestDatabaseConnection())
			{
				if (dbInstaller.CanUpgrade)
				{
					// Set a flag that the application is upgrading. This will redirect visitors to a
					// maintenance page.
					HttpContext.Current.Application.Lock();
					HttpContext.Current.Application["IsUpgrading"] = true;
					HttpContext.Current.Application.UnLock();

					HttpContext.Current.Response.Redirect("~/Install/Upgrade.aspx");
				}
				else if (dbInstaller.CanInstall)
				{
					HttpContext.Current.Response.Redirect("~/Install/Install.aspx");
				}
			}
			else
			{
				throw new Exception("Cuyahoga can't connect to the database. Please check your application settings.");
			}
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
			
		}
		
	
		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{
			if (Context != null && Context.IsCustomErrorEnabled)
			{
				Server.Transfer(ERROR_PAGE_LOCATION, false);
			}
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{
		
		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}


