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
			if (newParentCategory == this._parentCategory)
			{
				return; // don't do anything when the parent stays the same.
			}
			if (this._parentCategory != null)
			{
				this._parentCategory.ChildCategories.Remove(this);
				foreach (Category siblingCategory in this._parentCategory.ChildCategories)
				{
					siblingCategory.CalculatePositionAndPath();
				}
			}
			else if (this.Site.RootCategories.Contains(this))
			{
				this.Site.RootCategories.Remove(this);
				foreach (Category rootCategory in this.Site.RootCategories)
				{
					rootCategory.CalculatePositionAndPath();
				}
			}
			if (newParentCategory != null)
			{
				newParentCategory.ChildCategories.Add(this);
			}
			else
			{
				this.Site.RootCategories.Add(this);
			}
			this._parentCategory = newParentCategory;
		}

		/// <summary>
		/// Calculate the position and path of the category.
		/// </summary>
		public virtual void CalculatePositionAndPath()
		{
			if (this._parentCategory != null)
			{
				this._position = this._parentCategory.ChildCategories.IndexOf(this);
			}
			else
			{
				if (this.Site.RootCategories.Contains(this))
				{
					this._position = this.Site.RootCategories.IndexOf(this);
				}
				else
				{
					this._position = this.Site.RootCategories.Count;
				}
			}
			this._path = "." + this._position.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0');
			if (this._parentCategory != null)
			{
				this._path = this._parentCategory.Path + this._path;
			}
		}
	}
}