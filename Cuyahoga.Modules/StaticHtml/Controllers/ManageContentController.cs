using System;
using System.Linq;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Web.Mvc.Controllers;

namespace Cuyahoga.Modules.StaticHtml.Controllers
{
	public class ManageContentController : ModuleAdminController
	{
		private readonly IContentItemService<StaticHtmlContent> _contentItemService;

		public ManageContentController(IContentItemService<StaticHtmlContent> contentItemService, ISectionService sectionService)
		{
			_contentItemService = contentItemService;
		}

		public ActionResult Edit()
		{
			StaticHtmlContent htmlContent =
				this._contentItemService.FindContentItemsBySection(base.CurrentSection).FirstOrDefault()
				?? new StaticHtmlContent();
			return View(htmlContent);
		}

		[ValidateInput(false)]
		public ActionResult SaveContent()
		{
			StaticHtmlContent htmlContent =
				this._contentItemService.FindContentItemsBySection(base.CurrentSection).FirstOrDefault()
				?? new StaticHtmlContent();
			if (TryUpdateModel(htmlContent, new [] { "Content"}))
			{
				// TODO: handle CreatedBy etc. more generic.
				if (htmlContent.IsNew)
				{
					htmlContent.Title = base.CurrentSection.Title;
					htmlContent.CreatedBy = CuyahogaContext.CurrentUser;
					htmlContent.Section = base.CurrentSection;
				}
				htmlContent.ModifiedBy = CuyahogaContext.CurrentUser;
				htmlContent.ModifiedAt = DateTime.Now;
				this._contentItemService.Save(htmlContent);
				Messages.AddFlashMessage("Content is saved successfully.");
				return RedirectToAction("Edit");
			}
			return View("Edit", htmlContent);
		}
	}
}
