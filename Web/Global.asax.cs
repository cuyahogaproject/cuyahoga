using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Reflection;

using log4net;
using Castle.Windsor;

using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Util;
using Cuyahoga.Web.Components;

namespace Cuyahoga.Web
{
	public class Global : System.Web.HttpApplication, IContainerAccessor
	{
		private static ILog log = LogManager.GetLogger(typeof(Global));
		private static readonly string ERROR_PAGE_LOCATION = "~/Error.aspx";
		private static CuyahogaContainer _cuyahogaContainer;

		/// <summary>
		/// Obtain the container.
		/// </summary>
		public IWindsorContainer Container
		{
			get { return _cuyahogaContainer; }
		}

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			log4net.Config.XmlConfigurator.Configure();
			_cuyahogaContainer = new CuyahogaContainer();
			_cuyahogaContainer.Kernel.ComponentCreated += new Castle.MicroKernel.ComponentInstanceDelegate(Kernel_ComponentCreated);
			_cuyahogaContainer.Kernel.ComponentDestroyed += new Castle.MicroKernel.ComponentInstanceDelegate(Kernel_ComponentDestroyed);
			CheckInstaller();
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
			_cuyahogaContainer.Kernel.ComponentCreated -= new Castle.MicroKernel.ComponentInstanceDelegate(Kernel_ComponentCreated);
			_cuyahogaContainer.Kernel.ComponentDestroyed -= new Castle.MicroKernel.ComponentInstanceDelegate(Kernel_ComponentDestroyed);
			_cuyahogaContainer.Dispose();
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

		private void CheckInstaller()
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

		private void Kernel_ComponentCreated(Castle.Core.ComponentModel model, object instance)
		{
			log.Debug("Component created: " + instance.ToString());
		}

		private void Kernel_ComponentDestroyed(Castle.Core.ComponentModel model, object instance)
		{
			log.Debug("Component destroyed: " + instance.ToString());
		}
	}
}


