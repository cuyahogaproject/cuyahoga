using System;
using System.Collections;

using NHibernate;
using NHibernate.Expression;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// This is the repository for persistance of the Cuyahoga core classes. Maybe it 
	/// should be split up into several classes, but for simplicity we'll start with one
	/// repository for all core classes.
	/// </summary>
	public class CoreRepository
	{
		private ISessionFactory _factory;
		private ISession _activeSession;

		/// <summary>
		/// Create a repository for core objects.
		/// </summary>
		public CoreRepository() : this(false)
		{
		}

		/// <summary>
		/// Create a repository for core objects.
		/// </summary>
		/// <param name="openSession">Indicate if the CoreRepository should open a session and keep it in memory.</param>
		public CoreRepository(bool openSession)
		{
			this._factory = SessionFactory.GetInstance().GetNHibernateFactory();
			if (openSession)
			{
				this._activeSession = this._factory.OpenSession();
			}
		}

		/// <summary>
		/// Open a NHibernate session.
		/// </summary>
		public void OpenSession()
		{
			if (this._activeSession == null || ! this._activeSession.IsOpen)
			{
				this._activeSession = this._factory.OpenSession();
			}
			else
			{
				throw new InvalidOperationException("The repository already has an open session");
			}
		}

		/// <summary>
		/// Close the active NHibernate session
		/// </summary>
		public void CloseSession()
		{
			if (this._activeSession != null && this._activeSession.IsOpen)
			{
				this._activeSession.Close();
			}
		}

		/// <summary>
		/// Generic method for retrieving single objects by primary key
		/// </summary>
		/// <param name="type"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public object GetObjectById(Type type, int id)
		{
			if (this._activeSession != null)
			{
				return this._activeSession.Load(type, id);
			}
			else
			{
				throw new NullReferenceException("The repository doesn't have an active session");
			}
		}

		public IList GetAll(Type type)
		{
			return GetAll(type, null);
		}

		public IList GetAll(Type type, params string[] sortProperties)
		{
			ICriteria crit = this._activeSession.CreateCriteria(type);
			foreach (string sortProperty in sortProperties)
			{
				crit.AddOrder(Order.Asc(sortProperty));
			}
			return crit.List();
		}

		#region Node specific

		public IList GetRootNodes()
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(Node));
			crit.Add(Expression.IsNull("ParentNode"));
			crit.AddOrder(Order.Asc("Id")); 
			return crit.List();
		}

		public Node GetNodeByShortDescription(string shortDescription)
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(Node));
			crit.Add(Expression.Eq("ShortDescription", shortDescription));
			IList results = crit.List();
			if (results.Count == 1)
			{
				return (Node)results[0];
			}
			else if (results.Count > 1)
			{
				throw new Exception(String.Format("Multiple nodes found for ShortDescription {0}. The ShortDescription should be unique.", shortDescription));
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
