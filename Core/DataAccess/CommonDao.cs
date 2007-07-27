using System;
using System.Collections;

using NHibernate;
using NHibernate.Expression;
using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// Functionality for common simple data access. The class uses NHibernate.
	/// </summary>
	[Transactional]
	public class CommonDao : ICommonDao
	{
		private ISessionManager _sessionManager;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sessionManager"></param>
		public CommonDao(ISessionManager sessionManager)
		{
			this._sessionManager = sessionManager;
		}

		#region ICommonDao Members

		public object GetObjectById(Type type, int id)
		{
			ISession session = this._sessionManager.OpenSession();
			return session.Load(type, id);
		}

		public object GetObjectById(Type type, int id, bool allowNull)
		{
			if (! allowNull)
			{
				return GetObjectById(type, id);
			}
			else
			{
				ISession session = this._sessionManager.OpenSession();
				return session.Get(type, id);
			}
		}

		public object GetObjectByDescription(Type type, string propertyName, string description)
		{
			ISession session = this._sessionManager.OpenSession();
			ICriteria crit = session.CreateCriteria(type);
			crit.Add(Expression.Eq(propertyName, description));
			return crit.UniqueResult();
		}

		public IList GetAll(Type type)
		{
			return GetAll(type, null);
		}

		public IList GetAll(Type type, params string[] sortProperties)
		{
			ISession session = this._sessionManager.OpenSession();

			ICriteria crit = session.CreateCriteria(type);
			if (sortProperties != null)
			{
				foreach (string sortProperty in sortProperties)
				{
					crit.AddOrder(Order.Asc(sortProperty));
				}
			}
			return crit.List();
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void SaveOrUpdateObject(object obj)
		{
			ISession session = this._sessionManager.OpenSession();
			session.SaveOrUpdate(obj);
		}


        [Transaction(TransactionMode.Requires)]
        public virtual void SaveObject(object obj)
        {
            ISession session = this._sessionManager.OpenSession();
            session.Save(obj);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateObject(object obj)
        {
            ISession session = this._sessionManager.OpenSession();
            session.Update(obj);
        }

		[Transaction(TransactionMode.Requires)]
		public virtual void DeleteObject(object obj)
		{
			ISession session = this._sessionManager.OpenSession();
			session.Delete(obj);
		}

		public void MarkForDeletion(object obj)
		{
			ISession session = this._sessionManager.OpenSession();
			session.Delete(obj);
		}

		#endregion
	}
}
