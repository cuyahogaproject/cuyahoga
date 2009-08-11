using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using Cuyahoga.Core.Service.Membership;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// Default implementation of IFileManagerService.
	/// </summary>
	/// <remarks>
	/// Almost everything is delegated to IFileService, but transactions are managed here.
	/// </remarks>
	[Transactional]
	public class FileManagerService : IFileManagerService
	{
		private readonly IFileService _fileService;
		private readonly ICuyahogaContextProvider _contextProvider;

		public FileManagerService(IFileService fileService, ICuyahogaContextProvider contextProvider)
		{
			_fileService = fileService;
			_contextProvider = contextProvider;
		}

		public string GetRootDataPath()
		{
			if (this._contextProvider.GetContext().CurrentUser.HasRight(Rights.AccessRootDataFolder))
			{
				return "~/SiteData/";
			}
			return this._contextProvider.GetContext().CurrentSite.SiteDataDirectory + "UserFiles/";
		}

		[Transaction(TransactionMode.Requires)]
		public void CreateDirectory(string physicalDirectory)
		{
			this._fileService.CreateDirectory(physicalDirectory);
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void DeleteFilesAndDirectories(string[] filesToDelete, string[] directoriesToDelete)
		{
			foreach (string fileToDelete in filesToDelete)
			{
				this._fileService.DeleteFile(fileToDelete);
			}
			foreach (string directoryToDelete in directoriesToDelete)
			{
				this._fileService.DeleteDirectory(directoryToDelete);
			}
		}
	}
}
