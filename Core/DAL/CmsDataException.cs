using System;

namespace Cuyahoga.Core.DAL
{
	/// <summary>
	/// A simple exception class just to identify exceptions thrown by the data access layer.
	/// </summary>
	public class CmsDataException : System.ApplicationException
	{
		public CmsDataException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
