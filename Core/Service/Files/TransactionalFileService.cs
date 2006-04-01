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

		private IKernel _kernel;

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
				FileWriter fileWriter = new FileWriter();
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
				FileWriter fileWriter = new FileWriter();
				transaction.Enlist(fileWriter);
				fileWriter.DeleteFile(filePath);
			}
			else
			{
				// No transaction, just delete the file.
				File.Delete(filePath);
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
