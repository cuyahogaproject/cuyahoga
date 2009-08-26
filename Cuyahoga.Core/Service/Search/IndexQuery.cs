using System;
using System.Collections.Generic;

using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;

namespace Cuyahoga.Core.Service.Search
{
	/// <summary>
	/// The IndexQuery class provides functionality to search the Full-Text index.
	/// </summary>
	public class IndexQuery
	{
		private readonly Directory _indexDirectory;

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
		/// <param name="categoryNames"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="roleIds"></param>
		/// <returns></returns>
		public SearchResultCollection Find(string queryText, IList<string> categoryNames, int pageIndex, int pageSize, IEnumerable<int> roleIds)
		{
			long startTicks = DateTime.Now.Ticks;

			// the overall-query
			BooleanQuery query = new BooleanQuery();
			// add our parsed query
			if (!String.IsNullOrEmpty(queryText))
			{
				Query multiQuery = MultiFieldQueryParser.Parse(new[] { queryText, queryText, queryText }, new[] { "title", "summary", "contents" }, new StandardAnalyzer());
				query.Add(multiQuery, BooleanClause.Occur.MUST);
			}
			// add the security constraint - must be satisfied
			query.Add(this.BuildSecurityQuery(roleIds), BooleanClause.Occur.MUST);

			// Add the category query (if available)
			if (categoryNames != null)
			{
				query.Add(this.BuildCategoryQuery(categoryNames), BooleanClause.Occur.MUST);
			}

			IndexSearcher searcher = new IndexSearcher(this._indexDirectory);
			Hits hits = searcher.Search(query);
			int start = pageIndex * pageSize;
			int end = (pageIndex + 1) * pageSize;
			if (hits.Length() <= end)
			{
				end = hits.Length();
			}
			SearchResultCollection results = new SearchResultCollection(end);
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
				string[] categories = hits.Doc(i).GetValues("category");
				result.Category = categories != null ? String.Join(", ", categories) : String.Empty;
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

		private Query BuildCategoryQuery(IEnumerable<string> categoryNames)
		{
			BooleanQuery categoryQuery = new BooleanQuery();
			foreach (string name in categoryNames)
			{
				categoryQuery.Add(new TermQuery(new Term("category", name)), BooleanClause.Occur.SHOULD);
			}
			return categoryQuery;
		}

		private Query BuildSecurityQuery(IEnumerable<int> roleIds)
		{
			BooleanQuery bQueryContent = new BooleanQuery();
			foreach (int roleId in roleIds)
			{
				bQueryContent.Add(new TermQuery(new Term("viewroleid", roleId.ToString())), BooleanClause.Occur.SHOULD);
			}
			return bQueryContent;
		}
	}
}
