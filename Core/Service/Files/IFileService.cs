using System;

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
		/// Check if the given physical directory is writable for the current user.
		/// </summary>
		/// <param name="physicalDirectory"></param>
		/// <returns></returns>
		bool CheckIfDirectoryIsWritable(string physicalDirectory);
	}
}
