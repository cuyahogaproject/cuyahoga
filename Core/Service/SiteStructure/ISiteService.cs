using System;
using System.Collections;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality to manage site instances.
	/// </summary>
	public interface ISiteService
	{
		/// <summary>
		/// Get a single site by root url.
		/// </summary>
		/// <param name="siteUrl"></param>
		/// <returns></returns>
		Site GetSiteBySiteUrl(string siteUrl);

		/// <summary>
		/// Get a single site alias by root url.
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		SiteAlias GetSiteAliasByUrl(string url);

		/// <summary>
		/// Get all site aliases by a given site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList GetSiteAliasesBySite(Site site);
	}
}
