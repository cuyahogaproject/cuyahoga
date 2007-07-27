using System;
using System.Collections;

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
		/// Get all objects of a given type sorted on the given properties (ascending).
		/// </summary>
		/// <param name="type"></param>
		/// <param name="sortProperties"></param>
		/// <returns></returns>
		IList GetAll(Type type, params string[] sortProperties);

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
	}
}
