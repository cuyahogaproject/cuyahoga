using System;

using Castle.Core;
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
