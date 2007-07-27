using System;
using System.Collections.Generic;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Represents a category to classify content
	/// </summary>
	public class Category
	{
		private int id;
        private string path;
        private int level;
        private string key;
		private string name;
		private string description;
        private Category parentCategory;
        private IList<Category> childCategories;
        private IList<ContentItem> contentItems;

		#region Properties

        /// <summary>
        /// Persistent Id
        /// </summary>
		public virtual int Id
		{
			get{ return this.id; }
			set{ this.id = value; }
		}

        /// <summary>
        /// This is a 80 characters long path definition with 16 fragments in following format: .0000
        /// allowing 16 levels of categories with each a maximum of 9999 entries
        /// </summary>
		public virtual string Path
		{
			get { return this.path; }
            set { this.path = value; }
		}

        /// <summary>
        /// The level (number of parents) of the category
        /// </summary>
        public virtual int Level
        {
            get { return (this.path.Length / 5); }
        }

        /// <summary>
        /// The position (in a list of categories) of the category
        /// </summary>
        public virtual int Position
        {
            get 
            { 
            //get last 5 characters of the path
             string position = path.Substring(path.Length - 5);
            //convert to int
             return int.Parse(position.TrimStart('.', '0'));
            }
        }

        /// <summary>
        /// A shortcut name for the category
        /// </summary>
        public virtual string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// The category name
        /// </summary>
		public virtual string Name
		{
			get{ return this.name; }
			set{ this.name = value; }
		}

		public virtual string Description
		{
			get{ return this.description; }
			set{ this.description = value; }
		}

		public virtual Category ParentCategory
		{
			get{ return this.parentCategory; }
			set{ this.parentCategory = value; }
		}

		public virtual IList<Category> ChildCategories
		{
			get{ return this.childCategories; }
			set{ this.childCategories = value; }
		}

        public virtual IList<ContentItem> ContentItems
        {
            get { return this.contentItems; }
            set { this.contentItems = value; }
        }



		#endregion

		public Category()
		{
			this.id = -1;
		}
	}
}