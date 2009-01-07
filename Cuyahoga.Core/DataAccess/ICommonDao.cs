using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// Functionality for common simple data access.
	/// </summary>
	public interface ICommonDao
	{
		/// <summary>
		/// Get a single instance from the database by type and primary key.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		object GetObjectById(Type type, int id);

		/// <summary>
		/// Get a single instance from the database by type and primary key. Optionally indicate if the
		/// object may be null when it is not found.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="id"></param>
		/// <param name="allowNull"></param>
		/// <returns></returns>
		object GetObjectById(Type type, int id, bool allowNull);

		/// <summary>
		/// Get a single instance from the database by type and a string description of a given property.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyName"></param>
		/// <param name="description"></param>
		/// <returns></returns>
		object GetObjectByDescription(Type type, string propertyName, string description);

		/// <summary>
		/// Get all objects of a given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		IList GetAll(Type type);

		/// <summary>
		/// Get all objects of T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IList<T> GetAll<T>();

		/// <summary>
		/// Get all objects of a given type sorted on the given properties (ascending).
		/// </summary>
		/// <param name="type"></param>
		/// <param name="sortProperties"></param>
		/// <returns></returns>
		IList GetAll(Type type, params string[] sortProperties);

		/// <summary>
		/// Get all objects of T sorted on the given properties (ascending).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sortProperties"></param>
		/// <returns></returns>
		IList<T> GetAll<T>(params string[] sortProperties);

		/// <summary>
		/// Get all objects of T that match the given criteria.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="criteria">NHibernate DetachedCriteria instance.</param>
		/// <returns></returns>
		/// <remarks>
		/// Be careful to not use this one from the UI layer beacuse it ties the UI to NHibernate.
		/// </remarks>
		IList<T> GetAllByCriteria<T>(DetachedCriteria criteria);

		/// <summary>
		/// Get all objects of T for the given id's.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ids"></param>
		/// <returns></returns>
		IList<T> GetByIds<T>(int[] ids);

		/// <summary>
		/// Save or update a given object in the database.
		/// </summary>
		/// <param name="obj"></param>
		void SaveOrUpdateObject(object obj);

        /// <summary>
        /// Explicit update
        /// </summary>
        /// <param name="obj"></param>
        void UpdateObject(object obj);

        /// <summary>
        /// Explicit save
        /// </summary>
        /// <param name="obj"></param>
        void SaveObject(object obj);

		/// <summary>
		/// Delete a given object from the database;
		/// </summary>
		/// <param name="obj"></param>
		void DeleteObject(object obj);

		/// <summary>
		/// Mark a given object for deletion but don't delete it (yet) from the database.
		/// </summary>
		/// <param name="obj"></param>
		void MarkForDeletion(object obj);

		/// <summary>
		/// Remove a collection from the second level cache.
		/// </summary>
		/// <param name="roleName">The fully qualified name of the collection (e.g. Cuyahoga.Core.Domain.Node.ChildNodes).</param>
		/// <param name="id">The id of the object that has the collection.</param>
		void RemoveCollectionFromCache(string roleName, int id);

		/// <summary>
		/// Remove a query from the second level query cache.
		/// </summary>
		/// <param name="cacheRegion">The name of the cached query to remove</param>
		void RemoveQueryFromCache(string cacheRegion);
	}
}
