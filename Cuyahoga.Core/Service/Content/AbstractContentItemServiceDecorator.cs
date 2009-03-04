using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public virtual IList<T> GetByProperty(string propertyName, object propertyValue)
		{
			return _contentItemService.GetByProperty(propertyName, propertyValue);
		}

		public virtual IList<T> FindContentItemsBySite(Site site)
		{
			return _contentItemService.FindContentItemsBySite(site);
		}

		public virtual IList<T> FindContentItemsBySection(Section section)
		{
			return _contentItemService.FindContentItemsBySection(section);
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
