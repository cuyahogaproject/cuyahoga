using System;

using Castle.MicroKernel;
using Castle.Core;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Web.Util;
using System.Collections.Generic;
using System.Web;

namespace Cuyahoga.Web.Components
{
	/// <summary>
	/// Responsible for loading module assemblies and registering these in the application.
	/// </summary>
	public class ModuleLoader
	{
		private IKernel _kernel;
		private SessionFactoryHelper _sessionFactoryHelper;
		private bool _redirectSuspended;
		private System.Threading.ReaderWriterLock _getModulesLock = new System.Threading.ReaderWriterLock();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="kernel"></param>
		/// <param name="sessionFactoryHelper"></param>
		public ModuleLoader(IKernel kernel, SessionFactoryHelper sessionFactoryHelper)
		{
			this._kernel = kernel;
			this._sessionFactoryHelper = sessionFactoryHelper;
		}

		/// <summary>
		/// Get the module instance that is associated with the given section.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public ModuleBase GetModuleFromSection(Section section)
		{
			ModuleBase module = GetModuleFromType(section.ModuleType);
			module.Section = section;
			module.SectionUrl = UrlHelper.GetUrlFromSection(section);
			module.ReadSectionSettings();

			return module;
		}

		/// <summary>
		/// Get a module instance of a given type.
		/// </summary>
		/// <param name="moduleTypeMetaData"></param>
		/// <returns></returns>
		public ModuleBase GetModuleFromType(ModuleType moduleTypeMetaData)
		{
			_getModulesLock.AcquireReaderLock(-1);
			try
			{
				string assemblyQualifiedName = moduleTypeMetaData.ClassName + ", " + moduleTypeMetaData.AssemblyName;
				// First, try to get the CLR module type
				Type moduleType = Type.GetType(assemblyQualifiedName);
				if (moduleType == null)
				{
					throw new Exception("Could not find module: " + assemblyQualifiedName);
				}

				ModuleBase module = null;

				if (!this._kernel.HasComponent(moduleType))
				{
					module = AddModuleToContainer(moduleType, moduleTypeMetaData);
				}
				else
				{
					// Kernel 'knows' the module, return a fresh instance (ModuleBase is transient).
					module = this._kernel[moduleType] as ModuleBase;
				}

				// Add module to the list of loaded modules, so the page handler can clean up afterwards.
				List<ModuleBase> loadedModules = HttpContext.Current.Items["LoadedModules"] as List<ModuleBase>;
				if (loadedModules != null)
				{
					loadedModules.Add(module);
				}

				return module;
			}
			finally
			{
				_getModulesLock.ReleaseReaderLock();
			}
		}

		private ModuleBase AddModuleToContainer(Type moduleType, ModuleType moduleTypeMetaData)
		{
			_getModulesLock.UpgradeToWriterLock(-1);
			try
			{
				ModuleBase module;
				//Someone may have snuck and added the module between the time we released the read
				//lock and acquired the write lock
				if (this._kernel.HasComponent(moduleType))
				{
					return this._kernel[moduleType] as ModuleBase;	
				}
				// Module is not registered, do it now.

				// First, register optional module services that the module might depend on.
				foreach (ModuleService moduleService in moduleTypeMetaData.ModuleServices)
				{
					Type serviceType = Type.GetType(moduleService.ServiceType);
					Type classType = Type.GetType(moduleService.ClassType);
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

				// Register the module
				this._kernel.AddComponent("module." + moduleType.FullName, moduleType);

				// Retrieve a fresh instance of the registered module.
				module = this._kernel[moduleType] as ModuleBase;

				if (typeof(INHibernateModule).IsAssignableFrom(moduleType))
				{
					// Module needs its NHibernate mappings registered.
					this._sessionFactoryHelper.AddAssembly(moduleType.Assembly);
					if (!this._redirectSuspended)
					{
						RedirectAfterModuleAdded();
					}
				}
				return module;
			}
			finally
			{
				if(_getModulesLock.IsReaderLockHeld)
				{
					_getModulesLock.ReleaseWriterLock();
				}
			}
		}

		/// <summary>
		/// Suspend redirecting when a new NHibernate module is loaded.
		/// </summary>
		public void SuspendRedirect()
		{
			this._redirectSuspended = true;
		}

		/// <summary>
		/// Resume redirecting when a new NHibernate module is loaded.
		/// </summary>
		public void ResumeRedirect()
		{
			this._redirectSuspended = false;
		}

		private void RedirectAfterModuleAdded()
		{
			// A module that uses NHibernate was loaded for the first time.
			// Immediately redirect to the same page.
			// TODO: handle more elegantly?
			if (HttpContext.Current.Items["VirtualUrl"] != null)
			{
				HttpContext.Current.Response.Redirect(HttpContext.Current.Items["VirtualUrl"].ToString());
			}
			else
			{
				HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
			}
		}
	}
}
