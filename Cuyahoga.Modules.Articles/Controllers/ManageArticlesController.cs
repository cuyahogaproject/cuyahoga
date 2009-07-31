using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Modules.Articles.Domain;
using Cuyahoga.Web.Mvc.Controllers;

namespace Cuyahoga.Modules.Articles.Controllers
{
    public class ManageArticlesController : ModuleAdminController
    {
    	private readonly IContentItemService<Article> _contentItemService;
    	private readonly ICategoryService _categoryService;

    	public ManageArticlesController(IContentItemService<Article> contentItemService, ICategoryService categoryService, ArticleModelValidator modelValidator)
    	{
    		_contentItemService = contentItemService;
    		_categoryService = categoryService;
    		ModelValidator = modelValidator;
    	}

    	public ActionResult Index()
		{
    		IEnumerable<Article> articles = this._contentItemService.FindContentItemsBySection(CurrentSection,
    		                                                                             new ContentItemQuerySettings(
    		                                                                             	ContentItemSortBy.CreatedAt,
    		                                                                             	ContentItemSortDirection.DESC));
			return View(GetModuleAdminViewModel(articles));
		}

		public ActionResult New()
		{
			return View(GetModuleAdminViewModel(new Article()));
		}

		public ActionResult Edit(long id)
		{
			Article article = this._contentItemService.GetById(id);
			return View(GetModuleAdminViewModel(article));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		public ActionResult Create([Bind(Exclude = "Id")]Article article, string categoryids)
		{
			// TODO: handle Categories etc. more generic.
			// Categories
			article.Categories.Clear();
			if (!String.IsNullOrEmpty(categoryids))
			{
				foreach (string categoryIdString in categoryids.Split(','))
				{
					article.Categories.Add(this._categoryService.GetCategoryById(Convert.ToInt32(categoryIdString)));
				}
			}
			article.Section = CurrentSection;
			if (ValidateModel(article, new [] { "Title", "Summary", "Content", "PublishedAt", "PublishedUntil" }))
			{
				try
				{
					this._contentItemService.Save(article);
					Messages.AddFlashMessageWithParams("ArticleCreatedMessage", article.Title);
					return RedirectToAction("Index", GetNodeAndSectionParams());
				}
				catch (Exception ex)
				{
					Messages.AddException(ex);
				}
			}
			return View("New", GetModuleAdminViewModel(article));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		public ActionResult Update(long id, string categoryids)
		{
			Article article = this._contentItemService.GetById(id);
			// TODO: handle Categories etc. more generic.
			// Categories
			article.Categories.Clear();
			if (!String.IsNullOrEmpty(categoryids))
			{
				foreach (string categoryIdString in categoryids.Split(','))
				{
					article.Categories.Add(this._categoryService.GetCategoryById(Convert.ToInt32(categoryIdString)));
				}
			}
			if (TryUpdateModel(article, new[] { "Title", "Summary", "Content", "Syndicate", "PublishedAt", "PublishedUntil" })
				&& ValidateModel(article, new[] { "Title", "Summary", "Content", "PublishedAt", "PublishedUntil" }))
			{
				try
				{
					this._contentItemService.Save(article);
					Messages.AddFlashMessageWithParams("ArticleUpdatedMessage", article.Title);
					return RedirectToAction("Index", GetNodeAndSectionParams());
				}
				catch (Exception ex)
				{
					Messages.AddException(ex);
				}
				return RedirectToAction("Index", GetNodeAndSectionParams());
			}
			return View("Edit", GetModuleAdminViewModel(article));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(long id)
		{
			Article article = this._contentItemService.GetById(id);
			try
			{
				this._contentItemService.Delete(article);
				Messages.AddFlashMessageWithParams("ArticleDeletedMessage", article.Title);
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Index", GetNodeAndSectionParams());
		}
    }
}
