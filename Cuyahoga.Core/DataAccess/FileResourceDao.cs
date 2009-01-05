using System;
using System.IO;
using System.Collections;

using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;

using NHibernate;
using NHibernate.Criterion;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// FileResourceDao.
	/// </summary>
	[Transactional]
	public class FileResourceDao : IFileResourceDao
	{
		private ISessionManager sessionManager;

		public FileResourceDao(ISessionManager sessionManager)
		{
			this.sessionManager = sessionManager;
		}
		#region IFileResourceDao Members

		public IList FindFileResourcesByName(string searchString)
		{
			ISession session = this.sessionManager.OpenSession();

			string hql = "from FileResource fi where fi.Name like :searchString";
			IQuery query = session.CreateQuery(hql);
			query.SetString("searchString", string.Concat(searchString, "%"));
			return query.List();
		}

		public IList FindFileResourcesByExtension(string extension)
		{
			ISession session = this.sessionManager.OpenSession();

			string hql = "from FileResource fi where fi.Extension = :extension";
			IQuery query = session.CreateQuery(hql);
			query.SetString("extension", extension);
			return query.List();
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void SaveOrUpdateFileResource(FileResource FileResource)
		{
			ISession session = this.sessionManager.OpenSession();
			session.SaveOrUpdate(FileResource);
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void DeleteFileResource(FileResource FileResource)
		{
			ISession session = this.sessionManager.OpenSession();
			session.Delete(FileResource);
		}

		#endregion
	}
}
