using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Service.Versioning;

namespace Cuyahoga.Core.Decorators
{
    public class VersioningDecorator<T> : AbstractDaoDecorator<T> where T : IContentItem
    {
        private IVersioningService<T> versioningService;

        public VersioningDecorator(IContentItemDao<T> contentItemDao, IVersioningService<T> versioningService)
            : base(contentItemDao)
        {
            this.versioningService = versioningService;
        }

        #region Overrides

        public override T Save(T entity)
        {
            if(entity is IVersionableContent)
            {
                //call the versioning service to do the work
                this.versioningService.CreateNewVersion(entity);
            }
            //forward call to inner dao 
            return this.contentItemDao.Save(entity);
        }

        public override T SaveOrUpdate(T entity)
        {
            if (entity is IVersionableContent)
            {
                //call the versioning service to do the work
                this.versioningService.CreateNewVersion(entity);
            }
            //forward call to inner dao 
            return this.contentItemDao.SaveOrUpdate(entity);
        }

        public override void Delete(T entity)
        {
            //3 forward call to inner dao 
            this.contentItemDao.Delete(entity);
        }

        #endregion
    }
}
