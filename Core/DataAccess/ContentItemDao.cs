using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;

using NHibernate;
using NHibernate.Expression;

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
            return GetByCriteria();
        }

        /// <summary>
        /// Loads every instance of the requested type using the supplied <see cref="ICriterion" />.
        /// If no <see cref="ICriterion" /> is supplied, this behaves like <see cref="GetAll" />.
        /// </summary>
        public IList<T> GetByCriteria(params ICriterion[] criterion)
        {
            ICriteria criteria = this.GetNewSession().CreateCriteria(persistentType);

            foreach (ICriterion criterium in criterion)
            {
                criteria.Add(criterium);
            }
            return criteria.List<T>();

        }

        public T GetUniqueByProperty(string propertyName, object propertyValue)
        {
            ICriteria crit = this.GetNewSession().CreateCriteria(persistentType);
            crit.Add(Expression.Eq(propertyName, propertyValue));
            return crit.UniqueResult<T>();
        }

        public IList<T> GetByProperty(string propertyName, object propertyValue)
        {
            ICriteria crit = this.GetNewSession().CreateCriteria(persistentType);
            crit.Add(Expression.Eq(propertyName, propertyValue));
            return crit.List<T>();
        }

        protected ICriteria CreateCriteriaByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = this.GetNewSession().CreateCriteria(persistentType);
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }
            criteria.Add(example);
            return criteria;
        }

        public IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = CreateCriteriaByExample(exampleInstance, propertiesToExclude);
            return criteria.List<T>();
        }

        /// <summary>
        /// Looks for a single instance using the example provided.
        /// </summary>
        public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = CreateCriteriaByExample(exampleInstance, propertiesToExclude);
            return criteria.UniqueResult<T>();
        }

        /// <summary>
        /// For entities that have assigned ID's, you must explicitly call Save to add a new one.
        /// </summary>
        [Transaction(TransactionMode.Requires)]
        public T Save(T entity)
        {
            this.GetNewSession().Save(entity);
            return entity;
        }

        /// <summary>
        /// For entities with automatically generated IDs, such as identity, SaveOrUpdate may 
        /// be called when saving a new entity.  SaveOrUpdate can also be called to _update_ any 
        /// entity, even if its ID is assigned.
        /// </summary>
        [Transaction(TransactionMode.Requires)]
        public T SaveOrUpdate(T entity)
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
