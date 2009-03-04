using System;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;

using NHibernate;
using NHibernate.Criterion;

using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;

namespace Cuyahoga.Core.DataAccess
{
	public class ContentItemDao<T> : IContentItemDao<T> where T : IContentItem
	{
		protected Type persistentType = typeof(T);
		protected ISessionManager sessionManager;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sessionManager"></param>
		public ContentItemDao(ISessionManager sessionManager)
		{
			this.sessionManager = sessionManager;
		}

		/// <summary>
		/// Returns a newly created session from the ISessionManager
		/// </summary>
		protected ISession GetNewSession()
		{
			return this.sessionManager.OpenSession();
		}

		/// <summary>
		/// Loads an instance of type T from the DB based on its ID of type long.
		/// </summary>
		public T GetById(long id)
		{
			return (T)this.GetNewSession().Load(persistentType, id);
		}

		/// <summary>
		/// Loads an instance of type T from the DB based on its ID of type Guid.
		/// </summary>
		public T GetById(Guid id)
		{
			return (T)this.GetNewSession().Load(persistentType, id);
		}

		/// <summary>
		/// Loads every instance of the requested type with no filtering.
		/// </summary>
		public IList<T> GetAll()
		{
			ICriteria criteria = this.GetNewSession().CreateCriteria(persistentType);
			return criteria.List<T>();
		}

		public IList<T> GetBySite(Site site)
    	{
			ICriteria criteria = this.GetNewSession().CreateCriteria(persistentType)
				.CreateCriteria("Section", "s")
					.Add(Expression.Eq("Site", site));
			return criteria.List<T>();
    	}

		public IList<T> GetByProperty(string propertyName, object propertyValue)
		{
			ICriteria crit = this.GetNewSession().CreateCriteria(persistentType);
			crit.Add(Expression.Eq(propertyName, propertyValue));
			return crit.List<T>();
		}

		/// <summary>
		/// For entities with automatically generated IDs, such as identity, SaveOrUpdate may 
		/// be called when saving a new entity.  SaveOrUpdate can also be called to _update_ any 
		/// entity, even if its ID is assigned.
		/// </summary>
		[Transaction(TransactionMode.Requires)]
		public T Save(T entity)
		{
			this.GetNewSession().SaveOrUpdate(entity);
			return entity;
		}

		[Transaction(TransactionMode.Requires)]
		public void Delete(T entity)
		{
			this.GetNewSession().Delete(entity);
		}


	}
}
