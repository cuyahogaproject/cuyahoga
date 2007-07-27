using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Decorators
{
    public abstract class AbstractDaoDecorator<T> : IContentItemDao<T> where T : IContentItem
    {
        protected IContentItemDao<T> contentItemDao;
    
        public AbstractDaoDecorator(IContentItemDao<T> contentItemDao)
        {
            this.contentItemDao = contentItemDao;
        }

        #region IContentItemDao<T> Members

        public virtual T GetById(long id)
        {
            //forward call to inner dao
            return this.contentItemDao.GetById(id);
        }

        public virtual T GetById(Guid id)
        {
            //forward call to inner dao
            return this.contentItemDao.GetById(id);
        }

        public virtual System.Collections.Generic.IList<T> GetAll()
        {
            //forward call to inner dao
            return this.contentItemDao.GetAll();
        }

        public virtual T GetUniqueByProperty(string propertyName, object propertyValue)
        {
            //forward call to inner dao
            return this.contentItemDao.GetUniqueByProperty(propertyName, propertyValue);
        }

        public virtual System.Collections.Generic.IList<T> GetByProperty(string propertyName, object propertyValue)
        {
            //forward call to inner dao
            return this.contentItemDao.GetByProperty(propertyName, propertyValue);
        }

        public virtual System.Collections.Generic.IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            //forward call to inner dao
            return this.contentItemDao.GetByExample(exampleInstance, propertiesToExclude);
        }

        public virtual T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            //forward call to inner dao
            return this.contentItemDao.GetUniqueByExample(exampleInstance, propertiesToExclude);
        }

        public virtual T Save(T entity)
        {
            //forward call to inner dao 
            return this.contentItemDao.Save(entity);
        }

        public virtual T SaveOrUpdate(T entity)
        {
            //forward call to inner dao 
            return this.contentItemDao.SaveOrUpdate(entity);
        }

        public virtual void Delete(T entity)
        {
            //forward call to inner dao 
            this.contentItemDao.Delete(entity);
        }

        #endregion
    }
}
