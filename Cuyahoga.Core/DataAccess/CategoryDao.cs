using System.Collections.Generic;
using Cuyahoga.Core.Domain;
using NHibernate;
using Castle.Facilities.NHibernateIntegration;

namespace Cuyahoga.Core.DataAccess
{
    class CategoryDao : ICategoryDao
    {
        private readonly ISessionManager sessionManager;

        public CategoryDao(ISessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

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

    }
}
