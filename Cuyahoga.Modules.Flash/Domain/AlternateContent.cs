using System;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.Flash.Domain
{
	/// <summary>
	/// Summary description for StaticHtml.
	/// </summary>
	public class AlternateContent
	{
		private int _id;
		private string _title;
		private string _content;
		private Section _section;
		private User _createdBy;
		private User _modifiedBy;
		private DateTime _updateTimestamp;

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

		/// <summary>
		/// Property Section (Section)
		/// </summary>
		public Section Section
		{
			get { return this._section; }
			set { this._section = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
		public User CreatedBy
		{
			get { return this._createdBy; }
			set { this._createdBy = value; }
		}

		/// <summary>
		/// Property ModifiedBy (User)
		/// </summary>
		public User ModifiedBy
		{
			get { return this._modifiedBy; }
			set { this._modifiedBy = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
		}

		public AlternateContent()
		{
			this._id = -1;
		}
	}
}
