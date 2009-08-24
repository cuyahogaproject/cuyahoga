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
		void AddContent(IList<SearchContent> searchContents);

		void UpdateContent(IContentItem contentItem);
		void AddContent(IContentItem contentItem);
		void DeleteContent(IContentItem contentItem);

		SearchResultCollection FindContent(string queryText, IDictionary<string, string> keywordFilter, int pageIndex, int pageSize, User user);
		SearchResultCollection FindContent(string queryText, IDictionary<string, string> keywordFilter, IList<string> categoryName, int pageIndex, int pageSize, User user);

		/// <summary>
		/// Get information about the full-text index of the current site.
		/// </summary>
		/// <returns></returns>
		SearchIndexProperties GetIndexProperties();

		/// <summary>
		/// Rebuild the full-text index.
		/// </summary>
		/// <param name="searchableModules">A list of (legacy) searchable modules in the installation.</param>
		void RebuildIndex(IEnumerable<ISearchable> searchableModules);
	}
}
