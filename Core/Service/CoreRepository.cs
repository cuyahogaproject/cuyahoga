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
		/// Flushes the current active NHibernate session.
		/// </summary>
		public void FlushSession()
		{
			if (this._activeSession != null && this._activeSession.IsOpen)
			{
				this._activeSession.Flush();
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

		#region generic methods

		/// <summary>
		/// Generic method for retrieving single objects by primary key.
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

		/// <summary>
		/// Get all objects of a given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IList GetAll(Type type)
		{
			return GetAll(type, null);
		}

		/// <summary>
		/// Get all objects of a given type and add one or more names of properties to sort on.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="sortProperties"></param>
		/// <remarks>Sorting is Ascending order. Construct a specific query/method when the sort order
		/// should be different.</remarks>
		/// <returns></returns>
		public IList GetAll(Type type, params string[] sortProperties)
		{
			ICriteria crit = this._activeSession.CreateCriteria(type);
			foreach (string sortProperty in sortProperties)
			{
				crit.AddOrder(Order.Asc(sortProperty));
			}
			return crit.List();
		}

		/// <summary>
		/// Generic method to insert an object.
		/// </summary>
		/// <param name="obj"></param>
		public void SaveObject(object obj)
		{
			ITransaction trn = this._activeSession.BeginTransaction();
			try
			{
				this._activeSession.Save(obj);
				trn.Commit();
			}
			catch (Exception ex)
			{
				trn.Rollback();
				throw ex;
			}
		}

		/// <summary>
		/// Generic method to update an object.
		/// </summary>
		/// <param name="obj"></param>
		public void UpdateObject(object obj)
		{
			ITransaction trn = this._activeSession.BeginTransaction();
			try
			{
				this._activeSession.Update(obj);
				trn.Commit();
			}
			catch (Exception ex)
			{
				trn.Rollback();
				throw ex;
			}
		}

		/// <summary>
		/// Delete a specific object. Settings in the mapping file determine if this cascades
		/// to related objects.
		/// </summary>
		/// <param name="obj"></param>
		public void DeleteObject(object obj)
		{
			ITransaction trn = this._activeSession.BeginTransaction();
			try
			{
				this._activeSession.Delete(obj);
				trn.Commit();
			}
			catch (Exception ex)
			{
				trn.Rollback();
				throw ex;
			}
		}

		#endregion

		#region Node specific

		public IList GetRootNodes()
		{
			ICriteria crit = this._activeSession.CreateCriteria(typeof(Node));
			crit.Add(Expression.IsNull("ParentNode"));
			crit.AddOrder(Order.Asc("Position"));
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

		#region section specific


		#endregion
	}
}
