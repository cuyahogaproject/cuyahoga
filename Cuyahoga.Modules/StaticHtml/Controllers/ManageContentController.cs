using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Web.Mvc.Controllers;
using Cuyahoga.Web.Mvc.ViewModels;

namespace Cuyahoga.Modules.StaticHtml.Controllers
{
	public class ManageContentController : ModuleAdminController
	{
		private readonly IContentItemService<StaticHtmlContent> _contentItemService;
		private readonly ICategoryService _categoryService;

		public ManageContentController(IContentItemService<StaticHtmlContent> contentItemService, ICategoryService categoryService)
		{
			_contentItemService = contentItemService;
			_categoryService = categoryService;
		}

		public ActionResult Edit()
		{
			StaticHtmlContent htmlContent =
				this._contentItemService.FindContentItemsBySection(CurrentSection).FirstOrDefault()
				?? new StaticHtmlContent();
			return View(new ModuleAdminViewModel<StaticHtmlContent>(CurrentNode, CurrentSection, CuyahogaContext, htmlContent));
		}

		[ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult SaveContent(string categoryids)
		{
			StaticHtmlContent htmlContent =
				this._contentItemService.FindContentItemsBySection(CurrentSection).FirstOrDefault()
				?? new StaticHtmlContent();
			if (TryUpdateModel(htmlContent, new [] { "Content" }))
			{
				// TODO: handle Categories etc. more generic.
				// Categories
				htmlContent.Categories.Clear();
				foreach(string categoryIdString in categoryids.Split(','))
				{
					htmlContent.Categories.Add(this._categoryService.GetCategoryById(Convert.ToInt32(categoryIdString)));
				}
				if (htmlContent.IsNew)
				{
					htmlContent.Title = CurrentSection.Title;
					htmlContent.Section = CurrentSection;
				}
				try
				{
					this._contentItemService.Save(htmlContent);
					Messages.AddFlashMessage(Localizer.GetString("ContentSavedMessage"));
					return RedirectToAction("Edit", "ManageContent", GetNodeAndSectionParams());
				}
				catch (Exception ex)
				{
					Messages.AddException(ex);
				}
			}
			return View("Edit", new ModuleAdminViewModel<StaticHtmlContent>(CurrentNode, CurrentSection, CuyahogaContext, htmlContent));
		}
	}
}
