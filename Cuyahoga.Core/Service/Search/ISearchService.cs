using System;
using System.Collections.Generic;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;


namespace Cuyahoga.Core.Service.Search
{
    public interface ISearchService
    {
        void UpdateContent(SearchContent searchContent);
        void AddContent(SearchContent searchContent);
        void DeleteContent(SearchContent searchContent);
        void UpdateContent(IList<SearchContent> searchContents);
        void AddContent(IList<SearchContent> searchContents);
        void DeleteContent(IList<SearchContent> searchContents);
   
        void UpdateContent(IContentItem contentItem);
        void AddContent(IContentItem contentItem);
        void DeleteContent(IContentItem contentItem);
        void UpdateContent(IList<IContentItem> contentItems);
        void AddContent(IList<IContentItem> contentItems);
        void DeleteContent(IList<IContentItem> contentItems);

        SearchResultCollection FindContent(string queryText, IDictionary<string, string> keywordFilter, int pageIndex, int pageSize, User user);
        SearchResultCollection FindContent(string queryText, IDictionary<string, string> keywordFilter, IList<string> categoryName, int pageIndex, int pageSize, User user);

    }
}
