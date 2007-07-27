using System;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;


namespace Cuyahoga.Core.Service.Search
{
    public class SearchService: ISearchService
    {
        private string physicalIndexDir;
        private bool isRebuildingIndex = false;
        private static object lockObject = new object();
        private IndexBuilder indexBuilder;
        private IUserDao userDao;

        public SearchService(IUserDao userDao)
        {
            this.userDao = userDao;
            //TODO: test if this works on Mono?
            string rootDir = AppDomain.CurrentDomain.BaseDirectory;
            string indexDir = (Config.GetConfiguration()["SearchIndexDir"]).TrimStart('~', '/');
            this.physicalIndexDir = System.IO.Path.Combine(rootDir, indexDir);
        }

        #region ISearchService Members

        public void UpdateContent(SearchContent searchContent)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            this.indexBuilder.UpdateContent(searchContent);
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void AddContent(SearchContent searchContent)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            this.indexBuilder.AddContent(searchContent);
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void DeleteContent(SearchContent searchContent)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            this.indexBuilder.DeleteContent(searchContent);
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void UpdateContent(IList<SearchContent> searchContents)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            foreach (SearchContent searchContent in searchContents)
            {
                this.indexBuilder.UpdateContent(searchContent);
            }
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void AddContent(IList<SearchContent> searchContents)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            foreach (SearchContent searchContent in searchContents)
            {
                this.indexBuilder.AddContent(searchContent);
            }
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void DeleteContent(IList<SearchContent> searchContents)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            this.indexBuilder.DeleteContent(searchContents);
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void UpdateContent(IContentItem contentItem)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            this.indexBuilder.UpdateContent(contentItem);
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void AddContent(IContentItem contentItem)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            this.indexBuilder.AddContent(contentItem);
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void DeleteContent(IContentItem contentItem)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            this.indexBuilder.DeleteContent(contentItem);
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void UpdateContent(IList<IContentItem> contentItems)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            foreach (IContentItem contentItem in contentItems)
            {
                this.indexBuilder.UpdateContent(contentItem);
            }
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void AddContent(IList<IContentItem> contentItems)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            foreach (IContentItem contentItem in contentItems)
            {
                this.indexBuilder.AddContent(contentItem);
            }
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        public void DeleteContent(IList<IContentItem> contentItems)
        {
            this.indexBuilder = new IndexBuilder(this.physicalIndexDir, this.isRebuildingIndex);
            this.indexBuilder.DeleteContent(contentItems);
            if(!this.isRebuildingIndex) this.indexBuilder.Close();
        }

        //thread synchronisation: only one rebuild at a time allowed)
        public void StartRebuildingIndex()
        {
            //Monitor.Enter(lockObject);
            this.isRebuildingIndex = true;
        }

        public void EndRebuildingIndex()
        {
            this.isRebuildingIndex = false;
            if (this.indexBuilder != null)
            {
                this.indexBuilder.Close();
            }
            //Monitor.Exit(lockObject);
        }

        public SearchResultCollection FindContent(string queryText, IDictionary<string, string> keywordFilter, int pageIndex, int pageSize, User user)
        {
           return this.FindContent(queryText, keywordFilter, null, pageIndex, pageSize, user);
        }

        public SearchResultCollection FindContent(string queryText, IDictionary<string, string> keywordFilter, IList<string> categoryNames, int pageIndex, int pageSize, User user)
        {
            IList<Section> sections = null;
            IList<Role> roles = null;
            //use the user roles to determine viewable content
            if (user != null)
            {
                roles = new List<Role>( (new ArrayList(user.Roles)).ToArray(typeof(Role)) as Role[] );
                sections = this.userDao.GetViewableSectionsByUser(user);
            }
            //else assume anonymous access
            else
            {
                roles = this.userDao.GetRolesByAccessLevel(AccessLevel.Anonymous);
                sections = this.userDao.GetViewableSectionsByAccessLevel(AccessLevel.Anonymous);
            }
            List<int> sectionIds = new List<int>(sections.Count);
            List<int> roleIds = new List<int>(roles.Count);
            foreach (Section section in sections)
            {
                sectionIds.Add(section.Id);
            }
            foreach (Role role in roles)
            {
                roleIds.Add(role.Id);
            }
            IndexQuery query = new IndexQuery(this.physicalIndexDir);
            return query.Find(queryText, keywordFilter, categoryNames, pageIndex, pageSize, sectionIds, roleIds);
        }


        #endregion
    }
}
