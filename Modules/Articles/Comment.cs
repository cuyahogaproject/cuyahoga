using System;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// Summary description for Comment.
	/// </summary>
	public class Comment
	{
		private int _id;
		private int _userId;
		private string _commentText;
		private Article _article;
		private Cuyahoga.Core.Domain.User _user;

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
		/// Property Article (Article)
		/// </summary>
		public Article Article
		{
			get { return this._article; }
			set { this._article = value; }
		}

		/// <summary>
		/// Property User (Cuyahoga.Core.User)
		/// </summary>
		public Cuyahoga.Core.Domain.User User
		{
			get { return this._user; }
			set { this._user = value; }
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
		}

	}
}
