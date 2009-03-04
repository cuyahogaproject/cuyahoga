using System;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Util;


namespace Cuyahoga.Core.Service.Search
{
	public class SearchDecorator<T> : AbstractContentItemServiceDecorator<T> where T : IContentItem
	{
		private readonly ISearchService searchService;

		public SearchDecorator(IContentItemService<T> contentItemService, ISearchService searchService)
			: base(contentItemService)
		{
			this.searchService = searchService; 
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
			if (UseInstantIndexing && entity is ISearchableContent)
			{
				this.searchService.AddContent(entity);
			}
			return base.Save(entity);
		}

		public override void Delete(T entity)
		{
			if (UseInstantIndexing)
			{
				if (entity is ISearchableContent)
				{
					this.searchService.DeleteContent(entity);
				}
			}
			base.Delete(entity);
		}

		#endregion
	}
}