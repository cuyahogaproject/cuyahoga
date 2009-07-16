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
    }
}
