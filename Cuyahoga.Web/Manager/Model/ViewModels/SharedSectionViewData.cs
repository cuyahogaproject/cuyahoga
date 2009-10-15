using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class SharedSectionViewData
	{
		public Section Section { get; private set; }
		public string AttachedToTemplates { get; set; }

		public SharedSectionViewData(Section section)
		{
			Section = section;
		}
	}
}
