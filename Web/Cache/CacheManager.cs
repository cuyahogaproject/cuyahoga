using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Cache
{
	/// <summary>
	/// Summary description for CacheManager.
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
		/// Default constructor.
		/// </summary>
		public CacheManager(CoreRepository coreRepository)
		{
			this._coreRepository = coreRepository;
			this._hasChanges = false;
			if (HttpContext.Current.Cache["NodeCache"] == null)
			{
				this._nodeCache = new NodeCache();
				InitNodeCache();
			}
			else
			{
				this._nodeCache = (NodeCache)HttpContext.Current.Cache["NodeCache"];				
			}
		}

		/// <summary>
		/// Get a single Node. If it's not cached, it's loaded from the database and put in the cache.
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		public Node GetNodeById(int nodeId)
		{
			// MBO, 20041003: Disabled caching because of NHibernate Session sync issues and lazy-loading 
//			return (Node)this._coreRepository.GetObjectById(typeof(Node), nodeId);

			if (this._nodeCache.NodeIndex[nodeId] == null)
			{
				LoadNodeIntoCacheFromNodeId(nodeId);
			}
			else
			{
				// We need to attach the node to the current session to enable lazy-load.
				this._coreRepository.AttachNodeToCurrentSession((Node)this._nodeCache.NodeIndex[nodeId]);
			}
			return (Node)this._nodeCache.NodeIndex[nodeId];
		}

		public Node GetNodeByShortDescription(string shortDescription)
		{
//			// MBO, 20041003: Disabled caching because of NHibernate Session sync issues and lazy-loading 
//			return this._coreRepository.GetNodeByShortDescription(shortDescription);

			if (this._nodeCache.NodeShortDescriptionIndex[shortDescription] == null)
			{
				LoadNodeIntoCacheFromShortDescription(shortDescription);
			}
			else
			{
				// We need to attach the node to the current session to enable lazy-load.
				this._coreRepository.AttachNodeToCurrentSession((Node)this._nodeCache.NodeShortDescriptionIndex[shortDescription]);
			}
			return (Node)this._nodeCache.NodeShortDescriptionIndex[shortDescription];
		}

		public Node GetRootNode()
		{
			// TODO: handle multiple root nodes (language?). For the time being, we're getting the first
			// node in the List.

			// MBO, 20041003: Disabled caching because of NHibernate Session sync issues and lazy-loading 
//			IList rootNodes = this._coreRepository.GetRootNodes();
//			return (Node)rootNodes[0];
			Node node = (Node)this._nodeCache.RootNodes.GetByIndex(0);
			this._coreRepository.AttachNodeToCurrentSession(node);
			return node;
		}

		/// <summary>
		/// Store the content of the NodeCache object in the ASP.NET cache.
		/// </summary>
		public void SaveCache()
		{
			double nodeCacheDuration = Double.Parse(Config.GetConfiguration()["NodeCacheDuration"]);
			HttpContext.Current.Cache.Insert("NodeCache", this._nodeCache, null, DateTime.Now.AddSeconds(nodeCacheDuration), TimeSpan.Zero);
		}

		/// <summary>
		/// Initialization of the NodeCache. Load the root nodes. 
		/// TODO: handle the language thing?
		/// </summary>
		private void InitNodeCache()
		{
			IList nodes = this._coreRepository.GetRootNodes();
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
				// Temporarely store the ID as key. This should be replaced with the language identifier?
				this._nodeCache.RootNodes.Add(node.Id, node);
			}
			// Add an index to the NodeCache for easy retrieval.
			this._nodeCache.NodeIndex[node.Id] = node;
			// Add another index to the NodeCache to enable retrieval by ShortDescription
			this._nodeCache.NodeShortDescriptionIndex[node.ShortDescription] = node;
			// Set hasChanges to true, so the changes will be in the ASP.NET cache later.
			this._hasChanges = true;
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
			if (node.Id == -1)
			{
				throw new ArgumentException("No node found with the given Id", "nodeId");
			}
			else
			{
				RegisterNode(node, false);
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
