using System;
using System.Collections;
using System.Security.Principal;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// IFileResourceService. 
	/// TODO: merge with IFileService.
	/// </summary>
	public interface IFileResourceService
	{
		/// <summary>
		/// Check if download of the file is allowed for the given role.
		/// </summary>
		/// <param name="roleToCheck"></param>
		/// <returns></returns>
		bool IsDownloadAllowed(Role roleToCheck, FileResource FileResource);

		/// <summary>
		/// Check if download of the file is allowed for the given user.
		/// </summary>
		/// <param name="userToCheck"></param>
		/// <returns></returns>
		bool IsDownloadAllowed(IIdentity userToCheck, FileResource FileResource);

		/// <summary>
		/// Check if download of the file is allowed for anonymous users.
		/// </summary>
		/// <returns></returns>
		bool IsAnonymousDownloadAllowed(FileResource FileResource);

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

		/// <summary>
		/// Uses available information in FileResource 
		/// to create a file and updates meta info
		/// </summary>
		/// <param name="FileResource"></param>
		/// <param name="fileContent"></param>
		void SaveFileResource(FileResource FileResource, System.IO.Stream fileContent, bool appendIdBeforeExtension);

		/// <summary>
		/// Update (meta info only)
		/// </summary>
		/// <param name="FileResource"></param>
		void UpdateFileResource(FileResource FileResource);

		/// <summary>
		/// Delete meta info and physical file
		/// </summary>
		/// <param name="FileResource"></param>
		void DeleteFileResource(FileResource FileResource);


	}
}