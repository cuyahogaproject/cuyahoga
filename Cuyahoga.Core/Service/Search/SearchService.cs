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
	public class SearchService : ISearchService
	{
		private string physicalIndexDir;
		private bool isRebuildingIndex = false;
		private static object lockObject = new object();
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
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				indexBuilder.UpdateContent(searchContent);
			}
		}

		public void AddContent(SearchContent searchContent)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				indexBuilder.AddContent(searchContent);
			}
		}

		public void DeleteContent(SearchContent searchContent)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				indexBuilder.DeleteContent(searchContent);
			}
		}

		public void UpdateContent(IList<SearchContent> searchContents)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				foreach (SearchContent searchContent in searchContents)
				{
					indexBuilder.UpdateContent(searchContent);
				}
			}
		}

		public void AddContent(IList<SearchContent> searchContents)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				foreach (SearchContent searchContent in searchContents)
				{
					indexBuilder.AddContent(searchContent);
				}
			}
		}

		public void DeleteContent(IList<SearchContent> searchContents)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				indexBuilder.DeleteContent(searchContents);
			}
		}

		public void UpdateContent(IContentItem contentItem)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				indexBuilder.UpdateContent(contentItem);
			}
		}

		public void AddContent(IContentItem contentItem)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				indexBuilder.AddContent(contentItem);
			}
		}

		public void DeleteContent(IContentItem contentItem)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				indexBuilder.DeleteContent(contentItem);
			}
		}

		public void UpdateContent(IList<IContentItem> contentItems)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				foreach (IContentItem contentItem in contentItems)
				{
					indexBuilder.UpdateContent(contentItem);
				}
			}
		}

		public void AddContent(IList<IContentItem> contentItems)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				foreach (IContentItem contentItem in contentItems)
				{
					indexBuilder.AddContent(contentItem);
				}
			}
		}

		public void DeleteContent(IList<IContentItem> contentItems)
		{
			using (IndexBuilder indexBuilder = new IndexBuilder(this.physicalIndexDir, false))
			{
				indexBuilder.DeleteContent(contentItems);
			}
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
				roles = new List<Role>((new ArrayList(user.Roles)).ToArray(typeof(Role)) as Role[]);
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
