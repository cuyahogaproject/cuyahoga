using System.Collections.Generic;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// Wrapper class for a paged collection of objects of type T. 
	/// </summary>
	public class PagedResultSet<T> where T: class 
	{
		private IList<T> _results;
		private int _totalCount;

		/// <summary>
		/// The paged collection.
		/// </summary>
		public IList<T> Results
		{
			get { return _results; }
		}

		/// <summary>
		/// The total number of results.
		/// </summary>
		public int TotalCount
		{
			get { return _totalCount; }
		}

		/// <summary>
		/// Creates a new instance of the <see cref="PagedResultSet"></see> class
		/// </summary>
		/// <param name="results"></param>
		/// <param name="totalCount"></param>
		public PagedResultSet(IList<T> results, int totalCount)
		{
			this._results = results;
			this._totalCount = totalCount;
		}
	}
}
