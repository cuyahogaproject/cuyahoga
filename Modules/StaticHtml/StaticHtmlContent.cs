using System;

namespace Cuyahoga.Modules.StaticHtml
{
	/// <summary>
	/// Summary description for StaticHtml.
	/// </summary>
	public class StaticHtmlContent
	{
		private int _id;
		private string _title;
		private string _content;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
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

		public StaticHtmlContent()
		{
			this._id = -1;
			this._title = null;
			this._content = null;
		}
	}
}
