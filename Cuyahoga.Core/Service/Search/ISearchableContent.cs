using System;
using System.Collections.Generic;
using System.Text;

namespace Cuyahoga.Core.Service.Search
{
    public interface ISearchableContent
    {
        /// <summary>
        /// Get the full contents of this ContentItem for indexing
        /// </summary>
        /// <returns></returns>
        string ToSearchContent();

        /// <summary>
        /// Get a list of <see cref="CustomSearchField"/>s, if any
        /// </summary>
        /// <returns></returns>
        IList<CustomSearchField> GetCustomSearchFields();


    }
}
