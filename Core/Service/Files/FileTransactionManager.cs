using System;

using Castle.Model;
using Castle.Services.Transaction;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// ITransactionManager implementation.
	/// </summary>
	[PerThread]
	public class FileTransactionManager : DefaultTransactionManager
	{
		public FileTransactionManager()
		{
		}
	}
}
