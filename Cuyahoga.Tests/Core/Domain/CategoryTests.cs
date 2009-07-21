using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cuyahoga.Core.Domain;
using NUnit.Framework;

namespace Cuyahoga.Tests.Core.Domain
{
	[TestFixture]
	public class CategoryTests
	{
		private Site _siteWithRootCategories;

		[SetUp]
		public void Setup()
		{
			this._siteWithRootCategories = new Site();
			// Setup test with 3 root nodes and two child nodes under root node #2
			this._siteWithRootCategories.RootCategories.Add(new Category() {Id = 1, Site = this._siteWithRootCategories, Name = "Cat-1", Position = 0, Path = ".0000"});
			Category cat2 = new Category() { Id = 2, Site = this._siteWithRootCategories, Name = "Cat-2", Position = 1, Path = ".0001" };
			cat2.ChildCategories.Add(new Category() { Id = 3, ParentCategory = cat2, Site = this._siteWithRootCategories, Name = "Cat-2-1", Position = 0, Path = ".0001.0000" });
			cat2.ChildCategories.Add(new Category() { Id = 4, ParentCategory = cat2, Site = this._siteWithRootCategories, Name = "Cat-2-2", Position = 1, Path = ".0001.0001" });
			this._siteWithRootCategories.RootCategories.Add(cat2);
			this._siteWithRootCategories.RootCategories.Add(new Category() { Id = 5, Site = this._siteWithRootCategories, Name = "Cat-3", Position = 2, Path = ".0002" });
		}

		[Test]
		public void Setting_the_position_should_update_the_path()
		{
			Category newCategory = new Category() { Site = this._siteWithRootCategories, Name = "NewCat" };
			newCategory.SetPosition(0);
			Assert.That(newCategory.Position == 0);
			Assert.That(newCategory.Path == ".0000");
			newCategory.SetPosition(1);
			Assert.That(newCategory.Position == 1);
			Assert.That(newCategory.Path == ".0001");
		}

		[Test]
		public void Setting_the_position_should_update_the_path_recursively()
		{
			Category cat2 = this._siteWithRootCategories.RootCategories[1];
			cat2.SetPosition(3);
			Assert.That(cat2.Path, Is.EqualTo(".0003"));
			Assert.That(cat2.ChildCategories.Count, Is.EqualTo(2));
			Assert.That(cat2.ChildCategories[1].Path, Is.EqualTo(".0003.0001"));
		}

		[Test]
		public void Setting_an_empty_parent_category_to_a_root_category_should_not_change_position_and_path()
		{
			Category cat1 = this._siteWithRootCategories.RootCategories[0];
			cat1.SetParentCategory(null);
			Assert.That(cat1.Position, Is.EqualTo(0));
			Assert.That(cat1.Path, Is.EqualTo(".0000"));
		}

		[Test]
		public void Ensure_that_new_category_is_added_to_root_categories_when_parent_is_set_null()
		{
			Category newCategory = new Category() { Site = this._siteWithRootCategories, Name = "NewCat" };
			newCategory.SetParentCategory(null);
			Assert.That(this._siteWithRootCategories.RootCategories.Contains(newCategory));
		}

		[Test]
		public void Ensure_that_new_category_gets_correct_position_when_parent_is_set_null()
		{
			Category newCategory = new Category() { Site = this._siteWithRootCategories, Name = "NewCat" };
			newCategory.SetParentCategory(null);
			Assert.That(newCategory.Position, Is.EqualTo(3));
		}

		[Test]
		public void Ensure_that_new_category_gets_correct_path_when_parent_is_set_null()
		{
			Category newCategory = new Category() { Site = this._siteWithRootCategories, Name = "NewCat" };
			newCategory.SetParentCategory(null);
			Assert.That(newCategory.Path, Is.EqualTo(".0003"));
		}

		[Test]
		public void Ensure_that_new_category_is_added_to_child_catgories_of_parent()
		{
			Category newCategory = new Category() { Site = this._siteWithRootCategories, Name = "NewCat" };
			newCategory.SetParentCategory(this._siteWithRootCategories.RootCategories[1]);
			Assert.That(this._siteWithRootCategories.RootCategories[1].ChildCategories, Has.Member(newCategory));
		}

		[Test]
		public void Ensure_that_new_category_gets_correct_position_when_parent_is_set()
		{
			Category newCategory = new Category() { Site = this._siteWithRootCategories, Name = "NewCat" };
			newCategory.SetParentCategory(this._siteWithRootCategories.RootCategories[1]);
			Assert.That(newCategory.Position, Is.EqualTo(2));
		}

		[Test]
		public void Ensure_that_new_category_gets_correct_path_when_parent_is_set()
		{
			Category newCategory = new Category() { Site = this._siteWithRootCategories, Name = "NewCat" };
			newCategory.SetParentCategory(this._siteWithRootCategories.RootCategories[1]);
			Assert.That(newCategory.Path, Is.EqualTo(".0001.0002"));
		}

		[Test]
		public void Change_parent_should_recursively_update_paths()
		{
			// Moving cat-2 with children under cat-1
			Category cat1 = this._siteWithRootCategories.RootCategories[0];
			Category cat2 = this._siteWithRootCategories.RootCategories[1];
			cat2.SetParentCategory(cat1);
			Assert.That(cat2.Path, Is.EqualTo(".0000.0000"));
			Assert.That(cat2.ChildCategories.Count, Is.EqualTo(2));
			Assert.That(cat2.ChildCategories[0].Path, Is.EqualTo(".0000.0000.0000"));
			Assert.That(cat2.ChildCategories[1].Path, Is.EqualTo(".0000.0000.0001"));
		}
	}
}
