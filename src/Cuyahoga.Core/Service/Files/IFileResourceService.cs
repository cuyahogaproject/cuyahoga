using System.IO;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// IFileResourceService. 
	/// </summary>
	public interface IFileResourceService
	{
		/// <summary>
		/// Uses available information in FileResource 
		/// to create a file and updates meta info
		/// </summary>
		/// <param name="fileResource"></param>
		/// <param name="fileContents"></param>
		void SaveFileResource(FileResource fileResource, Stream fileContents);

		/// <summary>
		/// Update the meta-data of a file.
		/// </summary>
		/// <param name="fileResource"></param>
		void UpdateFileResource(FileResource fileResource);

		/// <summary>
		/// Delete meta info and physical file
		/// </summary>
		/// <param name="fileResource"></param>
		void DeleteFileResource(FileResource fileResource);

		/// <summary>
		/// Check if the physical directory is configured right. If the physical directory lies within the SiteData structure, 
		/// we create the directory if it not exists, otherwise, it has to be there.
		/// </summary>
		/// <param name="physicalDir"></param>
		void CheckPhysicalDirectory(string physicalDir);

		/// <summary>
		/// Reads the given physical file from disk and returns a stream.
		/// </summary>
		/// <param name="physicalFilePath"></param>
		/// <returns></returns>
		Stream ReadFile(string physicalFilePath);

		/// <summary>
		/// Increase the download count of the give file resource. 
		/// </summary>
		/// <param name="fileResource"></param>
		void IncreaseDownloadCount(FileResource fileResource);
	}
}