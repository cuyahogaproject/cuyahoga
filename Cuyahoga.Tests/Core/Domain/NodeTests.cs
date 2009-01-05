using Cuyahoga.Core.Domain;
using NUnit.Framework;

namespace Cuyahoga.Tests.Core.Domain
{
	[TestFixture]
	public class NodeTests
	{
		private Node _rootNode;

		[SetUp]
		public void Setup()
		{
			// Setup node structure
			this._rootNode = new Node() { Id = 1, Title = "root", Position = 0, Culture = "en-US" };
			this._rootNode.ChildNodes.Add(new Node() { Id = 2, Title = "page1", Position = 0, ParentNode = this._rootNode, Culture = "en-US" });
			Node page2 = new Node() { Id = 3, Title = "page2", Position = 1, ParentNode = this._rootNode, Culture = "en-US" };
			page2.ChildNodes.Add(new Node() { Id = 4, Title = "page2-1", Position = 0, ParentNode = page2, Culture = "en-US" });
			page2.ChildNodes.Add(new Node() { Id = 5, Title = "page2-2", Position = 1, ParentNode = page2, Culture = "en-US" });
			this._rootNode.ChildNodes.Add(page2);
			this._rootNode.ChildNodes.Add(new Node() { Id = 6, Title = "page3", Position = 2, ParentNode = this._rootNode, Culture = "nl-NL" });
		}

		[Test]
		public void ChangeParent_should_set_right_parent()
		{
			Node page2 = this._rootNode.ChildNodes[1];
			Node page2_2 = page2.ChildNodes[1];
			Assert.That(page2_2.ParentNode, Is.EqualTo(page2));
			page2_2.ChangeParent(this._rootNode);
			Assert.That(page2_2.ParentNode, Is.EqualTo(this._rootNode));
		}

		[Test]
		public void ChangeParent_should_update_childnodes()
		{
			Node page2 = this._rootNode.ChildNodes[1];
			Node page2_2 = page2.ChildNodes[1];
			page2_2.ChangeParent(this._rootNode);
			Assert.That(page2.ChildNodes.Count, Is.EqualTo(1));
			Assert.That(this._rootNode.ChildNodes.Count, Is.EqualTo(4));
		}

		[Test]
		public void ChangeParent_should_update_position()
		{
			Node page2 = this._rootNode.ChildNodes[1];
			Node page2_2 = page2.ChildNodes[1];
			Assert.That(page2_2.Position, Is.EqualTo(1));
			page2_2.ChangeParent(this._rootNode);
			Assert.That(page2_2.Position, Is.EqualTo(3));
		}

		[Test]
		public void ChangeParent_should_update_position_siblings()
		{
			Node page2 = this._rootNode.ChildNodes[1];
			Node page2_1 = page2.ChildNodes[0];
			Node page2_2 = page2.ChildNodes[1];
			Assert.That(page2_2.Position, Is.EqualTo(1));
			page2_1.ChangeParent(this._rootNode);
			Assert.That(page2_2.Position, Is.EqualTo(0));
		}

		[Test]
		public void Copy_verify_properties()
		{
			Node page2 = this._rootNode.ChildNodes[1];
			Node copyOfPage2 = page2.Copy(this._rootNode);
			// Properties from node to copy
			Assert.That(copyOfPage2.Site, Is.EqualTo(page2.Site));
			Assert.That(copyOfPage2.Title, Is.Not.EqualTo(page2.Title), "Title of nody copy must be different from original.");
			Assert.That(copyOfPage2.ShortDescription, Is.Not.EqualTo(page2.ShortDescription), "ShortDescription of node copy must be different from original.");
			Assert.That(copyOfPage2.Template, Is.EqualTo(page2.Template));
			Assert.That(copyOfPage2.LinkUrl, Is.EqualTo(page2.LinkUrl));
			Assert.That(copyOfPage2.LinkTarget, Is.EqualTo(page2.LinkTarget));
			Assert.That(copyOfPage2.MetaDescription, Is.EqualTo(page2.MetaDescription));
			Assert.That(copyOfPage2.MetaKeywords, Is.EqualTo(page2.MetaKeywords));
		}

		[Test]
		public void Copy_should_add_to_children_parent()
		{
			Assert.That(this._rootNode.ChildNodes.Count, Is.EqualTo(3));
			Node page2 = this._rootNode.ChildNodes[1];
			Node copyOfPage2 = page2.Copy(this._rootNode);
			Assert.That(this._rootNode.ChildNodes.Count, Is.EqualTo(4));
			Assert.That(this._rootNode.ChildNodes, Has.Some.EqualTo(copyOfPage2));
		}

		[Test]
		public void Copy_should_make_position_last()
		{
			Node page2 = this._rootNode.ChildNodes[1];
			Node copyOfPage2 = page2.Copy(this._rootNode);
			Assert.That(copyOfPage2.Position, Is.EqualTo(this._rootNode.ChildNodes.Count - 1));
		}

		[Test]
		public void Copy_should_include_sections()
		{
			Node page2 = this._rootNode.ChildNodes[1];
			page2.Sections.Add(new Section() { Id = 1, Node = page2, PlaceholderId = "main", Title = "Section 1" });
			page2.Sections.Add(new Section() { Id = 2, Node = page2, PlaceholderId = "main", Title = "Section 2" });
			Node copyOfPage2 = page2.Copy(this._rootNode);
			Assert.That(copyOfPage2.Sections.Count, Is.EqualTo(2));
			Assert.That(((Section)copyOfPage2.Sections[1]).Node, Is.EqualTo(copyOfPage2));
			Assert.That(((Section)copyOfPage2.Sections[1]).Title, Is.EqualTo("Section 2"));
		}

		public void Copy_should_inherit_culture_new_parent()
		{
			Node page2 = this._rootNode.ChildNodes[1];
			Node copyOfPage2 = page2.Copy(this._rootNode);
			Assert.That(copyOfPage2.ParentNode, Is.EqualTo(this._rootNode));
			Assert.That(copyOfPage2.Culture, Is.EqualTo(this._rootNode.Culture));
		}

		[Test]
		public void Copy_should_inherit_permissions_new_parent()
		{
			Role role1 = new Role();
			Role role2 = new Role();

			this._rootNode.NodePermissions.Add(new NodePermission { Id = 1, Node = this._rootNode, Role = role1, ViewAllowed = true, EditAllowed = true });
			this._rootNode.NodePermissions.Add(new NodePermission { Id = 2, Node = this._rootNode, Role = role2, ViewAllowed = true, EditAllowed = false });

			Node page2 = this._rootNode.ChildNodes[1];
			page2.NodePermissions.Add(new NodePermission { Id = 3, Node = page2, Role = role2, ViewAllowed = true, EditAllowed = true });
			Node copyOfPage2 = page2.Copy(this._rootNode);
			Assert.That(page2.NodePermissions.Count, Is.EqualTo(1));
			Assert.That(page2.EditAllowed(role1), Is.False);
			Assert.That(copyOfPage2.NodePermissions.Count, Is.EqualTo(2));
			Assert.That(copyOfPage2.EditAllowed(role1), Is.True);
		}
	}
}