using System;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Util;


namespace Cuyahoga.Core.Service.Search
{
	public class SearchDecorator<T> : AbstractContentItemServiceDecorator<T> where T : IContentItem
	{
		private readonly ISearchService _searchService;

		public SearchDecorator(IContentItemService<T> contentItemService, ISearchService searchService)
			: base(contentItemService)
		{
			this._searchService = searchService; 
		}

		/// <summary>
		/// Todo: move to properties.config
		/// </summary>
		private bool UseInstantIndexing
		{
			get
			{
				return (Boolean.Parse(Config.GetConfiguration()["InstantIndexing"]));
			}
		}

		#region Overrides

		public override T Save(T entity)
		{
			// First, save entity to the database, so it has an identifier. Otherwise, the entity should be indexed with the wrong path.
			entity = base.Save(entity);

			if (UseInstantIndexing && entity is ISearchableContent)
			{
				if (entity.IsNew)
				{
					this._searchService.AddContent(entity);
				}
				else
				{
					this._searchService.UpdateContent(entity);
				}
			}
			return entity;
		}

		public override void Delete(T entity)
		{
			if (UseInstantIndexing)
			{
				if (entity is ISearchableContent)
				{
					this._searchService.DeleteContent(entity);
				}
			}
			base.Delete(entity);
		}

		#endregion
	}
}