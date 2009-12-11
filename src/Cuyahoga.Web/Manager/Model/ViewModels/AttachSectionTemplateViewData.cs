using System.Collections.Generic;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class AttachSectionTemplateViewData
	{
		public Section Section { get; set; }
		public IList<Template> Templates { get; set; }
		public IDictionary<Template, string[]> PlaceHoldersByTemplate { get; set; }
		public IDictionary<int, SectionTemplateViewData> SectionTemplates { get; set; }

		public AttachSectionTemplateViewData()
		{
			Templates = new List<Template>();
			PlaceHoldersByTemplate = new Dictionary<Template, string[]>();
			SectionTemplates = new Dictionary<int, SectionTemplateViewData>();
		}
	}
}
