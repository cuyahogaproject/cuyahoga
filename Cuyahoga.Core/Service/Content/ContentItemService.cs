using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Content
{
	public class ContentItemService<T> : IContentItemService<T> where T : IContentItem
	{
		protected IContentItemDao<T> contentItemDao;

		public ContentItemService(IContentItemDao<T> contentItemDao)
		{
			this.contentItemDao = contentItemDao;
		}

		public T GetById(long id)
		{
			return this.contentItemDao.GetById(id);
		}

		public T GetById(Guid id)
		{
			return this.contentItemDao.GetById(id);
		}

		public IList<T> GetAll()
		{
			return this.contentItemDao.GetAll();
		}

		public IList<T> GetByProperty(string propertyName, object propertyValue)
		{
			return this.contentItemDao.GetByProperty(propertyName, propertyValue);
		}

		public IList<T> FindContentItemsBySite(Site site)
		{
			return this.contentItemDao.GetBySite(site);
		}

		public IList<T> FindContentItemsBySection(Section section)
		{
			return this.contentItemDao.GetByProperty("Section", section);
		}

		public T Save(T entity)
		{
			return this.contentItemDao.Save(entity);
		}

		public void Delete(T entity)
		{
			this.contentItemDao.Delete(entity);
		}
	}
}
