using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// Functionality for common simple data access. The class uses NHibernate.
	/// </summary>
	[Transactional]
	public class CommonDao : ICommonDao
	{
		private ISessionManager _sessionManager;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sessionManager"></param>
		public CommonDao(ISessionManager sessionManager)
		{
			this._sessionManager = sessionManager;
		}

		#region ICommonDao Members

		public object GetObjectById(Type type, int id)
		{
			ISession session = this._sessionManager.OpenSession();
			return session.Load(type, id);
		}

		public object GetObjectById(Type type, int id, bool allowNull)
		{
			if (! allowNull)
			{
				return GetObjectById(type, id);
			}
			else
			{
				ISession session = this._sessionManager.OpenSession();
				return session.Get(type, id);
			}
		}

		public T GetObjectById<T>(int id)
		{
			ISession session = this._sessionManager.OpenSession();
			return session.Get<T>(id);
		}

		public object GetObjectByDescription(Type type, string propertyName, string description)
		{
			ISession session = this._sessionManager.OpenSession();
			ICriteria crit = session.CreateCriteria(type);
			crit.Add(Expression.Eq(propertyName, description));
			return crit.UniqueResult();
		}

		public IList GetAll(Type type)
		{
			return GetAll(type, null);
		}

		public IList GetAll(Type type, params string[] sortProperties)
		{
			ISession session = this._sessionManager.OpenSession();

			ICriteria crit = session.CreateCriteria(type);
			if (sortProperties != null)
			{
				foreach (string sortProperty in sortProperties)
				{
					crit.AddOrder(Order.Asc(sortProperty));
				}
			}
			return crit.List();
		}

		/// <summary>
		/// Get all objects of T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IList<T> GetAll<T>()
		{
			return GetAll<T>(null);
		}

		/// <summary>
		/// Get all objects of T.
		/// </summary>
		/// <param name="sortProperties"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IList<T> GetAll<T>(params string[] sortProperties)
		{
			ISession session = this._sessionManager.OpenSession();

			ICriteria crit = session.CreateCriteria(typeof(T));
			if (sortProperties != null)
			{
				foreach (string sortProperty in sortProperties)
				{
					crit.AddOrder(Order.Asc(sortProperty));
				}
			}
			return crit.List<T>();
		}

		/// <summary>
		/// Get all objects of T that match the given criteria.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="criteria">NHibernate DetachedCriteria instance.</param>
		/// <returns></returns>
		/// <remarks>
		/// Be careful to not use this one from the UI layer beacuse it ties the UI to NHibernate.
		/// </remarks>
		public IList<T> GetAllByCriteria<T>(DetachedCriteria criteria)
		{
			using (ISession session = this._sessionManager.OpenSession())
			{
				ICriteria crit = criteria.GetExecutableCriteria(session);
				return crit.List<T>();
			}
		}

		/// <summary>
		/// Get all objects of T for the given id's.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ids"></param>
		/// <returns></returns>
		public IList<T> GetByIds<T>(int[] ids)
		{
			ISession session = this._sessionManager.OpenSession();
			ICriteria crit = session.CreateCriteria(typeof(T))
				.Add(Expression.In("Id", ids));
			return crit.List<T>();
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void SaveOrUpdateObject(object obj)
		{
			ISession session = this._sessionManager.OpenSession();
			session.SaveOrUpdate(obj);
		}


        [Transaction(TransactionMode.Requires)]
        public virtual void SaveObject(object obj)
        {
            ISession session = this._sessionManager.OpenSession();
            session.Save(obj);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateObject(object obj)
        {
            ISession session = this._sessionManager.OpenSession();
            session.Update(obj);
        }

		[Transaction(TransactionMode.Requires)]
		public virtual void DeleteObject(object obj)
		{
			ISession session = this._sessionManager.OpenSession();
			session.Delete(obj);
		}

		public void MarkForDeletion(object obj)
		{
			ISession session = this._sessionManager.OpenSession();
			session.Delete(obj);
		}

		public void RemoveCollectionFromCache(string roleName)
		{
			ISession session = this._sessionManager.OpenSession();
			session.SessionFactory.EvictCollection(roleName);
		}

		public void RemoveCollectionFromCache(string roleName, int id)
		{
			ISession session = this._sessionManager.OpenSession();
			session.SessionFactory.EvictCollection(roleName, id);
		}

		public void RemoveQueryFromCache(string cacheRegion)
		{
			ISession session = this._sessionManager.OpenSession();
			session.SessionFactory.EvictQueries(cacheRegion);
		}

		public void Flush()
		{
			ISession session = this._sessionManager.OpenSession();
			session.Flush();
		}

		#endregion
	}
}
