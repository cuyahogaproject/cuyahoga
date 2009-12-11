using System.Globalization;
using System.Web;

namespace Cuyahoga.Web.Mvc.Localization
{
	public class GlobalResourceLocalizer : ILocalizer
	{
		private readonly CultureInfo _defaultUICulture = CultureInfo.CurrentUICulture;
		private readonly string _defaultResourceName = "cuyahoga.web.manager.globalresources";

		public string GetString(string key)
		{
			return GetString(key, this._defaultResourceName);
		}

		public string GetString(string key, string baseName)
		{
			return GetString(key, baseName, this._defaultUICulture);
		}

		public string GetString(string key, string baseName, CultureInfo culture)
		{
			var objectFromResource = HttpContext.GetGlobalResourceObject(baseName, key, culture);
			if (objectFromResource != null)
			{
				return objectFromResource.ToString();
			}
			return key;
		}
	}
}
