using System;

namespace Cuyahoga.Core.Search
{
	/// <summary>
	/// Class that holds the content to be indexed for a single document.
	/// </summary>
	public class SearchContent
	{
		private string _title;
		private string _summary;
		private string _contents;
		private string _author;
		private string _moduleType;
		private string _path;
		private string _category;
		private string _site;
		private DateTime _dateCreated;
		private DateTime _dateModified;
		private int _sectionId;

		/// <summary>
		/// Property Title (string)
		/// </summary>
		public string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// Property Summary (string)
		/// </summary>
		public string Summary
		{
			get { return this._summary; }
			set { this._summary = value; }
		}

		/// <summary>
		/// Property Contents (string)
		/// </summary>
		public string Contents
		{
			get { return this._contents; }
			set { this._contents = value; }
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
		/// Property ModuleType (string)
		/// </summary>
		public string ModuleType
		{
			get { return this._moduleType; }
			set { this._moduleType = value; }
		}

		/// <summary>
		/// Property Path (string)
		/// </summary>
		public string Path
		{
			get { return this._path; }
			set { this._path = value; }
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
		/// Property Site (string)
		/// </summary>
		public string Site
		{
			get { return this._site; }
			set { this._site = value; }
		}

		/// <summary>
		/// Property DateCreated (DateTime)
		/// </summary>
		public DateTime DateCreated
		{
			get { return this._dateCreated; }
			set { this._dateCreated = value; }
		}

		/// <summary>
		/// Property DateModified (DateTime)
		/// </summary>
		public DateTime DateModified
		{
			get { return this._dateModified; }
			set { this._dateModified = value; }
		}

		/// <summary>
		/// Property SectionId (int)
		/// </summary>
		public int SectionId
		{
			get { return this._sectionId; }
			set { this._sectionId = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SearchContent()
		{
		}
	}
}
