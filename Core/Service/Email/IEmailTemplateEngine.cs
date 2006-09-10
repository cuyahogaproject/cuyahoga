using System;
using System.Collections.Generic;

namespace Cuyahoga.Core.Service.Email
{
	/// <summary>
	/// Manages email templates.
	/// </summary>
	public interface IEmailTemplateEngine
	{
		/// <summary>
		/// Loads an email template and merges the optional parameters into the template.
		/// </summary>
		/// <param name="templatePath">Physical path of the template</param>
		/// <param name="subjectParams"></param>
		/// <param name="bodyParams"></param>
		/// <returns>The merged email subject and body (0 is subject, 1 is body)</returns>
		string[] ProcessTemplate(string templatePath, Dictionary<string, string> subjectParams, Dictionary<string, string> bodyParams);
	}
}
