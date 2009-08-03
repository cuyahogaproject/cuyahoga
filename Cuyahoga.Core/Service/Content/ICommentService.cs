using System.Collections.Generic;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Content
{
	public interface ICommentService
	{
		/// <summary>
		/// Gets all comments for the given content item.
		/// </summary>
		/// <param name="contentItem"></param>
		/// <returns></returns>
		IEnumerable<Comment> GetCommentsByContentItem(IContentItem contentItem);

		/// <summary>
		/// Removes the comment with commentId from the given content item.
		/// </summary>
		/// <param name="contentItem"></param>
		/// <param name="commentId"></param>
		void DeleteCommentForContentItem(IContentItem contentItem, int commentId);
	}
}