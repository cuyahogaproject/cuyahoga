using System;
using System.Collections;

using NHibernate;
using NHibernate.Expression;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// The ArticleModule provides a news system (articles, comments, content expiration, rss feed).
	/// </summary>
	public class ArticleModule : Module
	{
		ISessionFactory _factory;

		public ArticleModule()
		{
			base.Section = null;
			SessionFactory sf = SessionFactory.GetInstance();
			// Register classes that are used by the ArticleModule
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Category));
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Article));
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Comment));
			sf.Rebuild();
			this._factory = sf.GetNHibernateFactory();
		}

		public IList GetAvailableCategories()
		{
			IList categories = null;
			ISession session = this._factory.OpenSession();
			try
			{
				categories = session.CreateCriteria(typeof(Category)).List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Categories", ex);
			}
			finally
			{
				session.Close();
			}
			return categories;
		}

		public IList GetAllArticles()
		{
			IList articles = null;
			ISession session = this._factory.OpenSession();
			try
			{
				articles = session.CreateCriteria(typeof(Article)).Add(Expression.Eq("SectionId", this.Section.Id)).List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Articles", ex);
			}
			finally
			{
				session.Close();
			}
			return articles;
		}

		public Article GetArticleById(int id)
		{
			Article article = null;
			ISession session = this._factory.OpenSession();
			try
			{
				article = (Article)session.Load(typeof(Article), id);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Article", ex);
			}
			finally
			{
				session.Close();
			}
			return article;
		}

		public void SaveArticle(Article article)
		{
			ISession session = this._factory.OpenSession();
			ITransaction tx = session.BeginTransaction();
			try
			{
				HandleCategory(article.Category, session);
				if (article.Id == -1)
				{
					session.Save(article);
				}
				else
				{
					session.Update(article);
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Article", ex);
			}
			finally
			{
				session.Close();
			}
		}

		private void HandleCategory(Category category, ISession session)
		{
			if (category.Id == -1)
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
			ISession session = this._factory.OpenSession();
			ITransaction tx = session.BeginTransaction();
			try
			{
				session.Delete(article);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Article", ex);
			}
			finally
			{
				session.Close();
			}
		}

		public override void LoadContent()
		{
		}

		public override void DeleteContent()
		{
		}
	}
}
