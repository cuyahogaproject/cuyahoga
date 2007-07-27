using System;
using System.Collections.Generic;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Base Class for CMS Content
	/// </summary>
	public abstract class ContentItem : IContentItem
	{
		protected long id;
		protected Guid globalId;
        protected WorkflowStatus workflowStatus;
		protected string title;
		protected string summary;
		protected int version;
		protected string locale;
		protected DateTime createdAt;
		protected DateTime? publishedAt;
        protected DateTime? publishedUntil;
		protected DateTime modifiedAt;
		protected User createdBy;
		protected User publishedBy;
		protected User modifiedBy;
        protected string urlFormat;
		protected Section section;
		protected IList<Category> categories;
        protected IList<ContentItemPermission> contentItemPermissions;
		
		#region Properties
		/// <summary>
		/// Property Id (long)
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		/// <summary>
		/// Property GlobalId (Guid)
		/// </summary>
        public virtual Guid GlobalId
		{
			get { return this.globalId; }
			set { this.globalId = value; }
		}

        /// <summary>
        /// Property WorkflowStatus (WorkflowStatus)
        /// </summary>
        public virtual WorkflowStatus WorkflowStatus
        {
            get { return this.workflowStatus; }
            set { this.workflowStatus = value; }
        }

		/// <summary>
		/// Property Title (string)
		/// </summary>
        public virtual string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		/// <summary>
		/// Property Summary(string)
		/// </summary>
        public virtual string Summary
		{
			get { return this.summary; }
			set { this.summary = value; }
		}

		/// <summary>
		/// Property Version (int)
		/// </summary>
        public virtual int Version
		{
			get { return this.version; }
			set { this.version = value; }
		}

		/// <summary>
		/// Property Locale (string)
		/// </summary>
        public virtual string Locale
		{
			get{ return this.locale; }
			set{ this.locale = value; }
		}

		/// <summary>
		/// Property CreatedAt (DateTime)
		/// </summary>
        public virtual DateTime CreatedAt
		{
			get { return this.createdAt; }
			set { this.createdAt = value; }
		}

		/// <summary>
		/// Property PublishedAt (DateTime)
		/// </summary>
        public virtual DateTime? PublishedAt
		{
			get { return this.publishedAt; }
			set { this.publishedAt = value; }
		}

        /// <summary>
        /// Property PublishedUntil (DateTime)
        /// </summary>
        public virtual DateTime? PublishedUntil
        {
            get { return this.publishedUntil; }
            set { this.publishedUntil = value; }
        }

		/// <summary>
		/// Property ModifiedAt (DateTime)
		/// </summary>
        public virtual DateTime ModifiedAt
		{
			get { return this.modifiedAt; }
			set { this.modifiedAt = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
        public virtual User CreatedBy
		{
			get { return this.createdBy; }
			set { this.createdBy = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
        public virtual User PublishedBy
		{
			get { return this.publishedBy; }
			set { this.publishedBy = value; }
		}

		/// <summary>
		/// Property ModifiedBy (User)
		/// </summary>
        public virtual User ModifiedBy
		{
			get { return this.modifiedBy; }
			set { this.modifiedBy = value; }
		}

        /// <summary>
        /// Property UrlFormat (string)
        /// </summary>
        public virtual string UrlFormat
        {
            get { return this.urlFormat; }
            set { this.urlFormat = value; }
        }

		/// <summary>
		/// Property Section (Section)
		/// </summary>
        public virtual Section Section
		{
			get { return this.section; }
			set { this.section = value; }
		}
		
		/// <summary>
		/// Property Categories (Category)
		/// </summary>
		public IList<Category> Categories
		{
			get { return this.categories; }
			set { this.categories = value; }
		}

        /// <summary>
        /// Property ContentItemPermissions (ContentItemPermission)
        /// </summary>
        public IList<ContentItemPermission> ContentItemPermissions
        {
            get { return this.contentItemPermissions; }
            set { this.contentItemPermissions = value; }
        }

		#endregion

		public ContentItem()
		{
			this.id = -1;
			this.globalId = Guid.NewGuid();
            this.workflowStatus = WorkflowStatus.Draft;
			this.createdAt = DateTime.Now;
            this.modifiedAt = DateTime.Now;
			this.version = 1;
            this.categories = new List<Category>();
            this.contentItemPermissions = new List<ContentItemPermission>();
		}
	}
}