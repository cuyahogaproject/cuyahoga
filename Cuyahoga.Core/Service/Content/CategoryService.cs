using System.Collections.Generic;
using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Content
{
    [Transactional]
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao categoryDao;
        private readonly ICommonDao commonDao;

        public CategoryService(ICategoryDao categoryDao, ICommonDao commonDao)
        {
            this.categoryDao = categoryDao;
            this.commonDao = commonDao;
        }

        public IList<Category> GetAllRootCategories()
        {
            return this.categoryDao.GetAllRootCategories();
        }

        public Category GetCategoryById(int categoryId)
        {
            return this.commonDao.GetObjectById(typeof(Category), categoryId) as Category;
        }

        [Transaction(TransactionMode.Requires)]
        public void SaveCategory(Category category)
        {
            this.commonDao.SaveObject(category);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteCategory(Category category)
        {
            this.commonDao.DeleteObject(category);
        }
    }
}
