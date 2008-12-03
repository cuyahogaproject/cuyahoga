using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

using Castle.Services.Transaction;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// The FileWriter class perform file actions in a transactional context.
	/// </summary>
	public class FileWriter : IResource
	{
		private readonly string _defaultTempDir = Environment.GetEnvironmentVariable("TEMP");
		private readonly string _transactionName;
		private readonly string _tempDir;
		private string _transactionDir;

		private IList<string> _createdDirectories = new List<string>();
		private IList<string[]> _createdFiles = new List<string[]>();
		private IList<string> _deletedFiles = new List<String>();
		private IList<string[]> _directoriesToCopy = new List<string[]>();
		private IList<string[]> _filesToCopy = new List<string[]>();

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
			string tempFilePath = Path.Combine(this._transactionDir, Path.GetFileName(filePath));

			FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
			StreamUtil.Copy(inputStream, fs);
			fs.Flush();
			fs.Close();

			this._createdFiles.Add(new string[2] {filePath, tempFilePath});
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
			string directoryName = IOUtil.GetDirectoryName(sourceDirectory);
			string tempCopyDir = Path.Combine(this._transactionDir, directoryName);
			IOUtil.CopyDirectory(sourceDirectory, tempCopyDir);
			this._directoriesToCopy.Add(new string[] { targetDirectory, tempCopyDir } );
		}

		public void CopyFile(string filePathToCopy, string targetDirectory)
		{
			string tempFilePath = Path.Combine(this._transactionDir, Path.GetFileName(filePathToCopy));
			this._filesToCopy.Add(new string[] { targetDirectory, tempFilePath });
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
				Directory.CreateDirectory(directoryPath);
			}

			// Copy directories to permanent location
			foreach (string[] directoryLocations in this._directoriesToCopy)
			{
				// index 0 is directory to copy to, index 1 is temp dir.
				IOUtil.CopyDirectory(directoryLocations[1], directoryLocations[0]);
			}
			
			// Copy files to permanent location
			foreach (string[] copyFileLocations in this._filesToCopy)
			{
				File.Copy(copyFileLocations[1], copyFileLocations[0]);
			}

			// Move temp new files to permanent location.
			foreach (string[] newFileLocations in this._createdFiles)
			{
				// index 0 is permanent location, index 1 = temp location
				File.Move(newFileLocations[1], newFileLocations[0]);
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
			foreach (string[] newFileLocations in this._createdFiles)
			{
				// index 0 is permanent location, index 1 = temp location
				string tempFile = newFileLocations[1];
				if (File.Exists(tempFile))
				{	
					File.Delete(tempFile);
				}
			}

			// Remove files to copy
			foreach (string[] copyFileLocations in this._filesToCopy)
			{
				// index 0 is permanent location, index 1 = temp location
				string tempFile = copyFileLocations[1];
				if (File.Exists(tempFile))
				{
					File.Delete(tempFile);
				}
			}

			// Remove direcories to copy
			foreach(string[] directoryLocations in this._directoriesToCopy)
			{
				// index 0 is permanent location, index 1 = temp location
				string tempCopyDir = directoryLocations[1];
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
}
