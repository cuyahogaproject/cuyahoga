using System;

using Castle.MicroKernel;
using Castle.Core;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Components
{
	/// <summary>
	/// Responsible for loading module assemblies and registering these in the application.
	/// </summary>
	[Transient]
	public class ModuleLoader
	{
		private IKernel _kernel;
		private SessionFactoryHelper _sessionFactoryHelper;

		/// <summary>
		/// Fires when a module was registered for the first time.
		/// </summary>
		public event EventHandler ModuleAdded;

		protected void OnNHibernateModuleAdded()
		{
			if (ModuleAdded != null)
			{
				ModuleAdded(this, EventArgs.Empty);
			}
		}

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
		/// <param name="moduleType"></param>
		/// <returns></returns>
		public ModuleBase GetModuleFromType(ModuleType moduleTypeMetaData)
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
					OnNHibernateModuleAdded();
				}
			}
			else
			{
				// Kernel 'knows' the module, return a fresh instance (ModuleBase is transient).
				module = this._kernel[moduleType] as ModuleBase;
			}

			return module;
		}
	}
}
