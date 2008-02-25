using System;
using System.IO;

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
			this._kernel.AddComponent("core.fileservice.transactionmanager", typeof(ITransactionManager), typeof(FileTransactionManager));
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
				FileWriter fileWriter = new FileWriter(this._tempDir);
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
				FileWriter fileWriter = new FileWriter(this._tempDir);
				transaction.Enlist(fileWriter);
				fileWriter.DeleteFile(filePath);
			}
			else
			{
				// No transaction, just delete the file.
				File.Delete(filePath);
			}
		}

		public bool CheckIfDirectoryIsWritable(string physicalDirectory)
		{
			// Check if the given directory is writable by creating a dummy file.
			string fileName = Path.Combine(physicalDirectory, "dummy.txt");

			try
			{
				using (StreamWriter sw = new StreamWriter(fileName))
				{
					// Add some text to the file.
					sw.WriteLine("DUMMY");
					sw.Flush();
				}
				File.Delete(fileName);
				return true;
			}
			catch (UnauthorizedAccessException)
			{
				log.WarnFormat("Checking access to physical directory {0} resulted in no access.", physicalDirectory);
				return false;
			}
			catch (Exception ex)
			{
				log.Error(String.Format("An unexpected error occured while checking write access for the directory {0}.", physicalDirectory), ex);
				throw;
			}
		}

		#endregion

		private ITransaction ObtainCurrentTransaction()
		{
			ITransactionManager transactionManager = this._kernel[typeof(ITransactionManager)] as ITransactionManager;

			return transactionManager.CurrentTransaction;
		}
	}
}
