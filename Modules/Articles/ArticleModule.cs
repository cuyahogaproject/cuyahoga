using System;
using System.Collections;
using System.Text.RegularExpressions;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Type;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// The ArticleModule provides a news system (articles, comments, content expiration, rss feed).
	/// </summary>
	public class ArticleModule : ModuleBase, ISyndicatable
	{
		private int _currentArticleId;
		private string _currentCategory;
		private bool _generateRss;

		/// <summary>
		/// Property CurrentArticleId (int)
		/// </summary>
		public int CurrentArticleId
		{
			get { return this._currentArticleId; }
		}

		/// <summary>
		/// Property CurrentCategory (string)
		/// </summary>
		public string CurrentCategory
		{
			get { return this._currentCategory; }
		}

		/// <summary>
		/// Property GenerateRss (bool)
		/// </summary>
		public bool GenerateRss
		{
			get { return this._generateRss; }
		}

		/// <summary>
		/// Default constructor. The ArticleModule registers its own Domain classes in the NHibernate
		/// SessionFactory.
		/// </summary>
		public ArticleModule()
		{
			this._generateRss = false;

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

		public IList GetRssArticles(int number)
		{
			try
			{
				string hql = "from Article a where a.Section.Id = :sectionId and a.Syndicate = true and a.DateOnline < :now and a.DateOffline > :now order by a.DateOnline asc ";
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("sectionId", base.Section.Id);
				q.SetDateTime("now", DateTime.Now);
				q.SetFirstResult(0);
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
					article.DateModified = DateTime.Now;
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

		public void SaveComment(Comment comment)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				if (comment.Id == -1)
				{
					comment.UpdateTimestamp = DateTime.Now;
					base.NHSession.Save(comment);
				}
				else
				{
					base.NHSession.Update(comment);
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Comment", ex);
			}
		}

		/// <summary>
		/// Parse the pathinfo. Translate pathinfo parameters into member variables.
		/// </summary>
		protected override void ParsePathInfo()
		{
			if (base.ModulePathInfo != null)
			{
				// try to find an articleId
				string expression = @"^\/(\d+)";
				Regex articleIdRegEx = new Regex(expression, RegexOptions.Singleline|RegexOptions.CultureInvariant|RegexOptions.Compiled);
				if (articleIdRegEx.IsMatch(base.ModulePathInfo))
				{					
					this._currentArticleId = Int32.Parse(articleIdRegEx.Match(base.ModulePathInfo).Groups[1].Value);
				}
				
				// try to find a category
				expression = @"^\/([^0-9])";
				Regex categoryRegex = new Regex(expression, RegexOptions.Singleline|RegexOptions.CultureInvariant|RegexOptions.Compiled);
				if (categoryRegex.IsMatch(base.ModulePathInfo))
				{
					this._currentCategory = articleIdRegEx.Match(base.ModulePathInfo).Groups[1].Value;
				}
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
					category.UpdateTimestamp = DateTime.Now;
					session.Save(category);
				}
			}
		}
		#region ISyndicatable Members

		public RssChannel GetRssFeed()
		{
			RssChannel channel = new RssChannel();
			channel.Title = this.Section.Title;
			// channel.Link = "" (can't determine link here)
			channel.Description = this.Section.Title; // TODO: Section needs a description.
			channel.Language = this.Section.Node.Culture;
			channel.PubDate = DateTime.Now;
			channel.LastBuildDate = DateTime.Now;
			int maxNumberOfItems = Int32.Parse(this.Section.Settings["NUMBER_OF_ARTICLES_IN_LIST"].ToString());
			DisplayType displayType = (DisplayType)Enum.Parse(typeof(DisplayType), this.Section.Settings["DISPLAY_TYPE"].ToString());
			IList articles = GetRssArticles(maxNumberOfItems);
			foreach (Article article in articles)
			{
				RssItem item = new RssItem();
				item.ItemId = article.Id;
				item.Title = article.Title;
				// item.Link = "" (can't determine link here)
				if (displayType == DisplayType.FullContent)
				{
					item.Description = article.Content;
				}
				else
				{
					item.Description = article.Summary;
				}
				item.Author = article.ModifiedBy.Email;
				item.Category = article.Category.Title;
				item.PubDate = article.DateOnline;
				channel.RssItems.Add(item);
			}
			return channel;
		}

		#endregion
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
