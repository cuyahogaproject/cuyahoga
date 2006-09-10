using System;
using System.Collections.Generic;

namespace Cuyahoga.Core.Service.Email
{
	/// <summary>
	/// Provides functionality to send emails based on templates.
	/// </summary>
	public interface IEmailService
	{
		/// <summary>
		/// The physical template directory.
		/// </summary>
		string templateDir
		{
			set;
		}

		/// <summary>
		/// Send an email based on a template. The template contains the subject and the body with optional
		/// placeholders for dynamic content.
		/// </summary>
		/// <param name="templateName">The name of the template</param>
		/// <param name="subjectParams">Dynamic subject parameters</param>
		/// <param name="bodyParams">Dynamic body parameters</param>
		void ProcessEmail(string templateName, Dictionary<string, string> subjectParams, Dictionary<string, string> bodyParams);
	}
}
