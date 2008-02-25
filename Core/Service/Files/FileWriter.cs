using System;
using System.IO;
using System.Collections;

using Castle.Services.Transaction;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// The FileWriter class perform file actions in a transactional context.
	/// </summary>
	public class FileWriter : IResource
	{
		private string _tempDir = Environment.GetEnvironmentVariable("TEMP");

		private IList _createdFiles = new ArrayList();
		private IList _deletedFiles = new ArrayList();

		/// <summary>
		/// Contructor.
		/// </summary>
		public FileWriter(string tempDir)
		{	
			if (! String.IsNullOrEmpty(tempDir))
			{
				this._tempDir = tempDir;
			}
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
			string tempFilePath = Path.Combine(this._tempDir, Path.GetFileName(filePath));

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

		#region IResource Members

		public void Start()
		{
			// Nothing
		}

		public void Rollback()
		{
			// Remove temp file.
			foreach (string[] newFileLocations in this._createdFiles)
			{
				// index 0 is permanent location, index 1 = temp location
				File.Delete(newFileLocations[1]);
			}
		}

		public void Commit()
		{
			// Move temp files to permanent location.
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
		}
		#endregion
	}
}
