using System;
using System.Collections;

using NHibernate;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.Facilities.NHibernateIntegration;
using Castle.Facilities.NHibernateIntegration.Internal;

using Castle.Services.Transaction;
using ITransaction = Castle.Services.Transaction.ITransaction;

namespace Cuyahoga.Core.Facilities
{
	/// <summary>
	/// The CuyahogaSessionManager serves as a replacement for Castle's DefaultSessionManager.
	/// The main difference is that it doesn't store the SessionFactory internally, but 
	/// gets it from the container. This allows for dynamically rebuilding the SessionFactory.
	/// </summary>
	public class CuyahogaSessionManager : ISessionManager
	{
		private readonly IKernel _kernel;
		private readonly ISessionStore _sessionStore;
		private readonly SessionFactoryResolver _sessionFactoryResolver;

		public CuyahogaSessionManager(ISessionStore sessionStore, IKernel kernel, SessionFactoryResolver sessionFactoryResolver)
		{
			this._kernel = kernel;
			this._sessionStore = sessionStore;
			this._sessionFactoryResolver = sessionFactoryResolver;
		}

		#region ISessionManager Members

		public void RegisterSessionFactory(string alias, NHibernate.ISessionFactory sessionFactory)
		{
			// We don't do anything here anymore. SessionFactories are obtained via the SessionFactoryResolver.
		}
		
		public ISession OpenSession()
		{
			return OpenSession(Constants.DefaultAlias);
		}

		public ISession OpenSession(string alias)
		{
			if (alias == null) throw new ArgumentNullException("alias");

			ITransaction transaction = ObtainCurrentTransaction();

			bool weAreSessionOwner = false;

			SessionDelegate wrapped = this._sessionStore.FindCompatibleSession(alias);

			ISession session = null;

			if (wrapped == null)
			{
				session = CreateSession(alias); weAreSessionOwner = true;

				wrapped = WrapSession(transaction != null, session);

				this._sessionStore.Store(alias, wrapped);

				EnlistIfNecessary(weAreSessionOwner, transaction, wrapped);
			}
			else
			{
				EnlistIfNecessary(weAreSessionOwner, transaction, wrapped);
				wrapped = WrapSession(true, wrapped.InnerSession);
			}
			
			return wrapped;
		}

		#endregion

		protected bool EnlistIfNecessary(bool weAreSessionOwner, 
			ITransaction transaction, SessionDelegate session)
		{
			if (transaction == null) return false;

			IList list = (IList) transaction.Context["nh.session.enlisted"];

			bool shouldEnlist = false;

			if (list == null)
			{
				list = new ArrayList();

				list.Add(session);

				transaction.Context["nh.session.enlisted"] = list;

				shouldEnlist = true;
			}
			else
			{
				shouldEnlist = true;

				foreach(ISession sess in list)
				{
					if (SessionDelegate.AreEqual(session, sess))
					{
						shouldEnlist = false;
						break;
					}
				}
			}

			if (shouldEnlist)
			{
				// TODO: propagate IsolationLevel, expose as transaction property

				transaction.Enlist( new ResourceAdapter(session.BeginTransaction()) );

				if (weAreSessionOwner)
				{
					transaction.RegisterSynchronization( 
						new SessionDisposeSynchronization(session) );
				}
			}

			return true;
		}

		private ITransaction ObtainCurrentTransaction()
		{
			ITransactionManager transactionManager = this._kernel[ typeof(ITransactionManager) ] as ITransactionManager;

			return transactionManager.CurrentTransaction;
		}

		private SessionDelegate WrapSession(bool hasTransaction, ISession session)
		{
			return new SessionDelegate( !hasTransaction, session, this._sessionStore );
		}

		private ISession CreateSession(string alias)
		{
			ISessionFactory sessionFactory = this._sessionFactoryResolver.GetSessionFactory(alias);

			if (sessionFactory == null)
			{
				throw new FacilityException("No ISessionFactory implementation " + 
					"associated with the given alias: " + alias);
			}

			if (this._kernel.HasComponent("nhibernate.session.interceptor"))
			{
				IInterceptor interceptor = (IInterceptor) this._kernel["nhibernate.session.interceptor"];
				
				return sessionFactory.OpenSession(interceptor);
			}
			else
			{
				return sessionFactory.OpenSession();
			}
		}
	}
}
