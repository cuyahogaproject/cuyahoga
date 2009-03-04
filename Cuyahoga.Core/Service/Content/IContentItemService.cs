using System;
using System.Collections.Generic;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Content
{
	/// <summary>
	/// Service responsible for content item management.
	/// </summary>
	public interface IContentItemService<T> where T : IContentItem
	{
		/// <summary>
		/// Gets a single content item by its primary key.
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
		/// Get all content items of T
		/// </summary>
		/// <returns></returns>
		IList<T> GetAll();

		/// <summary>
		/// Gets all content items of T that match a given property value.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="propertyValue"></param>
		/// <returns></returns>
		IList<T> GetByProperty(string propertyName, object propertyValue);

		/// <summary>
		/// Find ContentItems by site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList<T> FindContentItemsBySite(Site site);

		/// <summary>
		///  Find ContentItems by the section they're assigned to
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		IList<T> FindContentItemsBySection(Section section);

		/// <summary>
		/// Save a content item in the database.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		T Save(T entity);

		/// <summary>
		/// Delete a content item from the database.
		/// </summary>
		/// <param name="entity"></param>
		void Delete(T entity);
	}

}