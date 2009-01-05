using System;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;

using Castle.MicroKernel;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// The SessionFactory provides the NHibernate sessions and provides the possibility to register
	/// additional classes with NHibernate by modules.
	/// </summary>
	[Obsolete("Replaced with the SessionFactoryHelper and the ISessionManager")] 
	public class SessionFactory
	{
		private Configuration _nhibernateConfiguration;
		private ISessionFactory _nhibernateFactory;
		private static IKernel _kernel;
		private bool _classesAdded = false;

		public event EventHandler SessionFactoryRebuilt;

		protected void OnSessionfactoryRebuilt()
		{
			if (SessionFactoryRebuilt != null)
			{
				SessionFactoryRebuilt(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SessionFactory(Configuration nhConfiguration, IKernel kernel)
		{
			this._nhibernateConfiguration = nhConfiguration;
			_kernel = kernel;
		}

		public void ExternalInitialize(ISessionFactory nhSessionFactory)
		{
			this._nhibernateFactory = nhSessionFactory;
		}

		/// <summary>
		/// Gets the one instance of the SessionFactory. This is done with a singleton so we don't have
		/// to register mappings etc. with every request.
		/// HACK: this method now delegates 
		/// </summary>
		/// <returns></returns>
		public static SessionFactory GetInstance()
		{
			return _kernel[typeof(SessionFactory)] as SessionFactory;
		}

		/// <summary>
		/// GetNHibernateFactory returns the current NHibernate ISessionFactory.
		/// </summary>
		public ISessionFactory GetNHibernateFactory()
		{
			return this._nhibernateFactory;
		}

		/// <summary>
		/// Get a new NHibernate session.
		/// </summary>
		/// <returns></returns>
		public ISession GetSession()
		{
			return this._nhibernateFactory.OpenSession();
		}

		/// <summary>
		/// Add a class to the NHibernate mappings.
		/// If the class already is mapped, nothing will happen. 
		/// </summary>
		/// <param name="type"></param>
		public void RegisterPersistentClass(Type type)
		{
			if (this._nhibernateConfiguration.GetClassMapping(type) == null)
			{
				// Class isn't mapped yet, so do it now.
				this._nhibernateConfiguration.AddClass(type);
				this._classesAdded = true;
			}			
		}

		/// <summary>
		/// Rebuild the NHibernate ISessionFactory. Use it after registering new classes.
		/// </summary>
		public bool Rebuild()
		{
			// Rebuild NHibernate SessionFactory to activate the new mapping.
			if (this._classesAdded)
			{
				this._nhibernateFactory = this._nhibernateConfiguration.BuildSessionFactory();
				this._classesAdded = false;
				OnSessionfactoryRebuilt();
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
