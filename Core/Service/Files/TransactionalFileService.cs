using System;
using System.Collections.Generic;
using System.IO;
using Cuyahoga.Core.Util;
using log4net;
using Castle.MicroKernel;
using Castle.Services.Transaction;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// Transactional implementation of the <see cref="IFileService">IFileService</see> interface.
	/// </summary>
	public class TransactionalFileService : IFileService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(TransactionalFileService));
		private string _tempDir;
		private IKernel _kernel;
		
		/// <summary>
		/// Physical path for temporary file storage
		/// </summary>
		public string TempDir
		{
			set { this._tempDir = value; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public TransactionalFileService(IKernel kernel)
		{
			this._kernel = kernel;
		}

		#region IFileService Members

		public Stream ReadFile(string filePath)
		{
			try
			{
				FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				return fs;
			}
			catch (FileNotFoundException ex)
			{
				log.Error("File not found.", ex);
				throw;
			}
			catch (Exception ex)
			{
				log.Error("An unexpected error occured while reading a file.", ex);
				throw;
			}
		}

		public void WriteFile(string filePath, Stream fileContents)
		{
			ITransaction transaction = ObtainCurrentTransaction();
			if (transaction != null)
			{
				// We're participating in a transaction, use the FileWriter to write the file.
				FileWriter fileWriter = new FileWriter(this._tempDir, transaction.Name);
				transaction.Enlist(fileWriter);
				fileWriter.CreateFromStream(filePath, fileContents);
			}
			else
			{
				// No transaction, just write the stream to a file.
				FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
				StreamUtil.Copy(fileContents, fs);
				fs.Flush();
				fs.Close();
			}
		}

		public void DeleteFile(string filePath)
		{
			ITransaction transaction = ObtainCurrentTransaction();
			if (transaction != null)
			{
				// We're participating in a transaction, use the FileWriter to delete the file.
				FileWriter fileWriter = new FileWriter(this._tempDir, transaction.Name);
				transaction.Enlist(fileWriter);
				fileWriter.DeleteFile(filePath);
			}
			else
			{
				// No transaction, just delete the file.
				File.Delete(filePath);
			}
		}

		public void CreateDirectory(string physicalDirectory)
		{
			ITransaction transaction = ObtainCurrentTransaction();
			if (transaction != null)
			{
				FileWriter fileWriter = new FileWriter(this._tempDir, transaction.Name);
				transaction.Enlist(fileWriter);
				fileWriter.CreateDirectory(physicalDirectory);
			}
			else
			{
				Directory.CreateDirectory(physicalDirectory);
			}
		}

		public void CopyDirectory(string directoryToCopy, string directoryToCopyTo)
		{
			ITransaction transaction = ObtainCurrentTransaction();
			if (transaction != null)
			{
				FileWriter fileWriter = new FileWriter(this._tempDir, transaction.Name);
				transaction.Enlist(fileWriter);
				fileWriter.CopyDirectory(directoryToCopy, directoryToCopyTo);
			}
			else
			{
				IOUtil.CopyDirectory(directoryToCopy, directoryToCopyTo);
			}
		}

		public void CopyFile(string filePathToCopy, string directoryToCopyTo)
		{
			ITransaction transaction = ObtainCurrentTransaction();
			if (transaction != null)
			{
				FileWriter fileWriter = new FileWriter(this._tempDir, transaction.Name);
				transaction.Enlist(fileWriter);
				fileWriter.CopyFile(filePathToCopy, directoryToCopyTo);
			}
			else
			{
				File.Copy(filePathToCopy, Path.Combine(directoryToCopyTo, Path.GetFileName(filePathToCopy)), true);
			}
		}

		public bool CheckIfDirectoryIsWritable(string physicalDirectory)
		{
			try
			{
				bool isWritable = IOUtil.CheckIfDirectoryIsWritable(physicalDirectory);
				if (!isWritable)
				{
					log.WarnFormat("Checking access to physical directory {0} resulted in no access.", physicalDirectory);
				}
				return isWritable;
			}
			catch (Exception ex)
			{
				log.Error(String.Format("An unexpected error occured while checking write access for the directory {0}.", physicalDirectory), ex);
				throw;
			}
		}

		public string[] GetSubDirectories(string parentDirectory)
		{
			List<string> dirList = new List<string>();
			foreach (string dirName in Directory.GetDirectories(parentDirectory))
			{
				dirList.Add(IOUtil.GetLastPathFragment(dirName));
			}
			return dirList.ToArray();
		}

		public string[] GetFiles(string physicalDirectory)
		{
			List<string> filesList = new List<string>();
			foreach (string fileName in Directory.GetFiles(physicalDirectory))
			{
				filesList.Add(IOUtil.GetLastPathFragment(fileName));
			}
			return filesList.ToArray();
		}

		#endregion

		private ITransaction ObtainCurrentTransaction()
		{
			// We're obtaining the transaction manager explicitly because it probably has a different lifestyle 
			// than this service (much shorter lifespan, singleton vs. perthread).

			// Because we're also using the NHibernateIntegration facility, we already have ITransactionManager
			// registered in the container (kernel).
			ITransactionManager transactionManager = this._kernel.Resolve<ITransactionManager>();

			return transactionManager.CurrentTransaction;
		}
	}
}
