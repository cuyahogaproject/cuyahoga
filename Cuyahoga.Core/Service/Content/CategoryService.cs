using System;
using System.Collections.Generic;
using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Content
{
	[Transactional]
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryDao _categoryDao;
		private readonly ICommonDao _commonDao;

		public CategoryService(ICategoryDao categoryDao, ICommonDao commonDao)
		{
			this._categoryDao = categoryDao;
			this._commonDao = commonDao;
		}

		public IList<Category> GetAllRootCategories(Site site)
		{
			return this._categoryDao.GetAllRootCategories(site);
		}

		public IEnumerable<Category> GetAllCategories(Site site)
		{
			return this._categoryDao.GetAllCategories(site);
		}

		public Category GetCategoryById(int categoryId)
		{
			return this._commonDao.GetObjectById<Category>(categoryId);
		}

		[Transaction(TransactionMode.Requires)]
		public void CreateCategory(Category category)
		{
			category.CalculatePositionAndPath();
			this._commonDao.SaveObject(category);
		}

		[Transaction(TransactionMode.Requires)]
		public void UpdateCategory(Category category)
		{
			category.CalculatePositionAndPath();
			this._commonDao.UpdateObject(category);
		}

		[Transaction(TransactionMode.Requires)]
		public void DeleteCategory(Category category)
		{
			if (category.ChildCategories.Count > 0)
			{
				throw new ArgumentException("CategoryHasChildCategoriesException");
			}
			if (category.ContentItems.Count > 0)
			{
				throw new ArgumentException("CategoryHasContentItemsException");
			}
			this._commonDao.DeleteObject(category);
			this._commonDao.Flush();
			if (category.ParentCategory != null)
			{
				RemoveCategoryAndReorderSiblings(category, category.ParentCategory.ChildCategories);
			}
			else
			{
				RemoveCategoryAndReorderSiblings(category, category.Site.RootCategories);
			}
		}

		private void RemoveCategoryAndReorderSiblings(Category category, IList<Category> categories)
		{
			categories.Remove(category);
			foreach (Category siblingCategory in categories)
			{
				siblingCategory.CalculatePositionAndPath();
			}
		}
	}
}
