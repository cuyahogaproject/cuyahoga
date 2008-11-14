namespace Cuyahoga.Web.Mvc
{
	/// <summary>
	/// Message type that is displayed at the default messages location
	/// </summary>
	public class MessageType
	{
		/// <summary>
		/// A normal message.
		/// </summary>
		public const string Message = "message";

		/// <summary>
		/// A message that is displayed as error. 
		/// </summary>
		public const string Error = "error";

		/// <summary>
		/// A message that is displayed as exception.
		/// </summary>
		public const string Exception = "exception";

		public static string[] GetTypes()
		{
			return new string[3] {Message, Error, Exception};
		}
	}
}