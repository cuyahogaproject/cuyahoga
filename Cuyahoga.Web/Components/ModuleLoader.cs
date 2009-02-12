using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using System.Collections;
using System.Web;

using Castle.Core;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Web.Mvc;
using log4net;
using UrlHelper=Cuyahoga.Web.Util.UrlHelper;


namespace Cuyahoga.Web.Components
{
	/// <summary>
	/// Responsible for loading module assemblies and registering these in the application.
	/// </summary>
	public class ModuleLoader
	{
		private static ILog log = LogManager.GetLogger(typeof(ModuleLoader));
		private IKernel _kernel;
		private SessionFactoryHelper _sessionFactoryHelper;
		private IModuleTypeService _moduleServie;
		//static object for thread synchronisation
		private static object lockObject = "";

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="kernel"></param>
		/// <param name="sessionFactoryHelper"></param>
		/// <param name="moduleService"></param>
		public ModuleLoader(IKernel kernel, SessionFactoryHelper sessionFactoryHelper, IModuleTypeService moduleService)
		{
			this._kernel = kernel;
			this._sessionFactoryHelper = sessionFactoryHelper;
			this._moduleServie = moduleService;
		}


		/// <summary>
		/// Get the module instance that is associated with the given section.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public ModuleBase GetModuleFromSection(Section section)
		{
			ModuleBase module = this.GetModuleFromType(section.ModuleType);
			if (module != null)
			{
				module.Section = section;
				if (HttpContext.Current != null)
				{
					module.SectionUrl = UrlHelper.GetUrlFromSection(section);
				}
				module.ReadSectionSettings();
			}
			return module;
		}

		/// <summary>
		/// Get the module instance by its type
		/// </summary>
		/// <param name="moduleType"></param>
		public ModuleBase GetModuleFromType(ModuleType moduleType)
		{
			string modulekey = string.Concat("module.", moduleType.ClassName);
			if (this._kernel.HasComponent(modulekey))
			{
				return (ModuleBase)this._kernel[modulekey];
			}
			else
			{
				if (log.IsDebugEnabled)
				{
					log.DebugFormat("Unable to find module with key '{0}' in the container.", modulekey);
				}
				return null;
			}
		}

		/// <summary>
		/// Checks if the module is represent in IoC Container configuration
		/// </summary>
		/// <param name="moduleTypeType"></param>
		/// <returns></returns>
		public bool IsModuleActive(ModuleType moduleTypeType)
		{
			return this._kernel.HasComponent(string.Concat("module.", moduleTypeType.ClassName));
		}

		/// <summary>
		/// Activate all modules that have the AutoActivate property set to true
		/// </summary>
		public void RegisterActivatedModules()
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Entering module loading.");
			}
			HttpContext.Current.Application.Lock();
			HttpContext.Current.Application["IsModuleLoading"] = true;
			HttpContext.Current.Application.UnLock();

			IList moduleTypes = this._moduleServie.GetAllModuleTypes();
			foreach (ModuleType mt in moduleTypes)
			{
				if (mt.AutoActivate)
				{
					ActivateModule(mt);
				}
			}

			// Let the application know that the modules are loaded.
			HttpContext.Current.Application.Lock();
			HttpContext.Current.Application["ModulesLoaded"] = true;
			HttpContext.Current.Application["IsModuleLoading"] = false;
			HttpContext.Current.Application.UnLock();
			if (log.IsDebugEnabled)
			{
				log.Debug("Finished module loading.");
			}
		}

		public void ActivateModule(ModuleType moduleType)
		{
			//only one thread at a time
			System.Threading.Monitor.Enter(lockObject);

			string assemblyQualifiedName = moduleType.ClassName + ", " + moduleType.AssemblyName;
			if (log.IsDebugEnabled)
			{
				log.DebugFormat("Loading module {0}.", assemblyQualifiedName);
			}
			// First, try to get the CLR module type
			Type moduleTypeType = Type.GetType(assemblyQualifiedName);
			if (moduleTypeType == null)
			{
				throw new Exception("Could not find module: " + assemblyQualifiedName);
			}
			try
			{
				// double check, if we should continue
				if (this._kernel.HasComponent(moduleTypeType))
				{
					// Module is already registered
					if (log.IsDebugEnabled)
					{
						log.DebugFormat("The module with type {0} is already registered in the container.", moduleTypeType.ToString());
					}
					return;
				}

				// First, register optional module services that the module might depend on.
				foreach (ModuleService moduleService in moduleType.ModuleServices)
				{
					Type serviceType = Type.GetType(moduleService.ServiceType);
					Type classType = Type.GetType(moduleService.ClassType);
					if (log.IsDebugEnabled)
					{
						log.DebugFormat("Loading module service {0}, (1).", moduleService.ServiceKey, moduleService.ClassType);
					}
					LifestyleType lifestyle = LifestyleType.Singleton;
					if (moduleService.Lifestyle != null)
					{
						try
						{
							lifestyle = (LifestyleType)Enum.Parse(typeof(LifestyleType), moduleService.Lifestyle);
						}
						catch (ArgumentException ex)
						{
							throw new Exception(String.Format("Unable to load module service {0} with invalid lifestyle {1}."
															  , moduleService.ServiceKey, moduleService.Lifestyle), ex);
						}
					}
					this._kernel.AddComponent(moduleService.ServiceKey, serviceType, classType, lifestyle);
				}

				//Register the module
				string moduleTypeKey = "module." + moduleTypeType.FullName;
				this._kernel.AddComponent(moduleTypeKey, moduleTypeType);

				//Configure NHibernate mappings and make sure we haven't already added this assembly to the NHibernate config
				if (typeof(INHibernateModule).IsAssignableFrom(moduleTypeType) && ((HttpContext.Current.Application[moduleType.AssemblyName]) == null))
				{
					if (log.IsDebugEnabled)
					{
						log.DebugFormat("Adding module assembly {0} to the NHibernate mappings.", moduleTypeType.Assembly.ToString());
					}
					this._sessionFactoryHelper.AddAssembly(moduleTypeType.Assembly);
					//set application variable to remember the configurated assemblies
					HttpContext.Current.Application.Lock();
					HttpContext.Current.Application[moduleType.AssemblyName] = moduleType.AssemblyName;
					HttpContext.Current.Application.UnLock();
				}
				// Add routes to the routetable if the module supports MVC
				if (typeof(IMvcModule).IsAssignableFrom(moduleTypeType))
				{
					IMvcModule module = this._kernel.Resolve<IMvcModule>(moduleTypeKey);
					module.RegisterRoutes(RouteTable.Routes);
					this._kernel.Register(AllTypes
						.Of(typeof(IController))
						.FromAssembly(moduleTypeType.Assembly)
						.Configure(c => c.LifeStyle.Transient));
				}
			}
			finally
			{
				System.Threading.Monitor.Exit(lockObject);
			}

		}//end method

	}
}

