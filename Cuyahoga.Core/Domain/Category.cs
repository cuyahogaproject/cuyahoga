using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Castle.Components.Validator;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Represents a category to classify content.
	/// </summary>
	public class Category
	{
		private int _id;
		private string _path;
		private string _name;
		private string _description;
		private int _position;
		private Site _site;
		private Category _parentCategory;
		private IList<Category> _childCategories;
		private IList<ContentItem> _contentItems;

		#region Properties

		/// <summary>
		/// Persistent Id
		/// </summary>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// This is a 80 characters long path definition with 16 fragments in following format: .0000
		/// allowing 16 levels of categories with each a maximum of 9999 entries.
		/// </summary>
		/// <remarks>This one is used to simplify hierarchical queries etc (Materialized Path).</remarks>
		public virtual string Path
		{
			get { return this._path; }
			set { this._path = value; }
		}

		/// <summary>
		/// The level (number of parents) of the category
		/// </summary>
		public virtual int Level
		{
			get { return (this._path != null ? this._path.Length / 5 : 0); }
		}

		/// <summary>
		/// The position (in a list of categories) of the category
		/// </summary>
		public virtual int Position
		{
			get { return this._position; }
			set { this._position = value; }
		}

		/// <summary>
		/// The category name
		/// </summary>
		[ValidateNonEmpty("CategoryNameValidatorNonEmpty")]
		[ValidateLength(1, 100, "CategoryNameValidatorLength")]
		public virtual string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// A description of the category
		/// </summary>
		[ValidateLength(1, 255, "CategoryDescriptionValidatorLength")]
		public virtual string Description
		{
			get { return this._description; }
			set { this._description = value; }
		}

		[ValidateNonEmpty("CategorySiteValidatorNonEmpty")]
		public virtual Site Site
		{
			get { return this._site; }
			set { this._site = value; }
		}

		/// <summary>
		/// The parent caetegory.
		/// </summary>
		public virtual Category ParentCategory
		{
			get { return this._parentCategory; }
			set { this._parentCategory = value; }
		}

		/// <summary>
		/// Child categories.
		/// </summary>
		public virtual IList<Category> ChildCategories
		{
			get { return this._childCategories; }
			set { this._childCategories = value; }
		}

		/// <summary>
		/// Content items associated with this category.
		/// </summary>
		public virtual IList<ContentItem> ContentItems
		{
			get { return this._contentItems; }
			set { this._contentItems = value; }
		}

		#endregion

		/// <summary>
		/// Creates a new instance of the <see cref="Category"></see> class.
		/// </summary>
		public Category()
		{
			this._id = -1;
			this._childCategories = new List<Category>();
			this._contentItems = new List<ContentItem>();
		}

		public virtual void SetParentCategory(Category newParentCategory)
		{
			if (this._id != -1 && newParentCategory == this._parentCategory)
			{
				return; // don't do anything when the parent stays the same.
			}
			if (this._parentCategory != null)
			{
				IList<Category> categoryList = this._parentCategory.ChildCategories;
				categoryList.Remove(this);
				// Re-organize sibling positions.
				for (int i = 0; i < categoryList.Count; i++)
				{
					categoryList[i].SetPosition(i);
				}
			}
			else if (this.Site.RootCategories.Contains(this))
			{
				this.Site.RootCategories.Remove(this);
				// Re-organize sibling positions.
				for (int i = 0; i < this.Site.RootCategories.Count; i++)
				{
					this.Site.RootCategories[i].SetPosition(i);
				}
			}

			this._parentCategory = newParentCategory;
			if (newParentCategory != null)
			{
				SetPosition(newParentCategory.ChildCategories.Count);
				newParentCategory.ChildCategories.Add(this);
			}
			else
			{
				SetPosition(this.Site.RootCategories.Count);
				this.Site.RootCategories.Add(this);
			}
		}

		public virtual void SetPosition(int position)
		{
			this.Position = position;
			// update the path
			SyncPath();
		}

		/// <summary>
		/// Synchronize the path with the position and also of all child categories.
		/// </summary>
		private void SyncPath()
		{
			this.Path = "." + this.Position.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0');
			if (this.ParentCategory != null)
			{
				this.Path = this.ParentCategory.Path + this.Path;
			}
			// Recurse into child categories
			foreach (Category childCategory in this.ChildCategories)
			{
				childCategory.SyncPath();
			}
		}
	}
}