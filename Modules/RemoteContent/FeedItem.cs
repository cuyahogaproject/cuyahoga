using System;

namespace Cuyahoga.Modules.RemoteContent
{
	/// <summary>
	/// Feed item.
	/// </summary>
	public class FeedItem
	{
		private int _id;
		private string _url;
		private string _title;
		private string _content;
		private DateTime _pubDate;
		private string _author;
		private Feed _feed;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property Url (string)
		/// </summary>
		public string Url
		{
			get { return this._url; }
			set { this._url = value; }
		}

		/// <summary>
		/// Property Title (string)
		/// </summary>
		public string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// Property Content (string)
		/// </summary>
		public string Content
		{
			get { return this._content; }
			set { this._content = value; }
		}

		/// <summary>
		/// Property PubDate (DateTime)
		/// </summary>
		public DateTime PubDate
		{
			get { return this._pubDate; }
			set { this._pubDate = value; }
		}

		/// <summary>
		/// Property Author (string)
		/// </summary>
		public string Author
		{
			get { return this._author; }
			set { this._author = value; }
		}

		/// <summary>
		/// Property Feed (Feed)
		/// </summary>
		public Feed Feed
		{
			get { return this._feed; }
			set { this._feed = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public FeedItem()
		{
			this._id = -1;
		}
	}
}
