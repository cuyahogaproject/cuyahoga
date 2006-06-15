using System;

using Castle.MicroKernel;
using Castle.Model;

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
			string assemblyQualifiedName = section.ModuleType.ClassName + ", " + section.ModuleType.AssemblyName;
			Type moduleType = Type.GetType(assemblyQualifiedName);
			if (moduleType == null)
			{
				throw new Exception("Could not find module: " + assemblyQualifiedName);
			}
			else
			{
				ModuleBase module = GetModuleFromType(moduleType);
				module.Section = section;
				module.SectionUrl = UrlHelper.GetUrlFromSection(section);
				module.ReadSectionSettings();

				return module;
			}
		}

		/// <summary>
		/// Get a module instance of a given type.
		/// </summary>
		/// <param name="moduleType"></param>
		/// <returns></returns>
		public ModuleBase GetModuleFromType(Type moduleType)
		{
			ModuleBase module = null;

			if (!this._kernel.HasComponent(moduleType))
			{
				// Module is not registered, do it now.
				this._kernel.AddComponent("module." + moduleType.FullName, moduleType);

				// Retrieve a fresh instance of the registered module and call RegisterComponents()
				// to let the module register its own components.
				module = this._kernel[moduleType] as ModuleBase;
				module.RegisterComponents();

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
