using System;
using System.Collections;

namespace Cuyahoga.Core.Service.Search
{
    /// <summary>
    /// Summary description for SearchResultCollection.
    /// </summary>
    public class SearchResultCollection : CollectionBase
    {
        private int _totalCount;
        private int _pageIndex;
        private long _executionTime;

        /// <summary>
        /// Property TotalCount (int)
        /// </summary>
        public int TotalCount
        {
            get { return this._totalCount; }
            set { this._totalCount = value; }
        }

        /// <summary>
        /// Property PageIndex (int)
        /// </summary>
        public int PageIndex
        {
            get { return this._pageIndex; }
            set { this._pageIndex = value; }
        }

        /// <summary>
        /// The execution time of the query in ticks.
        /// </summary>
        public long ExecutionTime
        {
            get { return this._executionTime; }
            set { this._executionTime = value; }
        }

        /// <summary>
        /// Indexer property.
        /// </summary>
        public SearchResult this[int index]
        {
            get { return (SearchResult)this.List[index]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SearchResultCollection()
        {
        }

        public SearchResultCollection(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchResult"></param>
        public void Add(SearchResult searchResult)
        {
            this.List.Add(searchResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchResult"></param>
        public void Remove(SearchResult searchResult)
        {
            this.List.Remove(searchResult);
        }
    }
}
