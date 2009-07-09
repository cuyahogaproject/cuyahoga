using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Mvc.ViewModels
{
	public class ModuleAdminViewModel<T> where T: class
	{
		public Node Node { get; private set; }
		public Section Section { get; private set; }
		public T ModuleData { get; private set; }

		public ModuleAdminViewModel(Node node, Section section, T moduleData)
		{
			this.Node = node;
			this.Section = section;
			this.ModuleData = moduleData;
		}
	}
}
