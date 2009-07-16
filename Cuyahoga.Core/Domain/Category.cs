using System.Collections.Generic;

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
			get { return (this._path.Length / 5); }
		}

		/// <summary>
		/// The position (in a list of categories) of the category
		/// </summary>
		public virtual int Position
		{
			get { return this._position; }
			set
			{
				this._position = value;
			}
		}

		/// <summary>
		/// The category name
		/// </summary>
		public virtual string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// A description of the category
		/// </summary>
		public virtual string Description
		{
			get { return this._description; }
			set { this._description = value; }
		}

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
		}
	}
}