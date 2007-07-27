using System;
using System.Collections.Generic;

using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace Cuyahoga.Core.Service.Search
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
        public SearchResultCollection Find(string queryText, IDictionary<string, string> keywordFilter, IList<string> categoryNames, int pageIndex, int pageSize, IList<int> sectionIds, IList<int> userRoleIds)
        {
            long startTicks = DateTime.Now.Ticks;
      
            //the overall-query
            BooleanQuery query = new BooleanQuery();
            //add our parsed query
            if (queryText != null && queryText != string.Empty)
            {
                Query multiQuery = MultiFieldQueryParser.Parse(new string[] { queryText, queryText }, new string[] { "title", "contents" }, new StandardAnalyzer());
                query.Add(multiQuery, BooleanClause.Occur.MUST);
            }
            //add the security constraint - must be satisfied
            query.Add(this.BuildSecurityQuery(sectionIds, userRoleIds), BooleanClause.Occur.MUST);
            //add the category query (if available)
            if (categoryNames != null)
            {
                query.Add(this.BuildCategoryQuery(categoryNames), BooleanClause.Occur.MUST);
            }
            
            IndexSearcher searcher = new IndexSearcher(this._indexDirectory);
            Hits hits;
            if (keywordFilter != null && keywordFilter.Count > 0)
            {
                //QueryFilter qf = BuildQueryFilterFromKeywordFilter(keywordFilter);
                hits = searcher.Search(query);//, qf);
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
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				if (categories != null)
				{
					foreach (string c in categories)
					{
						sb.Append(c); sb.Append(", ");
					}
				}
				result.Category = sb.ToString().TrimEnd(' ', ',');
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

        private Query BuildCategoryQuery(IList<string> categoryNames)
        {
             BooleanQuery categoryQuery = new BooleanQuery();
             foreach (string name in categoryNames)
             {
                 categoryQuery.Add(new TermQuery(new Term("category", name)), BooleanClause.Occur.SHOULD);
             }
             return categoryQuery;
        }

        private Query BuildSecurityQuery(IList<int> sectionIds, IList<int> userRoleIds)
        {
            BooleanQuery bQuerySection = new BooleanQuery();
            foreach(int sectionId in sectionIds){
                bQuerySection.Add(new TermQuery(new Term("sectionid", sectionId.ToString())), BooleanClause.Occur.SHOULD);
            }

            BooleanQuery bQueryContent = new BooleanQuery();
            foreach (int roleId in userRoleIds)
            {
                bQueryContent.Add(new TermQuery(new Term("viewrole", roleId.ToString())), BooleanClause.Occur.SHOULD);
            }

            //the person should either have access to the section or to the content directly
            BooleanQuery sectionOrContent = new BooleanQuery();
            sectionOrContent.Add(bQuerySection, BooleanClause.Occur.SHOULD);
            sectionOrContent.Add(bQueryContent, BooleanClause.Occur.SHOULD);

            return sectionOrContent;
        }

		private QueryFilter BuildQueryFilterFromKeywordFilter(IDictionary<string, string> keywordFilter)
		{
			BooleanQuery bQuery = new BooleanQuery();
			foreach(string key in keywordFilter.Keys)
			{
                string field = key;
                string keyword = keywordFilter[key];
				bQuery.Add(new TermQuery(new Term(field, keyword)), BooleanClause.Occur.MUST);
			}
			return new QueryFilter(bQuery);
		}
	}
}
