using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Cuyahoga.Web.Mvc.Localization
{
	public class ResourceLocalizer : ILocalizer
	{
		private readonly CultureInfo _defaultUICulture = CultureInfo.CurrentUICulture;
		private readonly string _defaultResourceNamespace = "resources.cuyahoga.web.manager.globalresources";
		private IDictionary<string, ResourceManager> _resourceManagers = new Dictionary<string, ResourceManager>();

		public ResourceLocalizer()
		{
			InitializeResourceManagers();
		}

		private void InitializeResourceManagers()
		{
			Type buildManagerType = typeof(System.Web.Compilation.BuildManager);

			PropertyInfo propertyInfo = buildManagerType.GetProperty("AppResourcesAssembly",
				BindingFlags.Static | BindingFlags.GetField | BindingFlags.NonPublic);
			Assembly assembly = (Assembly)propertyInfo.GetValue(null, null);
			string[] names = assembly.GetManifestResourceNames();
			foreach (string resourceName in names)
			{
				string baseName = resourceName.Substring(0, resourceName.LastIndexOf('.'));
				this._resourceManagers[baseName] = new ResourceManager(baseName, assembly);
			}
		}

		public string GetString(string key)
		{
			return GetString(key, this._defaultResourceNamespace);
		}

		public string GetString(string key, string baseName)
		{
			return GetString(key, baseName, CultureInfo.CurrentUICulture);
		}

		public string GetString(string key, string baseName, CultureInfo culture)
		{
			if (! this._resourceManagers.ContainsKey(baseName))
			{
				throw new ArgumentException("No resource manager for {0} found.", baseName);
			}
			return this._resourceManagers[baseName].GetString(key, culture) ?? key;
		}
	}
}
