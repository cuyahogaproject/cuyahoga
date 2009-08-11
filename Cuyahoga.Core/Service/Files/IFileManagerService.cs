namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// Public interface for file manipulation.
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
		/// Delete the given files and directories.
		/// </summary>
		/// <param name="filesToDelete">List of physical files to delete.</param>
		/// <param name="directoriesToDelete">List of physical directories to delete.</param>
		void DeleteFilesAndDirectories(string[] filesToDelete, string[] directoriesToDelete);
	}
}