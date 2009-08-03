using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Web.Mvc.Controllers;

namespace Cuyahoga.Web.Modules.Shared.Controllers
{
	public class CommentsController : BaseController
	{
		private readonly ICommentService _commentService;
		private readonly IContentItemService<ContentItem> _contentItemService;

		public CommentsController(ICommentService commentService, IContentItemService<ContentItem> contentItemService)
		{
			_commentService = commentService;
			_contentItemService = contentItemService;
		}

		public ActionResult ViewByContentItem(long contentItemId)
		{
			IContentItem contentItem = this._contentItemService.GetById(contentItemId);
			return View("CommentsList", contentItem.Comments);
		}

		public ActionResult DeleteCommentForContentItem(int id, long contentItemId)
		{
			IContentItem contentItem = this._contentItemService.GetById(contentItemId);
			this._commentService.DeleteCommentForContentItem(contentItem, id);
			return RedirectToAction("ViewByContentItem", new {contentitemid = contentItemId});
		}
	}
}
