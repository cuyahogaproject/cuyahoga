using System.Collections;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// Provides data access for FileResource objects
	/// </summary>
	public interface IFileResourceDao
	{
		/// <summary>
		/// Find FileResources by name (begins with).
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		IList FindFileResourcesByName(string searchString);

		/// <summary>
		/// Find FileResources by their extension
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		IList FindFileResourcesByExtension(string extension);

	}
}