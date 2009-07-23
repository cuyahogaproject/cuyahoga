using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Search;

namespace Cuyahoga.Core.Service.Search
{
	public class SearchService : ISearchService
	{
		private readonly IUserDao _userDao;
		private readonly ICuyahogaContextProvider _cuyahogaContextProvider;

		public SearchService(IUserDao userDao, ICuyahogaContextProvider cuyahogaContextProvider)
		{
			this._userDao = userDao;
			_cuyahogaContextProvider = cuyahogaContextProvider;
		}

		#region ISearchService Members

		public void UpdateContent(SearchContent searchContent)
		{
			using (IndexBuilder indexBuilder = CreateIndexBuilder())
			{
				indexBuilder.UpdateContent(searchContent);
			}
		}

		public void AddContent(SearchContent searchContent)
		{
			using (IndexBuilder indexBuilder = CreateIndexBuilder())
			{
				indexBuilder.AddContent(searchContent);
			}
		}

		public void DeleteContent(SearchContent searchContent)
		{
			using (IndexBuilder indexBuilder = CreateIndexBuilder())
			{
				indexBuilder.DeleteContent(searchContent);
			}
		}

		public void AddContent(IList<SearchContent> searchContents)
		{
			using (IndexBuilder indexBuilder = CreateIndexBuilder())
			{
				foreach (SearchContent searchContent in searchContents)
				{
					indexBuilder.AddContent(searchContent);
				}
			}
		}

		public void UpdateContent(IContentItem contentItem)
		{
			using (IndexBuilder indexBuilder = CreateIndexBuilder())
			{
				indexBuilder.UpdateContent(contentItem);
			}
		}

		public void AddContent(IContentItem contentItem)
		{
			using (IndexBuilder indexBuilder = CreateIndexBuilder())
			{
				indexBuilder.AddContent(contentItem);
			}
		}

		public void DeleteContent(IContentItem contentItem)
		{
			using (IndexBuilder indexBuilder = CreateIndexBuilder())
			{
				indexBuilder.DeleteContent(contentItem);
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
				sections = this._userDao.GetViewableSectionsByUser(user);
			}
			//else assume anonymous access
			else
			{
				roles = this._userDao.GetRolesByAccessLevel(AccessLevel.Anonymous);
				sections = this._userDao.GetViewableSectionsByAccessLevel(AccessLevel.Anonymous);
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
			IndexQuery query = new IndexQuery(GetIndexDirectory());
			return query.Find(queryText, keywordFilter, categoryNames, pageIndex, pageSize, sectionIds, roleIds);
		}


		#endregion

		private IndexBuilder CreateIndexBuilder()
		{
			return new IndexBuilder(GetIndexDirectory(), false);
		}

		private string GetIndexDirectory()
		{
			ICuyahogaContext cuyahogaContext = this._cuyahogaContextProvider.GetContext();
			return Path.Combine(cuyahogaContext.PhysicalSiteDataDirectory, "index");
		}
	}
}
