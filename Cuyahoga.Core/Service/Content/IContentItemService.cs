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
		/// Find ContentItems by site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList<T> FindContentItemsBySite(Site site);

		/// <summary>
		///  Find ContentItems by the section they're assigned to.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		IList<T> FindContentItemsBySection(Section section);

		/// <summary>
		/// Find ContentItems by the section they're assigned to.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		IList<T> FindContentItemsBySection(Section section, ContentItemQuerySettings querySettings);

		/// <summary>
		/// Find the currently visible ContentItems by the section they're assigned to.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		IList<T> FindVisibleContentItemsBySection(Section section, ContentItemQuerySettings querySettings);

		/// <summary>
		/// Find currently visible ContentItems for a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		IList<T> FindVisibleContentItemsByCategory(Category category, ContentItemQuerySettings querySettings);

		/// <summary>
		/// Find the archived content items for a given section (published until before today)
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		IList<T> FindArchivedContentItemsBySection(Section section, ContentItemQuerySettings querySettings);

		/// <summary>
		/// Find the syndicated content items for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		IList<T> FindSyndicatedContentItemsBySection(Section section, ContentItemQuerySettings querySettings);

		/// <summary>
		/// Find the syndicated content items for a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="querySettings"></param>
		/// <returns></returns>
		IList<T> FindSyndicatedContentItemsByCategory(Category category, ContentItemQuerySettings querySettings);

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