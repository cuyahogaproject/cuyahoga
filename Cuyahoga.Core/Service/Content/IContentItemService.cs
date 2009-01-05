using System;
using System.Collections.Generic;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Content
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContentItemService<T> where T : IContentItem
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

        /// <summary>
        ///  Find ContentItems by the section they're assigned to
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        IList<T> FindContentItemsBySection(Section section);

        /// <summary>
        /// Find ContentItems by title (begins with).
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        IList<T> FindContentItemsByTitle(string title);

        /// <summary>
        /// Find ContentItems by the user who created them
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IList<T> FindContentItemsByCreator(User user);

        /// <summary>
        /// Find ContentItems by the user who published them
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IList<T> FindContentItemsByPublisher(User user);

        /// <summary>
        /// Find ContentItems by the user who last modified them 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IList<T> FindContentItemsByModifier(User user);
    }

}