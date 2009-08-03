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
		/// Gets a single comment by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Comment GetById(int id)
		{
			return this._commonDao.GetObjectById<Comment>(id);
		}

		/// <summary>
		/// Saves a comment.
		/// </summary>
		/// <param name="comment"></param>
		[Transaction(TransactionMode.Requires)]
		public void SaveComment(Comment comment)
		{
			comment.ContentItem.Comments.Add(comment);
			this._commonDao.SaveObject(comment);
		}

		/// <summary>
		/// Delete a comment.
		/// </summary>
		/// <param name="comment"></param>
		[Transaction(TransactionMode.Requires)]
		public void DeleteComment(Comment comment)
		{
			comment.ContentItem.Comments.Remove(comment);
			this._commonDao.DeleteObject(comment);
		}
	}
}