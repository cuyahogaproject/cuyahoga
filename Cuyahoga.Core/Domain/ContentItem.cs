
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Castle.Components.Validator;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Base Class for CMS Content
	/// </summary>
	public abstract class ContentItem : IContentItem
	{
		private long _id;
		private Guid _globalId;
		private WorkflowStatus _workflowStatus;
		private string _title;
		private string _summary;
		private int _version;
		private string _locale;
		private bool _syndicate;
		private DateTime _createdAt;
		private DateTime? _publishedAt;
		private DateTime? _publishedUntil;
		private DateTime _modifiedAt;
		private User _createdBy;
		private User _publishedBy;
		private User _modifiedBy;
		private Section _section;
		private IList<Category> _categories;
		private IList<ContentItemPermission> _contentItemPermissions;
		private IList<Comment> _comments;

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
		[ValidateNonEmpty("ContentItemTitleValidatorNonEmpty")]
		[ValidateLength(1, 100, "ContentItemTitleValidatorLength")]
        public virtual string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// Property Summary(string)
		/// </summary>
		[ValidateLength(1, 255, "ContentItemSummaryValidatorLength")]
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
		/// Indicates if the content item should be syndicated.
		/// </summary>
		public virtual bool Syndicate
		{
			get { return this._syndicate; }
			set { this._syndicate = value; }
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
		[CuyValidateDateTime("ContentItemPublishedAtValidatorDateTime")]
        public virtual DateTime? PublishedAt
		{
			get { return this._publishedAt; }
			set { this._publishedAt = value; }
		}

        /// <summary>
        /// Property PublishedUntil (DateTime)
        /// </summary>
		[CuyValidateDateTime("ContentItemPublishedUntilValidatorDateTime")]
		[ValidateIsGreater(IsGreaterValidationType.DateTime, "PublishedAt", "ContentItemPublishedUntilValidatorGreaterThanPublishedAt")]
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
		/// Comments
		/// </summary>
		public virtual IList<Comment> Comments
		{
			get { return this._comments; }
			set { this._comments = value; }
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

		/// <summary>
		/// Indicates if the content item supports item-level permissions. If not, the permissions for the related section
		/// are used.
		/// </summary>
		public virtual bool SupportsItemLevelPermissions
		{
			get { return false; }
		}

		/// <summary>
		/// The roles that are allowed to view the content item.
		/// </summary>
		public virtual IEnumerable<Role> ViewRoles
		{
			get
			{
				if (SupportsItemLevelPermissions)
				{
					return this._contentItemPermissions.Where(cip => cip.ViewAllowed).Select(cip => cip.Role);
				}
				else
				{
					return this._section.SectionPermissions.Where(sp => sp.ViewAllowed).Select(sp => sp.Role);
				}
			}
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected ContentItem()
		{
			this._id = -1;
			this._globalId = Guid.NewGuid();
            this._workflowStatus = WorkflowStatus.Draft;
			this._createdAt = DateTime.Now;
            this._modifiedAt = DateTime.Now;
			this._version = 1;
			this._syndicate = true;
            this._categories = new List<Category>();
            this._contentItemPermissions = new List<ContentItemPermission>();
			this._comments = new List<Comment>();
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

		public virtual bool IsViewAllowed(IPrincipal currentPrincipal)
		{
			
			if (!currentPrincipal.Identity.IsAuthenticated)
			{
				return this.ViewRoles.Any(vr => vr.HasRight(Rights.Anonymous));
			}
			if (currentPrincipal is User)
			{
				return IsViewAllowedForUser((User) currentPrincipal);
			}		
			return false;
		}

		public virtual bool IsViewAllowedForUser(User user)
		{
			return this.ViewRoles.Any(user.IsInRole);
		}
	}
}