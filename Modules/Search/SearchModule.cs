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
		public SearchModule()
		{
		}

		public SearchResultCollection GetSearchResults(string queryText, int pageIndex, int pageSize, string indexDir)
		{
			IndexQuery query = new IndexQuery(indexDir);
			Hashtable keywordFilter = new Hashtable();
			keywordFilter.Add("site", this.Section.Node.Site.Name);
			return query.Find(queryText, keywordFilter, pageIndex, pageSize);
		}
	}
}
