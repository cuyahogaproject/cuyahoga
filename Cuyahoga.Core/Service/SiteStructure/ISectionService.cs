using System;
using System.Collections;
using System.Collections.Generic;
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
		/// <param name="module"></param>
		void DeleteSection(Section section, ModuleBase module);

		/// <summary>
		/// Delete section and remove it from the node with the given id.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="nodeId"></param>
		/// <param name="module"></param>
		void DeleteSection(Section section, int nodeId, ModuleBase module);

		/// <summary>
		/// Detach the given section from the node with given id
		/// </summary>
		/// <param name="section"></param>
		/// <param name="nodeId"></param>
		void DetachSection(Section section, int nodeId);

		/// <summary>
		/// Get all available module types sorted by name.
		/// </summary>
		/// <returns></returns>
		IList<ModuleType> GetSortedActiveModuleTypes();

		/// <summary>
		/// Get a ModuleType instance by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		ModuleType GetModuleTypeById(int id);

		/// <summary>
		/// Arrange (sort) sections with given section id's and placeholder.
		/// </summary>
		/// <param name="placeholder">The placeholder in the template where the sections should go</param>
		/// <param name="orderedSectionIds">The ordered id's of the sections that go in the placeholder</param>
		void ArrangeSections(string placeholder, int[] orderedSectionIds);
	}
}
