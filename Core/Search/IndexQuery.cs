using System;
using System.Collections;

using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace Cuyahoga.Core.Search
{
	/// <summary>
	/// The IndexQuery class provides functionality to search the Full-Text index.
	/// </summary>
	public class IndexQuery
	{
		private Directory _indexDirectory;

		/// <summary>
		/// Default constructor.
		/// <param name="physicalIndexDir">The physical directory where the search index resides.</param>
		/// </summary>
		public IndexQuery(string physicalIndexDir)
		{
			this._indexDirectory = FSDirectory.GetDirectory(physicalIndexDir, false);
		}

		/// <summary>
		/// Searches the index.
		/// </summary>
		/// <param name="queryText"></param>
		/// <param name="keywordFilter">A Hashtable where the key is the fieldname of the keyword and 
		/// the value the keyword itself.</param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public SearchResultCollection Find(string queryText, Hashtable keywordFilter, int pageIndex, int pageSize)
		{
			long startTicks = DateTime.Now.Ticks;

			Query query = MultiFieldQueryParser.Parse(queryText, new string[]{"title", "contents"}, new StandardAnalyzer());
			IndexSearcher searcher = new IndexSearcher(this._indexDirectory);
			Hits hits;
			if (keywordFilter != null && keywordFilter.Count > 0)
			{
				QueryFilter qf = BuildQueryFilterFromKeywordFilter(keywordFilter);
				hits = searcher.Search(query, qf);
			}
			else
			{
				hits = searcher.Search(query);
			}
			int start = pageIndex * pageSize;
			int end = (pageIndex + 1) * pageSize;
			if (hits.Length() <= end)
			{
				end = hits.Length();
			}
			SearchResultCollection results = new SearchResultCollection();
			results.TotalCount = hits.Length();
			results.PageIndex = pageIndex;
			
			for (int i = start; i < end; i++)
			{
				SearchResult result = new SearchResult();
				result.Title = hits.Doc(i).Get("title");
				result.Summary = hits.Doc(i).Get("summary");
				result.Author = hits.Doc(i).Get("author");
				result.ModuleType = hits.Doc(i).Get("moduletype");
				result.Path = hits.Doc(i).Get("path");
				result.Category = hits.Doc(i).Get("category");
				result.DateCreated = DateTime.Parse((hits.Doc(i).Get("datecreated")));
				result.Score = hits.Score(i);
				result.Boost = hits.Doc(i).GetBoost();
				result.SectionId = Int32.Parse(hits.Doc(i).Get("sectionid"));
				results.Add(result);
			}
			searcher.Close();
			results.ExecutionTime = DateTime.Now.Ticks - startTicks;

			return results;
		}

		private QueryFilter BuildQueryFilterFromKeywordFilter(Hashtable keywordFilter)
		{
			BooleanQuery bQuery = new BooleanQuery();
			foreach(DictionaryEntry keywordFilterTerm in keywordFilter)
			{
				string field = keywordFilterTerm.Key.ToString();
				string keyword = keywordFilterTerm.Value.ToString();
				bQuery.Add(new TermQuery(new Term(field, keyword)), true, false);
			}

			return new QueryFilter(bQuery);
		}
	}
}
