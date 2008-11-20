using System.Collections.Generic;
using System.Linq;
using Castle.Components.Validator;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Validation
{
	public class CastleModelValidator<T> : IModelValidator<T> where T : class
	{
		private readonly ILocalizedValidatorRegistry _validatorRegistry;
		private readonly IValidatorRunner _validatorRunner;
		private IDictionary<string, ICollection<string>> _errors = new Dictionary<string, ICollection<string>>();

		/// <summary>
		/// Creates a new instance of the <see cref="CastleModelValidator{T}"/> class.
		/// </summary>
		/// <param name="validatorRegistry"></param>
		public CastleModelValidator(ILocalizedValidatorRegistry validatorRegistry)
		{
			this._validatorRegistry = validatorRegistry;
			this._validatorRunner = new ValidatorRunner(validatorRegistry);
		}

		protected IValidatorRegistry ValidatorRegistry
		{
			get { return this._validatorRegistry; }
		}

		/// <summary>
		/// Checks if the given object is valid.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <returns>True when the object is valid</returns>
		public virtual bool IsValid(T objectToValidate)
		{
			return IsValid(objectToValidate, null);
		}

		/// <summary>
		/// Checks if the given object is valid. Only checks the given properties.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="includeProperties">An array of property names to check.</param>
		/// <returns>True when the object is valid</returns>
		public virtual bool IsValid(T objectToValidate, ICollection<string> includeProperties)
		{
			this._errors.Clear();

			PerformValidation(objectToValidate, includeProperties);

			return this._errors.Count == 0;
		}

		protected virtual void PerformValidation(T objectToValidate, ICollection<string> includeProperties)
		{
			if (!this._validatorRunner.IsValid(objectToValidate))
			{
				// Check if the error properties match any of the given properties. If not return true (invalid object, but
				// the caller didn't care about the invalid properties.
				ErrorSummary errorSummary = this._validatorRunner.GetErrorSummary(objectToValidate);
				if (errorSummary.HasError)
				{
					//bool hasErrorWeCareAbout = false;
					foreach (string property in errorSummary.InvalidProperties)
					{
						if ((includeProperties == null || includeProperties.Count == 0) || includeProperties.Contains(property))
						{
							//hasErrorWeCareAbout = true;
							string[] errorsForProperty = errorSummary.GetErrorsForProperty(property);
							foreach (string error in errorsForProperty)
							{
								AddError(property, error, false);
							}
						}
					}
					//return !hasErrorWeCareAbout;
				}
			}
		}

		/// <summary>
		/// Add an error message for a given property name to the errors collection.
		/// </summary>
		/// <param name="property">The name of the invalid property.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="requiresTranslation">Indicates if the validator should try translate the errorMessage.</param>
		protected void AddError(string property, string errorMessage, bool requiresTranslation)
		{
			if (! this._errors.ContainsKey(property))
			{
				this._errors.Add(property, new List<string>());
			}
			if (requiresTranslation)
			{
				errorMessage = this._validatorRegistry.TranslateErrorMessage(errorMessage);
			}
			this._errors[property].Add(errorMessage);
		}

		/// <summary>
		/// Checks if the given object is valid.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <returns>True when the object is valid</returns>
		public bool IsValid(object objectToValidate)
		{
			return IsValid((T)objectToValidate);
		}

		/// <summary>
		/// Checks if the given object is valid. Only checks the given properties.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="includeProperties">An array of property names to check.</param>
		/// <returns>True when the object is valid</returns>
		public bool IsValid(object objectToValidate, ICollection<string> includeProperties)
		{
			return IsValid((T)objectToValidate, includeProperties);
		}

		/// <summary>
		/// Get the last error(s). Make sure to call IsValid() before getting the errors. 
		/// </summary>
		/// <returns>A dictionary that contains the errors for each property where the key is the property name.</returns>
		public IDictionary<string, ICollection<string>> GetErrors()
		{
			return this._errors;
		}
	}
}