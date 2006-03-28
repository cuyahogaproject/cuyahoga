using System;
using System.Collections;

using log4net;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality to manage site instances.
	/// </summary>
	public class SiteService : ISiteService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SiteService));
		private ISiteStructureDao _siteStructureDao;
		private ICommonDao _commonDao;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="siteStructureDao"></param>
		public SiteService(ISiteStructureDao siteStructureDao, ICommonDao commonDao)
		{
			this._siteStructureDao = siteStructureDao;
			this._commonDao = commonDao;
		}

		#region ISiteService Members

		public Site GetSiteById(int siteId)
		{
			return (Site)this._commonDao.GetObjectById(typeof(Site), siteId);
		}

		public Site GetSiteBySiteUrl(string siteUrl)
		{
			return this._siteStructureDao.GetSiteBySiteUrl(siteUrl);
		}

		public SiteAlias GetSiteAliasById(int siteAliasId)
		{
			return (SiteAlias)this._commonDao.GetObjectById(typeof(SiteAlias), siteAliasId);
		}

		public SiteAlias GetSiteAliasByUrl(string url)
		{
			return this._siteStructureDao.GetSiteAliasByUrl(url);
		}

		public IList GetSiteAliasesBySite(Site site)
		{
			return this._siteStructureDao.GetSiteAliasesBySite(site);
		}

		public IList GetAllSites()
		{
			return this._commonDao.GetAll(typeof(Site));
		}

		public void SaveSite(Site site)
		{
			try
			{
				// We need to use a specific DAO to also enable clearing the query cache.
				this._siteStructureDao.SaveSite(site);
			}
			catch (Exception ex)
			{
				log.Error("Error saving site", ex);
				throw;
			}
		}

		public void DeleteSite(Site site)
		{
			if (site.RootNodes.Count > 0)
			{
				throw new Exception("Can't delete a site when there are still related nodes. Please delete all nodes before deleting an entire site.");
			}
			else
			{
				IList aliases = this._siteStructureDao.GetSiteAliasesBySite(site);
				if (aliases.Count > 0)
				{
					throw new Exception("Unable to delete a site when a site has related aliases.");
				}
				else
				{
					try
					{
						// We need to use a specific DAO to also enable clearing the query cache.
						this._siteStructureDao.DeleteSite(site);
					}
					catch (Exception ex)
					{
						log.Error("Error deleting site", ex);
						throw;
					}

				}				
			}
		}

		public void SaveSiteAlias(SiteAlias siteAlias)
		{
			try
			{
				// We need to use a specific DAO to also enable clearing the query cache.
				this._siteStructureDao.SaveSiteAlias(siteAlias);
			}
			catch (Exception ex)
			{
				log.Error("Error saving site alias", ex);
				throw;
			}
		}

		public void DeleteSiteAlias(SiteAlias siteAlias)
		{
			try
			{
				// We need to use a specific DAO to also enable clearing the query cache.
				this._siteStructureDao.DeleteSiteAlias(siteAlias);
			}
			catch (Exception ex)
			{
				log.Error("Error deleting site alias", ex);
				throw;
			}
		}

		#endregion
	}
}
