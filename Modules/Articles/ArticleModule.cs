using System;
using System.Collections;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Type;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// The ArticleModule provides a news system (articles, comments, content expiration, rss feed).
	/// </summary>
	public class ArticleModule : ModuleBase
	{
		public ArticleModule()
		{
			SessionFactory sf = SessionFactory.GetInstance();
			// Register classes that are used by the ArticleModule
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Category));
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Article));
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Comment));

			base.SessionFactoryRebuilt = sf.Rebuild();
		}

		public IList GetAvailableCategories()
		{
			try
			{
				return base.NHSession.CreateCriteria(typeof(Category)).List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Categories", ex);
			}
		}

		public IList GetAllArticles()
		{
			try
			{
				string hql = "from Article a where a.Section.Id = ? order by a.DateOnline desc ";
				return base.NHSession.Find(hql, this.Section.Id, TypeFactory.GetInt32Type());
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Articles", ex);
			}
		}

		public IList GetDisplayArticles(int number)
		{
			try
			{
				string hql = "from Article a where a.Section.Id = :sectionId and a.DateOnline < :now and a.DateOffline > :now order by a.DateOnline desc ";
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("sectionId", base.Section.Id);
				q.SetDateTime("now", DateTime.Now);
				q.SetMaxResults(number);
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Articles", ex);
			}
		}

		public Article GetArticleById(int id)
		{
			try
			{
				return (Article)base.NHSession.Load(typeof(Article), id);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Article", ex);
			}
		}

		public void SaveArticle(Article article)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				HandleCategory(article.Category, base.NHSession);
				if (article.Id == -1)
				{
					base.NHSession.Save(article);
				}
				else
				{
					base.NHSession.Update(article);
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Article", ex);
			}
		}

		private void HandleCategory(Category category, ISession session)
		{
			if (category != null && category.Id == -1)
			{
				// Unknown category, this could be a new one or maybe still an existing one.
				IList categories = session.CreateCriteria(typeof(Category)).Add(Expression.Eq("Title", category.Title)).List();
				if (categories.Count > 0)
				{
					// Use the existing one.
					category = (Category)categories[0];
				}
				else
				{
					// Insert the new one, so the Id will be generated and retrieved.
					session.Save(category);
				}
			}
		}

		public void DeleteArticle(Article article)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(article);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Article", ex);
			}
		}
	}

	/// <summary>
	/// The displaytype of the articles in the list.
	/// </summary>
	public enum DisplayType
	{
		HeadersOnly,
		HeadersAndSummary,
		FullContent,
	}
}
