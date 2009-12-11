using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Content
{
	public interface ICommentService
	{
		/// <summary>
		/// Get a single comment by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Comment GetById(int id);

		/// <summary>
		/// Saves a comment.
		/// </summary>
		/// <param name="comment"></param>
		void SaveComment(Comment comment);

		/// <summary>
		/// Delete a comment.
		/// </summary>
		/// <param name="comment"></param>
		void DeleteComment(Comment comment);
	}
}