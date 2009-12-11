using System.IO;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// Public interface for file manipulation. This provides a higher-level interface over the low-level <see cref="IFileService"/> interface.
	/// </summary>
	public interface IFileManagerService
	{
		/// <summary>
		/// Get virtual root data directory for the current user.
		/// </summary>
		/// <returns></returns>
		string GetRootDataPath();

		/// <summary>
		/// Create a new directory.
		/// </summary>
		/// <param name="physicalDirectory"></param>
		void CreateDirectory(string physicalDirectory);

		/// <summary>
		/// Copy files and directories to the given directory.
		/// </summary>
		/// <param name="filesToCopy"></param>
		/// <param name="directoriesToCopy"></param>
		/// <param name="toDirectory"></param>
		void CopyFilesAndDirectories(string[] filesToCopy, string[] directoriesToCopy, string toDirectory);

		/// <summary>
		/// Move files and directories to the given directory.
		/// </summary>
		/// <param name="filesToMove"></param>
		/// <param name="directoriesToMove"></param>
		/// <param name="toDirectory"></param>
		void MoveFilesAndDirectories(string[] filesToMove, string[] directoriesToMove, string toDirectory);

		/// <summary>
		/// Delete the given files and directories.
		/// </summary>
		/// <param name="filesToDelete">List of physical files to delete.</param>
		/// <param name="directoriesToDelete">List of physical directories to delete.</param>
		void DeleteFilesAndDirectories(string[] filesToDelete, string[] directoriesToDelete);

		/// <summary>
		/// Upload a single file.
		/// </summary>
		/// <param name="physicalFilePath"></param>
		/// <param name="uploadFileStream"></param>
		/// <returns>The file name of the upload file on the server.
		string UploadFile(string physicalFilePath, Stream uploadFileStream);
	}
}