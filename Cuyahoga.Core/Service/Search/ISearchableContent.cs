using System;
using System.Collections.Generic;
using System.Text;

namespace Cuyahoga.Core.Service.Search
{
    public interface ISearchableContent
    {
        /// <summary>
        /// Get the full contents of the ContentItem for indexing
        /// </summary>
        /// <param name="textExtractor">Text extractor that is supplied by the search infrastructure to perform external text extraction.</param>
        /// <returns></returns>
        string ToSearchContent(ITextExtractor textExtractor);

        /// <summary>
        /// Get a list of <see cref="CustomSearchField"/>s, if any
        /// </summary>
        /// <returns></returns>
        IList<CustomSearchField> GetCustomSearchFields();


    }
}
