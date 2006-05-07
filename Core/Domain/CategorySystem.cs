using System;
using System.Collections;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// CategorySystem, holds references to categories,
	/// is assigned to a node or section, else global
	/// </summary>
	public class CategorySystem
	{
		private int id;
		private string name;
		private string description;
		private Node assignedNode;
		private Section assignedSection;
		private Category rootCategory;
		private IList categories;

		#region Properties

		public int Id
		{
			get{ return this.id; }
			set{ this.id = value; }
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

		public Node AssignedNode
		{
			get{ return this.assignedNode; }
			set{ this.assignedNode = value; }
		}

		public Section AssignedSection
		{
			get{ return this.assignedSection; }
			set{ this.assignedSection = value; }
		}

		public Category RootCategory
		{
			get{ return this.rootCategory; }
			set{ this.rootCategory = value; }
		}

		public IList Categories
		{
			get{ return this.categories; }
			set{ this.categories = value; }
		}


		#endregion
		

		public CategorySystem()
		{
			this.id = -1;
		}
	}
}
