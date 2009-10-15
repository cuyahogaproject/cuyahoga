using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides services to manage templates.
	/// </summary>
	public interface ITemplateService
	{
		/// <summary>
		/// Get all templates ordered by name.
		/// </summary>
		IList GetAllTemplates();

		/// <summary>
		/// Get all system templates (standard templates that come with the Cuyahoga distribution).
		/// </summary>
		/// <returns></returns>
		IList<Template> GetAllSystemTemplates();

		/// <summary>
		/// Get a single template by id.
		/// </summary>
		/// <param name="templateId"></param>
		/// <returns></returns>
		Template GetTemplateById(int templateId);

		/// <summary>
		/// Get all templates that belong to the given site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList<Template> GetAllTemplatesBySite(Site site);

		/// <summary>
		/// Saves a template to the database.
		/// </summary>
		/// <param name="template"></param>
		void SaveTemplate(Template template);

		/// <summary>
		/// Deletes the given template from the database.
		/// </summary>
		/// <param name="template"></param>
		void DeleteTemplate(Template template);

		/// <summary>
		/// Extract the package file in the templates directory.
		/// </summary>
		/// <param name="packageFilePath"></param>
		/// <param name="packageStream"></param>
		void ExtractTemplatePackage(string packageFilePath, Stream packageStream);

		/// <summary>
		/// Attach a section to a template in the given placeholder.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="template"></param>
		/// <param name="placeholder"></param>
		void AttachSectionToTemplate(Section section, Template template, string placeholder);

		/// <summary>
		/// Remove the given section from the given template.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="template"></param>
		void RemoveSectionFromTemplate(Section section, Template template);

	}
}
