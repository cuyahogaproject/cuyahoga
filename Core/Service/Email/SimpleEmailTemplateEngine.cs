using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Cuyahoga.Core.Service.Email
{
	/// <summary>
	/// Implements IEmailTemplateEngine. The template is a simple text file with tags that indicate the subject part
	/// and the body part.
	/// </summary>
	/// <example>
	/// Example template:
	/// [subject]
	/// This is an example subject with dynamic placeholder $placeholder
	/// [/subject]
	/// [body]
	/// This is an example email body text with another placeholder: $anotherPlaceholder.
	/// [/body]
	/// </example>
	public class SimpleEmailTemplateEngine : IEmailTemplateEngine
	{
		#region IEmailTemplateEngine Members

		public string[] ProcessTemplate(string templatePath, Dictionary<string, string> subjectParams, Dictionary<string, string> bodyParams)
		{
			string emailTemplateContent;
			using (StreamReader sr = new StreamReader(templatePath))
			{
				emailTemplateContent = sr.ReadToEnd();
			}

			Regex subjectRegex = new Regex(@"\[subject\]\r\n(.*)\r\n\[\/subject\]"
				, RegexOptions.Compiled | RegexOptions.Singleline);
			Regex bodyRegex = new Regex(@"\[body\]\r\n(.*)\r\n\[/body\]"
				, RegexOptions.Compiled | RegexOptions.Singleline);

			string subject = subjectRegex.Match(emailTemplateContent).Groups[1].Value;
			string body = bodyRegex.Match(emailTemplateContent).Groups[1].Value;

			subject = ReplacePlaceholdersWithValues(subjectParams, subject);
			body = ReplacePlaceholdersWithValues(bodyParams, body);

			return new string[2] { subject, body };
		}

		private string ReplacePlaceholdersWithValues(Dictionary<string, string> parameters, string textWithPlaceholders)
		{
			string processedText = textWithPlaceholders;
			foreach (KeyValuePair<string, string> param in parameters)
			{
				processedText = processedText.Replace(param.Key, param.Value);
			}
			return processedText;
		}

		#endregion
	}
}
