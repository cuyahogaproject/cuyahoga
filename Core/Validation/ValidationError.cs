namespace Cuyahoga.Core.Validation
{
	/// <summary>
	/// Represents a single validation error.
	/// </summary>
	public class ValidationError
	{
		/// <summary>
		/// Name (probably the property name).
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// The invalid value.
		/// </summary>
		public string InvalidValue { get; private set; }
		/// <summary>
		/// The error message.
		/// </summary>
		public string ErrorMessage { get; private set; }

		/// <summary>
		/// Creates a new instance of a <see cref="ValidationError"></see> instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="invalidValue"></param>
		/// <param name="errorMessage"></param>
		public ValidationError(string name, string invalidValue, string errorMessage)
		{
			this.Name = name;
			this.InvalidValue = invalidValue;
			this.ErrorMessage = errorMessage;
		}
	}
}
