using System;
using System.Collections.Generic;
using System.Web;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Service.Search;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Components;

namespace Cuyahoga.Web.Util
{
	/// <summary>
	/// The SearchHelper class contains utilty methods for maintaining the full text index.
	/// </summary>
	public class SearchHelper
	{
		private SearchHelper()
		{
		}

		/// <summary>
		/// Update the search index with the contents of the section.
		/// </summary>
		/// <param name="section"></param>
		public static void UpdateIndexFromSection(Section section)
		{
            CuyahogaContainer container =  ContainerAccessorUtil.GetContainer();
			// Get ModuleLoader from the container. This needs to happen explicit here because
			// this is a static method
            ModuleLoader moduleLoader = container.Resolve<ModuleLoader>();
            ISearchService searchService = container.Resolve<ISearchService>();
            IContentItemService<ContentItem> contentItemService = container.Resolve<IContentItemService<ContentItem>>();

			if (moduleLoader == null)
			{
				throw new NullReferenceException("Unable to find the ModuleLoader instance");
			}
			string indexDir = HttpContext.Current.Server.MapPath(Config.GetConfiguration()["SearchIndexDir"]);

			ModuleBase module = moduleLoader.GetModuleFromSection(section);
			if (module is ISearchable)
			{
                ISearchable searchableModule = module as ISearchable;
                if (searchableModule != null)
                {
                    SearchContent[] searchContentList = searchableModule.GetAllSearchableContent();
                    foreach (SearchContent searchContent in searchContentList)
                    {
                        searchService.UpdateContent(searchContent);
                    }
                }
            }
            //check for IContentItems
            else
            {
                IList<ContentItem> contents = contentItemService.FindContentItemsBySection(section);
                foreach (ContentItem content in contents)
                {
                    if (content is ISearchableContent)
                    {
                        searchService.UpdateContent(content);
                    }
                }

            }

		}//end method
	}
}
