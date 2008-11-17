using System;
using System.Collections.Generic;
using System.Linq;
using Cuyahoga.Web.Mvc.Paging;

namespace Cuyahoga.Web.Mvc.Paging
{
	public class PagedList<T> : List<T>, IPagedList<T>
	{
		public PagedList(IEnumerable<T> source, int index, int pageSize) : this(source, index, pageSize, null)
		{
		}

		public PagedList(IEnumerable<T> source, int index, int pageSize, int? totalCount)
		{
			Initialize(source.AsQueryable(), index, pageSize, totalCount);
		}

		public PagedList(IQueryable<T> source, int index, int pageSize) : this(source, index, pageSize, null)
		{
		}

		public PagedList(IQueryable<T> source, int index, int pageSize, int? totalCount)
		{
			Initialize(source, index, pageSize, totalCount);
		}

		#region IPagedList Members

		public int PageCount { get; private set; }
		public int TotalItemCount { get; private set; }
		public int PageIndex { get; private set; }
		public int PageNumber { get { return PageIndex + 1; } }
		public int PageSize { get; private set; }
		public bool HasPreviousPage { get; private set; }
		public bool HasNextPage { get; private set; }
		public bool IsFirstPage { get; private set; }
		public bool IsLastPage { get; private set; }

		#endregion

		protected void Initialize(IQueryable<T> source, int index, int pageSize, int? totalCount)
		{
			//### argument checking
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("PageIndex cannot be below 0.");
			}
			if (pageSize < 1)
			{
				throw new ArgumentOutOfRangeException("PageSize cannot be less than 1.");
			}

			//### set source to blank list if source is null to prevent exceptions
			if (source == null)
			{
				source = new List<T>().AsQueryable();
			}

			//### set properties
			// If totalCount is given, we assume we assume that the caller has already filtered the list
			if (totalCount.HasValue)
			{
				TotalItemCount = totalCount.Value;
				AddRange(source);
			}
			else
			{
				TotalItemCount = source.Count();
				AddRange(source.Skip((index) * pageSize).Take(pageSize).ToList());
			}
			PageSize = pageSize;
			PageIndex = index;
			if (TotalItemCount > 0)
			{
				PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
			}
			else
			{
				PageCount = 0;
			}
			HasPreviousPage = (PageIndex > 0);
			HasNextPage = (PageIndex < (PageCount - 1));
			IsFirstPage = (PageIndex <= 0);
			IsLastPage = (PageIndex >= (PageCount - 1));
		}
	}
}