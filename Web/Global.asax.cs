using System;
using System.Reflection;
using System.Web;
using Castle.Core;
using Castle.MicroKernel;
using Castle.Windsor;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Components;
using log4net;
using log4net.Config;

namespace Cuyahoga.Web
{
	public class Global : HttpApplication, IContainerAccessor
	{
		private static ILog log = LogManager.GetLogger(typeof(Global));
		private static readonly string ERROR_PAGE_LOCATION = "~/Error.aspx";

		/// <summary>
		/// Obtain the container.
		/// </summary>
		public IWindsorContainer Container
		{
			get { return IoC.Container; }
		}

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			XmlConfigurator.Configure();
			IWindsorContainer container = new CuyahogaContainer();
			container.Kernel.ComponentCreated += new ComponentInstanceDelegate(Kernel_ComponentCreated);
			container.Kernel.ComponentDestroyed += new ComponentInstanceDelegate(Kernel_ComponentDestroyed);
			
			IoC.Initialize(container);
			CheckInstaller();

            ModuleLoader loader = Container.Resolve<ModuleLoader>();
            loader.RegisterActivatedModules();

            //on app startup re-load the requested page (to avoid conflicts with first-time configured NHibernate modules )
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
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
			Container.Kernel.ComponentCreated -= new ComponentInstanceDelegate(Kernel_ComponentCreated);
			Container.Kernel.ComponentDestroyed -= new ComponentInstanceDelegate(Kernel_ComponentDestroyed);
			Container.Dispose();
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

		private void Kernel_ComponentCreated(ComponentModel model, object instance)
		{
			log.Debug("Component created: " + instance.ToString());
		}

		private void Kernel_ComponentDestroyed(ComponentModel model, object instance)
		{
			log.Debug("Component destroyed: " + instance.ToString());
		}
	}
}


