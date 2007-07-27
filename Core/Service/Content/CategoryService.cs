using System;
using System.Collections.Generic;
using System.Text;

using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Content
{
    [Transactional]
    public class CategoryService : ICategoryService
    {
        private ICategoryDao categoryDao;
        private ICommonDao commonDao;

        public CategoryService(ICategoryDao categoryDao, ICommonDao commonDao)
        {
            this.categoryDao = categoryDao;
            this.commonDao = commonDao;
        }

        #region ICategoryService Implementation

        public IList<Category> GetAllRootCategories()
        {
            return this.categoryDao.GetAllRootCategories();
        }

        public Category GetByKey(string key)
        {
            return this.categoryDao.GetByKey(key);
        }

        public Category GetById(int categoryId)
        {
            return this.commonDao.GetObjectById(typeof(Category), categoryId) as Category;
        }

        public IList<Category> GetByKeyIncludingSubcategories(string key)
        {
            return this.categoryDao.GetByKeyIncludingSubcategories(key);
        }

        public IList<Category> GetByPathStartsWith(string path)
        {
            return this.categoryDao.GetByPathStartsWith(path);
        }

        public Category GetByExactPath(string path)
        {
            return this.categoryDao.GetByExactPath(path);
        }

        [Transaction(TransactionMode.RequiresNew)]
        public void SaveCategory(Category category)
        {
            this.commonDao.SaveObject(category);
        }

        [Transaction(TransactionMode.RequiresNew)]
        public void SaveOrUpdateCategory(Category category)
        {
            this.commonDao.SaveOrUpdateObject(category);
        }

        [Transaction(TransactionMode.RequiresNew)]
        public void DeleteCategory(Category category)
        {
            this.commonDao.DeleteObject(category);
        }

        public void MoveCategoryToNewPath(string oldPath, string newPath)
        {
            //look for common ancestor
            int minLength = oldPath.Length;
            if (newPath.Length < oldPath.Length) minLength = newPath.Length;
            int i = 0;
            for (i = 5; i <= minLength; i = i + 5)
            {
                if (oldPath.Substring(0, i) != newPath.Substring(0, i)) break;
            }
            string commonAncestorPath = oldPath.Substring(0, i - 5);
            //a category can't be moved to one of its own descendant categories 
            if (commonAncestorPath == oldPath)
            {
                throw new ArgumentException("Can't move to descendant category");
            }
            //common ancestor is parent to both categories --> we've siblings    
            else if (commonAncestorPath == this.GetParentPath(oldPath) && commonAncestorPath == this.GetParentPath(newPath))
            {
                this.MoveCategorySiblings(oldPath, newPath, commonAncestorPath);
            }
            //we're moving from/to different subtrees
            else
            {
                this.MoveCategories(oldPath, newPath, commonAncestorPath);
            }
        }

        public int GetPositionFromPath(string path)
        {
            //get last 5 characters of the path
            string position = path.Substring(path.Length - 5);
            //convert to int
            return int.Parse(position.TrimStart('.', '0'));
        }

        public int GetPositionFromPathFragment(string pathFragment)
        {
            if (pathFragment.Length > 5) throw new ArgumentOutOfRangeException("Path fragments can only be up to 5 characters long (.0000)");
            else
            {
                return int.Parse(pathFragment.TrimStart('.', '0'));
            }
        }

        public string GetPathFragmentFromPosition(int position)
        {
            string pathFragment = position.ToString();
            if (pathFragment.Length > 4)
            {
                throw new ArgumentOutOfRangeException("Only up to 9999 categories are possible");
            }
            else
            {
                pathFragment = pathFragment.PadLeft(4, '0');
                return string.Concat(".", pathFragment);
            }
        }

        #endregion

        #region Category Movement

        /// <summary>
        /// Moves a category from oldPath to position of a category with newPath (both having the same parent category)
        /// The movement will be calculated using a "fill-the-gap" approach, since no add/removal takes place
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <param name="parentPath"></param>
        [Transaction(TransactionMode.RequiresNew)]
        protected void MoveCategorySiblings(string oldPath, string newPath, string parentPath)
        {
            int pathLength = newPath.Length; //same as oldPath.Length
            int oldPathPos = this.GetPositionFromPath(oldPath);
            int newPathPos = this.GetPositionFromPath(newPath);
            bool moveUp = (oldPathPos > newPathPos);
            //everything between the new path and the old path needs to be re-calculated
            IList<Category> categories = this.categoryDao.GetByPathStartsWith(parentPath);
            foreach (Category c in categories)
            {
                if (c.Path.Length >= pathLength)
                {
                    string prefix = c.Path.Substring(0, pathLength);
                    int pos = this.GetPositionFromPath(prefix);
                    if (pos == oldPathPos)
                    {
                        //replace all old prefixes with the new path prefix
                        prefix = newPath.Substring(0, pathLength);
                        c.Path = string.Concat(prefix, c.Path.Substring(prefix.Length));
                        this.commonDao.UpdateObject(c);
                    }
                    //check the movement direction 
                    else
                    {                    
                       if (moveUp)
                        {
                            //we're moving up and we're between the old and new path position
                            if (pos < oldPathPos && pos >= newPathPos)
                            {
                                prefix = this.MoveDownOnePosition(prefix);
                                c.Path = string.Concat(prefix, c.Path.Substring(prefix.Length));
                                this.commonDao.UpdateObject(c);
                            }
                        }
                        else
                        {   //we're moving down and we're between the old and new path position
                            if (pos > oldPathPos && pos <= newPathPos)
                            {
                                prefix = this.MoveUpOnePosition(prefix);
                                c.Path = string.Concat(prefix, c.Path.Substring(prefix.Length));
                                this.commonDao.UpdateObject(c);
                            }
                        }
                    }

                }

            }//end foreach

        }

        /// <summary>
        /// Moves one category to the position of another category (with both having different parent categories)
        /// The category will be removed at oldPath and added at newPath (correcting the parent category reference)
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <param name="commonAncestorPath"></param>
        [Transaction(TransactionMode.RequiresNew)]
        protected void MoveCategories(string oldPath, string newPath, string commonAncestorPath)
        {
            int oldPathPos = this.GetPositionFromPath(oldPath);
            string oldPathParent = this.GetParentPath(oldPath);
            //check if newPath is a descendant of an oldPath's sibling with a lower position 
            if (newPath.Length >= oldPath.Length && newPath.StartsWith(oldPathParent))
            {
                string prefix = newPath.Substring(0, oldPath.Length);
                int pos = this.GetPositionFromPath(prefix);
                //newPath will change because of oldPath movement, so anticipate the change
                if (pos > oldPathPos)
                {
                    prefix = this.MoveUpOnePosition(prefix);
                    newPath = string.Concat(prefix, newPath.Substring(prefix.Length));
                }
            }
            //remember the unchanged newPath
            string origNewPath = newPath;
            //here, we're always adding one position below the selected path
            newPath = this.MoveDownOnePosition(newPath);
            int newPathPos = this.GetPositionFromPath(newPath);
            string newPathParent = this.GetParentPath(newPath);
            Category oldPathCategory = null;
            Category newPathCategory = null;

            //get all (eventually) affected categories
            IList<Category> categories = this.categoryDao.GetByPathStartsWith(commonAncestorPath);
            foreach (Category c in categories)
            {
                //the paths starting with old path will be prefixed with the corresponding new path prefix
                if (c.Path.StartsWith(oldPath))
                {
                    if (c.Path == oldPath)
                    {
                        c.Path = newPath;
                        //we'll update this category last (still have to correct the parent reference)
                        oldPathCategory = c;
                    }
                    else
                    {
                        c.Path = string.Concat(newPath, c.Path.Substring(oldPath.Length));
                        this.commonDao.UpdateObject(c);
                    }
                }
                else
                {
                    bool update = false;
                    //every path below old path moves one up
                    if (c.Path.Length >= oldPath.Length && c.Path.StartsWith(oldPathParent))
                    {
                        string prefix = c.Path.Substring(0, oldPath.Length);
                        int pos = this.GetPositionFromPath(prefix);
                        if (pos > oldPathPos)
                        {
                            prefix = this.MoveUpOnePosition(prefix);
                            c.Path = string.Concat(prefix, c.Path.Substring(prefix.Length));
                            update = true;
                        }
                    }
                    //every path below or equal to new path has to be moved one down
                    if (c.Path.Length >= newPath.Length && c.Path.StartsWith(newPathParent))
                    {
                        if (c.Path == origNewPath)
                        {
                            newPathCategory = c;
                        }
                        string prefix = c.Path.Substring(0, newPath.Length);
                        int pos = this.GetPositionFromPath(prefix);
                        if (pos >= newPathPos)
                        {
                            prefix = this.MoveDownOnePosition(prefix);
                            c.Path = string.Concat(prefix, c.Path.Substring(prefix.Length));
                            update = true;
                        }
                    }
                    //update occurs conditionally, since a path could be changed 0-2 times
                    if (update)
                    {
                        this.commonDao.UpdateObject(c);
                    }
                }
            }// end foreach
            //correct the parent category reference
            oldPathCategory.ParentCategory.ChildCategories.Remove(oldPathCategory);
            newPathCategory.ParentCategory.ChildCategories.Add(oldPathCategory);
            oldPathCategory.ParentCategory = newPathCategory.ParentCategory;
            this.commonDao.UpdateObject(oldPathCategory);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Returns the supplied path, decremented by one in the last path fragment
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string MoveUpOnePosition(string path)
        {
            string fragment = this.GetPathFragmentFromPosition(this.GetPositionFromPath(path) - 1);
            return string.Concat(path.Substring(0, path.Length - 5), fragment);
        }

        /// <summary>
        /// Returns the supplied path, incremented by one in the last path fragment
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string MoveDownOnePosition(string path)
        {
            string fragment = this.GetPathFragmentFromPosition(this.GetPositionFromPath(path) + 1);
            return string.Concat(path.Substring(0, path.Length - 5), fragment);
        }

        /// <summary>
        /// Returns the computed parent path of the supplied path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string GetParentPath(string path)
        {
            return path.Substring(0, path.Length - 5);
        }

        #endregion

    }
}
