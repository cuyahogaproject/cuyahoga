using System;
using System.Collections;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality to manage site instances.
	/// </summary>
	public class SiteService : ISiteService
	{
		private ISiteStructureDao _siteStructureDao;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="siteStructureDao"></param>
		public SiteService(ISiteStructureDao siteStructureDao)
		{
			this._siteStructureDao = siteStructureDao;
		}

		#region ISiteService Members

		public Site GetSiteBySiteUrl(string siteUrl)
		{
			return this._siteStructureDao.GetSiteBySiteUrl(siteUrl);
		}

		public SiteAlias GetSiteAliasByUrl(string url)
		{
			return this._siteStructureDao.GetSiteAliasByUrl(url);
		}

		public IList GetSiteAliasesBySite(Site site)
		{
			return this._siteStructureDao.GetSiteAliasesBySite(site);
		}

		#endregion
	}
}
