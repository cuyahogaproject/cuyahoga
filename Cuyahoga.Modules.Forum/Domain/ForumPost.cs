using System;

namespace Cuyahoga.Modules.Forum.Domain
{
	public class ForumPost
	{
		private int			_id;
		private DateTime	_dateCreated;
		private DateTime	_dateModified;
		private string 		_topic;
		private int			_replytoid;
		private int			_userid;
		private string		_username;
		private string		_ip;
		private string		_message;
		private int			_forumid;
		private int			_views;
		private int			_replies;
		private int			_attachmentid;

		#region Properties
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		public DateTime DateModified
		{
			get { return this._dateModified; }
			set { this._dateModified = value; }
		}

		public DateTime DateCreated
		{
			get { return this._dateCreated; }
			set { this._dateCreated = value; }
		}

		public string Topic
		{
			get { return this._topic; }
			set { this._topic = value; }
		}

		public int ReplytoId
		{
			get { return this._replytoid; }
			set { this._replytoid = value; }
		}

		public int UserId
		{
			get { return this._userid; }
			set { this._userid = value; }
		}

		public string UserName
		{
			get { return this._username; }
			set { this._username = value; }
		}

		public string Ip
		{
			get { return this._ip; }
			set { this._ip = value; }
		}

		public string Message
		{
			get { return this._message; }
			set { this._message = value; }
		}

		public int ForumId
		{
			get { return this._forumid; }
			set { this._forumid = value; }
		}

		public int Views
		{
			get { return this._views; }
			set { this._views = value; }
		}

		public int Replies
		{
			get { return this._replies; }
			set { this._replies = value; }
		}

		public int AttachmentId
		{
			get { return this._attachmentid; }
			set { this._attachmentid = value; }
		}

		#endregion

		public ForumPost()
		{
			this._id			= -1;
			this._dateCreated	= DateTime.Now;
			this._dateModified	= DateTime.Now;
			this._attachmentid	= 0;
		}
	}
}
