using System;
using System.Collections;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Expression;

using Cuyahoga.Core;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// The ArticleModule provides a news system (articles, comments, content expiration, rss feed).
	/// </summary>
	public class ArticleModule : Module
	{
		Configuration _config;
		ISessionFactory _factory;

		public ArticleModule()
		{
			base.Section = null;

			this._config = new Configuration();
			this._config.AddClass(typeof(Cuyahoga.Modules.Articles.Category));
			this._config.AddClass(typeof(Cuyahoga.Modules.Articles.Article));
			this._config.AddClass(typeof(Cuyahoga.Modules.Articles.Comment));
			this._factory = this._config.BuildSessionFactory();
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
