using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Service.Content;
using log4net;
using Lucene.Net.Index;

namespace Cuyahoga.Core.Service.Search
{
	public class SearchService : ISearchService
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (SearchService));
		private readonly IUserDao _userDao;
		private readonly ICuyahogaContextProvider _cuyahogaContextProvider;
		private readonly ITextExtractor _textExtractor;
		private readonly IContentItemService<IContentItem> _contentItemService;

		public SearchService(IUserDao userDao, ICuyahogaContextProvider cuyahogaContextProvider, ITextExtractor textExtractor, IContentItemService<IContentItem> contentItemService)
		{
			this._userDao = userDao;
			this._cuyahogaContextProvider = cuyahogaContextProvider;
			this._textExtractor = textExtractor;
			this._contentItemService = contentItemService;
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

		public SearchIndexProperties GetIndexProperties()
		{
			SearchIndexProperties indexProperties = new SearchIndexProperties();
			indexProperties.IndexDirectory = GetIndexDirectory();

			IndexReader indexReader = IndexReader.Open(indexProperties.IndexDirectory);
			try
			{
				indexProperties.NumberOfDocuments = indexReader.NumDocs();
			}
			finally
			{
				indexReader.Close();
			}
			indexProperties.LastModified = new DateTime(IndexReader.LastModified(indexProperties.IndexDirectory));
			return indexProperties;
		}

		/// <summary>
		/// Rebuild the full-text index.
		/// </summary>
		/// <param name="searchableModules">A list of (legacy) searchable modules in the installation.</param>
		public void RebuildIndex(IEnumerable<ISearchable> searchableModules)
		{
			Site currentSite = this._cuyahogaContextProvider.GetContext().CurrentSite;
			string indexDirectory = GetIndexDirectory();

			using (IndexBuilder indexBuilder = new IndexBuilder(indexDirectory, true, this._textExtractor))
			{
				// Add all content items
				var contentItemsToIndex = this._contentItemService.FindContentItemsBySite(currentSite);
				foreach (IContentItem contentItem in contentItemsToIndex)
				{
					if (contentItem is ISearchableContent)
					{
						try
						{
							indexBuilder.AddContent(contentItem);
						}
						catch (Exception ex)
						{
							Logger.Error(string.Format("Error while indexing ContentItem with id {0}", contentItem.Id), ex);
							throw;
						}						
					}
				}

				// Add legacy searchable content
				if (searchableModules != null)
				{
					foreach (ISearchable searchableModule in searchableModules)
					{
						foreach (SearchContent searchContent in searchableModule.GetAllSearchableContent())
						{
							try
							{
								indexBuilder.AddContent(searchContent);
							}
							catch (Exception ex)
							{
								Logger.Error(string.Format("Indexing of legacy searchContent item with path {0} failed.", searchContent.Path),
								             ex);
								throw;
							}
						}
					}
				}
			}
		}

		#endregion

		private IndexBuilder CreateIndexBuilder()
		{
			return new IndexBuilder(GetIndexDirectory(), false, this._textExtractor);
		}

		private string GetIndexDirectory()
		{
			ICuyahogaContext cuyahogaContext = this._cuyahogaContextProvider.GetContext();
			return Path.Combine(cuyahogaContext.PhysicalSiteDataDirectory, "index");
		}
	}
}
