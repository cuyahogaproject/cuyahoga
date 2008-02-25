using System;
using System.Collections;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.Articles.Domain
{
	/// <summary>
	/// Summary description for Article.
	/// </summary>
	public class Article
	{
		private int _id;
		private int _createdById;
		private int _modifiedById;
		private string _title;
		private string _summary;
		private string _content;
		private bool _syndicate;
		private DateTime _dateOnline;
		private DateTime _dateOffline;
		private DateTime _dateCreated;
		private DateTime _dateModified;
		private Section _section;
		private Cuyahoga.Core.Domain.User _createdBy;
		private Cuyahoga.Core.Domain.User _modifiedBy;
		private Category _category;
		private IList _comments;

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
		/// Property Content (string)
		/// </summary>
		public virtual string Content
		{
			get { return this._content; }
			set { this._content = value; }
		}

		/// <summary>
		/// Property DateOnline (DateTime)
		/// </summary>
		public virtual DateTime DateOnline
		{
			get { return this._dateOnline; }
			set { this._dateOnline = value; }
		}

		/// <summary>
		/// Property DateOffline (DateTime)
		/// </summary>
		public virtual DateTime DateOffline
		{
			get { return this._dateOffline; }
			set { this._dateOffline = value; }
		}

		/// <summary>
		/// Property DateCreated (DateTime)
		/// </summary>
		public virtual DateTime DateCreated
		{
			get { return this._dateCreated; }
			set { this._dateCreated = value; }
		}

		/// <summary>
		/// Property DateModified (DateTime)
		/// </summary>
		public virtual DateTime DateModified
		{
			get { return this._dateModified; }
			set { this._dateModified = value; }
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
		/// Property Section (Section)
		/// </summary>
		public virtual Section Section
		{
			get { return this._section; }
			set { this._section = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
		public virtual Cuyahoga.Core.Domain.User CreatedBy
		{
			get { return this._createdBy; }
			set { this._createdBy = value; }
		}

		/// <summary>
		/// Property CreatedById (int)
		/// </summary>
		public virtual int CreatedById
		{
			get { return this._createdById; }
			set { this._createdById = value; }
		}

		/// <summary>
		/// Property ModifiedBy (User)
		/// </summary>
		public virtual Cuyahoga.Core.Domain.User ModifiedBy
		{
			get { return this._modifiedBy; }
			set { this._modifiedBy = value; }
		}

		/// <summary>
		/// Property ModifiedById (int)
		/// </summary>
		public virtual int ModifiedById
		{
			get { return this._modifiedById; }
			set { this._modifiedById = value; }
		}

		/// <summary>
		/// Property Category (Category)
		/// </summary>
		public virtual Category Category
		{
			get { return this._category; }
			set { this._category = value; }
		}

		/// <summary>
		/// Property Comments (IList)
		/// </summary>
		public virtual IList Comments
		{
			get { return this._comments; }
			set { this._comments = value; }
		}

		#endregion

		#region constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Article()
		{
			this._id = -1;
			this._createdById = -1;
			this._modifiedById = -1;
			this._syndicate = true;
			this._dateOnline = DateTime.MinValue;
			this._dateOffline = DateTime.MinValue;
			this._dateCreated = DateTime.Now;
			this._comments = new ArrayList();
		}

		#endregion
	}
}
