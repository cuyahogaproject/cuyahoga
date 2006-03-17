using System;
using System.Collections;
using System.Collections.Specialized;

using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using NHibernate;

namespace Cuyahoga.Core.Facilities
{
	/// <summary>
	/// The SessionFactoryResolver class facilitates retrieval of SessionFactories
	/// by the ISessionManager by alias.
	/// </summary>
	public class SessionFactoryResolver
	{
		private IKernel _kernel;
		private IDictionary _idAliasMap = new HybridDictionary(true);

		public SessionFactoryResolver(IKernel kernel)
		{
			this._kernel = kernel;
		}

		public void RegisterAliasComponentIdMapping(string alias, string componentId)
		{
			if (this._idAliasMap.Contains(alias))
			{
				throw new ArgumentException("A mapping already exists with the following alias: " + alias);
			}
			this._idAliasMap.Add(alias, componentId);
		}

		public ISessionFactory GetSessionFactory(string alias)
		{
			string componentId = this._idAliasMap[alias] as string;
			if (componentId == null)
			{
				throw new FacilityException("An ISessionFactory component was not mapped with the following alias: " + alias);
			}
			return this._kernel[componentId] as ISessionFactory;
		}
	}
}
