using System;
using System.Collections;
using System.Text.RegularExpressions;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Type;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// The ArticleModule provides a news system (articles, comments, content expiration, rss feed).
	/// </summary>
	public class ArticleModule : ModuleBase, ISyndicatable, ISearchable
	{
		private int _currentArticleId;
		private int _currentCategoryId;
		private SortBy _sortBy;
		private SortDirection _sortDirection;

		/// <summary>
		/// Property CurrentArticleId (int)
		/// </summary>
		public int CurrentArticleId
		{
			get { return this._currentArticleId; }
		}

		/// <summary>
		/// Property CurrentCategory (int)
		/// </summary>
		public int CurrentCategoryId
		{
			get { return this._currentCategoryId; }
		}

		/// <summary>
		/// Default constructor. The ArticleModule registers its own Domain classes in the NHibernate
		/// SessionFactory.
		/// </summary>
		public ArticleModule(Section section) : base(section)
		{
			this._currentArticleId = -1;
			this._currentCategoryId = -1;

			SessionFactory sf = SessionFactory.GetInstance();
			// Register classes that are used by the ArticleModule
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Category));
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Article));
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Articles.Comment));

			base.SessionFactoryRebuilt = sf.Rebuild();

			try
			{
				this._sortBy = (SortBy)Enum.Parse(typeof(SortBy), section.Settings["SORT_BY"].ToString());
				this._sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), section.Settings["SORT_DIRECTION"].ToString());
			}
			catch
			{
				// Only if the module settings are not in the database yet for some reason.
				this._sortBy = SortBy.DateOnline;
				this._sortDirection = SortDirection.DESC;
			}
		}

		/// <summary>
		/// Get the available categories for the current site (via Article -> Section -> Node -> Site). 
		/// </summary>
		/// <returns></returns>
		public IList GetAvailableCategories()
		{
			try
			{
				string hql = "select distinct c from Category c, Article a where a.Category = c and a.Section.Node.Site.Id = :siteId";
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("siteId", base.Section.Node.Site.Id);
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Categories", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public Category GetCategoryById(int categoryId)
		{
			try
			{
				return (Category)base.NHSession.Load(typeof(Category), categoryId);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Category", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList GetAllArticles()
		{
			try
			{
				string hql = "from Article a where a.Section.Id = ? " + GetOrderByClause("a");
				return base.NHSession.Find(hql, this.Section.Id, TypeFactory.GetInt32Type());
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Articles", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public IList GetDisplayArticles(int number)
		{
			try
			{
				string hql = "from Article a where a.Section.Id = :sectionId and a.DateOnline < :now and a.DateOffline > :now " + GetOrderByClause("a");
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

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList GetDisplayArticlesByCategory()
		{
			try
			{
				string hql = "from Article a where a.Category.Id = :categoryId and a.DateOnline < :now and a.DateOffline > :now " + GetOrderByClause("a");
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("categoryId", this._currentCategoryId);
				q.SetDateTime("now", DateTime.Now);
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Articles", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList GetRssArticlesByCategory()
		{
			try
			{
				string hql = "from Article a where a.Category.Id = :categoryId and a.Syndicate = :syndicate and a.DateOnline < :now and a.DateOffline > :now " + GetOrderByClause("a");
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("categoryId", this._currentCategoryId);
				q.SetBoolean("syndicate", true);
				q.SetDateTime("now", DateTime.Now);
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Articles", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public IList GetRssArticles(int number)
		{
			try
			{
				string hql = "from Article a where a.Section.Id = :sectionId and a.Syndicate = :syndicate and a.DateOnline < :now and a.DateOffline > :now " + GetOrderByClause("a");
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("sectionId", base.Section.Id);
				q.SetBoolean("syndicate", true);
				q.SetDateTime("now", DateTime.Now);
				q.SetMaxResults(number);
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Articles", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="article"></param>
		public void SaveArticle(Article article)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				article.Category = HandleCategory(article.Category, base.NHSession);
				if (article.Id == -1)
				{
					article.DateModified = DateTime.Now;
					base.NHSession.Save(article);
					OnContentCreated(new IndexEventArgs(ArticleToSearchContent(article)));
				}
				else
				{
					base.NHSession.Update(article);
					OnContentUpdated(new IndexEventArgs(ArticleToSearchContent(article)));
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Article", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="article"></param>
		public void DeleteArticle(Article article)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(article);
				OnContentDeleted(new IndexEventArgs(ArticleToSearchContent(article)));
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Article", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="comment"></param>
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
		/// 
		/// </summary>
		/// <param name="comment"></param>
		public void DeleteComment(Comment comment)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(comment);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Comment", ex);
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
				
				// try to find a categoryid
				expression = @"^\/category\/(\d+)";
				Regex categoryRegex = new Regex(expression, RegexOptions.Singleline|RegexOptions.CultureInvariant|RegexOptions.Compiled);
				if (categoryRegex.IsMatch(base.ModulePathInfo))
				{
					this._currentCategoryId = Int32.Parse(categoryRegex.Match(base.ModulePathInfo).Groups[1].Value);
				}
			}
		}

		public override void DeleteModuleContent()
		{
			if (this.GetAllArticles().Count > 0)
			{
				throw new ActionForbiddenException("You have to manually delete the related Articles before you can delete the Section.");
			}
		}


		private Category HandleCategory(Category category, ISession session)
		{
			if (category != null && category.Id == -1)
			{
				// Unknown category, this could be a new one or maybe still an existing one.
				// HACK: checking if a category exists should occur before attaching it to an article.
				// Now we have to temporarily disable autoflush to prevent flushing while searching
				// for a category.
				session.FlushMode = FlushMode.Commit;
				IList categories = session.CreateCriteria(typeof(Category)).Add(Expression.Eq("Title", category.Title)).List();
				session.FlushMode = FlushMode.Auto;

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
			return category;
		}

		private SearchContent ArticleToSearchContent(Article article)
		{
			SearchContent sc = new SearchContent();
			sc.Title = article.Title;
			if (article.Summary == null || article.Summary == String.Empty)
			{
				sc.Summary = Text.TruncateText(article.Content, 200); // truncate summary to 200 chars
			}
			else
			{
				sc.Summary = article.Summary;
			}
			sc.Contents = article.Content;
			sc.Author = article.ModifiedBy.FullName;
			sc.ModuleType = this.Section.ModuleType.Name;
			sc.Path = this.SectionUrl + "/" + article.Id; // article ID has to be added as pathinfo parameter.
			if (article.Category != null)
			{
				sc.Category = article.Category.Title;
			}
			else
			{
				sc.Category = String.Empty;
			}
			sc.Site = this.Section.Node.Site.Name;
			sc.DateCreated = article.DateCreated;
			sc.DateModified = article.DateModified;
			sc.SectionId = this.Section.Id;

			return sc;
		}

		private string GetOrderByClause(string articleAlias)
		{
			if (this._sortBy == SortBy.None)
			{
				return String.Empty;
			}
			else
			{
				switch (this._sortBy)
				{
					case SortBy.DateCreated:
					case SortBy.DateModified:
					case SortBy.DateOnline:
					case SortBy.Title:
						return String.Format("order by {0}.{1} {2}", articleAlias, this._sortBy.ToString(), this._sortDirection.ToString());
					case SortBy.Category:
						return String.Format("order by {0}.Category.Title {1}", articleAlias, this._sortDirection.ToString());
					case SortBy.CreatedBy:
						return String.Format("order by {0}.CreatedBy.UserName {1}", articleAlias, this._sortDirection.ToString());
					case SortBy.ModifiedBy:
						return String.Format("order by {0}.ModifiedBy.UserName {1}", articleAlias, this._sortDirection.ToString());
					default:
						return String.Empty;
				}
			}
		}

		#region ISyndicatable Members

		public RssChannel GetRssFeed()
		{
			RssChannel channel = new RssChannel();
			channel.Title = this.Section.Title;
			// channel.Link = "" (can't determine link here because the ArticleModule has no notion of any URL context).
			channel.Description = this.Section.Title; // TODO: Section needs a description.
			channel.Language = this.Section.Node.Culture;
			channel.PubDate = DateTime.Now;
			channel.LastBuildDate = DateTime.Now;
			IList articles = null;
			// We can have an rss feed for the normal article list but also for the category view.
			if (base.ModulePathInfo.ToLower().IndexOf("category") > 0)
			{
				string expression = @"^\/category\/(\d+)";
				Regex categoryRegex = new Regex(expression, RegexOptions.Singleline|RegexOptions.CultureInvariant|RegexOptions.Compiled);
				if (categoryRegex.IsMatch(base.ModulePathInfo))
				{
					this._currentCategoryId = Int32.Parse(categoryRegex.Match(base.ModulePathInfo).Groups[1].Value);
					articles = GetRssArticlesByCategory();
					// We still need the category for the title.
					Category category = (Category)base.NHSession.Load(typeof(Category), this._currentCategoryId);
					channel.Title += String.Format(" ({0}) ", category.Title);
				}
			}
			else
			{
				int maxNumberOfItems = Int32.Parse(this.Section.Settings["NUMBER_OF_ARTICLES_IN_LIST"].ToString());
				articles = GetRssArticles(maxNumberOfItems);
			}
			
			DisplayType displayType = (DisplayType)Enum.Parse(typeof(DisplayType), this.Section.Settings["DISPLAY_TYPE"].ToString());
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
				item.Author = article.ModifiedBy.FullName;
				if (article.Category != null)
				{
					item.Category = article.Category.Title;
				}
				else
				{
					item.Category = String.Empty;
				}
				item.PubDate = article.DateOnline;
				channel.RssItems.Add(item);
			}
			return channel;
		}

		#endregion

		#region ISearchable Members

		public SearchContent[] GetAllSearchableContent()
		{
			ArrayList searchableContent = new ArrayList();
			IList articles = GetAllArticles();
			foreach (Article article in articles)
			{
				searchableContent.Add(ArticleToSearchContent(article));
			}
			return (SearchContent[])searchableContent.ToArray(typeof(SearchContent));
		}

		public event Cuyahoga.Core.Search.IndexEventHandler ContentCreated;

		protected void OnContentCreated(IndexEventArgs e)
		{
			if (ContentCreated != null)
			{
				ContentCreated(this, e);
			}
		}

		public event Cuyahoga.Core.Search.IndexEventHandler ContentDeleted;

		protected void OnContentDeleted(IndexEventArgs e)
		{
			if (ContentDeleted != null)
			{
				ContentDeleted(this, e);
			}
		}

		public event Cuyahoga.Core.Search.IndexEventHandler ContentUpdated;

		protected void OnContentUpdated(IndexEventArgs e)
		{
			if (ContentUpdated != null)
			{
				ContentUpdated(this, e);
			}
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

	/// <summary>
	/// The property to sort the articles by.
	/// </summary>
	public enum SortBy
	{
		/// <summary>
		/// Sort by DateOnline.
		/// </summary>
		DateOnline,
		/// <summary>
		/// Sort by DateCreated.
		/// </summary>
		DateCreated,
		/// <summary>
		/// Sort by DateModified.
		/// </summary>
		DateModified,
		/// <summary>
		/// Sort by Title.
		/// </summary>
		Title,
		/// <summary>
		/// Sort by Category.
		/// </summary>
		Category,
		/// <summary>
		/// Sort by the user who created the article.
		/// </summary>
		CreatedBy,
		/// <summary>
		/// Sort by the user who modified the article most recently.
		/// </summary>
		ModifiedBy,
		/// <summary>
		/// Don't sort the articles.
		/// </summary>
		None
	}
	
	/// <summary>
	/// The sort direction of the articles in the list.
	/// </summary>
	public enum SortDirection
	{
		/// <summary>
		/// Sort descending.
		/// </summary>
		DESC,
		/// <summary>
		/// Sort ascending.
		/// </summary>
		ASC
	}
}
