using System;
using System.Collections.Generic;
using System.Text;

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
        Category GetById(int id);

        /// <summary>
        /// Get by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Category GetByKey(string key);


        /// <summary>
        /// Gets all categories that have no parent category
        /// </summary>
        /// <returns></returns>
        IList<Category> GetAllRootCategories();


        /// <summary>
        /// Get categories by key (with descendant categories)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<Category> GetByKeyIncludingSubcategories(string key);

        /// <summary>
        /// Gets all categories that start with the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IList<Category> GetByPathStartsWith(string path);

        /// <summary>
        /// Gets one category matching the supplied path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Category GetByExactPath(string path);

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="category"></param>
        void SaveCategory(Category category);

        /// <summary>
        /// Save or Update
        /// </summary>
        /// <param name="category"></param>
        void SaveOrUpdateCategory(Category category);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="category"></param>
        void DeleteCategory(Category category);

        /// <summary>
        /// Moves on category specified by oldPath to newPath, correcting the parent category reference if necessary
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        void MoveCategoryToNewPath(string oldPath, string newPath);

        /// <summary>
        /// Gets the position from the last path fragment
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        int GetPositionFromPath(string path);

        /// <summary>
        /// Gets the position from the supplied path fragment
        /// </summary>
        /// <param name="pathFragment"></param>
        /// <returns></returns>
        int GetPositionFromPathFragment(string pathFragment);

        /// <summary>
        /// Gets the path fragment for a given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        string GetPathFragmentFromPosition(int position);


        
    }
}
