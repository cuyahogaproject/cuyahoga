using System;
using System.Collections;

using Cuyahoga.Core;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// Summary description for Category.
	/// </summary>
	public class Category
	{
		private int _id;
		private string _title;
		private string _summary;
		private bool _syndicate;
		private IList _articles;

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
		/// Property Syndicate (bool)
		/// </summary>
		public bool Syndicate
		{
			get { return this._syndicate; }
			set { this._syndicate = value; }
		}

		/// <summary>
		/// Property Articles (IList)
		/// </summary>
		public IList Articles
		{
			get { return this._articles; }
			set { this._articles = value; }
		}

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Category()
		{
			this._id = -1;
			this._syndicate = true;
		}
	}
}
