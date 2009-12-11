using System;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Content
{
	public abstract class AbstractContentItemServiceDecorator<T> : IContentItemService<T> where T: IContentItem
	{
		private readonly IContentItemService<T> _contentItemService;

		protected IContentItemService<T> ContentItemService
		{
			get { return this._contentItemService; }
		}

		public AbstractContentItemServiceDecorator(IContentItemService<T> contentItemService)
		{
			_contentItemService = contentItemService;
		}

		public virtual T GetById(long id)
		{
			return _contentItemService.GetById(id);
		}

		public virtual T GetById(Guid id)
		{
			return _contentItemService.GetById(id);
		}

		public virtual IList<T> GetAll()
		{
			return _contentItemService.GetAll();
		}

		public virtual IList<T> FindContentItemsBySite(Site site)
		{
			return _contentItemService.FindContentItemsBySite(site);
		}

		public virtual IList<T> FindContentItemsBySection(Section section)
		{
			return _contentItemService.FindContentItemsBySection(section);
		}

		/// <summary>
		/// Find ContentItems by the section they're assigned to.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public virtual IList<T> FindContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
		{
			return _contentItemService.FindContentItemsBySection(section, querySettings);
		}

		/// <summary>
		/// Find the currently visible ContentItems by the section they're assigned to.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public virtual IList<T> FindVisibleContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
		{
			return _contentItemService.FindVisibleContentItemsBySection(section, querySettings);
		}

		/// <summary>
		/// Find currently visible ContentItems for a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public virtual IList<T> FindVisibleContentItemsByCategory(Category category, ContentItemQuerySettings querySettings)
		{
			return _contentItemService.FindVisibleContentItemsByCategory(category, querySettings);
		}

		/// <summary>
		/// Find the archived content items for a given section (published until before today)
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public virtual IList<T> FindArchivedContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
		{
			return _contentItemService.FindArchivedContentItemsBySection(section, querySettings);
		}

		/// <summary>
		/// Find the syndicated content items for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public virtual IList<T> FindSyndicatedContentItemsBySection(Section section, ContentItemQuerySettings querySettings)
		{
			return _contentItemService.FindSyndicatedContentItemsBySection(section, querySettings);
		}

		/// <summary>
		/// Find the syndicated content items for a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		public virtual IList<T> FindSyndicatedContentItemsByCategory(Category category, ContentItemQuerySettings querySettings)
		{
			return _contentItemService.FindSyndicatedContentItemsByCategory(category, querySettings);
		}

		public virtual T Save(T entity)
		{
			return _contentItemService.Save(entity);
		}

		public virtual void Delete(T entity)
		{
			_contentItemService.Delete(entity);
		}
	}
}
