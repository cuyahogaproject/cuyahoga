using System;
using System.IO;
using System.Text.RegularExpressions;
using Castle.Services.Transaction;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;

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
			string parentDirectory = Path.GetDirectoryName(physicalDirectory) + Path.DirectorySeparatorChar;
			if (! DirectoryCreateAllowed(parentDirectory))
			{
				throw new ArgumentException(string.Format("Creating directory {0} is not allowed", physicalDirectory));
			}
			if (!Directory.Exists(physicalDirectory))
			{
				this._fileService.CreateDirectory(physicalDirectory);
			}
		}

		[Transaction(TransactionMode.Requires)]
		public void CopyFilesAndDirectories(string[] filesToCopy, string[] directoriesToCopy, string toDirectory)
		{
			foreach (string fileToCopy in filesToCopy)
			{
				this._fileService.CopyFile(fileToCopy, toDirectory);
			}
			foreach (string directoryToCopy in directoriesToCopy)
			{
				string newDirectory = Path.Combine(toDirectory, IOUtil.GetLastPathFragment(directoryToCopy));
				DirectoryInfo dirInfo = new DirectoryInfo(directoryToCopy);
				if (dirInfo.Parent.FullName.ToLowerInvariant() == Path.GetDirectoryName(toDirectory).ToLowerInvariant())
				{
					throw new ArgumentException(string.Format("Can not copy directory to self {0}", directoryToCopy));
				}
				CreateDirectory(newDirectory);
				this._fileService.CopyDirectoryContents(directoryToCopy, newDirectory);
			}
		}

		[Transaction(TransactionMode.Requires)]
		public void MoveFilesAndDirectories(string[] filesToMove, string[] directoriesToMove, string toDirectory)
		{
			foreach (string fileToMove in filesToMove)
			{
				this._fileService.CopyFile(fileToMove, toDirectory);
				this._fileService.DeleteFile(fileToMove);
			}
			foreach (string directoryToMove in directoriesToMove)
			{
				if (! DirectoryDeleteAllowed(directoryToMove))
				{
					throw new ArgumentException(string.Format("Moving a system data directory is not allowed ({0})", directoryToMove));
				}
				string newDirectory = Path.Combine(toDirectory, IOUtil.GetLastPathFragment(directoryToMove));
				DirectoryInfo dirInfo = new DirectoryInfo(directoryToMove);
				if (dirInfo.Parent.FullName.ToLowerInvariant() == Path.GetDirectoryName(toDirectory).ToLowerInvariant())
				{
					throw new ArgumentException(string.Format("Can not move directory to self {0}", directoryToMove));
				}
				// Check if we're not moving to one of our own child directories
				if (toDirectory.Contains(directoryToMove))
				{
					throw new ArgumentException(string.Format("Can not move directory to child directory of self {0}", directoryToMove));
				}
				CreateDirectory(newDirectory);
				this._fileService.CopyDirectoryContents(directoryToMove, newDirectory);
				this._fileService.DeleteDirectory(directoryToMove);
			}
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
				if (!DirectoryDeleteAllowed(directoryToDelete))
				{
					throw new ArgumentException(string.Format("Deleting a system data directory is not allowed ({0})", directoryToDelete));
				}
				this._fileService.DeleteDirectory(directoryToDelete);
			}
		}

		private bool DirectoryCreateAllowed(string parentPath)
		{
			// We don't allow creating new directories in the SiteData/number directories
			string[] siteDataPatterns = new[] { @"\{0}SiteData\{0}(\d+)\{0}$"};
			foreach (string pattern in siteDataPatterns)
			{
				// Replace directory separat0r
				string testPattern = String.Format(pattern, Path.DirectorySeparatorChar);
				if (Regex.IsMatch(parentPath, testPattern))
				{
					return false;
				}
			}
			return true;
		}

		private bool DirectoryDeleteAllowed(string path)
		{
			// We don't allow deleting the following directories: SiteData/number and SiteData/number/text/.
			string[] siteDataPatterns = new[] { @"\{0}SiteData\{0}(\d+)\{0}$", @"\{0}SiteData\{0}(\d+)\{0}(UserFiles|index|Templates)\{0}$" };
			foreach (string pattern in siteDataPatterns)
			{
				// Replace directory separator
				string testPattern = String.Format(pattern, Path.DirectorySeparatorChar);
				if (Regex.IsMatch(path, testPattern))
				{
					return false;
				}
			}
			return true;
		}
	}
}
