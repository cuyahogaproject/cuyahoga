using System;
using System.Collections.Generic;

namespace Cuyahoga.Core.Service.Email
{
	/// <summary>
	/// Default implementation of IEmailService
	/// </summary>
	public class DefaultEmailService : IEmailService
	{
		private string _templateDir;
		private IEmailSender _emailSender;
		private IEmailTemplateEngine _templateEngine;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="emailSender"></param>
		/// <param name="templateEngine"></param>
		public DefaultEmailService(IEmailSender emailSender, IEmailTemplateEngine templateEngine)
		{
			this._emailSender = emailSender;
			this._templateEngine = templateEngine;
		}

		#region IEmailService Members

		public string templateDir
		{
			set { this._templateDir = value; }
		}

		public void ProcessEmail(string templateName, Dictionary<string, string> subjectParams, Dictionary<string, string> bodyParams)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
