using System.Collections.Generic;
using System.Linq;
using Castle.Components.Validator;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Validation
{
	public class CastleModelValidator : IModelValidator
	{
		private readonly IValidatorRunner _validatorRunner;
		private IDictionary<string, string[]> _errors = new Dictionary<string, string[]>();

		/// <summary>
		/// Creates a new instance of the <see cref="CastleModelValidator"/> class.
		/// </summary>
		/// <param name="validatorRegistry"></param>
		public CastleModelValidator(IValidatorRegistry validatorRegistry)
		{
			this._validatorRunner = new ValidatorRunner(validatorRegistry);
		}

		/// <summary>
		/// Checks if the given object is valid.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <returns>True when the object is valid</returns>
		public bool IsValid(object objectToValidate)
		{
			return IsValid(objectToValidate, null);
		}

		/// <summary>
		/// Checks if the given object is valid. Only checks the given properties.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="includeProperties">An array of property names to check.</param>
		/// <returns>True when the object is valid</returns>
		public bool IsValid(object objectToValidate, string[] includeProperties)
		{
			this._errors.Clear();

			if (!this._validatorRunner.IsValid(objectToValidate))
			{
				// Check if the error properties match any of the given properties. If not return true (invalid object, but
				// the caller didn't care about the invalid properties.
				ErrorSummary errorSummary = this._validatorRunner.GetErrorSummary(objectToValidate);
				if (errorSummary.HasError)
				{
					bool hasErrorWeCareAbout = false;
					foreach (string property in errorSummary.InvalidProperties)
					{
						if ((includeProperties == null || includeProperties.Length == 0) || includeProperties.Contains(property))
						{
							hasErrorWeCareAbout = true;
							this._errors.Add(property, errorSummary.GetErrorsForProperty(property));
						}
					}
					return !hasErrorWeCareAbout;
				}
			}
			return true;
		}

		/// <summary>
		/// Get the last error(s). Make sure to call IsValid() before getting the errors. 
		/// </summary>
		/// <returns>A dictionary that contains the errors for each property where the key is the property name.</returns>
		public IDictionary<string, string[]> GetErrors()
		{
			return this._errors;
		}
	}
}