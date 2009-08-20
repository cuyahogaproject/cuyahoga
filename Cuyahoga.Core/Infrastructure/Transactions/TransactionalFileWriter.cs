using System;
using System.Collections.Generic;
using System.IO;
using Castle.Services.Transaction;
using Cuyahoga.Core.Service.Files;
using Cuyahoga.Core.Util;
using log4net;

namespace Cuyahoga.Core.Infrastructure.Transactions
{
	/// <summary>
	/// The TransactionalFileWriter class performs file actions in a transactional context.
	/// </summary>
	public class TransactionalFileWriter : IResource
	{
		private static readonly ILog logger = LogManager.GetLogger(typeof (TransactionalFileWriter));
		private readonly string _defaultTempDir = Environment.GetEnvironmentVariable("TEMP");
		private readonly string _transactionName;
		private readonly string _tempDir;
		private string _transactionDir;

		private IList<string> _createdDirectories = new List<string>();
		private IList<string> _createdFiles = new List<string>();
		private IList<CopyLocations> _deletedFiles = new List<CopyLocations>();
		private IList<CopyLocations> _deletedDirectories = new List<CopyLocations>();
		private IList<string> _copiedFiles = new List<string>();

		/// <summary>
		/// Constructor.
		/// </summary>
		public TransactionalFileWriter(string tempDir, string transactionName)
		{	
			if (String.IsNullOrEmpty(tempDir))
			{
				this._tempDir = this._defaultTempDir;
			}
			else
			{
				this._tempDir = tempDir;
			}
			this._transactionName = transactionName;
		}

		/// <summary>
		/// Create a file
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="inputStream"></param>
		public void CreateFromStream(string filePath, Stream inputStream)
		{
			if (File.Exists(filePath))
			{
				throw new ArgumentException("The file already exists", filePath);
			}
			string directoryName = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(directoryName) && ! this._createdDirectories.Contains(directoryName))
			{
				throw new DirectoryNotFoundException(String.Format("The physical upload directory {0} for the file does not exist", directoryName));
			}
			// Check if the target directory is writable (only with existing directories
			if (! this._createdDirectories.Contains(directoryName) && !IOUtil.CheckIfDirectoryIsWritable(directoryName))
			{
				throw new UnauthorizedAccessException(string.Format("Unable to copy files and directories to {0}. Access denied.", directoryName));
			}
			FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			StreamUtil.Copy(inputStream, fs);
			fs.Flush();
			fs.Close();

			this._createdFiles.Add(filePath);
		}

		/// <summary>
		/// Delete a file.
		/// </summary>
		/// <param name="filePath"></param>
		public void DeleteFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("The file was not found so it could not be deleted.", filePath);
			}
			string tempFilePath = Path.Combine(this._transactionDir, Path.GetFileName(filePath));
			File.Move(filePath, tempFilePath);
			this._deletedFiles.Add(new CopyLocations(filePath, tempFilePath));
		}

		/// <summary>
		/// Delete a directory.
		/// </summary>
		/// <param name="directoryToDelete"></param>
		public void DeleteDirectory(string directoryToDelete)
		{
			if (!Directory.Exists(directoryToDelete))
			{
				throw new DirectoryNotFoundException(string.Format("The directory {0} was not found so it could not be deleted.", directoryToDelete));
			}
			string directoryName = IOUtil.GetLastPathFragment(directoryToDelete);
			string tempDeleteDir = Path.Combine(this._transactionDir, directoryName);
			Directory.CreateDirectory(tempDeleteDir);
			IOUtil.CopyDirectory(directoryToDelete, tempDeleteDir);
			Directory.Delete(directoryToDelete, true);
			this._deletedDirectories.Add(new CopyLocations(directoryToDelete, tempDeleteDir));
		}

		/// <summary>
		/// Create a new directory.
		/// </summary>
		/// <param name="physicalDirectoryPath"></param>
		public void CreateDirectory(string physicalDirectoryPath)
		{
			Directory.CreateDirectory(physicalDirectoryPath);
			this._createdDirectories.Add(physicalDirectoryPath);
		}

		/// <summary>
		/// Copy the contents of the given source directory to the given target directory. The target directory will be created
		/// when it doesn't exist.
		/// </summary>
		/// <param name="sourceDirectory"></param>
		/// <param name="targetDirectory"></param>
		public void CopyDirectory(string sourceDirectory, string targetDirectory)
		{
			if (!Directory.Exists(targetDirectory))
			{
				CreateDirectory(targetDirectory);
			}
			else
			{
				// Check if the target directory is writable
				if (!IOUtil.CheckIfDirectoryIsWritable(targetDirectory))
				{
					throw new UnauthorizedAccessException(string.Format("Unable to copy files and directories to {0}. Access denied.", targetDirectory));
				}
			}
			// Recursively copy directory contents.
			CopyDirectoryContents(sourceDirectory, targetDirectory);
		}

		private void CopyDirectoryContents(string sourceDirectory, string targetDirectory)
		{
			foreach (string sourceFile in Directory.GetFiles(sourceDirectory))
			{
				CopyFile(sourceFile, Path.GetFileName(sourceFile), targetDirectory);
			}
			foreach (string sourceSubDirectory in Directory.GetDirectories(sourceDirectory))
			{
				string targetSubDirectory = Path.Combine(targetDirectory, IOUtil.GetLastPathFragment(sourceSubDirectory));
				CreateDirectory(targetSubDirectory);
				CopyDirectoryContents(sourceSubDirectory, targetSubDirectory);
			}
		}

		/// <summary>
		/// Copy a file to a new location.
		/// </summary>
		/// <param name="filePathToCopy"></param>
		/// <param name="targetFileName"></param>
		/// <param name="targetDirectory"></param>
		public void CopyFile(string filePathToCopy, string targetFileName, string targetDirectory)
		{
			if (!Directory.Exists(targetDirectory))
			{
				CreateDirectory(targetDirectory);
			}
			else
			{
				// Check if the target directory is writable
				if (!IOUtil.CheckIfDirectoryIsWritable(targetDirectory))
				{
					throw new UnauthorizedAccessException(string.Format("Unable to copy files and directories to {0}. Access denied.", targetDirectory));
				}
			}
			string targetFilePath = Path.Combine(targetDirectory, targetFileName);
			File.Copy(filePathToCopy, targetFilePath);
			this._copiedFiles.Add(targetFilePath);
		}

		#region IResource Members

		public void Start()
		{
			// Create a transaction directory
			this._transactionDir = Path.Combine(this._tempDir, this._transactionName);
			try
			{
				if (!Directory.Exists(this._transactionDir))
				{
					Directory.CreateDirectory(this._transactionDir);
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("An unexpected error occured while creating the temporary transaction directory. Make sure you have access to {0}.", this._transactionDir), ex);
				throw;
			}
		}

		public void Rollback()
		{
			// Delete all newly created files
			foreach (string createdFile in _createdFiles)
			{
				File.Delete(createdFile);
			}

			// Delete all copied files
			foreach (string copiedFile in _copiedFiles)
			{
				File.Delete(copiedFile);
			}

			// Delete newly created directories
			foreach (string createdDirectory in _createdDirectories)
			{
				Directory.Delete(createdDirectory, true);
			}

			// Move deleted directories back to their original locations
			foreach (CopyLocations deletedDirectory in _deletedDirectories)
			{
				Directory.Move(deletedDirectory.TargetLocation, deletedDirectory.SourceLocation);
			}

			// Move deleted files back to their original locations
			foreach (CopyLocations deletedFile in _deletedFiles)
			{
				File.Move(deletedFile.TargetLocation, deletedFile.SourceLocation);
			}

			// Cleanup
			CleanUpTransactionDir();
		}

		public void Commit()
		{
			CleanUpTransactionDir();
		}

		#endregion

		private void CleanUpTransactionDir()
		{
			// Remove temp files.
			foreach (CopyLocations deletedFile in this._deletedFiles)
			{
				string tempDeletedFile = deletedFile.TargetLocation;
				if (File.Exists(tempDeletedFile))
				{	
					File.Delete(tempDeletedFile);
				}
			}

			// Remove temp deleted directories
			foreach(CopyLocations deletedDirectory in this._deletedDirectories)
			{
				string tempDeletedDir = deletedDirectory.SourceLocation;
				if (Directory.Exists(tempDeletedDir))
				{
					Directory.Delete(tempDeletedDir, true);
				}
			}

			// Remove transaction dir when empty
			if (Directory.GetFileSystemEntries(this._transactionDir).Length == 0)
			{
				Directory.Delete(this._transactionDir);
			}
		}
	}

	internal class CopyLocations
	{
		private readonly string _sourceLocation;
		private readonly string _targetLocation;

		public string SourceLocation
		{
			get { return _sourceLocation; }
		}

		public string TargetLocation
		{
			get { return _targetLocation; }
		}

		public CopyLocations(string sourceLocation, string targetLocation)
		{
			_sourceLocation = sourceLocation;
			_targetLocation = targetLocation;
		}
	}
}