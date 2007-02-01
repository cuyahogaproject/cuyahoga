using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using ArticleCategory = Cuyahoga.Modules.Articles.Domain.Category;
using Cuyahoga.Core.Domain;
using System.Collections;
using NHibernate;
using System.Globalization;

namespace Cuyahoga.Modules.Articles.DataAccess
{
	/// <summary>
	/// Specific Data Access functionality for Article module.
	/// </summary>
	[Transactional]
	public class ArticleDao : Cuyahoga.Modules.Articles.DataAccess.IArticleDao
	{
		private ISessionManager _sessionManager;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sessionManager"></param>
		public ArticleDao(ISessionManager sessionManager)
		{
			this._sessionManager = sessionManager;
		}

		/// <summary>
		/// Get all available categories.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		public IList GetAvailableCategoriesBySite(Site site)
		{
			string hql = "select c from Cuyahoga.Modules.Articles.Domain.Category c where c.Site.Id = :siteId order by c.Title";
			IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
			q.SetInt32("siteId", site.Id);
			return q.List();
		}

		/// <summary>
		/// Find a category with a given title and site.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		[Transaction(TransactionMode.Supported)]
		public virtual ArticleCategory FindCategoryByTitleAndSite(string title, Site site)
		{
			string hql = "from Cuyahoga.Modules.Articles.Domain.Category c where lower(c.Title) = :title and c.Site.Id = :siteId";
			ISession session = this._sessionManager.OpenSession();
			
			// HACK: set the FlushMode of the session to temporarily to Commit because this method is being called in a transaction
			// (in ArticleModule.cs) and we have to prevent Flushing until the transaction is comitted.
			FlushMode originalFlushMode = session.FlushMode;
			session.FlushMode = FlushMode.Commit;

			IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
			q.SetString("title", title.ToLower(CultureInfo.InvariantCulture));
			q.SetInt32("siteId", site.Id);
			ArticleCategory category = q.UniqueResult() as ArticleCategory;
			session.FlushMode = originalFlushMode;
			return category;
		}

		/// <summary>
		/// Get all articles for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		public IList GetAllArticlesBySection(Section section, SortBy sortBy, SortDirection sortDirection)
		{
			string hql = "from Article a left join fetch a.Category where a.Section.Id = :sectionId " 
				+ GetOrderByClause(sortBy, sortDirection, "a");
			IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
			q.SetInt32("sectionId", section.Id);
			return q.List();
		}

		/// <summary>
		/// Get all online articles for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		public IList GetDisplayArticlesBySection(Section section, SortBy sortBy, SortDirection sortDirection)
		{
			string hql = "from Article a left join fetch a.Category where a.Section.Id = :sectionId and a.DateOnline < :now and a.DateOffline > :now " 
				+ GetOrderByClause(sortBy, sortDirection, "a");
			IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
			q.SetInt32("sectionId", section.Id);
			q.SetDateTime("now", DateTime.Now);
			return q.List();
		}

		/// <summary>
		/// Get all online articles for a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public IList GetDisplayArticlesByCategory(ArticleCategory category, SortBy sortBy, SortDirection sortDirection)
		{
			string hql = "from Article a where a.Category.Id = :categoryId and a.DateOnline < :now and a.DateOffline > :now " 
				+ GetOrderByClause(sortBy, sortDirection, "a");
			IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
			q.SetInt32("categoryId", category.Id);
			q.SetDateTime("now", DateTime.Now);
			return q.List();
		}

		/// <summary>
		/// Get all archived articles for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public IList GetArchivedArticlesBySection(Section section, SortBy sortBy, SortDirection sortDirection)
		{
			string hql = "from Article a left join fetch a.Category where a.Section.Id = :sectionId and a.DateOffline <= :now " 
				+ GetOrderByClause(sortBy, sortDirection, "a");
			IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
			q.SetInt32("sectionId", section.Id);
			q.SetDateTime("now", DateTime.Now);
			return q.List();
		}

		/// <summary>
		/// Get all online articles of a given category for syndication.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public IList GetRssArticlesByCategory(ArticleCategory category, SortBy sortBy, SortDirection sortDirection)
		{
			string hql = "from Article a where a.Category.Id = :categoryId and a.Syndicate = :syndicate and a.DateOnline < :now and a.DateOffline > :now "
				+ GetOrderByClause(sortBy, sortDirection, "a");
			IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
			q.SetInt32("categoryId", category.Id);
			q.SetBoolean("syndicate", true);
			q.SetDateTime("now", DateTime.Now);
			return q.List();
		}

		/// <summary>
		/// Get all online articles of a given section for syndication. Limit the number to the given amount.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="maxNumberOfArticles"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		public IList GetRssArticles(Section section, int maxNumberOfArticles, SortBy sortBy, SortDirection sortDirection)
		{
			string hql = "from Article a left join fetch a.Category where a.Section.Id = :sectionId and a.Syndicate = :syndicate and a.DateOnline < :now and a.DateOffline > :now "
				+ GetOrderByClause(sortBy, sortDirection, "a");
			IQuery q = this._sessionManager.OpenSession().CreateQuery(hql);
			q.SetInt32("sectionId", section.Id);
			q.SetBoolean("syndicate", true);
			q.SetDateTime("now", DateTime.Now);
			q.SetMaxResults(maxNumberOfArticles);
			return q.List();
		}

		private string GetOrderByClause(SortBy sortBy, SortDirection sortDirection, string articleAlias)
		{
			if (sortBy == SortBy.None)
			{
				return String.Empty;
			}
			else
			{
				switch (sortBy)
				{
					case SortBy.DateCreated:
					case SortBy.DateModified:
					case SortBy.DateOnline:
					case SortBy.Title:
						return String.Format("order by {0}.{1} {2}", articleAlias, sortBy.ToString(), sortDirection.ToString());
					case SortBy.Category:
						return String.Format("order by {0}.Category.Title {1}", articleAlias, sortDirection.ToString());
					case SortBy.CreatedBy:
						return String.Format("order by {0}.CreatedBy.UserName {1}", articleAlias, sortDirection.ToString());
					case SortBy.ModifiedBy:
						return String.Format("order by {0}.ModifiedBy.UserName {1}", articleAlias, sortDirection.ToString());
					default:
						return String.Empty;
				}
			}
		}
	}
}
