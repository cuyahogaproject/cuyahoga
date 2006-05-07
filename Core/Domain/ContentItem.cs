using System;
using System.Collections;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Base Class for CMS Content
	/// </summary>
	public abstract class ContentItem : IContentItem
	{
		protected long id;
		protected Guid globalId;
		protected string title;
		protected string description;
		protected int version;
		protected string typeInfo;
		//protected ContentItemClass classification;
		protected DateTime createdAt;
		protected DateTime publishedAt;
		protected DateTime modifiedAt;
		protected User createdBy;
		protected User publishedBy;
		protected User modifiedBy;
		protected Section section;
		//protected IList categories;
		
		#region Properties
		/// <summary>
		/// Property Id (long)
		/// </summary>
		public long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		/// <summary>
		/// Property GlobalId (Guid)
		/// </summary>
		public Guid GlobalId
		{
			get { return this.globalId; }
			set { this.globalId = value; }
		}

		/// <summary>
		/// Property Title (string)
		/// </summary>
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		/// <summary>
		/// Property Description (string)
		/// </summary>
		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

		/// <summary>
		/// Property Version (int)
		/// </summary>
		public int Version
		{
			get { return this.version; }
			set { this.version = value; }
		}

		/// <summary>
		/// Property TypeInfo (string)
		/// </summary>
		public string TypeInfo
		{
			get{ return this.typeInfo; }
			set{ this.typeInfo = value; }
		}

		/*
		/// <summary>
		/// Property Classification (ContentItemClass)
		/// </summary>
		public ContentItemClass Classification
		{
			get{ return this.classification; }
			set{ this.classification = value; }
		}
		*/

		/// <summary>
		/// Property CreatedAt (DateTime)
		/// </summary>
		public DateTime CreatedAt
		{
			get { return this.createdAt; }
			set { this.createdAt = value; }
		}

		/// <summary>
		/// Property PublishedAt (DateTime)
		/// </summary>
		public DateTime PublishedAt
		{
			get { return this.publishedAt; }
			set { this.publishedAt = value; }
		}

		/// <summary>
		/// Property ModifiedAt (DateTime)
		/// </summary>
		public DateTime ModifiedAt
		{
			get { return this.modifiedAt; }
			set { this.modifiedAt = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
		public User CreatedBy
		{
			get { return this.createdBy; }
			set { this.createdBy = value; }
		}

		/// <summary>
		/// Property CreatedBy (User)
		/// </summary>
		public User PublishedBy
		{
			get { return this.publishedBy; }
			set { this.publishedBy = value; }
		}

		/// <summary>
		/// Property ModifiedBy (User)
		/// </summary>
		public User ModifiedBy
		{
			get { return this.modifiedBy; }
			set { this.modifiedBy = value; }
		}

		/// <summary>
		/// Property Section (Section)
		/// </summary>
		public Section Section
		{
			get { return this.section; }
			set { this.section = value; }
		}

		
		/// <summary>
		/// Property Categories (Category)
		/// </summary>
		/*public IList Categories
		{
			get { return this.categories; }
			set { this.categories = value; }
		}*/


		#endregion

		public ContentItem()
		{
			this.id = -1;
			this.globalId = Guid.NewGuid();
			this.createdAt = DateTime.Now;
			this.version = 1;

		}
	}
}