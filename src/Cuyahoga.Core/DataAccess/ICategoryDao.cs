using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
    public interface ICategoryDao
    {
        /// <summary>
        /// Get all categories that have no parent category for a given site.
        /// </summary>
        /// <returns></returns>
        IList<Category> GetAllRootCategories(Site site);

		/// <summary>
		/// Get all categories for a given site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
    	IEnumerable<Category> GetAllCategories(Site site);
    }
}
