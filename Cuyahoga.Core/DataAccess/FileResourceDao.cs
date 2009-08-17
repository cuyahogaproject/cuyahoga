using System.Collections;

using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;

using NHibernate;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// FileResourceDao.
	/// </summary>
	[Transactional]
	public class FileResourceDao : IFileResourceDao
	{
		private readonly ISessionManager _sessionManager;

		public FileResourceDao(ISessionManager sessionManager)
		{
			this._sessionManager = sessionManager;
		}
		#region IFileResourceDao Members

		public IList FindFileResourcesByName(string searchString)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from FileResource fi where fi.FileName like :searchString";
			IQuery query = session.CreateQuery(hql);
			query.SetString("searchString", string.Concat(searchString, "%"));
			return query.List();
		}

		public IList FindFileResourcesByExtension(string extension)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from FileResource fi where fi.FileName like :extension";
			IQuery query = session.CreateQuery(hql);
			query.SetString("extension", string.Concat("%", extension));
			return query.List();
		}

		#endregion
	}
}
