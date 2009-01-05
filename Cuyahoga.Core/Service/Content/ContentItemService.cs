using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Content
{
    public class ContentItemService<T> : IContentItemService<T> where T: IContentItem
    {
        protected IContentItemDao<T> contentItemDao;

        public ContentItemService(/*IContentItemDao<T> contentItemDao*/){
			this.contentItemDao = Util.IoC.Container.Resolve<IContentItemDao<T>>("core.searchdecorator");
        }

        public T GetById(long id)
        {
            return this.contentItemDao.GetById(id);
        }

        public T GetById(Guid id)
        {
            return this.contentItemDao.GetById(id);
        }

        public IList<T> GetAll()
        {
            return this.contentItemDao.GetAll();
        }


        public T GetUniqueByProperty(string propertyName, object propertyValue)
        {
            return this.contentItemDao.GetUniqueByProperty(propertyName, propertyValue);
        }

        public IList<T> GetByProperty(string propertyName, object propertyValue)
        {
            return this.contentItemDao.GetByProperty(propertyName, propertyValue);
        }

        public IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            return this.GetByExample(exampleInstance, propertiesToExclude);
        }

        public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            return this.GetUniqueByExample(exampleInstance, propertiesToExclude);
        }

        public T Save(T entity)
        {
            return this.contentItemDao.Save(entity);
        }

       public T SaveOrUpdate(T entity)
        { 
            return this.contentItemDao.SaveOrUpdate(entity);
        }

        public void Delete(T entity)
        {
            this.contentItemDao.Delete(entity);
        }

        #region Convenience Methods

        public IList<T> FindContentItemsBySection(Section section)
        {
            return this.contentItemDao.GetByProperty("Section", section);
        }

        public IList<T> FindContentItemsByTitle(string title)
        {
            return this.contentItemDao.GetByProperty("Title", title);
        }

        public IList<T> FindContentItemsByCreator(User user)
        {
            return this.contentItemDao.GetByProperty("Creator", user);
        }

        public IList<T> FindContentItemsByPublisher(User user)
        {
            return this.contentItemDao.GetByProperty("Publisher", user);
        }

        public IList<T> FindContentItemsByModifier(User user)
        {
            return this.contentItemDao.GetByProperty("Modifier", user);
        }

        #endregion



    }
}
