using System;
using System.Collections.Generic;
using System.Text;
using Cuyahoga.Core.Domain;
using NHibernate;
using NHibernate.Expression;
using Castle.Facilities.NHibernateIntegration;

namespace Cuyahoga.Core.DataAccess
{
    class CategoryDao : ICategoryDao
    {
        private ISessionManager sessionManager;

        public CategoryDao(ISessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        #region ICategoryDao Members

        /// <summary>
        /// Gets all root categories, ordered by path
        /// </summary>
        /// <returns></returns>
        public IList<Category> GetAllRootCategories()
        {
            ISession session = this.sessionManager.OpenSession();
            string hql = "from Cuyahoga.Core.Domain.Category c where c.ParentCategory is null order by c.Path asc";
            IQuery query = session.CreateQuery(hql);
            return query.List<Category>();
        }

        public Category GetByKey(string key)
        {
            ISession session = this.sessionManager.OpenSession();
            string hql = "from Cuyahoga.Core.Domain.Category c where c.Key = :key";
            IQuery query = session.CreateQuery(hql);
            query.SetString("key", key);
            return query.UniqueResult<Category>();
        }


        public IList<Category> GetByKeyIncludingSubcategories(string key)
        {
            ISession session = this.sessionManager.OpenSession();
            string hql = "select c from Cuyahoga.Core.Domain.Category c, Cuyahoga.Core.Domain.Category c2 where c2.Key = :key and c.Path like c2.Path + '%' order by c.Path asc";
            IQuery query = session.CreateQuery(hql);
            query.SetString("key", key);
            return query.List<Category>();
        }

        /// <summary>
        /// Gets categories by the specified partial category path, ordered by path
        /// </summary>
        /// <param name="path"></param>
        public IList<Category> GetByPathStartsWith(string path)
        {
            ISession session = this.sessionManager.OpenSession();
            string hql = "from Cuyahoga.Core.Domain.Category c where c.Path like :path order by c.Path asc";
            IQuery query = session.CreateQuery(hql);
            query.SetString("path", string.Concat(path, "%"));
            return query.List<Category>();
        }

        /// <summary>
        /// Gets a category by the specified category path
        /// </summary>
        /// <param name="path"></param>
        public Category GetByExactPath(string path)
        {
            ISession session = this.sessionManager.OpenSession();
            string hql = "from Cuyahoga.Core.Domain.Category c where c.Path = :path";
            IQuery query = session.CreateQuery(hql);
            query.SetString("path", path);
            return query.UniqueResult<Category>();
        }


        #endregion
    }
}
