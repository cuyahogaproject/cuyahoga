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
	}
}
