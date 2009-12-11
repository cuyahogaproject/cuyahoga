using System;
using System.Collections.Generic;
using Castle.Services.Transaction;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using NHibernate.Criterion;

namespace Cuyahoga.Core.Service.Content
{
	[Transactional]
	public class ContentItemService<T> : IContentItemService<T> where T : IContentItem
	{
		private readonly IContentItemDao<T> _contentItemDao;
		private readonly ICuyahogaContextProvider _cuyahogaContextProvider;

		protected IContentItemDao<T> ContentItemDao
		{
			get { return this._contentItemDao; }
		}

		protected ICuyahogaContextProvider CuyahogaContextProvider
		{
			get { return this._cuyahogaContextProvider; }
		}

		public ContentItemService(IContentItemDao<T> contentItemDao, ICuyahogaContextProvider contextProvider)
		{
			this._contentItemDao = contentItemDao;
			this._cuyahogaContextProvider = contextProvider;
		}

		public T GetById(long id)
		{
			return this._contentItemDao.GetById(id);
		}

		public T GetById(Guid id)
		{
			return this._contentItemDao.GetById(id);
		}

		public IList<T> GetAll()
		{
			return this._contentItemDao.GetAll();
		}

		/// <summary>
		/// Find all content items for the given site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		public IList<T> FindContentItemsBySite(Site site)
		{
			return this._contentItemDao.GetBySite(site);
		}

		/// <summary>
		/// Find ContentItems by the section they're assigned to.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public IList<T> FindContentItemsBySection(Section section)
		{
			return FindContentItemsBySection(section, new ContentItemQuerySettings(ContentItemSortBy.None, ContentItemSortDirection.None));
		}

		/// <summary>
		/// Find ContentItems by the section they're assigned to.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public IList<T> FindContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
		{
			DetachedCriteria criteria = GetCriteriaForSection(section, querySettings);
			return this._contentItemDao.GetByCriteria(criteria);
		}

		/// <summary>
		/// Find the currently visible ContentItems by the section they're assigned to.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public IList<T> FindVisibleContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
		{
			DetachedCriteria criteria = GetCriteriaForSection(section, querySettings)
				.Add(Restrictions.Lt("PublishedAt", DateTime.Now))
				.Add(Restrictions.Or(Restrictions.IsNull("PublishedUntil"), Restrictions.Gt("PublishedUntil", DateTime.Now)));
			return this._contentItemDao.GetByCriteria(criteria);
		}

		/// <summary>
		/// Find currently visible ContentItems for a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public IList<T> FindVisibleContentItemsByCategory(Category category, ContentItemQuerySettings querySettings)
		{
			DetachedCriteria criteria = GetCriteriaForCategory(category, querySettings)
				.Add(Restrictions.Lt("PublishedAt", DateTime.Now))
				.Add(Restrictions.Or(Restrictions.IsNull("PublishedUntil"), Restrictions.Gt("PublishedUntil", DateTime.Now)));
			return this._contentItemDao.GetByCriteria(criteria);
		}

		/// <summary>
		/// Find the archived content items for a given section (published until before today)
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public IList<T> FindArchivedContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
		{
			DetachedCriteria criteria = GetCriteriaForSection(section, querySettings)
				.Add(Restrictions.Lt("PublishedUntil", DateTime.Now));
			return this._contentItemDao.GetByCriteria(criteria);
		}

		/// <summary>
		/// Find the syndicated content items for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public IList<T> FindSyndicatedContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
		{
			DetachedCriteria criteria = GetCriteriaForSection(section, querySettings)
				.Add(Restrictions.Eq("Syndicate", true));
			return this._contentItemDao.GetByCriteria(criteria);
		}

		/// <summary>
		/// Find the syndicated content items for a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public IList<T> FindSyndicatedContentItemsByCategory(Category category, ContentItemQuerySettings querySettings)
		{
			DetachedCriteria criteria = GetCriteriaForCategory(category, querySettings)
				.Add(Restrictions.Eq("Syndicate", true));
			return this._contentItemDao.GetByCriteria(criteria);
		}

		[Transaction(TransactionMode.Requires)]
		public T Save(T entity)
		{
			return this._contentItemDao.Save(entity);
		}

		[Transaction(TransactionMode.Requires)]
		public void Delete(T entity)
		{
			this._contentItemDao.Delete(entity);
		}

		private DetachedCriteria GetCriteriaForSection(Section section, ContentItemQuerySettings querySettings)
		{
			return GetContentItemCriteria()
				.Add(Restrictions.Eq("Section", section))
				.ApplyOrdering(querySettings.SortBy, querySettings.SortDirection)
				.ApplyPaging(querySettings.PageSize, querySettings.PageNumber);
		}

		private DetachedCriteria GetCriteriaForCategory(Category category, ContentItemQuerySettings querySettings)
		{
			return GetContentItemCriteria()
				.CreateAlias("Categories", "cat")
					.Add(Restrictions.Eq("cat.Id", category.Id))
				.ApplyOrdering(querySettings.SortBy, querySettings.SortDirection)
				.ApplyPaging(querySettings.PageSize, querySettings.PageNumber);
		}

		private DetachedCriteria GetContentItemCriteria()
		{
			return DetachedCriteria.For(typeof(T));
		}
	}
}
