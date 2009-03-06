
using System;
using System.Collections.Generic;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Base Class for CMS Content
	/// </summary>
	public abstract class ContentItem : IContentItem
	{
		protected long _id;
		protected Guid _globalId;
        protected WorkflowStatus _workflowStatus;
		protected string _title;
		protected string _summary;
		protected int _version;
		protected string _locale;
		protected DateTime _createdAt;
		protected DateTime? _publishedAt;
        protected DateTime? _publishedUntil;
		protected DateTime _modifiedAt;
		protected User _createdBy;
		protected User _publishedBy;
		protected User _modifiedBy;
		protected Section _section;
		protected IList<Category> _categories;
        protected IList<ContentItemPermission> _contentItemPermissions;
		
		#region Properties
		/// <summary>
		/// Property Id (long)
		/// </summary>
		public virtual long Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property GlobalId (Guid)
		/// </summary>
        public virtual Guid GlobalId
		{
			get { return this._globalId; }
			set { this._globalId = value; }
		}

        /// <summary>
        /// Property WorkflowStatus (WorkflowStatus)
        /// </summary>
        public virtual WorkflowStatus WorkflowStatus
        {
            get { return this._workflowStatus; }
            set { this._workflowStatus = value; }
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
		/// Property Summary(string)
		/// </summary>
        public virtual string Summary
		{
			get { return this._summary; }
			set { this._summary = value; }
		}

		/// <summary>
		/// Property Version (int)
		/// </summary>
        public virtual int Version
		{
			get { return this._version; }
			set { this._version = value; }
		}

		/// <summary>
		/// Property Locale (string)
		/// </summary>
        public virtual string Locale
		{
			get{ return this._locale; }
			set{ this._locale = value; }
		}

		/// <summary>
		/// Property CreatedAt (DateTime)
		/// </summary>
        public virtual DateTime CreatedAt
		{
			get { return this._createdAt; }
			set { this._createdAt = value; }
		}

		/// <summary>
		/// Property PublishedAt (DateTime)
		/// </summary>
        public virtual DateTime? PublishedAt
		{
			get { return this._publishedAt; }
			set { this._publishedAt = value; }
		}

        /// <summary>
        /// Property PublishedUntil (DateTime)
        /// </summary>
        public virtual DateTime? PublishedUntil
        {
            get { return this._publishedUntil; }
            set { this._publishedUntil = value; }
        }

		/// <summary>
		/// Property ModifiedAt (DateTime)
		/// </summary>
        public virtual DateTime ModifiedAt
		{
			get { return this._modifiedAt; }
			set { this._modifiedAt = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
        public virtual User CreatedBy
		{
			get { return this._createdBy; }
			set { this._createdBy = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
        public virtual User PublishedBy
		{
			get { return this._publishedBy; }
			set { this._publishedBy = value; }
		}

		/// <summary>
		/// Property ModifiedBy (User)
		/// </summary>
        public virtual User ModifiedBy
		{
			get { return this._modifiedBy; }
			set { this._modifiedBy = value; }
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
		/// Property Categories (Category)
		/// </summary>
		public virtual IList<Category> Categories
		{
			get { return this._categories; }
			set { this._categories = value; }
		}

        /// <summary>
        /// Property ContentItemPermissions (ContentItemPermission)
        /// </summary>
        public virtual IList<ContentItemPermission> ContentItemPermissions
        {
            get { return this._contentItemPermissions; }
            set { this._contentItemPermissions = value; }
        }

		/// <summary>
		/// Indicates if the content item is new.s
		/// </summary>
		public virtual bool IsNew
		{
			get { return this._id == -1; }
		}

		#endregion

		public ContentItem()
		{
			this._id = -1;
			this._globalId = Guid.NewGuid();
            this._workflowStatus = WorkflowStatus.Draft;
			this._createdAt = DateTime.Now;
            this._modifiedAt = DateTime.Now;
			this._version = 1;
            this._categories = new List<Category>();
            this._contentItemPermissions = new List<ContentItemPermission>();
		}

		/// <summary>
		/// Gets the url that corresponds to the content. Inheritors can override this for custom url formatting.
		/// </summary>
		public virtual string GetContentUrl()
		{
			string defaultUrlFormat = "{0}/section.aspx/{1}";
			if (this._section == null)
			{
				throw new InvalidOperationException("Unable to get the url for the content because the associated section is missing.");
			}
			return String.Format(defaultUrlFormat, this._section.Id, this._id);
		}
	}
}