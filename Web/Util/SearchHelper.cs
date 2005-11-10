using System;
using System.Web;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;

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
			string indexDir = HttpContext.Current.Server.MapPath(Config.GetConfiguration()["SearchIndexDir"]);
			IndexBuilder ib = new IndexBuilder(indexDir, false);

			ModuleBase module = section.CreateModule(Util.UrlHelper.GetUrlFromSection(section));
			if (module is ISearchable)
			{
				ib.UpdateContentFromModule(module);
			}

			ib.Close();
		}
	}
}
