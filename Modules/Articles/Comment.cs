using System;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// Summary description for Comment.
	/// </summary>
	public class Comment
	{
		private int _id;
		private string _commentText;
		private Article _article;
		private Cuyahoga.Core.Domain.User _user;
		private string _name;
		private string _website;
		private string _userIp;
		private DateTime _updateTimestamp;

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
		/// Property Name (string)
		/// </summary>
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property Website (string)
		/// </summary>
		public string Website
		{
			get { return this._website; }
			set { this._website = value; }
		}

		/// <summary>
		/// Property CommentText (string)
		/// </summary>
		public string CommentText
		{
			get { return this._commentText; }
			set { this._commentText = value; }
		}

		/// <summary>
		/// Property UserIp (string)
		/// </summary>
		public string UserIp
		{
			get { return this._userIp; }
			set { this._userIp = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
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
