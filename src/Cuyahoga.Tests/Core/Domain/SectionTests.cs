using Cuyahoga.Core.Domain;
using NUnit.Framework;

namespace Cuyahoga.Tests.Core.Domain
{
	[TestFixture]
	public class SectionTests
	{
		private ModuleType _moduleType;

		[SetUp]
		public void Setup()
		{
			this._moduleType = new ModuleType() { ModuleTypeId = 1, Name = "TestModule", AutoActivate = true, AssemblyName = "Cuyahoga.Tests", ClassName = "TestModule" };
		}

		[Test]
		public void Copy_verify_properties()
		{
			Section section = CreateSection(1, "Section 1", "main", 1);
			Section copyOfSection = section.Copy();
			Assert.That(copyOfSection.ModuleType, Is.EqualTo(section.ModuleType));
			Assert.That(copyOfSection.Title, Is.EqualTo(section.Title));
			Assert.That(copyOfSection.ShowTitle, Is.EqualTo(section.ShowTitle));
			Assert.That(copyOfSection.PlaceholderId, Is.EqualTo(section.PlaceholderId));
			Assert.That(copyOfSection.Position, Is.EqualTo(section.Position));
			Assert.That(copyOfSection.CacheDuration, Is.EqualTo(section.CacheDuration));
		}

		[Test]
		public void Copy_should_not_include_permissions()
		{
			Role role1 = new Role() { Name = "Role 1" };
			Role role2 = new Role() { Name = "Role 2" };

			Section section = CreateSection(1, "Section 1", "main", 1);
			section.SectionPermissions.Add(new SectionPermission { Id = 1, Section = section, Role = role1, ViewAllowed = true, EditAllowed = true });
			section.SectionPermissions.Add(new SectionPermission { Id = 2, Section = section, Role = role2, ViewAllowed = true, EditAllowed = false });
			Assert.That(section.SectionPermissions.Count, Is.EqualTo(2));
			Assert.That(section.EditAllowed(role1), Is.True);
			Assert.That(section.EditAllowed(role2), Is.False);

			Section copyOfSection = section.Copy();
			Assert.That(copyOfSection.SectionPermissions, Is.Empty);
		}

		[Test]
		public void Copy_should_include_settings()
		{
			Section section = CreateSection(1, "Section 1", "main", 1);
			section.Settings.Add("Setting1", "True");
			section.Settings.Add("Setting2", "A string value");
			Section copyOfSection = section.Copy();
			Assert.That(copyOfSection.Settings.Count, Is.EqualTo(2));
			Assert.That(copyOfSection.Settings["Setting2"], Is.EqualTo("A string value"));
		}

		private Section CreateSection(int id, string title, string placeHolderId, int position)
		{
			return new Section() { Id = id, Title = title, ModuleType = this._moduleType, PlaceholderId = placeHolderId, Position = position, ShowTitle = true, CacheDuration = 10 };
		}
	}
}
