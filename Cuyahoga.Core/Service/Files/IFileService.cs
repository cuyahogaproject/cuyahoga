using System;
using System.Collections.Generic;
using System.IO;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// The FileService provides services for file manipulation.
	/// </summary>
	public interface IFileService
	{
		/// <summary>
		/// Read the contents of a file into a stream.
		/// </summary>
		/// <remarks>
		/// NOTE: The caller is responsible for closing the stream.
		/// </remarks>
		/// <param name="filePath">The physical path where the file is located.</param>
		/// <returns></returns>
		Stream ReadFile(string filePath);

		/// <summary>
		/// Write the contents from the given stream to a file.
		/// </summary>
		/// <remarks>
		/// NOTE: The caller is responsible for closing the stream.
		/// </remarks>
		/// <param name="filePath">The physical file path</param>
		/// <param name="fileContents"></param>
		void WriteFile(string filePath, Stream fileContents);

		/// <summary>
		/// Delete a single file.
		/// </summary>
		/// <param name="filePath">The physical file path</param>
		void DeleteFile(string filePath);

		/// <summary>
		/// Recursively delete an entire directory.
		/// </summary>
		/// <param name="directoryPath"></param>
		void DeleteDirectory(string directoryPath);

		/// <summary>
		/// Create a new directory.
		/// </summary>
		/// <param name="physicalDirectory">The physical path of the new directory</param>
		void CreateDirectory(string physicalDirectory);

		/// <summary>
		/// Recursively copy the contents of a given directory to a new location.
		/// </summary>
		/// <param name="directoryToCopy">Physical path of the directory to copy</param>
		/// <param name="directoryToCopyTo">Physical path of the directory where directory and contents are copied to</param>
		void CopyDirectoryContents(string directoryToCopy, string directoryToCopyTo);

		/// <summary>
		/// Copy a file to a new location. Doesn't overwrite existing files, but adds a suffix in that case.
		/// </summary>
		/// <param name="filePathToCopy"></param>
		/// <param name="directoryToCopyTo"></param>
		/// <returns>The file path.</returns>
		string CopyFile(string filePathToCopy, string directoryToCopyTo);

		/// <summary>
		/// Check if the given physical directory is writable for the current user.
		/// </summary>
		/// <param name="physicalDirectory"></param>
		/// <returns></returns>
		bool CheckIfDirectoryIsWritable(string physicalDirectory);

		/// <summary>
		/// Get all directories located below the given physical directory.
		/// </summary>
		/// <param name="parentDirectory"></param>
		string[] GetSubDirectories(string parentDirectory);

		/// <summary>
		/// Get all files located in the given physical directory.
		/// </summary>
		/// <param name="physicalDirectory"></param>
		/// <returns></returns>
		string[] GetFiles(string physicalDirectory);

	}
}
