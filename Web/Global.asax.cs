using System;
using System.IO;
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
			// Initialize log4net 
			XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/") + "config/logging.config"));

			// Initialize Cuyahoga environment
			
			// Set application level flags.
			HttpContext.Current.Application.Lock();
			HttpContext.Current.Application["ModulesLoaded"] = false;
			HttpContext.Current.Application["IsModuleLoading"] = false;
			HttpContext.Current.Application["IsInstalling"] = false;
			HttpContext.Current.Application["IsUpgrading"] = false;
			HttpContext.Current.Application.UnLock();

			try
			{
				// Initialize Windsor
				IWindsorContainer container = new CuyahogaContainer();
				container.Kernel.ComponentCreated += new ComponentInstanceDelegate(Kernel_ComponentCreated);
				container.Kernel.ComponentDestroyed += new ComponentInstanceDelegate(Kernel_ComponentDestroyed);

				// Inititialize the static Windsor helper class. 
				IoC.Initialize(container);

				// Add ICuyahogaContext to the container.
				container.AddComponentWithLifestyle("cuyahoga.context", typeof (ICuyahogaContext), typeof (CuyahogaContext),
				                                    LifestyleType.PerWebRequest);

				// Check for any new versions
				CheckInstaller();

				// Register modules
				ModuleLoader loader = Container.Resolve<ModuleLoader>();
				loader.RegisterActivatedModules();
			}
			catch (Exception ex)
			{
				log.Error("Error initializing application.", ex);
				throw;
			}

			// On app startup re-load the requested page (to avoid conflicts with first-time configured NHibernate modules )
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
		}

		protected void Session_Start(Object sender, EventArgs e)
		{

		}


		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			// Initialize CuyahogaContext.
			ICuyahogaContext cuyahogaContext = Container.Resolve<ICuyahogaContext>();
			cuyahogaContext.Initialize(HttpContext.Current);

			// Load active modules. This can't be done in Application_Start because the Installer might kick in
			// before modules are loaded.
			if (! (bool)HttpContext.Current.Application["ModulesLoaded"]
				&& !(bool)HttpContext.Current.Application["IsModuleLoading"]
				&& !(bool)HttpContext.Current.Application["IsInstalling"]
				&& !(bool)HttpContext.Current.Application["IsUpgrading"])
			{
				LoadModules();
			}
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
			IWindsorContainer container = IoC.Container;
			container.Kernel.ComponentCreated -= new ComponentInstanceDelegate(Kernel_ComponentCreated);
			container.Kernel.ComponentDestroyed -= new ComponentInstanceDelegate(Kernel_ComponentDestroyed);
			container.Dispose();
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
					HttpContext.Current.Application.Lock();
					HttpContext.Current.Application["IsUpgrading"] = true;
					HttpContext.Current.Application.UnLock();

					HttpContext.Current.Response.Redirect("~/Install/Upgrade.aspx");
				}
				else if (dbInstaller.CanInstall)
				{
					HttpContext.Current.Application.Lock();
					HttpContext.Current.Application["IsInstalling"] = true;
					HttpContext.Current.Application.UnLock();
					HttpContext.Current.Response.Redirect("~/Install/Install.aspx");
				}
			}
			else
			{
				throw new Exception("Cuyahoga can't connect to the database. Please check your application settings.");
			}
		}

		private void LoadModules()
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Entering module loading.");
			}
			// Load module types into the container.
			ModuleLoader loader = Container.Resolve<ModuleLoader>();
			loader.RegisterActivatedModules();
			
			if (log.IsDebugEnabled)
			{
				log.Debug("Finished module loading. Now redirecting to self.");
			}
			// Re-load the requested page (to avoid conflicts with first-time configured NHibernate modules )
			HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);

		}

		private void Kernel_ComponentCreated(ComponentModel model, object instance)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Component created: " + instance.ToString());
			}
		}

		private void Kernel_ComponentDestroyed(ComponentModel model, object instance)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Component destroyed: " + instance.ToString());
			}
		}
	}
}



