using System;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Threading;


namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Summary description for LocalizedUserControl.
	/// </summary>
	public class LocalizedUserControl : UserControl
	{
		private ResourceManager _resMan;
		private CultureInfo _currentUICulture;

		public LocalizedUserControl()
		{
			// Base name of the resources consists of Namespace.Resources.Strings
			string baseName = this.GetType().BaseType.Namespace + ".Resources.Strings";
			this._resMan = new ResourceManager(baseName, this.GetType().BaseType.Assembly);
			this._currentUICulture = Thread.CurrentThread.CurrentUICulture;
		}

		/// <summary>
		/// Get a localized text string for a given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected string GetText(string key)
		{
			return this._resMan.GetString(key, this._currentUICulture);
		}
	}
}
