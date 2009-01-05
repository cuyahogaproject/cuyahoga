using System.Resources;
using Castle.Components.Validator;

namespace Cuyahoga.Core.Validation
{
	/// <summary>
	/// A validator registry that allows for external configuration of the resource manager.
	/// </summary>
	public interface ILocalizedValidatorRegistry : IValidatorRegistry
	{
		/// <summary>
		/// The resource manager.
		/// </summary>
		ResourceManager ResourceManager
		{
			get; set;
		}

		/// <summary>
		/// Get a localized error message from the given message.
		/// </summary>
		/// <param name="originalMessage">The original message. This would probably be a resource key.</param>
		/// <returns>The translated message, or the original if no translation was found.</returns>
		string TranslateErrorMessage(string originalMessage);
	}
}
