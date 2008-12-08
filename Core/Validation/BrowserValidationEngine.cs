using System;
using System.Text;
using Castle.Components.Validator;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Validation
{
	/// <summary>
	/// Responsible for generating client-side validation scripts.
	/// </summary>
	public class BrowserValidationEngine
	{
		private ILocalizedValidatorRegistry _validatorRegistry;
		private IBrowserValidatorProvider _browserValidatorProvider;

		/// <summary>
		/// Creates a new instance of the <see cref="BrowserValidationEngine"></see> class.
		/// </summary>
		/// <param name="validatorRegistry">The validator registry where the validators are obtained.</param>
		/// <param name="browserValidatorProvider">The provider that generates client script for the validators.</param>
		public BrowserValidationEngine(ILocalizedValidatorRegistry validatorRegistry, IBrowserValidatorProvider browserValidatorProvider)
		{
			_validatorRegistry = validatorRegistry;
			_browserValidatorProvider = browserValidatorProvider;
		}

		/// <summary>
		/// Generates a client-side validation script based on the attributes of the modelToValidate.
		/// </summary>
		/// <param name="modelToValidate">The model to validate.</param>
		/// <param name="formId">The client ID of the form.</param>
		/// <param name="propertyNameToElementId">Delegate that can translate properties of the model to form elements.</param>
		/// <returns></returns>
		public string GenerateClientScript(object modelToValidate, string formId, Func<string, string> propertyNameToElementId)
		{
			// Create a BrowserValidationConfiguration instance to store all validation rules.
			BrowserValidationConfiguration config = this._browserValidatorProvider.CreateConfiguration(null);
			// Create a ValidatorRunner that is needed to obtain the validators for the given modelToValidate.
			var validatorRunner = new ValidatorRunner(this._validatorRegistry);
			// Create a script generator and that is linked to the BrowserValidationConfiguration
			IBrowserValidationGenerator generator = 
				_browserValidatorProvider.CreateGenerator(config, InputElementType.Undefined, null);
			// Get all validators for the given modelToValidate
			IValidator[] validators = 
				_validatorRegistry.GetValidators(validatorRunner, modelToValidate.GetType(), RunWhen.Everytime);
			// Iterate the validators and call ApplyBrowserValidation to generate the validation rules and messages into the BrowserValidationConfiguration.
			foreach (var validator in validators)
			{
				validator.ApplyBrowserValidation(config, InputElementType.Undefined, generator, null, propertyNameToElementId(validator.Property.Name));
			}

			// Generate the validation script block
			var sb = new StringBuilder();
			sb.Append("<script type=\"text/javascript\">" + Environment.NewLine);
			// Call CreateBeforeFormClosed of the BrowserValidationConfiguration. This generates the client script. 
			// Note: it's called CreateBeforeFormClosed because originally (in Monorail), it was used to only 
			// generate some extra scripts for validation.
			sb.Append(config.CreateBeforeFormClosed(formId));
			sb.Append("</script>" + Environment.NewLine);
			return sb.ToString();
		}
	}
}