using System;
using System.Collections;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// A container class for a RSS feed (for generation purposes).
	/// </summary>
	public class RssChannel
	{
		private string _title;
		private string _link;
		private string _description;
		private string _language;
		private DateTime _pubDate;
		private DateTime _lastBuildDate;
		private string _generator;
		private int _ttl;
		private IList _rssItems;

		/// <summary>
		/// Property Title (string)
		/// </summary>
		public string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// Property Link (string)
		/// </summary>
		public string Link
		{
			get { return this._link; }
			set { this._link = value; }
		}

		/// <summary>
		/// Property Description (string)
		/// </summary>
		public string Description
		{
			get { return this._description; }
			set { this._description = value; }
		}

		/// <summary>
		/// Property Language (string)
		/// </summary>
		public string Language
		{
			get { return this._language; }
			set { this._language = value; }
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
		/// Property LastBuildDate (DateTime)
		/// </summary>
		public DateTime LastBuildDate
		{
			get { return this._lastBuildDate; }
			set { this._lastBuildDate = value; }
		}

		/// <summary>
		/// Property Generator (string)
		/// </summary>
		public string Generator
		{
			get { return this._generator; }
			set { this._generator = value; }
		}

		/// <summary>
		/// Property Ttl (int)
		/// </summary>
		public int Ttl
		{
			get { return this._ttl; }
			set { this._ttl = value; }
		}

		/// <summary>
		/// Property RssItems (IList)
		/// </summary>
		public IList RssItems
		{
			get { return this._rssItems; }
			set { this._rssItems = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public RssChannel()
		{
			// Set some defaults.
			this._generator = "Cuyahoga Website Framework";
			this._ttl = 60;
			this._rssItems = new ArrayList();
		}
	}

	/// <summary>
	/// A RSS feed item.
	/// </summary>
	public class RssItem
	{
		private int _itemId;
		private string _title;
		private string _link;
		private string _description;
		private string _author;
		private string _category;
		private DateTime _pubDate;

		/// <summary>
		/// Property ItemId (int)
		/// </summary>
		public int ItemId
		{
			get { return this._itemId; }
			set { this._itemId = value; }
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
		/// Property Link (string)
		/// </summary>
		public string Link
		{
			get { return this._link; }
			set { this._link = value; }
		}

		/// <summary>
		/// Property Description (string)
		/// </summary>
		public string Description
		{
			get { return this._description; }
			set { this._description = value; }
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
		/// Property Category (string)
		/// </summary>
		public string Category
		{
			get { return this._category; }
			set { this._category = value; }
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
		/// Default constructor.
		/// </summary>
		public RssItem()
		{
			this._itemId = -1;
		}
	}
}
