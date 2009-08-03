using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Services.Transaction;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Content
{
	/// <summary>
	/// Comment services.
	/// </summary>
	[Transactional]
	public class CommentService : ICommentService
	{
		private readonly ICommonDao _commonDao;

		/// <summary>
		/// Creates a new instance of the <see cref="CommentService" /> class.
		/// </summary>
		/// <param name="commonDao"></param>
		public CommentService(ICommonDao commonDao)
		{
			_commonDao = commonDao;
		}

		/// <summary>
		/// Gets all comments for the given content item.
		/// </summary>
		/// <param name="contentItem"></param>
		/// <returns></returns>
		public IEnumerable<Comment> GetCommentsByContentItem(IContentItem contentItem)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the comment with commentId from the given content item.
		/// </summary>
		/// <param name="contentItem"></param>
		/// <param name="commentId"></param>
		[Transaction(TransactionMode.Requires)]
		public void DeleteCommentForContentItem(IContentItem contentItem, int commentId)
		{
			Comment commentToDelete = contentItem.Comments.SingleOrDefault(c => c.Id == commentId);
			if (commentToDelete == null)
			{
				throw new ArgumentException(
					string.Format("The comment with id {0} could not be found in the comments of the given content item with id {1}.",
					              commentId, contentItem.Id));
			}
			contentItem.Comments.Remove(commentToDelete);
			this._commonDao.DeleteObject(commentToDelete);
		}
	}
}