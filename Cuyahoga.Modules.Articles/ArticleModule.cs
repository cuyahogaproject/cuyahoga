using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.Web.Mvc;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Communication;
using Cuyahoga.Modules.Articles.Domain;
using Cuyahoga.Web.Mvc;
using Cuyahoga.Web.Mvc.Areas;
using Cuyahoga.Core.DataAccess;
using Castle.Services.Transaction;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// The ArticleModule provides a news system (articles, comments, content expiration, rss feed).
	/// </summary>
	[Transactional]
	public class ArticleModule : ModuleBase, IActionProvider, INHibernateModule, IMvcModule
	{
		private readonly ICommonDao _commonDao;
		private readonly IContentItemService<Article> _contentItemService;
		private readonly ICategoryService _categoryService;

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
		public ArticleModule(ICommonDao commonDao, IContentItemService<Article> contentItemService, ICategoryService categoryService)
		{
			this._commonDao = commonDao;
			this._contentItemService = contentItemService;
			this._categoryService = categoryService;
			this._currentArticleId = -1;
			this._currentCategoryId = -1;
			this._currentAction = ArticleModuleAction.List;
		}

		#region ModuleBase overrides

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
			if (this._contentItemService.FindContentItemsBySection(base.Section).Count > 0)
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

		#endregion

		#region DataAccess for WebForms controls

		/// <summary>
		/// Get this list of articles
		/// </summary>
		/// <returns></returns>
		public IList<Article> GetArticleList()
		{
			switch (this._currentAction)
			{
				case ArticleModuleAction.List:
					return this._contentItemService.FindVisibleContentItemsBySection(base.Section, CreateQuerySettingsForModule());
				case ArticleModuleAction.Category:
					return this._contentItemService.FindVisibleContentItemsByCategory(
						this._commonDao.GetObjectById<Category>(this._currentCategoryId), CreateQuerySettingsForModule());
				case ArticleModuleAction.Archive:
					return this._contentItemService.FindArchivedContentItemsBySection(base.Section, CreateQuerySettingsForModule());
				default:
					return null;
			}
		}

		/// <summary>
		/// Gets a single article by Id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Article GetArticleById(int id)
		{
			return this._commonDao.GetObjectById<Article>(id);
		}

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
				return String.Format("{0}/ViewProfile/{1}", UrlUtil.GetUrlFromSection(sectionTo), userId);
			}
			return null;
		}


		public void SaveComment(Comment comment)
		{
			throw new NotImplementedException();
		}

		public Category GetCategoryById(int categoryId)
		{
			return this._categoryService.GetCategoryById(categoryId);
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

		#region IMvcModule members

		public void RegisterRoutes(RouteCollection routes)
		{
			routes.CreateArea("Modules/Articles", "Cuyahoga.Modules.Articles.Controllers",
				routes.MapRoute("ArticlesRoute", "Modules/Articles/{controller}/{action}/{id}", new { action = "Index", controller = "", id = "" })
			);
		}

		#endregion

		private ContentItemQuerySettings CreateQuerySettingsForModule()
		{
			return CreateQuerySettingsForModule(null, null);
		}

		private ContentItemQuerySettings CreateQuerySettingsForModule(int? pageSize, int? pageNumber)
		{
			// Map sortby and sortdirection to the ones of the content item. We don't use these in the articles module
			// because we need to convert all module settings for. Maybe sometime in the future.
			ContentItemSortBy sortBy = ContentItemSortBy.None;
			switch(this._sortBy)
			{
				case SortBy.CreatedBy:
					sortBy = ContentItemSortBy.CreatedBy;
					break;
				case SortBy.DateCreated:
					sortBy = ContentItemSortBy.CreatedAt;
					break;
				case SortBy.DateModified:
					sortBy = ContentItemSortBy.ModifiedAt;
					break;
				case SortBy.DateOnline:
					sortBy = ContentItemSortBy.PublishedAt;
					break;
				case SortBy.ModifiedBy:
					sortBy = ContentItemSortBy.ModifiedBy;
					break;
				case SortBy.Title:
					sortBy = ContentItemSortBy.Title;
					break;
			}
			ContentItemSortDirection sortDirection = this._sortDirection == SortDirection.ASC
			                                         	? ContentItemSortDirection.ASC
			                                         	: ContentItemSortDirection.DESC;
			return new ContentItemQuerySettings(sortBy, sortDirection, pageSize, pageNumber);
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
