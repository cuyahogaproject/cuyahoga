using System.Collections.Generic;

namespace Cuyahoga.Core.Validation
{
	public interface IModelValidator
	{
		/// <summary>
		/// Checks if the given object is valid.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <returns>True when the object is valid</returns>
		bool IsValid(object objectToValidate);

		/// <summary>
		/// Checks if the given object is valid. Only checks the given properties.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="includeProperties">An array of property names to check.</param>
		/// <returns>True when the object is valid</returns>
		bool IsValid(object objectToValidate, string[] includeProperties);

		/// <summary>
		/// Get the last error(s). Make sure to call IsValid() before getting the errors. 
		/// </summary>
		/// <returns>A dictionary that contains the errors for each property where the key is the property name.</returns>
		IDictionary<string, string[]> GetErrors();
	}
}