using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Reflection;
using System.Diagnostics;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DAL;

namespace Cuyahoga.Core
{
	/// <summary>
	/// Get Module instance based on concrete classname and assemblyname.
	/// </summary>
	public class ModuleFactory
	{
		public static Cuyahoga.Core.Domain.Module GetInstance(string assemblyName, string className)
		{
			// HACK: added 'bin/' to the path because I couldn't find a more decent way to get the physical
			//       path of the bin directory. 
			String assemblyPath = AppDomain.CurrentDomain.BaseDirectory + "bin/" + assemblyName;

			// Use reflection to store the constructor of the module
			try 
			{
				ConstructorInfo	moduleConstructor = Assembly.LoadFrom(assemblyPath).GetType(className).GetConstructor(new Type[0]);
				// return an instance of the desired module
				return (Cuyahoga.Core.Domain.Module)(((ConstructorInfo)moduleConstructor).Invoke(null));
			}
			catch (Exception ex) 
			{
				throw ex;
			}
		}

		public static Cuyahoga.Core.Domain.Module GetNewInstanceFromCache(string className)
		{
			Hashtable modules = GetModulesFromCache();
			if (modules != null)
			{
				// return a new instance of the desired module that already is in the cache.
                return (Cuyahoga.Core.Domain.Module)Activator.CreateInstance(modules[className].GetType());                				
			}
			else
			{
				// Nope, unable to create instance.
				return null;
			}
		}

		public static Cuyahoga.Core.Domain.Module GetNewInstanceFromCache(int moduleId)
		{
            Hashtable modules = GetModulesFromCache();
			foreach (DictionaryEntry moduleEntry in modules)
			{
				Cuyahoga.Core.Domain.Module module = (Cuyahoga.Core.Domain.Module)moduleEntry.Value;
				if (module.ModuleId.Equals(moduleId))
				{
					// Found, create a new instance and fill the Id.
					Cuyahoga.Core.Domain.Module newModule = (Cuyahoga.Core.Domain.Module)Activator.CreateInstance(module.GetType());
					newModule.ModuleId = moduleId;
					return newModule;
				}
			}
			return null;
		}

		private static Hashtable GetModulesFromCache()
		{
			if (HttpContext.Current.Cache["Modules"] == null)
			{
				// Cache seems to be empty, fill it
				// HACK: Should the caching be a task of the DAL, or should it occur here?
				ICmsDataProvider dp = CmsDataFactory.GetInstance();
				dp.ReadAndCacheAllModules();
			}
            return (Hashtable)HttpContext.Current.Cache["Modules"];
		}
	}
}
