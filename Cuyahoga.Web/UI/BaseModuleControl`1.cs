using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Generic base class for module user controls.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BaseModuleControl<T> : BaseModuleControl where T: ModuleBase
	{
		/// <summary>
		/// The associated module.
		/// </summary>
		protected new T Module
		{
			get { return (T)base.Module; }
		}
	}
}
