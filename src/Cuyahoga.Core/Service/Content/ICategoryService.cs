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
		IList<Category> GetAllRootCategories(Site site);

		/// <summary>
		/// Delete a single category.
		/// </summary>
		/// <param name="category"></param>
		void DeleteCategory(Category category);

		/// <summary>
		/// Get all categories ordered by hierarchy (via Path).
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IEnumerable<Category> GetAllCategories(Site site);

		/// <summary>
		/// Create a new category.
		/// </summary>
		/// <param name="category"></param>
		void CreateCategory(Category category);

		/// <summary>
		/// Update an existing category.
		/// </summary>
		/// <param name="category"></param>
		void UpdateCategory(Category category);
	}
}
