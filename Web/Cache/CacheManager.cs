using System;
using System.Collections;
using System.Web;

using Cuyahoga;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Cache
{
	/// <summary>
	/// The CacheManager stores the site structure objects (Site, Node, Section) in the ASP.NET cache and 
	/// provides methods for retrieval of these objects. 
	/// </summary>
	public class CacheManager
	{
		private CoreRepository _coreRepository;
		private NodeCache _nodeCache;
		private bool _hasChanges;

		/// <summary>
		/// Property HasChanges (bool)
		/// </summary>
		public bool HasChanges
		{
			get { return this._hasChanges; }
		}

		/// <summary>
		/// Default constructor. If the cache is empty, it will be initialized based on the 
		/// given siteUrl.
		/// </summary>
		/// <param name="coreRepository">The repository service where the CacheManager can retrieve objects.</param>
		/// <param name="siteUrl">The base url for the site.</param>
		public CacheManager(CoreRepository coreRepository, string siteUrl)
		{
			this._coreRepository = coreRepository;
			this._hasChanges = false;
			if (HttpContext.Current.Cache[siteUrl] == null)
			{
				this._nodeCache = new NodeCache();
				this._nodeCache.Site = coreRepository.GetSiteBySiteUrl(siteUrl);
				if (this._nodeCache.Site != null)
				{
					InitNodeCache();
				}
				else
				{
					throw new Cuyahoga.Core.SiteNullException("No site found at following base url: " + siteUrl);
				}
			}
			else
			{
				this._nodeCache = (NodeCache)HttpContext.Current.Cache[siteUrl];
			}
		}

		/// <summary>
		/// Get a single Node. If it's not cached, it's loaded from the database and put in the cache.
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		public Node GetNodeById(int nodeId)
		{
			if (this._nodeCache.NodeIndex[nodeId] == null)
			{
				LoadNodeIntoCacheFromNodeId(nodeId);
			}
			else
			{
				// We need to attach the node to the current session to enable lazy-load.
				Node tmpNode = (Node)this._nodeCache.NodeIndex[nodeId];
				AttachNodeToCurrentSession(tmpNode);
			}
			return (Node)this._nodeCache.NodeIndex[nodeId];
		}

		/// <summary>
		/// Load a cached Section. If it's not found, it will be loaded from the database with its
		/// associated Node and then cached.
		/// </summary>
		/// <param name="sectionId"></param>
		/// <returns></returns>
		public Section GetSectionById(int sectionId)
		{
			if (this._nodeCache.SectionIndex[sectionId] == null)
			{
				// Always cache a Section related to its Node so there won't be
				// any orphaned Sections in the cache.
				Section section = this._coreRepository.GetObjectById(typeof(Section), sectionId) as Section;
				if (section != null)
				{
					// Putting the node into the cache will also make an entry in the SectionIndex 
					// table of the NodeCache.
					LoadNodeIntoCache(section.Node);
				}
				else
				{
					throw new Cuyahoga.Core.SectionNullException("The section was not found: " + sectionId.ToString());
				}
			}
			else
			{
				Section section = (Section)this._nodeCache.SectionIndex[sectionId];
				// Attach the related Node to the current session to enable lazy-load.
				// NOTE: when attaching a Node the Sections are also attached.
				AttachNodeToCurrentSession(section.Node);
			}
			return this._nodeCache.SectionIndex[sectionId] as Section;
		}

		public Node GetNodeByShortDescription(string shortDescription)
		{
			if (this._nodeCache.NodeShortDescriptionIndex[shortDescription] == null)
			{
				LoadNodeIntoCacheFromShortDescription(shortDescription);
			}
			else
			{
				// We need to attach the node to the current session to enable lazy-load.
				AttachNodeToCurrentSession((Node)this._nodeCache.NodeShortDescriptionIndex[shortDescription]);
			}
			return (Node)this._nodeCache.NodeShortDescriptionIndex[shortDescription];
		}

		/// <summary>
		/// Get the root node for the current site. This is the root node that has the same culture as the site.
		/// If not found, this method returns the first root node found or null if none are available.
		/// </summary>
		/// <returns></returns>
		public Node GetRootNode()
		{
			// Get the root node that has the same culture as the default culture of the site.
			if (this._nodeCache.RootNodes.Count > 0)
			{
				Node node = (Node)this._nodeCache.RootNodes[this._nodeCache.Site.DefaultCulture];
				if (node == null)
				{
					// No node found for the default culture of the site, return the first node in the list.
					node = (Node)this._nodeCache.RootNodes.GetByIndex(0);
				}
				this._coreRepository.AttachNodeToCurrentSession(node);
				return node;
			}
			else
			{
				throw new Cuyahoga.Core.NodeNullException("No root node was found for the current site: " + this._nodeCache.Site.SiteUrl);
			}
		}

		/// <summary>
		/// Store the content of the NodeCache object in the ASP.NET cache.
		/// </summary>
		public void SaveCache()
		{
			double nodeCacheDuration = Double.Parse(Config.GetConfiguration()["NodeCacheDuration"]);
			HttpContext.Current.Cache.Insert(this._nodeCache.Site.SiteUrl, this._nodeCache, null, DateTime.Now.AddSeconds(nodeCacheDuration), TimeSpan.Zero);
		}

		/// <summary>
		/// Initialization of the NodeCache. Load the root nodes. 
		/// </summary>
		private void InitNodeCache()
		{
			IList nodes = this._coreRepository.GetRootNodes(this._nodeCache.Site);
			foreach (Node node in nodes)
			{
				RegisterNode(node, true);
			}
		}

		/// <summary>
		/// Adds a node to the NodeCache.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="isRootNode"></param>
		private void RegisterNode(Node node, bool isRootNode)
		{
			// Register events
			node.ChildrenLoaded += new Node.LoadChildrenHandler(Node_ChildrenLoaded);
			node.NodeUpdated += new Node.UpdateHandler(Node_NodeUpdated);
			// Add node to NodeCache. It is not yet put in the ASP.NET cache though.
			if (isRootNode)
			{
				// Store the culture of the root node as key.
				this._nodeCache.RootNodes.Add(node.Culture, node);
			}
			// Add an index to the NodeCache for easy retrieval.
			this._nodeCache.NodeIndex[node.Id] = node;
			// Add another index to the NodeCache to enable retrieval by ShortDescription
			this._nodeCache.NodeShortDescriptionIndex[node.ShortDescription] = node;
			// Register Sections
			foreach (Section section in node.Sections)
			{
				this._nodeCache.SectionIndex[section.Id] = section;
			}
			// Set hasChanges to true, so the changes will be in the ASP.NET cache later.
			this._hasChanges = true;
		}

		/// <summary>
		/// Attach the node to the current NHibernate Session.
		/// </summary>
		/// <param name="node"></param>
		private void AttachNodeToCurrentSession(Node node)
		{
			// HACK: also register the parent nodes since they are not lazy loaded (yet)
			// TODO: some AOP-ish solution would provide a much more elegant solution.
			this._coreRepository.AttachNodeToCurrentSession(node);
			while (node.ParentNode != null)
			{
				node = node.ParentNode;
				this._coreRepository.AttachNodeToCurrentSession(node);
			}
		}

		/// <summary>
		/// See LoadNodeIntoCache().
		/// </summary>
		/// <param name="nodeId"></param>
		private void LoadNodeIntoCacheFromNodeId(int nodeId)
		{
			Node node = (Node)this._coreRepository.GetObjectById(typeof(Node), nodeId);
			LoadNodeIntoCache(node);			
		}

		/// <summary>
		/// See LoadNodeIntoCache().
		/// </summary>
		/// <param name="shortDescription"></param>
		private void LoadNodeIntoCacheFromShortDescription(string shortDescription)
		{
			Node node = this._coreRepository.GetNodeByShortDescription(shortDescription);
			LoadNodeIntoCache(node);
		}
		
		/// <summary>
		/// Cache a particular node.
		/// </summary>
		/// <param name="node"></param>
		private void LoadNodeIntoCache(Node node)
		{
			if (node != null)
			{
				RegisterNode(node, false);
			}
			else
			{
				throw new Cuyahoga.Core.NodeNullException("Node was not found");
			}
		}

		/// <summary>
		/// The childnodes seem to have been loaded from the database. Let's store them in the NodeCache.
		/// </summary>
		/// <param name="sender"></param>
		private void Node_ChildrenLoaded(object sender)
		{
//			Node node = sender as Node;
//			if (node != null)
//			{
//				foreach (Node childNode in node.ChildNodes)
//				{
//					RegisterNode(childNode, false);
//				}
//			}
		}

		/// <summary>
		/// Something happened to a Node that invalidates the NodeCache. We have to make sure that the
		/// NodeCache is synchronized again.
		/// </summary>
		/// <param name="sender"></param>
		private void Node_NodeUpdated(object sender)
		{
			// TODO
		}
	}
}
