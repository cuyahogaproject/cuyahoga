using System;
using Cuyahoga.Core;
using Cuyahoga.Modules.StaticHtml;

namespace Cuyahoga.Modules.DAL
{
	/// <summary>
	/// Summary description for IModuleDataProvider.
	/// </summary>
	public interface IModulesDataProvider
	{
		#region StaticHtmlModule

		void GetStaticHtmlContentBySectionId(int sectionId, StaticHtmlContent staticHtmlContent);
		void InsertStaticHtmlContent(int sectionId, int userId, StaticHtmlContent staticHtmlContent);
		void UpdateStaticHtmlContent(int sectionId, int userId, StaticHtmlContent staticHtmlContent);
		void DeleteStaticHtmlContent(int sectionId);

		#endregion
	}
}
