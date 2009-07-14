using System.Web.Routing;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Mvc.ViewModels
{
	public class ModuleAdminViewModel<T> where T: class
	{
		public Node Node { get; private set; }
		public Section Section { get; private set; }
		public ICuyahogaContext CuyahogaContext { get; private set; }
		public T ModuleData { get; private set; }

		public ModuleAdminViewModel(Node node, Section section, ICuyahogaContext cuyahogaContext, T moduleData)
		{
			this.Node = node;
			this.Section = section;
			this.CuyahogaContext = cuyahogaContext;
			this.ModuleData = moduleData;
		}

		public RouteValueDictionary GetNodeAndSectionParams()
		{
			RouteValueDictionary nodeAndSectionParams = new RouteValueDictionary();
			if (this.Node != null)
			{
				nodeAndSectionParams.Add("nodeid", this.Node.Id);
			}
			if (this.Section != null)
			{
				nodeAndSectionParams.Add("sectionid", this.Section.Id);
			}
			return nodeAndSectionParams;
		}
	}
}
