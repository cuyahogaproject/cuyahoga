using System.Collections.Generic;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Content
{
	public interface ICategoryService
	{
		/// <summary>
		/// Get by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Category GetCategoryById(int id);

		/// <summary>
		/// Gets all categories that have no parent category
		/// </summary>
		/// <returns></returns>
		IList<Category> GetAllRootCategories();

		/// <summary>
		/// Save a single category.
		/// </summary>
		/// <param name="category"></param>
		void SaveCategory(Category category);

		/// <summary>
		/// Delete a single category.
		/// </summary>
		/// <param name="category"></param>
		void DeleteCategory(Category category);
	}
}
