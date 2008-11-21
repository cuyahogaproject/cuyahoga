using System;
using System.Text;
using Castle.Components.Validator;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Validation
{
	public class BrowserValidationEngine
	{
		private ILocalizedValidatorRegistry _validatorRegistry;
		private IBrowserValidatorProvider _browserValidatorProvider;

		public BrowserValidationEngine(ILocalizedValidatorRegistry validatorRegistry, IBrowserValidatorProvider browserValidatorProvider)
		{
			_validatorRegistry = validatorRegistry;
			_browserValidatorProvider = browserValidatorProvider;
		}

		public string GenerateClientScript(object modelToValidate, string formId, Func<string, string> propertyNameToElementId)
		{
			BrowserValidationConfiguration config = this._browserValidatorProvider.CreateConfiguration(null);
			var validatorRunner = new ValidatorRunner(this._validatorRegistry);
			IBrowserValidationGenerator generator = 
				_browserValidatorProvider.CreateGenerator(config, InputElementType.Undefined, null);
			IValidator[] validators = 
				_validatorRegistry.GetValidators(validatorRunner, modelToValidate.GetType(), RunWhen.Everytime);
			foreach (var validator in validators)
			{
				validator.ApplyBrowserValidation(config, InputElementType.Undefined, generator, null, propertyNameToElementId(validator.Property.Name));
			}

			var sb = new StringBuilder();
			sb.Append("<script type=\"text/javascript\">" + Environment.NewLine);
			sb.Append(config.CreateBeforeFormClosed(formId));
			sb.Append("</script>" + Environment.NewLine);
			return sb.ToString();
		}
	}
}