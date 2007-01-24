using System;
using System.Collections;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.Articles.Domain
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
		private Site _site;
		private DateTime _updateTimestamp;

		#region properties

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property Title (string)
		/// </summary>
		public virtual string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// Property Summary (string)
		/// </summary>
		public virtual string Summary
		{
			get { return this._summary; }
			set { this._summary = value; }
		}

		/// <summary>
		/// Property Syndicate (bool)
		/// </summary>
		public virtual bool Syndicate
		{
			get { return this._syndicate; }
			set { this._syndicate = value; }
		}

		/// <summary>
		/// Property Articles (IList)
		/// </summary>
		public virtual IList Articles
		{
			get { return this._articles; }
			set { this._articles = value; }
		}

		/// <summary>
		/// The site where this category belongs to.
		/// </summary>
		public virtual Site Site
		{
			get { return this._site; }
			set { this._site = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public virtual DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
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
