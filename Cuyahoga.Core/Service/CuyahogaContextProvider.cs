using Castle.MicroKernel;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// Provides access to the current Cuyahoga context.
	/// </summary>
	public class CuyahogaContextProvider : ICuyahogaContextProvider
	{
		private IKernel _kernel;

		/// <summary>
		/// Creates a new instance of the <see cref="CuyahogaContextProvider"></see> class.
		/// </summary>
		/// <param name="kernel"></param>
		public CuyahogaContextProvider(IKernel kernel)
		{
			_kernel = kernel;
		}

		/// <summary>
		/// Gets the Cuyahoga context (for the current request).
		/// </summary>
		/// <returns></returns>
		public ICuyahogaContext GetContext()
		{
			return _kernel.Resolve<ICuyahogaContext>();
		}
	}
}