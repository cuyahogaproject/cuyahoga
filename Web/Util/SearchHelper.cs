using System;
using System.Web;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;
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
			// Get ModuleLoader from the container. This needs to happen explicit here because
			// this is a static method
			ModuleLoader moduleLoader = ContainerAccessorUtil.GetContainer()[typeof(ModuleLoader)] as ModuleLoader;
			if (moduleLoader == null)
			{
				throw new NullReferenceException("Unable to find the ModuleLoader instance");
			}
			string indexDir = HttpContext.Current.Server.MapPath(Config.GetConfiguration()["SearchIndexDir"]);
			IndexBuilder ib = new IndexBuilder(indexDir, false);

			ModuleBase module = moduleLoader.GetModuleFromSection(section);
			if (module is ISearchable)
			{
				ib.UpdateContentFromModule(module);
			}

			ib.Close();
		}
	}
}
