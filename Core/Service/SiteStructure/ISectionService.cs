using System;
using System.Collections;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality for section management.
	/// </summary>
	public interface ISectionService
	{
		/// <summary>
		/// Get a single section by id.
		/// </summary>
		/// <param name="sectionId"></param>
		/// <returns></returns>
		Section GetSectionById(int sectionId);

		/// <summary>
		/// Get (sorted by placeholder and position) sections that are related to a given node.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		IList GetSortedSectionsByNode(Node node);

		/// <summary>
		/// Get all orphaned sections (sections that are not related to a node).
		/// </summary>
		/// <returns></returns>
		IList GetUnconnectedSections();

		/// <summary>
		/// Get all templates that have the given section attached.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		IList GetTemplatesBySection(Section section);

		/// <summary>
		/// Get all sections that have modules of the given moduletypes.
		/// </summary>
		/// <param name="moduleTypes"></param>
		/// <returns></returns>
		IList GetSectionsByModuleTypes(IList moduleTypes);

		/// <summary>
		/// Save a new section.
		/// </summary>
		/// <param name="section"></param>
		void SaveSection(Section section);

		/// <summary>
		/// Update an exisiting section.
		/// </summary>
		/// <param name="section"></param>
		void UpdateSection(Section section);

		/// <summary>
		/// Delete a section.
		/// </summary>
		/// <param name="section"></param>
		void DeleteSection(Section section);
	}
}
