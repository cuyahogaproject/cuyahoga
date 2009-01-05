using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
    public interface IContentItemDao<T> where T : IContentItem
    {
        T GetById(long id);
        T GetById(Guid id);
        IList<T> GetAll();
        T GetUniqueByProperty(string propertyName, object propertyValue);
        IList<T> GetByProperty(string propertyName, object propertyValue);
        IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);
        T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);
        T Save(T entity);
        T SaveOrUpdate(T entity);
        void Delete(T entity);

    }
}
