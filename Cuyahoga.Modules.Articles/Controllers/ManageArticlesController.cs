using System.Collections.Generic;
using System.Web.Mvc;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Modules.Articles.Domain;
using Cuyahoga.Web.Mvc.Controllers;
using Cuyahoga.Web.Mvc.ViewModels;

namespace Cuyahoga.Modules.Articles.Controllers
{
    public class ManageArticlesController : ModuleAdminController
    {
    	private readonly IContentItemService<Article> _contentItemService;

    	public ManageArticlesController(IContentItemService<Article> contentItemService)
    	{
    		_contentItemService = contentItemService;
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
    }
}
