using System;
using System.Collections.Generic;

namespace Cuyahoga.Core.Validation
{
	/// <summary>
	/// Exception for validation errors.
	/// </summary>
	public class ValidationException : Exception
	{
		/// <summary>
		/// Gets a list of individual validation errors.
		/// </summary>
		public IList<ValidationError> ValidationErrors { get; private set; }

		/// <summary>
		/// Creates a new instance of a <see cref="ValidationException"></see> class.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="validationErrors"></param>
		public ValidationException(string message, IList<ValidationError> validationErrors) : base(message)
		{
			this.ValidationErrors = validationErrors;
		}
	}
}
