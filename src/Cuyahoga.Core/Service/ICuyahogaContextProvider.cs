using System;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// Provides access to the current Cuyahoga context.
	/// </summary>
	public interface ICuyahogaContextProvider
	{
		/// <summary>
		/// Gets the Cuyahoga context (for the current request).
		/// </summary>
		/// <returns></returns>
		ICuyahogaContext GetContext();
	}
}
