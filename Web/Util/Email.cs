using System;
using System.Web.Mail;

using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Util
{
	/// <summary>
	/// Helper class for sending email messages via System.Web.Mail.
	/// </summary>
	public class Email
	{
		private Email()
		{
		}

		/// <summary>
		/// Send an email.
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		public static void Send(string to, string from, string subject, string body)
		{
			MailMessage message = new MailMessage();
			message.From = from;
			message.To = to;
			message.Subject = subject;
			message.BodyFormat = MailFormat.Text;
			message.Body = body;

			SmtpMail.SmtpServer = Config.GetConfiguration()["SMTPServer"];
			SmtpMail.Send(message);
		}
	}
}
