using System;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// The SessionFactory provides the NHibernate sessions and provides the possibility to register
	/// additional classes with NHibernate by modules.
	/// </summary>
	public class SessionFactory
	{
		private static SessionFactory _sessionFactory;
		private Configuration _nhibernateConfiguration;
		private ISessionFactory _nhibernateFactory;
		private bool _classesAdded = false;

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected SessionFactory()
		{
			RegisterCoreClasses();
		}

		/// <summary>
		/// Gets the one instance of the SessionFactory. This is done with a singleton so we don't have
		/// to register mappings etc. with every request.
		/// </summary>
		/// <returns></returns>
		public static SessionFactory GetInstance()
		{
			if (_sessionFactory == null)
			{
				_sessionFactory = new SessionFactory();
			}
			return _sessionFactory;
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
		/// Add a class to the NHibernate mappings and rebuild the NHibernate SessionFactory. 
		/// If the class already is mapped, nothing will happen. Call RefreshFactory() after
		/// adding classes. They are not automatically added because of possible dependencies
		/// between the classes.
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
				return true;
			}
			else
			{
				return false;
			}
		}

		private void RegisterCoreClasses()
		{
			Configuration config = new Configuration();
			this._nhibernateConfiguration = config.AddAssembly(this.GetType().Assembly);
			this._nhibernateFactory = this._nhibernateConfiguration.BuildSessionFactory();
		}
	}
}
