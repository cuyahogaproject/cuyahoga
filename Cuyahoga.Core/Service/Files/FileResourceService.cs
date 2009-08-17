using System;
using System.IO;
using Castle.Services.Transaction;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// FileResourceService. 
	/// </summary>
	[Transactional]
	public class FileResourceService : IFileResourceService
	{
		private readonly IFileService _fileService;
		private readonly ICuyahogaContextProvider _cuyahogaContextProvider;
		private readonly ICommonDao _commonDao;

		public FileResourceService(IFileService fileService, ICuyahogaContextProvider cuyahogaContextProvider, ICommonDao commonDao)
		{
			this._fileService = fileService;
			this._cuyahogaContextProvider = cuyahogaContextProvider;
			this._commonDao = commonDao;
		}

		#region IFileResourceService Members

		[Transaction(TransactionMode.Requires)]
		public virtual void SaveFileResource(FileResource fileResource, Stream fileContent)
		{
			// Save physical file.
			this._fileService.WriteFile(fileResource.FileName, fileContent);
			// Save meta info
			this._commonDao.SaveObject(fileResource);
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void UpdateFileResource(FileResource FileResource)
		{
			this._commonDao.UpdateObject(FileResource);
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void DeleteFileResource(FileResource FileResource)
		{
			// Delete physical file.
			this._fileService.DeleteFile(FileResource.FileName);
			// Delete meta info.
			this._commonDao.DeleteObject(FileResource);
		}

		/// <summary>
		/// Check if the physical directory is configured right. If the physical directory lies within the SiteData structure, 
		/// we create the directory if it not exists, otherwise, it has to be there.
		/// </summary>
		/// <param name="physicalDir"></param>
		public void CheckPhysicalDirectory(string physicalDir)
		{
			string siteDataDir = this._cuyahogaContextProvider.GetContext().PhysicalSiteDataDirectory;
			if (physicalDir.StartsWith(siteDataDir))
			{
				if (! Directory.Exists(physicalDir))
				{
					// We have to create the physical directory starting from the sitedata root
					string[] subDirectoriesFromSiteData =
						physicalDir.Substring(siteDataDir.Length).Split(new[] {Path.DirectorySeparatorChar},
						                                                StringSplitOptions.RemoveEmptyEntries);
					string tmpPhysicalDir = siteDataDir;
					foreach (string subDirectory in subDirectoriesFromSiteData)
					{
						tmpPhysicalDir = Path.Combine(tmpPhysicalDir, subDirectory);
						if (! Directory.Exists(tmpPhysicalDir))
						{
							Directory.CreateDirectory(tmpPhysicalDir);
						}
					}
				}
			}
			else
			{
				// We're outside the sitedata root, the directory has to exist.
				if (! Directory.Exists(physicalDir))
				{
					throw new ArgumentException(string.Format("The directory {0} does not exist.", physicalDir));
				}
			}
			if (! this._fileService.CheckIfDirectoryIsWritable(physicalDir))
			{
				throw new ArgumentException(string.Format("The directory {0} is not writable therefore we can't use it to store files.", physicalDir));
			}
		}

		public Stream ReadFile(string physicalFilePath)
		{
			return this._fileService.ReadFile(physicalFilePath);
		}

		#endregion
	}
}