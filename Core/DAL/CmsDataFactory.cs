using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using System.Reflection;

namespace Cuyahoga.Core.DAL
{
	/// <summary>
	/// Summary description for CmsDataFactory.
	/// </summary>
	public class CmsDataFactory
	{
//		private CmsDataFactory()
//		{
//		}

		public static ICmsDataProvider GetInstance() 
		{
			Cache cache = System.Web.HttpContext.Current.Cache;

			if ( cache["ICmsDataProvider"] == null ) 
			{
				NameValueCollection context = Util.Config.GetConfiguration();

				String assemblyPath = context["CoreDataProviderAssembly"];
				String className = context["CoreDataProviderClassName"];

				if (assemblyPath.Substring(0,1) == "/") 
				{
					// assemblyPath presented in virtual form, must convert to physical path
					assemblyPath = HttpContext.Current.Server.MapPath(assemblyPath);					
				}

				// Use reflection to store the constructor of the class that implements ICmsDataProvider
				try 
				{
					cache.Insert("ICmsDataProvider", Assembly.LoadFrom(assemblyPath).GetType(className).GetConstructor(new Type[0]), new CacheDependency(assemblyPath));
				}
				catch (Exception e) 
				{
					// could not locate DLL file
					HttpContext.Current.Response.Write("<b>ERROR:</b> Could not locate file: <code>" + assemblyPath + "</code> or could not locate class <code>" + className + "</code> in file.");
					HttpContext.Current.Response.Write("<br>" + e.Message + "<p>" + e.StackTrace);
					HttpContext.Current.Response.End();
				}
			}
			return (ICmsDataProvider)(((ConstructorInfo)cache["ICmsDataProvider"]).Invoke(null));			
		}
	}
}
