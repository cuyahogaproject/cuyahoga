using System;
using System.Collections.Generic;
using System.IO;
using Castle.Services.Transaction;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// The FileWriter class performs file actions in a transactional context.
	/// </summary>
	public class FileWriter : IResource
	{
		private readonly string _defaultTempDir = Environment.GetEnvironmentVariable("TEMP");
		private readonly string _transactionName;
		private readonly string _tempDir;
		private string _transactionDir;

		private IList<string> _createdDirectories = new List<string>();
		private IList<CopyLocations> _createdFiles = new List<CopyLocations>();
		private IList<string> _deletedFiles = new List<String>();
		private IList<CopyLocations> _directoriesToCopy = new List<CopyLocations>();
		private IList<CopyLocations> _filesToCopy = new List<CopyLocations>();

		/// <summary>
		/// Contructor.
		/// </summary>
		public FileWriter(string tempDir, string transactionName)
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
			if (!Directory.Exists(directoryName))
			{
				throw new DirectoryNotFoundException(String.Format("The physical upload directory {0} for the file does not exist", directoryName));
			}
			// Check if the target directory is writable
			if (!IOUtil.CheckIfDirectoryIsWritable(directoryName))
			{
				throw new UnauthorizedAccessException(string.Format("Unable to copy files and directories to {0}. Access denied.", directoryName));
			}
			string tempFilePath = Path.Combine(this._transactionDir, Path.GetFileName(filePath));

			FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
			StreamUtil.Copy(inputStream, fs);
			fs.Flush();
			fs.Close();

			this._createdFiles.Add(new CopyLocations(tempFilePath, filePath));
		}

		/// <summary>
		/// Mark a file for deletion by adding it to the list of file that are going to be deleted.
		/// </summary>
		/// <param name="filePath"></param>
		public void DeleteFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("The file was not found so it could not be deleted.", filePath);
			}
			this._deletedFiles.Add(filePath);
		}

		public void CreateDirectory(string physicalDirectoryPath)
		{
			this._createdDirectories.Add(physicalDirectoryPath);
		}

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
			string directoryName = IOUtil.GetDirectoryName(sourceDirectory);
			string tempCopyDir = Path.Combine(this._transactionDir, directoryName);
			IOUtil.CopyDirectory(sourceDirectory, tempCopyDir);
			this._directoriesToCopy.Add(new CopyLocations(tempCopyDir, targetDirectory));
		}

		public void CopyFile(string filePathToCopy, string targetDirectory)
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
			string tempFilePath = Path.Combine(this._transactionDir, Path.GetFileName(filePathToCopy));
			this._filesToCopy.Add(new CopyLocations(tempFilePath, targetDirectory));
		}

		#region IResource Members

		public void Start()
		{
			// Create a transaction directory
			this._transactionDir = Path.Combine(this._tempDir, this._transactionName);
			if (!Directory.Exists(this._transactionDir))
			{
				Directory.CreateDirectory(this._transactionDir);
			}
		}

		public void Rollback()
		{
			// Cleanup
			CleanUpTransactionDir();
		}

		public void Commit()
		{
			// Create all new directories
			foreach (string directoryPath in this._createdDirectories)
			{
				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}
			}

			// Copy directories to permanent location
			foreach (CopyLocations directoryLocations in this._directoriesToCopy)
			{
				IOUtil.CopyDirectory(directoryLocations.SourceLocation, directoryLocations.TargetLocation);
			}
			
			// Copy files to permanent location
			foreach (CopyLocations copyFileLocations in this._filesToCopy)
			{
				File.Copy(copyFileLocations.SourceLocation, copyFileLocations.TargetLocation);
			}

			// Move temp new files to permanent location.
			foreach (CopyLocations newFileLocations in this._createdFiles)
			{
				// index 0 is permanent location, index 1 = temp location
				File.Move(newFileLocations.SourceLocation, newFileLocations.TargetLocation);
			}

			// Delete scheduled deletions
			foreach (string fileToBeDeleted in this._deletedFiles)
			{
				File.Delete(fileToBeDeleted);
			}

			CleanUpTransactionDir();
		}

		#endregion

		private void CleanUpTransactionDir()
		{
			// Remove temp files.
			foreach (CopyLocations newFileLocations in this._createdFiles)
			{
				string tempFile = newFileLocations.SourceLocation;
				if (File.Exists(tempFile))
				{	
					File.Delete(tempFile);
				}
			}

			// Remove files to copy
			foreach (CopyLocations copyFileLocations in this._filesToCopy)
			{
				string tempFile = copyFileLocations.SourceLocation;
				if (File.Exists(tempFile))
				{
					File.Delete(tempFile);
				}
			}

			// Remove direcories to copy
			foreach(CopyLocations directoryLocations in this._directoriesToCopy)
			{
				string tempCopyDir = directoryLocations.SourceLocation;
				if (Directory.Exists(tempCopyDir))
				{
					Directory.Delete(tempCopyDir, true);
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
