using System;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
	public interface IContentItemDao<T> where T : IContentItem
	{
		/// <summary>
		/// Gets a single content item by its prmary key.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T GetById(long id);

		/// <summary>
		/// Gets a single content item by its unique identifier.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T GetById(Guid id);

		/// <summary>
		/// Gets all content items of T.
		/// </summary>
		/// <returns></returns>
		IList<T> GetAll();

		/// <summary>
		/// Gets all content items of T by the given site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList<T> GetBySite(Site site);
		
		/// <summary>
		/// Gets all content items of T for the given property and property value.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="propertyValue"></param>
		/// <returns></returns>
		IList<T> GetByProperty(string propertyName, object propertyValue);

		/// <summary>
		/// Save a single content item in the database.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		T Save(T entity);

		/// <summary>
		/// Delete a single content item from the database.
		/// </summary>
		/// <param name="entity"></param>
		void Delete(T entity);
	}
}
