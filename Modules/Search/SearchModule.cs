using System;
using System.Collections;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;

namespace Cuyahoga.Modules.Search
{
	/// <summary>
	/// The searchmodule provides search capabilities on the DotLucene search index.
	/// <seealso cref="Cuyahoga.Core.Search"/>
	/// </summary>
	public class SearchModule : ModuleBase
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public SearchModule()
		{
		}

		/// <summary>
		/// Get paged search results.
		/// </summary>
		/// <param name="queryText">The query to search for.</param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="indexDir"></param>
		/// <returns></returns>
		public SearchResultCollection GetSearchResults(string queryText, int pageIndex, int pageSize, string indexDir)
		{
			IndexQuery query = new IndexQuery(indexDir);
			Hashtable keywordFilter = new Hashtable();
			keywordFilter.Add("site", this.Section.Node.Site.Name);
			return query.Find(queryText, keywordFilter, pageIndex, pageSize);
		}

		/// <summary>
		/// Get all search results with a maximum of 200.
		/// </summary>
		/// <param name="queryText"></param>
		/// <param name="indexDir"></param>
		/// <returns></returns>
		public SearchResultCollection GetSearchResults(string queryText, string indexDir)
		{
			return GetSearchResults(queryText, 0, 200, indexDir);
		}
	}
}
