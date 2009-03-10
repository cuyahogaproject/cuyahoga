using System;
using System.Collections;
using System.Text.RegularExpressions;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Communication;
using Cuyahoga.Modules.Articles.Domain;

using ArticleCategory = Cuyahoga.Modules.Articles.Domain.Category;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Articles.DataAccess;
using Cuyahoga.Core.DataAccess;
using Castle.Services.Transaction;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// The ArticleModule provides a news system (articles, comments, content expiration, rss feed).
	/// </summary>
	[Transactional]
	public class ArticleModule : ModuleBase, ISyndicatable, ISearchable, IActionProvider, INHibernateModule
	{
		private ICommonDao _commonDao;
		private IArticleDao _articleDao;

		private int _currentArticleId;
		private int _currentCategoryId;
		private bool _isArchive;
		private bool _allowComments;
		private bool _allowAnonymousComments;
		private bool _allowSyndication;
		private bool _showArchive;
		private bool _showCategory;
		private bool _showAuthor;
		private bool _showDateTime;
		private int _numberOfArticlesInList;
		private DisplayType _displayType;
		private SortBy _sortBy;
		private SortDirection _sortDirection;
		private ArticleModuleAction _currentAction;

		#region properties

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
		/// 
		/// </summary>
		public ArticleModuleAction CurrentAction
		{
			get { return this._currentAction; }
		}

		/// <summary>
		/// Show archived articles instead of the current ones.
		/// </summary>
		public bool IsArchive
		{
			get { return this._isArchive; }
		}

		/// <summary>
		/// Allow syndication of articles?
		/// </summary>
		public bool AllowSyndication
		{
			get { return this._allowSyndication; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowComments
		{
			get { return this._allowComments; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowAnonymousComments
		{
			get { return this._allowAnonymousComments; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int NumberOfArticlesInList
		{
			get { return this._numberOfArticlesInList; }
		}

		/// <summary>
		/// Show a link to archived articles?
		/// </summary>
		public bool ShowArchive
		{
			get { return this._showArchive; }
		}

		/// <summary>
		/// Show the article category?
		/// </summary>
		public bool ShowCategory
		{
			get { return _showCategory; }
		}

		/// <summary>
		/// Show article author?
		/// </summary>
		public bool ShowAuthor
		{
			get { return this._showAuthor; }
		}

		/// <summary>
		/// Show article date and time?
		/// </summary>
		public bool ShowDateTime
		{
			get { return _showDateTime;	}
		}

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ArticleModule(ICommonDao commonDao, IArticleDao articleDao)
		{
			this._commonDao = commonDao;
			this._articleDao = articleDao;
			this._currentArticleId = -1;
			this._currentCategoryId = -1;
			this._currentAction = ArticleModuleAction.List;
		}

		public override void ReadSectionSettings()
		{
			base.ReadSectionSettings();

			try
			{
				this._allowComments = Convert.ToBoolean(base.Section.Settings["ALLOW_COMMENTS"]);
				this._allowAnonymousComments = Convert.ToBoolean(base.Section.Settings["ALLOW_ANONYMOUS_COMMENTS"]);
				this._allowSyndication = Convert.ToBoolean(base.Section.Settings["ALLOW_SYNDICATION"]);
				this._showArchive = Convert.ToBoolean(base.Section.Settings["SHOW_ARCHIVE"]);
				this._showAuthor = Convert.ToBoolean(base.Section.Settings["SHOW_AUTHOR"]);
				this._showCategory = Convert.ToBoolean(base.Section.Settings["SHOW_CATEGORY"]);
				this._showDateTime = Convert.ToBoolean(base.Section.Settings["SHOW_DATETIME"]);
				this._numberOfArticlesInList = Convert.ToInt32(base.Section.Settings["NUMBER_OF_ARTICLES_IN_LIST"]);
				this._displayType = (DisplayType)Enum.Parse(typeof(DisplayType), base.Section.Settings["DISPLAY_TYPE"].ToString());
				this._sortBy = (SortBy)Enum.Parse(typeof(SortBy), base.Section.Settings["SORT_BY"].ToString());
				this._sortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), base.Section.Settings["SORT_DIRECTION"].ToString());
			}
			catch
			{
				// Only if the module settings are not in the database yet for some reason.
				this._sortBy = SortBy.DateOnline;
				this._sortDirection = SortDirection.DESC;
			}
		}

		/// <summary>
		/// This module doesn't automatically delete content.
		/// </summary>
		public override void DeleteModuleContent()
		{
			if (this.GetAllArticles().Count > 0)
			{
				throw new ActionForbiddenException("You have to manually delete the related Articles before you can delete the Section.");
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
				if (Regex.IsMatch(base.ModulePathInfo, expression, RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled))
				{
					this._currentArticleId = Int32.Parse(Regex.Match(base.ModulePathInfo, expression).Groups[1].Value);
					this._currentAction = ArticleModuleAction.Details;
					return;
				}

				// try to find a categoryid
				expression = @"^\/category\/(\d+)";
				if (Regex.IsMatch(base.ModulePathInfo, expression, RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled))
				{
					this._currentCategoryId = Int32.Parse(Regex.Match(base.ModulePathInfo, expression).Groups[1].Value);
					this._currentAction = ArticleModuleAction.Category;
					return;
				}

				expression = @"^\/archive";
				if (Regex.IsMatch(base.ModulePathInfo, expression, RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled))
				{
					this._isArchive = true;
					this._currentAction = ArticleModuleAction.Archive;
					return;
				}
			}
		}

		/// <summary>
		/// The current view user control based on the action that was set while parsing the pathinfo.
		/// </summary>
		public override string CurrentViewControlPath
		{
			get
			{
				string basePath = "Modules/Articles/";
				switch (this._currentAction)
				{
					case ArticleModuleAction.List:
					case ArticleModuleAction.Category:
					case ArticleModuleAction.Archive:
						return basePath + "Articles.ascx";
					case ArticleModuleAction.Details:
						return basePath + "ArticleDetails.ascx";
					default:
						return basePath + "Articles.ascx";
				}
			}
		}

		#region Data Access

		/// <summary>
		/// Get the available categories for the current site (via Article -> Section -> Node -> Site). 
		/// </summary>
		/// <returns></returns>
		public IList GetAvailableCategories()
		{
			return this._articleDao.GetAvailableCategoriesBySite(base.Section.Node.Site);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		public ArticleCategory GetCategoryById(int categoryId)
		{
			return (ArticleCategory)this._commonDao.GetObjectById(typeof(ArticleCategory), categoryId);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList GetAllArticles()
		{
			return this._articleDao.GetAllArticlesBySection(base.Section, this._sortBy, this._sortDirection);
		}

		/// <summary>
		/// Get this list of articles
		/// </summary>
		/// <returns></returns>
		public IList GetArticleList()
		{
			switch (this._currentAction)
			{
				case ArticleModuleAction.List:
					return this._articleDao.GetDisplayArticlesBySection(base.Section, this._sortBy, this._sortDirection);
				case ArticleModuleAction.Category:
					return this._articleDao.GetDisplayArticlesByCategory((ArticleCategory)this._commonDao.GetObjectById(typeof(ArticleCategory)
						, this._currentCategoryId), this._sortBy, this._sortDirection);
				case ArticleModuleAction.Archive:
					return this._articleDao.GetArchivedArticlesBySection(base.Section, this._sortBy, this._sortDirection);
				default:
					return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList GetRssArticlesByCategory()
		{
			return this._articleDao.GetRssArticlesByCategory((ArticleCategory)this._commonDao.GetObjectById(typeof(ArticleCategory), this._currentCategoryId)
				, this._sortBy, this._sortDirection);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public IList GetRssArticles(int number)
		{
			return this._articleDao.GetRssArticles(base.Section, this._numberOfArticlesInList, this._sortBy, this._sortDirection);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Article GetArticleById(int id)
		{
			return (Article)this._commonDao.GetObjectById(typeof(Article), id);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="article"></param>
		[Transaction(TransactionMode.RequiresNew)]
		public virtual void SaveArticle(Article article)
		{			
			article.Category = HandleCategory(article.Category);
			if (article.Id == -1)
			{
				this._commonDao.SaveOrUpdateObject(article);
				OnContentCreated(new IndexEventArgs(ArticleToSearchContent(article)));
			}
			else
			{
				this._commonDao.SaveOrUpdateObject(article);
				OnContentUpdated(new IndexEventArgs(ArticleToSearchContent(article)));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="article"></param>
		[Transaction(TransactionMode.RequiresNew)]
		public virtual void DeleteArticle(Article article)
		{
			this._commonDao.DeleteObject(article);
			OnContentDeleted(new IndexEventArgs(ArticleToSearchContent(article)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="comment"></param>
		[Transaction(TransactionMode.RequiresNew)]
		public virtual void SaveComment(Comment comment)
		{
			this._commonDao.SaveOrUpdateObject(comment);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="comment"></param>
		[Transaction(TransactionMode.RequiresNew)]
		public virtual void DeleteComment(Comment comment)
		{
			this._commonDao.DeleteObject(comment);
		}

		private ArticleCategory HandleCategory(ArticleCategory category)
		{
			if (category != null && category.Id == -1)
			{
				// Unknown category, this could be a new one or maybe still an existing one.
				ArticleCategory cat = this._articleDao.FindCategoryByTitleAndSite(category.Title, base.Section.Node.Site);
				

				if (cat != null)
				{
					// Category already exists.
					category = cat;
				}
				else
				{
					// Insert the new one, so the Id will be generated and retrieved.
					category.Site = base.Section.Node.Site;
					this._commonDao.SaveOrUpdateObject(category);
				}
			}
			return category;
		}

		#endregion

		/// <summary>
		/// Get the url of a user profile. This only works if this module is connected to a UserProfile module.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public string GetProfileUrl(int userId)
		{
			Section sectionTo = this.Section.Connections["ViewProfile"] as Section;
			if (sectionTo != null)
			{
				return UrlHelper.GetUrlFromSection(sectionTo) + "/ViewProfile/" + userId.ToString();
			}
			else
			{
				return null;
			}
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
			sc.Category = (article.Category != null ? article.Category.Title : sc.Category = String.Empty);
			sc.Site = (this.Section.Node != null ? this.Section.Node.Site.Name : String.Empty);
			sc.DateCreated = article.DateCreated;
			sc.DateModified = article.DateModified;
			sc.SectionId = this.Section.Id;

			return sc;
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
					ArticleCategory category = (ArticleCategory)base.NHSession.Load(typeof(ArticleCategory), this._currentCategoryId);
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

		#region IActionProvider Members
		
		/// <summary>
		/// Returns a list of outbound actions.
		/// </summary>
		/// <returns></returns>
		public ModuleActionCollection GetOutboundActions()
		{
			ModuleActionCollection moduleActions = new ModuleActionCollection();
			// This action is for example compatible with the ViewProfile inbound action of 
			// the ProfileModule but it's also possible to write your own Profile module that
			// has a compatible inbound action.
			moduleActions.Add(new ModuleAction("ViewProfile", new string[1] {"UserId"}));
			return moduleActions;
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

	public enum ArticleModuleAction
	{
		/// <summary>
		/// Show the list of articles.
		/// </summary>
		List,
		/// <summary>
		/// Show a single article.
		/// </summary>
		Details,
		/// <summary>
		/// Show all articles for a particular category.
		/// </summary>
		Category,
		/// <summary>
		/// Show all expired articles.
		/// </summary>
		Archive
	}
}
