using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Castle.Services.Transaction;
using Cuyahoga.Core.Service.Files;
using log4net;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality to manage site instances.
	/// </summary>
	[Transactional]
	public class SiteService : ISiteService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SiteService));
		private ISiteStructureDao _siteStructureDao;
		private ICommonDao _commonDao;
		private IFileService _fileService;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="siteStructureDao"></param>
		/// <param name="commonDao"></param>
		/// <param name="fileService"></param>
		public SiteService(ISiteStructureDao siteStructureDao, ICommonDao commonDao, IFileService fileService)
		{
			this._siteStructureDao = siteStructureDao;
			this._commonDao = commonDao;
			this._fileService = fileService;
		}

		#region ISiteService Members

		public Site GetSiteById(int siteId)
		{
			return (Site)this._commonDao.GetObjectById(typeof(Site), siteId);
		}

		public Site GetSiteBySiteUrl(string siteUrl)
		{
			Site site = this._siteStructureDao.GetSiteBySiteUrl(siteUrl);
			// Try to resolve the site via SiteAlias
			if (site == null)
			{
				SiteAlias sa = this._siteStructureDao.GetSiteAliasByUrl(siteUrl);
				if (sa != null)
				{
					site = sa.Site;
				}
			}
			return site;
		}

		public SiteAlias GetSiteAliasById(int siteAliasId)
		{
			return (SiteAlias)this._commonDao.GetObjectById(typeof(SiteAlias), siteAliasId);
		}

		public SiteAlias GetSiteAliasByUrl(string url)
		{
			return this._siteStructureDao.GetSiteAliasByUrl(url);
		}

		public IList<SiteAlias> GetSiteAliasesBySite(Site site)
		{
			return this._siteStructureDao.GetSiteAliasesBySite(site);
		}

		public IList GetAllSites()
		{
			return this._commonDao.GetAll(typeof(Site));
		}

		[Transaction(TransactionMode.RequiresNew)]
		public virtual void CreateSite(Site site, string siteDataRoot, IList<Template> templatesToCopy, string systemTemplatesDirectory)
		{
			try
			{
				// 1. Add global roles to site
				IList<Role> roles = this._commonDao.GetAll<Role>();
				foreach (Role role in roles)
				{
					if (role.IsGlobal)
					{
						site.Roles.Add(role);
					}
				}

				// 2. Save site in database
				this._commonDao.SaveObject(site);

				// 3. Create SiteData folder structure
				if (! this._fileService.CheckIfDirectoryIsWritable(siteDataRoot))
				{
					throw new IOException(string.Format("Unable to create the site because the directory {0} is not writable.", siteDataRoot));
				}
				string siteDataPhysicalDirectory = Path.Combine(siteDataRoot, site.Id.ToString());
				this._fileService.CreateDirectory(siteDataPhysicalDirectory);
				this._fileService.CreateDirectory(Path.Combine(siteDataPhysicalDirectory, "UserFiles"));
				this._fileService.CreateDirectory(Path.Combine(siteDataPhysicalDirectory, "index"));
				string siteTemplatesDirectory = Path.Combine(siteDataPhysicalDirectory, "Templates");
				this._fileService.CreateDirectory(siteTemplatesDirectory);

				// 4. Copy templates
				IList<string> templateDirectoriesToCopy = new List<string>();
				foreach (Template template in templatesToCopy)
				{
					string templateDirectoryName = template.BasePath.Substring(template.BasePath.IndexOf("/") + 1);
					if (! templateDirectoriesToCopy.Contains(templateDirectoryName))
					{
						templateDirectoriesToCopy.Add(templateDirectoryName);
					}
					Template newTemplate = template.GetCopy();
					newTemplate.Site = site;
					site.Templates.Add(newTemplate);
					this._commonDao.SaveOrUpdateObject(newTemplate);
					this._commonDao.SaveOrUpdateObject(site);
				}
				foreach (string templateDirectory in templateDirectoriesToCopy)
				{
					string sourceDir = Path.Combine(systemTemplatesDirectory, templateDirectory);
					string targetDir = Path.Combine(siteTemplatesDirectory, templateDirectory);
					this._fileService.CopyDirectoryContents(sourceDir, targetDir);
				}
			}
			catch (Exception ex)
			{
				log.Error("An unexpected error occured while creating a new site.", ex);
				throw;
			}
		}

		[Transaction(TransactionMode.RequiresNew)]
		public virtual void SaveSite(Site site)
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

		[Transaction(TransactionMode.RequiresNew)]
		public virtual void DeleteSite(Site site)
		{
			if (site.RootNodes.Count > 0)
			{
				throw new Exception("Can't delete a site when there are still related nodes. Please delete all nodes before deleting an entire site.");
			}
			else
			{
				IList<SiteAlias> aliases = this._siteStructureDao.GetSiteAliasesBySite(site);
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

		[Transaction(TransactionMode.RequiresNew)]
		public virtual void SaveSiteAlias(SiteAlias siteAlias)
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

		[Transaction(TransactionMode.RequiresNew)]
		public virtual void DeleteSiteAlias(SiteAlias siteAlias)
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
