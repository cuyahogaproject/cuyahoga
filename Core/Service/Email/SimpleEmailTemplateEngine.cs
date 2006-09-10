using System;
using System.Collections.Generic;
using System.Text;

namespace Cuyahoga.Core.Service.Email
{
	public class SimpleEmailTemplateEngine : IEmailTemplateEngine
	{
		#region IEmailTemplateEngine Members

		public string[] ProcessTemplate(string templatePath, Dictionary<string, string> subjectParams, Dictionary<string, string> bodyParams)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
