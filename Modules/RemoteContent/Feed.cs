using System;
using System.Collections;

namespace Cuyahoga.Modules.RemoteContent
{
	/// <summary>
	/// An rss feed to be consumed.
	/// </summary>
	public class Feed
	{
		private int _id;
		private string _url;
		private string _title;
		private DateTime _pubDate;
		private int _numberOfItems;
		private DateTime _updateTimestamp;
		private IList _feedItems;

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
		/// Property PubDate (DateTime)
		/// </summary>
		public DateTime PubDate
		{
			get { return this._pubDate; }
			set { this._pubDate = value; }
		}

		/// <summary>
		/// Property NumberOfItems (int)
		/// </summary>
		public int NumberOfItems
		{
			get { return this._numberOfItems; }
			set { this._numberOfItems = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
		}

		/// <summary>
		/// Property Items (IList)
		/// </summary>
		public IList FeedItems
		{
			get { return this._feedItems; }
			set { this._feedItems = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Feed()
		{
			this._id = -1;
			this._numberOfItems = 5;
		}
	}
}
