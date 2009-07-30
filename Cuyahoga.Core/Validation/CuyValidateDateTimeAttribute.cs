using System;
using Castle.Components.Validator;

namespace Cuyahoga.Core.Validation
{
	/// <summary>
	/// Validate that the datetime is a valid.
	/// </summary>
	/// <remarks>
	/// This checks the format of the date/time
	/// </remarks>
	[Serializable, CLSCompliant(false)]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Parameter, AllowMultiple = true)]
	public class CuyValidateDateTimeAttribute : AbstractValidationAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidateDateTimeExAttribute"/> class.
		/// </summary>
		public CuyValidateDateTimeAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidateDateTimeExAttribute"/> class.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		public CuyValidateDateTimeAttribute(string errorMessage)
			: base(errorMessage)
		{
		}

		public override IValidator Build()
		{
			IValidator validator = new CuyDateTimeValidator();
			ConfigureValidatorMessage(validator);
			return validator;
		}
	}
}
