using Castle.Components.Validator;

namespace Cuyahoga.Core.Validation
{
	public class CuyDateTimeValidator : DateTimeValidator
	{
		public override void ApplyBrowserValidation(BrowserValidationConfiguration config, InputElementType inputType, IBrowserValidationGenerator generator, System.Collections.IDictionary attributes, string target)
		{
			base.ApplyBrowserValidation(config, inputType, generator, attributes, target);
			generator.SetDate(target, BuildErrorMessage());
		}
	}
}
