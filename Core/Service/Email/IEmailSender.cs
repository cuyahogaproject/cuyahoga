using System;

namespace Cuyahoga.Core.Service.Email
{
	/// <summary>
	/// The IEmailSender is solely responsible for sending emails.
	/// </summary>
	public interface IEmailSender
	{
		/// <summary>
		/// Send an email with the given parameters (that speak for themselves).
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		void Send(string from, string to, string subject, string body);

		/// <summary>
		/// Send an email with the given parameters (that speak for themselves).
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="cc"></param>
		/// <param name="bcc"></param>
		void Send(string from, string to, string subject, string body, string[] cc, string[] bcc);
	}
}
