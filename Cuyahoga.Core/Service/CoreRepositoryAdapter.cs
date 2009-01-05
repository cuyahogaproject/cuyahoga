using System;
using System.Collections;

using NHibernate;
using log4net;
using Castle.Facilities.NHibernateIntegration;
using Castle.Core;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Service.SiteStructure;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// Adapter for the Cuyahoga 1.0 CoreRepository that delegates everything to the new services.
	/// </summary>
	//[Transient]
	public class CoreRepositoryAdapter : CoreRepository
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(CoreRepositoryAdapter));

		private ICommonDao _commonDao;
		private ISiteStructureDao _siteStructureDao;
		private IUserDao _userDao;
		private INodeService _nodeService;
		private ISessionManager _sessionManager;

		public override ISession ActiveSession
		{
			get { return this._sessionManager.OpenSession(); }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="commonDao"></param>
		/// <param name="siteStructureDao"></param>
		/// <param name="userDao"></param>
		public CoreRepositoryAdapter(ICommonDao commonDao, ISiteStructureDao siteStructureDao, IUserDao userDao, INodeService nodeService, ISessionManager sessionManager)
		{
			this._commonDao = commonDao;
			this._siteStructureDao = siteStructureDao;
			this._userDao = userDao;
			this._nodeService = nodeService;
			this._sessionManager = sessionManager;
		}

		public override void FlushSession()
		{
			this.ActiveSession.Flush();
		}

		public override object GetObjectById(Type type, int id)
		{
			return this._commonDao.GetObjectById(type, id);
		}

		public override object GetObjectById(Type type, int id, bool allowNull)
		{
			return this._commonDao.GetObjectById(type, id, allowNull);
		}

		public override object GetObjectByDescription(Type type, string propertyName, string description)
		{
			return this._commonDao.GetObjectByDescription(type, propertyName, description);
		}

		public override IList GetAll(Type type)
		{
			return this._commonDao.GetAll(type);
		}

		public override IList GetAll(Type type, params string[] sortProperties)
		{
			return this._commonDao.GetAll(type, sortProperties);
		}

		public override void SaveObject(object obj)
		{
			this._commonDao.SaveOrUpdateObject(obj);
		}

		public override void UpdateObject(object obj)
		{
			this._commonDao.SaveOrUpdateObject(obj);
		}

		public override void DeleteObject(object obj)
		{
			this._commonDao.DeleteObject(obj);
		}

		public override void MarkForDeletion(object obj)
		{
			this._commonDao.MarkForDeletion(obj);
		}

		public override void ClearCache(Type type)
		{
			log.Info("Clearing cache for type " + type.Name);
			this.ActiveSession.SessionFactory.Evict(type);
		}

		public override void ClearCollectionCache(string roleName)
		{
			log.Info("Clearing cache for collection property " + roleName);
			this.ActiveSession.SessionFactory.EvictCollection(roleName);
		}
	
		public override void ClearQueryCache(string cacheRegion)
		{
			log.Info("Clearing query cache for cacheregion " + cacheRegion);
			this.ActiveSession.SessionFactory.EvictQueries(cacheRegion);
		}

		public override Site GetSiteBySiteUrl(string siteUrl)
		{
			return this._siteStructureDao.GetSiteBySiteUrl(siteUrl);
		}

		public override SiteAlias GetSiteAliasByUrl(string url)
		{
			return this._siteStructureDao.GetSiteAliasByUrl(url);
		}

		public override IList GetSiteAliasesBySite(Site site)
		{
			return this._siteStructureDao.GetSiteAliasesBySite(site);
		}

		public override IList GetRootNodes(Site site)
		{
			return this._siteStructureDao.GetRootNodes(site);
		}

		public override Node GetRootNodeByCultureAndSite(string culture, Site site)
		{
			return this._siteStructureDao.GetRootNodeByCultureAndSite(culture, site);
		}

		public override Node GetNodeByShortDescriptionAndSite(string shortDescription, Site site)
		{
			return this._siteStructureDao.GetNodeByShortDescriptionAndSite(shortDescription, site);
		}

		public override IList GetNodesByTemplate(Template template)
		{
			return this._siteStructureDao.GetNodesByTemplate(template);
		}

		public override void UpdateNode(Node node, bool propagatePermissionsToChildNodes, bool propagatePermissionsToSections)
		{
			this._nodeService.UpdateNode(node, propagatePermissionsToChildNodes, propagatePermissionsToSections);
		}

		public override void DeleteNode(Node node)
		{
			this._nodeService.DeleteNode(node);
		}

		public override IList GetMenusByRootNode(Node rootNode)
		{
			return this._siteStructureDao.GetMenusByRootNode(rootNode);
		}

		public override IList GetSortedSectionsByNode(Node node)
		{
			return this._siteStructureDao.GetSortedSectionsByNode(node);
		}

		public override IList GetSectionsByModuleTypes(IList moduleTypes)
		{
			return this._siteStructureDao.GetSectionsByModuleTypes(moduleTypes);
		}

		public override IList GetTemplatesBySection(Section section)
		{
			return this._siteStructureDao.GetTemplatesBySection(section);
		}

		public override IList GetUnconnectedSections()
		{
			return this._siteStructureDao.GetUnconnectedSections();
		}

		public override User GetUserByUsernameAndPassword(string username, string password)
		{
			return this._userDao.GetUserByUsernameAndPassword(username, password);
		}

		public override User GetUserByUsernameAndEmail(string username, string email)
		{
			return this._userDao.GetUserByUsernameAndEmail(username, email);
		}

		public override IList FindUsersByUsername(string searchString)
		{
			return this._userDao.FindUsersByUsername(searchString);
		}
	}
}