using System;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;
using Castle.MicroKernel;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// Provides utility methods to maintain the NHibernate SessionFactory.
	/// </summary>
	public class SessionFactoryHelper
	{
		private IKernel _kernel;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="kernel"></param>
		public SessionFactoryHelper(IKernel kernel)
		{
			this._kernel = kernel;
		}

		/// <summary>
		/// Add a new assembly to the configuration and build a new SessionFactory.
		/// </summary>
		/// <param name="assembly"></param>
		public void AddAssembly(Assembly assembly)
		{
			Configuration nhConfiguration = this._kernel[typeof(Configuration)] as Configuration;
			nhConfiguration.AddAssembly(assembly);
			ISessionFactory newSessionFactory = nhConfiguration.BuildSessionFactory();
			ReplaceSessionFactory(newSessionFactory);
		}

		/// <summary>
		/// Configure the 'old' Cuyahoga SessionFactory wrapper.
		/// </summary>
		public void ConfigureLegacySessionFactory()
		{
			// TODO: get rid of this solution as soon as possible!
			this._kernel.AddComponent("core.legacysessionfactory", typeof(SessionFactory));
			SessionFactory cuyahogaSessionFactory = this._kernel[typeof(SessionFactory)] as SessionFactory;
			// We can't auto-wire the ISessionFactory via the constructor because it's
			// impossible to remove the old ISessonFactory from the container after a rebuild.
			cuyahogaSessionFactory.ExternalInitialize(this._kernel[typeof(ISessionFactory)] as ISessionFactory);
			cuyahogaSessionFactory.SessionFactoryRebuilt += new EventHandler(cuyahogaSessionFactory_SessionFactoryRebuilt);
		}

		private void cuyahogaSessionFactory_SessionFactoryRebuilt(object sender, EventArgs e)
		{
			ISessionFactory newNhSessionFactory = ((SessionFactory)this._kernel[typeof(SessionFactory)]).GetNHibernateFactory();
			ReplaceSessionFactory(newNhSessionFactory);
		}

		private void ReplaceSessionFactory(ISessionFactory nhSessionFactory)
		{
			this._kernel.RemoveComponent("nhibernate.factory");
			this._kernel.AddComponentInstance("nhibernate.factory", nhSessionFactory);
		}
	}
}
