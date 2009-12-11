using System;
using NHibernate.Criterion;

namespace Cuyahoga.Core.Service.Content
{
	/// <summary>
	/// Class that contains optional settings of how to perform the query (sorting, paging)
	/// </summary>
	public class ContentItemQuerySettings
	{
		/// <summary>
		/// Indicates on which property to sort.
		/// </summary>
		public ContentItemSortBy SortBy { get; private set; }
		/// <summary>
		/// Indicates the sort direction.
		/// </summary>
		public ContentItemSortDirection SortDirection { get; private set; }
		/// <summary>
		/// The page size.
		/// </summary>
		public int? PageSize { get; private set; }
		/// <summary>
		/// The page number (starting from 1).
		/// </summary>
		public int? PageNumber { get; private set; }

		/// <summary>
		/// Creates a new instance of the <see cref="ContentItemQuerySettings"></see> class.
		/// </summary>
		/// <param name="sortBy">Indicates on which property to sort.</param>
		/// <param name="sortDirection">Indicates the sort direction.</param>
		public ContentItemQuerySettings(ContentItemSortBy sortBy, ContentItemSortDirection sortDirection) : this(sortBy, sortDirection, null, null)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="ContentItemQuerySettings"></see> class.
		/// </summary>
		/// <param name="sortBy">Indicates on which property to sort.</param>
		/// <param name="sortDirection">Indicates the sort direction.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="pageNumber">The page number (starting from 1 as the first page).</param>
		public ContentItemQuerySettings(ContentItemSortBy sortBy, ContentItemSortDirection sortDirection, int? pageSize, int? pageNumber)
		{
			SortBy = sortBy;
			SortDirection = sortDirection;
			PageSize = pageSize;
			PageNumber = pageNumber;
		}
	}

	/// <summary>
	/// Helper extensions for DetachedCriteria that work in conjunction with the ContentItemQuerySettings parameters.
	/// </summary>
	public static class DetachedCriteriaExtensions
	{
		/// <summary>
		/// Adds sorting to the DetachedCriteria based on the given parameters.
		/// </summary>
		/// <param name="criteria"></param>
		/// <param name="contentItemSortBy"></param>
		/// <param name="contentItemSortDirection"></param>
		/// <returns></returns>
		public static DetachedCriteria ApplyOrdering(this DetachedCriteria criteria, ContentItemSortBy contentItemSortBy, ContentItemSortDirection contentItemSortDirection)
		{
			DetachedCriteria orderedCriteria = criteria;

			if (contentItemSortDirection != ContentItemSortDirection.None)
			{
				switch (contentItemSortDirection)
				{
					case ContentItemSortDirection.ASC:
						orderedCriteria = criteria.AddOrder(Order.Asc(contentItemSortBy.ToString()));
						break;
					case ContentItemSortDirection.DESC:
						orderedCriteria = criteria.AddOrder(Order.Desc(contentItemSortBy.ToString()));
						break;
				}
			}
			return orderedCriteria;
		}

		/// <summary>
		/// Adds paging to the DetachedCriteria based on the given parameters.
		/// </summary>
		/// <param name="criteria"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageNumber"></param>
		/// <returns></returns>
		public static DetachedCriteria ApplyPaging(this DetachedCriteria criteria, int? pageSize, int? pageNumber)
		{
			if (pageSize.HasValue)
			{
				criteria.SetMaxResults(pageSize.Value);
			}
			if (pageNumber.HasValue)
			{
				if (!pageSize.HasValue)
				{
					throw new ArgumentException("Unable to determine the object to return for the given page number when the pagesize is unknown.");
				}
				criteria.SetFirstResult(pageSize.Value * (pageNumber.Value - 1));
			}
			return criteria;
		}
	}
}
