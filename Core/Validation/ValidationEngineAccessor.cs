using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Validation
{
	public class ValidationEngineAccessor
	{
		private static ValidationEngineAccessor _instance = new ValidationEngineAccessor();
		private ValidationEngine _validationEngine;

		public static ValidationEngineAccessor Current
		{
			get { return _instance; }
		}

		public ValidationEngine ValidationEngine
		{
			get { return _validationEngine; }
		}

		public void SetValidationEngine(ValidationEngine validationEngine)
		{
			_validationEngine = validationEngine;
		}
	}
}