using System;
using System.IO;
using Castle.Services.Transaction;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Service.Search;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// FileResourceService that suports transactions. 
	/// </summary>
	[Transactional]
	public class FileResourceService : IFileResourceService
	{
		private readonly ICommonDao _commonDao;
		private readonly IFileService _fileService;
		private readonly ICuyahogaContextProvider _cuyahogaContextProvider;
		private readonly IContentItemService<FileResource> _contentItemService;
		private readonly ITextExtractor _textExtractor;

		public FileResourceService(IFileService fileService, ICuyahogaContextProvider cuyahogaContextProvider, IContentItemService<FileResource> contentItemService, ICommonDao commonDao, ITextExtractor textExtractor)
		{
			this._commonDao = commonDao;
			this._fileService = fileService;
			this._contentItemService = contentItemService;
			this._cuyahogaContextProvider = cuyahogaContextProvider;
			this._textExtractor = textExtractor;
		}

		#region IFileResourceService Members

		[Transaction(TransactionMode.Requires)]
		public virtual void SaveFileResource(FileResource fileResource, Stream fileContent)
		{
			// Save physical file.
			fileResource.PhysicalFilePath = IOUtil.EnsureUniqueFilePath(fileResource.PhysicalFilePath);
			this._fileService.WriteFile(fileResource.PhysicalFilePath, fileContent);
			// Save meta info
			this._contentItemService.Save(fileResource);
		}

		[Transaction(TransactionMode.Requires)]
		public void UpdateFileResource(FileResource fileResource)
		{
			this._contentItemService.Save(fileResource);
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void DeleteFileResource(FileResource FileResource)
		{
			// Delete physical file.
			this._fileService.DeleteFile(FileResource.PhysicalFilePath);
			// Delete meta info.
			this._contentItemService.Delete(FileResource);
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

		[Transaction(TransactionMode.Requires)]
		public void IncreaseDownloadCount(FileResource fileResource)
		{
			fileResource.DownloadCount++;
			this._commonDao.UpdateObject(fileResource);
		}

		#endregion
	}
}