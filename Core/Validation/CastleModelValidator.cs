using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.Validator;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Validation
{
	public class CastleModelValidator<T> : IModelValidator<T> where T : class
	{
		private ILocalizedValidatorRegistry _validatorRegistry;
		private IValidatorRunner _validatorRunner;
		private IDictionary<string, ICollection<string>> _errors = new Dictionary<string, ICollection<string>>();

		/// <summary>
		/// Creates a new instance of the <see cref="CastleModelValidator{T}"/> class.
		/// </summary>
		public CastleModelValidator()
		{
		}

		/// <summary>
		/// Gets or sets the ValidatorRegistry. We have chosen setter injection so that inheritors 
		/// aren't bothered with a ILocalizedValidatorRegistry in the constructor.
		/// </summary>
		public ILocalizedValidatorRegistry ValidatorRegistry
		{
			protected get { return this._validatorRegistry; }
			set
			{
				this._validatorRegistry = value;
				this._validatorRunner = new ValidatorRunner(this._validatorRegistry);
			}
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

		/// <summary>
		/// Performs the actual validation. Override this one to perform custom validation.
		/// </summary>
		/// <param name="objectToValidate"></param>
		/// <param name="includeProperties"></param>
		protected virtual void PerformValidation(T objectToValidate, ICollection<string> includeProperties)
		{
			// Check the validatorrunner.
			if (this._validatorRunner == null)
			{
				throw new InvalidOperationException("Unable to validate the object, because there is no ValidationRunner available.");
			}
			if (!this._validatorRunner.IsValid(objectToValidate))
			{
				// Check if the error properties match any of the given properties. If not return true (invalid object, but
				// the caller didn't care about the invalid properties.
				ErrorSummary errorSummary = this._validatorRunner.GetErrorSummary(objectToValidate);
				if (errorSummary.HasError)
				{
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
				}
			}
		}

		/// <summary>
		/// Checks if a given property should be validated.
		/// </summary>
		/// <param name="propertyName">The name of the property</param>
		/// <param name="includeProperties">The list of the properties to validate. If null or empty, we'll assume that all properties need to be checked</param>
		/// <returns></returns>
		protected bool ShouldValidateProperty(string propertyName, ICollection<string> includeProperties)
		{
			return 
				(includeProperties == null || includeProperties.Count == 0) 
				|| includeProperties.Contains(propertyName);
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
			if (requiresTranslation && this._validatorRegistry != null)
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