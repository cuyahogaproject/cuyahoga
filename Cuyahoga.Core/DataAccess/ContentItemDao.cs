using System;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;

using NHibernate;
using NHibernate.Criterion;

using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;

namespace Cuyahoga.Core.DataAccess
{
	[Transactional]
	public class ContentItemDao<T> : IContentItemDao<T> where T : IContentItem
	{
		protected readonly ISessionManager SessionManager;
		protected readonly Type PersistentType = typeof(T);

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sessionManager"></param>
		public ContentItemDao(ISessionManager sessionManager)
		{
			this.SessionManager = sessionManager;
		}

		/// <summary>
		/// Returns the session from the ISessionManager
		/// </summary>
		protected ISession GetSession()
		{
			return this.SessionManager.OpenSession();
		}

		/// <summary>
		/// Loads an instance of type T from the DB based on its ID of type long.
		/// </summary>
		public T GetById(long id)
		{
			return this.GetSession().Get<T>(id);
		}

		/// <summary>
		/// Loads an instance of type T from the DB based on its ID of type Guid.
		/// </summary>
		public T GetById(Guid id)
		{
			return this.GetSession().Get<T>(id); // MB: does this work????
		}

		/// <summary>
		/// Loads every instance of the requested type with no filtering.
		/// </summary>
		public IList<T> GetAll()
		{
			ICriteria criteria = this.GetSession().CreateCriteria(PersistentType);
			return criteria.List<T>();
		}

		public IList<T> GetBySite(Site site)
    	{
			ICriteria criteria = this.GetSession().CreateCriteria(PersistentType)
				.CreateCriteria("Section", "s")
					.Add(Expression.Eq("Site", site));
			return criteria.List<T>();
    	}

		public IList<T> GetByCriteria(DetachedCriteria detachedCriteria)
		{
			ICriteria criteria = detachedCriteria.GetExecutableCriteria(GetSession());
			return criteria.List<T>();
		}

		/// <summary>
		/// For entities with automatically generated IDs, such as identity, SaveOrUpdate may 
		/// be called when saving a new entity.  SaveOrUpdate can also be called to _update_ any 
		/// entity, even if its ID is assigned.
		/// </summary>
		[Transaction(TransactionMode.Requires)]
		public T Save(T entity)
		{
			this.GetSession().SaveOrUpdate(entity);
			return entity;
		}

		[Transaction(TransactionMode.Requires)]
		public void Delete(T entity)
		{
			this.GetSession().Delete(entity);
		}


	}
}
