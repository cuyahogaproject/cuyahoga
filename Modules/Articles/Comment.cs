using System;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// Summary description for Comment.
	/// </summary>
	public class Comment
	{
		private int _id;
		private int _articleId;
		private int _userId;
		private string _commentText;

		#region properties

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property ArticleId (int)
		/// </summary>
		public int ArticleId
		{
			get { return this._articleId; }
			set { this._articleId = value; }
		}

		/// <summary>
		/// Property UserId (int)
		/// </summary>
		public int UserId
		{
			get { return this._userId; }
			set { this._userId = value; }
		}

		/// <summary>
		/// Property CommentText (string)
		/// </summary>
		public string CommentText
		{
			get { return this._commentText; }
			set { this._commentText = value; }
		}

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Comment()
		{
			this._id = -1;
			this._articleId = -1;
			this._userId = -1;
		}
	}
}
