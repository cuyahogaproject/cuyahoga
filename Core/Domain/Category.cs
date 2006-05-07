using System;
using System.Collections;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Represents a Category to classify content
	/// </summary>
	public class Category
	{
		private int id;
		private int position;
		private string name;
		private string description;
		Category parentCategory;
		IList childCategories;

		#region Properties

		public int Id
		{
			get{ return this.id; }
			set{ this.id = value; }
		}

		public int Position
		{
			get{ return this.position; }
			set{ this.position = value; }
		}

		public string Name
		{
			get{ return this.name; }
			set{ this.name = value; }
		}

		public string Description
		{
			get{ return this.description; }
			set{ this.description = value; }
		}

		public Category ParentCategory
		{
			get{ return this.parentCategory; }
			set{ this.parentCategory = value; }
		}

		public IList ChildCategories
		{
			get{ return this.childCategories; }
			set{ this.childCategories = value; }
		}



		#endregion

		public Category()
		{
			this.id = -1;
		}
	}
}