using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Service.Search;
using Cuyahoga.Core.Util;


namespace Cuyahoga.Core.Decorators
{
    public class SearchDecorator<T> : AbstractDaoDecorator<T> where T : IContentItem
    {
        private ISearchService searchService;

        public SearchDecorator(IContentItemDao<T> contentItemDao, ISearchService searchService)
            : base(contentItemDao)
        {
            this.searchService = searchService; 
        }


        private bool UseInstantIndexing
        {
            get
            {
                return (Boolean.Parse(Config.GetConfiguration()["InstantIndexing"]));
            }
        }

        #region Overrides

        public override T Save(T entity)
        {
            if (UseInstantIndexing)
            {
                if (entity is ISearchableContent)
                {
                    this.searchService.AddContent(entity);
                }
            }
            //forward call to inner dao 
            return this.contentItemDao.Save(entity);
        }

        public override T SaveOrUpdate(T entity)
        {
            if (UseInstantIndexing)
            {
                if (entity is ISearchableContent)
                {
                    this.searchService.UpdateContent(entity);
                }
            }
            //forward call to inner dao 
            return this.contentItemDao.SaveOrUpdate(entity);
        }

        public override void Delete(T entity)
        {
            if (UseInstantIndexing)
            {
                if (entity is ISearchableContent)
                {
                    this.searchService.DeleteContent(entity);
                }
            }
            // forward call to inner dao 
            this.contentItemDao.Delete(entity);
        }

        #endregion
    }
}
