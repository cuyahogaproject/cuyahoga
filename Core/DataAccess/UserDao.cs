using System;
using System.Collections;

using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using NHibernate;
using NHibernate.Expression;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// Provides data access for user-related components.
	/// </summary>
	[Transactional]
	public class UserDao : IUserDao
	{
		private ISessionManager _sessionManager;
		private ICommonDao _commonDao;

		/// <summary>
		/// Default constructor;
		/// </summary>
		/// <param name="sessionManager"></param>
		public UserDao(ISessionManager sessionManager, ICommonDao commonDao)
		{
			this._sessionManager = sessionManager;
			this._commonDao = commonDao;
		}

		#region IUserDao Members

		public User GetUserByUsernameAndPassword(string username, string password)
		{
			ISession session = this._sessionManager.OpenSession();

			ICriteria crit = session.CreateCriteria(typeof(User));
			crit.Add(Expression.Eq("UserName", username));
			crit.Add(Expression.Eq("Password", password));
			IList results = crit.List();
			if (results.Count == 1)
			{
				return (User)results[0];
			}
			else if (results.Count > 1)
			{
				throw new Exception(String.Format("Multiple users found with the give username and password. Something is pretty wrong here"));
			}
			else
			{
				return null;
			}
		}

		public User GetUserByUsernameAndEmail(string username, string email)
		{
			ISession session = this._sessionManager.OpenSession();

			ICriteria crit = session.CreateCriteria(typeof(User));
			crit.Add(Expression.Eq("UserName", username));
			crit.Add(Expression.Eq("Email", email));
			IList results = crit.List();
			if (results.Count == 1)
			{
				return (User)results[0];
			}
			else
			{
				return null;
			}
		}

		public IList FindUsersByUsername(string searchString)
		{
			if (searchString.Length > 0)
			{
				ISession session = this._sessionManager.OpenSession();

				string hql = "from User u where u.UserName like ? order by u.UserName ";
				return session.Find(hql, searchString + "%", NHibernateUtil.String);
			}
			else
			{
				return this._commonDao.GetAll(typeof(User), "UserName");
			}
		}

		[Transaction(TransactionMode.Requires)]
		public void SaveOrUpdateUser(User user)
		{
			ISession session = this._sessionManager.OpenSession();
			session.SaveOrUpdate(user);
		}

		[Transaction(TransactionMode.Requires)]
		public void DeleteUser(User user)
		{
			ISession session = this._sessionManager.OpenSession();
			session.Delete(user);
		}

		#endregion
	}
}
