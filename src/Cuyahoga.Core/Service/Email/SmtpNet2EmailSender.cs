using System;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace Cuyahoga.Core.Service.Email
{
	/// <summary>
	/// Implements IEmailSender using the System.Net.Email classes from the .NET 2.0 framework.
	/// </summary>
	public class SmtpNet2EmailSender : IEmailSender
	{
		private string _host;
		private int _port;
		private string _smtpUsername;
		private string _smtpPassword;
		private Encoding _encoding;
		
		/// <summary>
		/// SMTP port (default 25).
		/// </summary>
		public int Port
		{
			set { this._port = value; }
		}

		/// <summary>
		/// SMTP Username
		/// </summary>
		public string SmtpUsername
		{
			set { this._smtpUsername = value; }
		}

		/// <summary>
		/// SMTP Password
		/// </summary>
		public string SmtpPassword
		{
			set { this._smtpPassword = value; }
		}

		/// <summary>
		/// Email body encoding
		/// </summary>
		public string EmailEncoding
		{
			set 
			{
				if (!String.IsNullOrEmpty(value))
				{
					this._encoding = Encoding.GetEncoding(value);
				}
			}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="host">SMTP hostname is required for this service to work.</param>
		public SmtpNet2EmailSender(string host)
		{
			this._host = host;
			this._port = 25;
			this._encoding = Encoding.Default;
		}

		#region IEmailSender Members

		public void Send(string from, string to, string subject, string body)
		{
			Send(from, to, subject, body, null, null);
		}

		public void Send(string from, string to, string subject, string body, string[] cc, string[] bcc)
		{
			// Create mail message
			MailMessage message = new MailMessage(from, to, subject, body);
			message.BodyEncoding = this._encoding;

			if (cc != null && cc.Length > 0)
			{
				foreach (string ccAddress in cc)
				{
					message.CC.Add(new MailAddress(ccAddress));
				}
			}
			if (bcc != null && bcc.Length > 0)
			{
				foreach (string bccAddress in bcc)
				{
					message.Bcc.Add(new MailAddress(bccAddress));
				}
			}

			// Send email
			SmtpClient client = new SmtpClient(this._host, this._port);
			if (!String.IsNullOrEmpty(this._smtpUsername) && !String.IsNullOrEmpty(this._smtpPassword))
			{
				client.Credentials = new NetworkCredential(this._smtpUsername, this._smtpPassword);
			}
			client.Send(message);
		}

		#endregion
	}
}
