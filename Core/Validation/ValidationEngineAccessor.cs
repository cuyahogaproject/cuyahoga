using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Validation
{
	public class ValidationEngineAccessor
	{
		private static ValidationEngineAccessor _instance = new ValidationEngineAccessor();
		private BrowserValidationEngine _browserValidationEngine;

		public static ValidationEngineAccessor Current
		{
			get { return _instance; }
		}

		public BrowserValidationEngine BrowserValidationEngine
		{
			get { return _browserValidationEngine; }
		}

		public void SetValidationEngine(BrowserValidationEngine browserValidationEngine)
		{
			_browserValidationEngine = browserValidationEngine;
		}
	}
}