using System;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Collections.Specialized;

using Cuyahoga.Core;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Modules.DAL
{
	/// <summary>
	/// Summary description for ModuleDataFactory.
	/// </summary>
	public class ModulesDataProvider
	{
		private ModulesDataProvider()
		{
		}

		public static IModulesDataProvider GetInstance() 
		{
			Cache cache = System.Web.HttpContext.Current.Cache;

			if ( cache["IModulesDataProvider"] == null ) 
			{
				NameValueCollection context = Config.GetConfiguration();

				String assemblyPath = context["ModuleDataProviderAssembly"];
				String className = context["ModuleDataProviderClassName"];

				if (assemblyPath.Substring(0,1) == "/") 
				{
					// assemblyPath presented in virtual form, must convert to physical path
					assemblyPath = HttpContext.Current.Server.MapPath(assemblyPath);					
				}

				// Use reflection to store the constructor of the class that implements IModulesDataProvider
				try 
				{
					cache.Insert("IModulesDataProvider", Assembly.LoadFrom(assemblyPath).GetType(className).GetConstructor(new Type[0]), new CacheDependency(assemblyPath));
				}
				catch (Exception e) 
				{
					// could not locate DLL file
					HttpContext.Current.Response.Write("<b>ERROR:</b> Could not locate file: <code>" + assemblyPath + "</code> or could not locate class <code>" + className + "</code> in file.");
					HttpContext.Current.Response.Write("<br>" + e.Message + "<p>" + e.StackTrace);
					HttpContext.Current.Response.End();
				}
			}
			return (IModulesDataProvider)(((ConstructorInfo)cache["IModulesDataProvider"]).Invoke(null));			
		}
	}
}
