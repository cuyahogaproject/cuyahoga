using System;
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
        /// Gets all root categories, ordered by path for a given site
        /// </summary>
        /// <returns></returns>
        public IList<Category> GetAllRootCategories(Site site)
        {
            ISession session = this.sessionManager.OpenSession();
            string hql = "from Cuyahoga.Core.Domain.Category c where c.Site = :site and c.ParentCategory is null order by c.Path asc";
            IQuery query = session.CreateQuery(hql);
        	query.SetParameter("site", site);
            return query.List<Category>();
        }

    	public IEnumerable<Category> GetAllCategories(Site site)
    	{
			ISession session = this.sessionManager.OpenSession();
			string hql = "from Cuyahoga.Core.Domain.Category c where c.Site = :site order by c.Path asc";
			IQuery query = session.CreateQuery(hql);
			query.SetParameter("site", site);
			return query.List<Category>();
    	}
    }
}
