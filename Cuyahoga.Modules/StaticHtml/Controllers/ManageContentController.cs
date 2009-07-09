using System;
using System.Linq;
using System.Web.Mvc;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Web.Mvc.Controllers;
using Cuyahoga.Web.Mvc.ViewModels;

namespace Cuyahoga.Modules.StaticHtml.Controllers
{
	public class ManageContentController : ModuleAdminController
	{
		private readonly IContentItemService<StaticHtmlContent> _contentItemService;

		public ManageContentController(IContentItemService<StaticHtmlContent> contentItemService)
		{
			_contentItemService = contentItemService;
		}

		public ActionResult Edit()
		{
			StaticHtmlContent htmlContent =
				this._contentItemService.FindContentItemsBySection(CurrentSection).FirstOrDefault()
				?? new StaticHtmlContent();
			return View(new ModuleAdminViewModel<StaticHtmlContent>(CurrentNode, CurrentSection, htmlContent));
		}

		[ValidateInput(false)]
		public ActionResult SaveContent()
		{
			StaticHtmlContent htmlContent =
				this._contentItemService.FindContentItemsBySection(CurrentSection).FirstOrDefault()
				?? new StaticHtmlContent();
			if (TryUpdateModel(htmlContent, new [] { "Content"}))
			{
				// TODO: handle CreatedBy etc. more generic.
				if (htmlContent.IsNew)
				{
					htmlContent.Title = CurrentSection.Title;
					htmlContent.CreatedBy = CuyahogaContext.CurrentUser;
					htmlContent.Section = CurrentSection;
				}
				htmlContent.ModifiedBy = CuyahogaContext.CurrentUser;
				htmlContent.ModifiedAt = DateTime.Now;
				try
				{
					this._contentItemService.Save(htmlContent);
					Messages.AddFlashMessage("Content is saved successfully.");
					return RedirectToAction("Edit", "ManageContent", GetNodeAndSectionParams());
				}
				catch (Exception ex)
				{
					Messages.AddException(ex);
				}
			}
			return View("Edit", htmlContent);
		}
	}
}
