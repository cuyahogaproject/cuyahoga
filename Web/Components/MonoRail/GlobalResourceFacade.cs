using System;
using System.Collections;
using System.Globalization;
using System.Web;
using Castle.MonoRail.Framework;

namespace Cuyahoga.Web.Components.MonoRail
{
	public class GlobalResourceFacade : IResource
	{
		private CultureInfo _currentCulture;
		private string _resourceName;

		public GlobalResourceFacade(string resourceName, CultureInfo currentCulture)
		{
			this._resourceName = resourceName;
			this._currentCulture = currentCulture;
		}

		public string GetString(string key)
		{
			return (string) GetObject(key);
		}

		public object GetObject(string key)
		{
			return HttpContext.GetGlobalResourceObject(this._resourceName, key, this._currentCulture);
		}

		public object this[string key]
		{
			get { return GetObject(key); }
		}

		///<summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
		}

		///<summary>
		///Returns an enumerator that iterates through a collection.
		///</summary>
		///
		///<returns>
		///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		///</returns>
		///<filterpriority>2</filterpriority>
		public IEnumerator GetEnumerator()
		{
			throw new NotSupportedException("Unable to retrieve an enumerator for global resources.");
		}
	}
}
