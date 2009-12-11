using System;
using System.Collections.Generic;
using System.IO;

using log4net;
using Castle.Core;

namespace Cuyahoga.Core.Service.Email
{
	/// <summary>
	/// Default implementation of IEmailService.
	/// </summary>
	[Transient]
	public class DefaultEmailService : IEmailService
	{
		private static ILog log = LogManager.GetLogger(typeof(DefaultEmailService));
		private static readonly string defaultExtension = ".txt";
		private string _templateDir;
		private string _language;
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

		/// <summary>
		/// 
		/// </summary>
		public string TemplateDir
		{
			set { this._templateDir = value; }
		}

		/// <summary>
		/// If a language is specified, a language extension is added after template name (for example, MyTemplate.en.txt).
		/// </summary>
		public string Language
		{
			set { this._language = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="templateName"></param>
		/// <param name="subjectParams"></param>
		/// <param name="bodyParams"></param>
		public void ProcessEmail(string from, string to, string templateName, Dictionary<string, string> subjectParams, Dictionary<string, string> bodyParams)
		{
			string templatePath = DetermineTemplatePath(templateName);
			try
			{
				string[] subjectAndBody = this._templateEngine.ProcessTemplate(templatePath, subjectParams, bodyParams);
				try
				{
					this._emailSender.Send(from, to, subjectAndBody[0], subjectAndBody[1]);
				}
				catch (Exception ex)
				{
					throw new EmailException("Unable to send email", ex);
				}
			}
			catch (Exception ex)
			{
				log.Error("Unable to process email message", ex);
				throw;
			}
		}

		#endregion

		/// <summary>
		/// By default, the physical template file name consists of the template name with a .txt extension.
		/// If a language is specified, a language extension is added after template name (for example, MyTemplate.en.txt).
		/// If a language is specified, but no template is found, the method tries to find a template without the 
		/// language extension.
		/// </summary>
		/// <param name="templateName"></param>
		/// <returns></returns>
		protected virtual string DetermineTemplatePath(string templateName)
		{
			string fileName = templateName + defaultExtension;
			if (this._language != null)
			{
				string fileNameWithLanguage = templateName + "." + this._language.ToLower() + defaultExtension;
				string filePathWithLanguage = Path.Combine(this._templateDir, fileNameWithLanguage);
				// Check if file exists. If yes, return the filePathWithLanguage, otherwise continue.
				if (File.Exists(filePathWithLanguage))
				{
					return filePathWithLanguage;
				}
			}
			string filePath = Path.Combine(this._templateDir, fileName);
			if (File.Exists(filePath))
			{
				return filePath;
			}
			else
			{
				throw new FileNotFoundException("Unable to find the email template: " + templateName);
			}
		}
	}
}
