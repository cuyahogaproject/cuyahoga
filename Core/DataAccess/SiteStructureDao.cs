using System;
using System.Collections;

using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using NHibernate;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// Provides data access for site structure components. NHibernate is being used here.
	/// </summary>
	[Transactional]
	public class SiteStructureDao : ISiteStructureDao
	{
		private ISessionManager _sessionManager;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="sessionManager"></param>
		public SiteStructureDao(ISessionManager sessionManager)
		{
			this._sessionManager = sessionManager;
		}

		#region ISiteStructureDao Members

		public Site GetSiteBySiteUrl(string siteUrl)
		{
			ISession session = this._sessionManager.OpenSession();

			// The query is case insensitive.
			string hql = "from Site s where lower(s.SiteUrl) = :siteUrl1 or lower(s.SiteUrl) = :siteUrl2";
			IQuery q = session.CreateQuery(hql);
			q.SetString("siteUrl1", siteUrl.ToLower());
			q.SetString("siteUrl2", siteUrl.ToLower() + "/"); // Also allow trailing slashes
			q.SetCacheable(true);
			q.SetCacheRegion("Sites");
			return q.UniqueResult() as Site;
		}

		public SiteAlias GetSiteAliasByUrl(string url)
		{
			ISession session = this._sessionManager.OpenSession();

			// The query is case insensitive.
			string hql = "from SiteAlias sa where lower(sa.Url) = :url1 or lower(sa.Url) = :url2";
			IQuery q = session.CreateQuery(hql);
			q.SetString("url1", url.ToLower());
			q.SetString("url2", url.ToLower() + "/"); // Also allow trailing slashes
			q.SetCacheable(true);
			q.SetCacheRegion("Sites");
			return q.UniqueResult() as SiteAlias;
		}

		public IList GetSiteAliasesBySite(Site site)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from SiteAlias sa where sa.Site.Id = :siteId ";
			IQuery query = session.CreateQuery(hql);
			query.SetInt32("siteId", site.Id);
			return query.List();
		}

		public IList GetRootNodes(Site site)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from Node n where n.ParentNode is null and n.Site.Id = :siteId order by n.Position";
			IQuery q = session.CreateQuery(hql);
			q.SetInt32("siteId", site.Id);
			q.SetCacheable(true);
			q.SetCacheRegion("Nodes");
			return q.List();
		}

		public Node GetRootNodeByCultureAndSite(string culture, Site site)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from Node n where n.ParentNode is null and n.Culture = :culture and n.Site.Id = :siteId";
			IQuery q = session.CreateQuery(hql);
			q.SetString("culture", culture);
			q.SetInt32("siteId", site.Id);
			q.SetCacheable(true);
			q.SetCacheRegion("Nodes");
			IList results = q.List();
			if (results.Count == 1)
			{
				return results[0] as Node;
			}
			else if (results.Count == 0)
			{
				throw new NodeNullException(String.Format("No root node found for culture {0} and site {1}.", culture, site.Id));
			}
			else
			{
				throw new Exception(String.Format("Multiple root nodes found for culture {0} and site {1}.", culture, site.Id));
			}
		}

		public Node GetNodeByShortDescriptionAndSite(string shortDescription, Site site)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from Node n where n.ShortDescription = :shortDescription and n.Site.Id = :siteId";
			IQuery q = session.CreateQuery(hql);
			q.SetString("shortDescription", shortDescription);
			q.SetInt32("siteId", site.Id);
			q.SetCacheable(true);
			q.SetCacheRegion("Nodes");
			IList results = q.List();
			if (results.Count == 1)
			{
				return (Node)results[0];
			}
			else if (results.Count > 1)
			{
				throw new Exception(String.Format("Multiple nodes found for ShortDescription {0}. The ShortDescription should be unique.", shortDescription));
			}
			else
			{
				return null;
			}
		}

		public IList GetNodesByTemplate(Template template)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from Node n where n.Template.Id = :templateId ";
			IQuery q = session.CreateQuery(hql);
			q.SetInt32("templateId", template.Id);
			return q.List();
		}

		public IList GetMenusByRootNode(Node rootNode)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from CustomMenu m where m.RootNode.Id = :rootNodeId";
			IQuery q = session.CreateQuery(hql);
			q.SetInt32("rootNodeId", rootNode.Id);
			q.SetCacheable(true);
			q.SetCacheRegion("Menus");
			return q.List();
		}

		public IList GetMenusByParticipatingNode(Node node)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "select m from CustomMenu m join m.Nodes n where n.Id = :nodeId";
			IQuery q = session.CreateQuery(hql);
			q.SetInt32("nodeId", node.Id);
			return q.List();
		}

		public IList GetSortedSectionsByNode(Node node)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from Section s where s.Node.Id = :nodeId order by s.PlaceholderId, s.Position ";
			IQuery q = session.CreateQuery(hql);
			q.SetInt32("nodeId", node.Id);
			return q.List();
		}

		public IList GetUnconnectedSections()
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from Section s where s.Node is null order by s.Title";
			IQuery q = session.CreateQuery(hql);
			return q.List();
		}

		public IList GetTemplatesBySection(Section section)
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "from Template t where :section in elements(t.Sections)";
			IQuery q = session.CreateQuery(hql);
			q.SetParameter("section", section);
			return q.List();
		}

		public IList GetSectionsByModuleTypes(IList moduleTypes)
		{
			if (moduleTypes.Count > 0)
			{
				string[] ids = new string[moduleTypes.Count];
				int idx = 0;
				foreach (ModuleType mt in moduleTypes)
				{
					ids[idx] = mt.ModuleTypeId.ToString();
					idx++;
				}
				ISession session = this._sessionManager.OpenSession();

				string hql = "from Section s where s.ModuleType.ModuleTypeId in (" + String.Join(",", ids) + ")";
				IQuery q = session.CreateQuery(hql);
				return q.List();
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Get all module types that are currently in use (that have related sections) 
		/// in the Cuyahoga installation.
		/// </summary>
		/// <returns></returns>
		public IList GetAllModuleTypesInUse()
		{
			ISession session = this._sessionManager.OpenSession();

			string hql = "select distinct mt from Section s join s.ModuleType mt";
			IQuery q = session.CreateQuery(hql);
			return q.List();
		}


		[Transaction(TransactionMode.Requires)]
		public void SaveSite(Site site)
		{
			ISession session = this._sessionManager.OpenSession();

			// Clear query cache first
			session.SessionFactory.EvictQueries("Sites");

			// Save site
			session.SaveOrUpdate(site);
		}

		[Transaction(TransactionMode.Requires)]
		public void DeleteSite(Site site)
		{
			ISession session = this._sessionManager.OpenSession();

			// Clear query cache first
			session.SessionFactory.EvictQueries("Sites");

			// Delete site
			session.Delete(site);
		}

		[Transaction(TransactionMode.Requires)]
		public void SaveSiteAlias(SiteAlias siteAlias)
		{
			ISession session = this._sessionManager.OpenSession();

			// Clear query cache first
			session.SessionFactory.EvictQueries("Sites");

			// Save site
			session.SaveOrUpdate(siteAlias);
		}

		[Transaction(TransactionMode.Requires)]
		public void DeleteSiteAlias(SiteAlias siteAlias)
		{
			ISession session = this._sessionManager.OpenSession();

			// Clear query cache first
			session.SessionFactory.EvictQueries("Sites");

			// Delete site
			session.Delete(siteAlias);
		}

		#endregion
	}
}
