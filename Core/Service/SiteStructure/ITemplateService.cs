using System;
using System.Collections;
using System.Collections.Generic;

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
	}
}
