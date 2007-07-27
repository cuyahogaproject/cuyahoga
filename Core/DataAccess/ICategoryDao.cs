using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
    public interface ICategoryDao
    {
        /// <summary>
        /// Gets all categories that have no parent category
        /// </summary>
        /// <returns></returns>
        IList<Category> GetAllRootCategories();

        Category GetByKey(string key);

        IList<Category> GetByKeyIncludingSubcategories(string key);

        /// <summary>
        /// Gets all categories that start with the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IList<Category> GetByPathStartsWith(string path);

        /// <summary>
        /// Gets one category matching the supplied path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Category GetByExactPath(string path);
    }
}
