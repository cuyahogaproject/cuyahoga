using System;
using System.Web;
using System.Web.Caching;

using Cuyahoga.Core;
using Cuyahoga.Core.DAL;
using Cuyahoga.Core.Collections;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Cache
{
	/// <summary>
	/// Summary description for CacheManager.
	/// </summary>
	public class CacheManager
	{
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
		public CacheManager()
		{
			this._hasChanges = false;
			if (HttpContext.Current.Cache["NodeCache"] == null)
			{
				this._nodeCache = new NodeCache();
				InitNodeCache();
			}
			else
				this._nodeCache = (NodeCache)HttpContext.Current.Cache["NodeCache"];				
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
				LoadNodeIntoCache(nodeId);
			}
			return (Node)this._nodeCache.NodeIndex[nodeId];
		}

		public Node GetRootNode()
		{
			// TODO: handle multiple root nodes (language?). For the time being, we're getting the first
			// node in the SortedList.
			return (Node)this._nodeCache.RootNodes.GetByIndex(0);
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
			ICmsDataProvider dp = CmsDataFactory.GetInstance();
			NodeCollection nodes = new NodeCollection();
			dp.GetNodesByParent(null, nodes);
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
				// Temporarely store the ID as key. This should be replaced with the language identifier.
				this._nodeCache.RootNodes[node.Id] = node;
			}
			// Add an index to the NodeCache for easy retrieval.
			this._nodeCache.NodeIndex[node.Id] = node;
			// Set hasChanges to true, so the changes will be in the ASP.NET cache later.
			this._hasChanges = true;
		}

		/// <summary>
		/// Try to find a node. If the node is found, build the Node tree up to the position
		/// where that particular node is located. A slightly redundant database call is made to
		/// determine if the node at least exists, but that prevents unnessecary node traversals
		/// when a node is requested that doesn't exist at all.
		/// </summary>
		/// <param name="nodeId"></param>
		private void LoadNodeIntoCache(int nodeId)
		{
			Node node = new Node(nodeId);
			if (node.Id == -1)
			{
				throw new ArgumentException("No node found with the given Id", "nodeId");
			}
			else
			{
				foreach (Node rootNode in this._nodeCache.RootNodes.Values)
				{
					// Recursively search the node tree. All nodes that are loaded from the 
					// database while searching will be automatically stored in the NodeCache
					// due to the ChildrenLoaded event.
					FindAndCacheNode(rootNode.ChildNodes, nodeId);
				}
			}
		}

		private void FindAndCacheNode(NodeCollection nodes, int nodeId)
		{
			foreach (Node node in nodes)
			{
				if (node.Id == nodeId)
				{
					// Yep, found. We can quit searching now. The node also is already in the NodeCache 
					// (due to the ChildrenLoad event).
					break;
				}
				FindAndCacheNode(node.ChildNodes, nodeId);
			}
		}

		/// <summary>
		/// The childnodes seem to have been loaded from the database. Let's store them in the NodeCache.
		/// </summary>
		/// <param name="sender"></param>
		private void Node_ChildrenLoaded(object sender)
		{
			Node node = sender as Node;
			if (node != null)
			{
				foreach (Node childNode in node.ChildNodes)
				{
					RegisterNode(childNode, false);
				}
			}
		}

		/// <summary>
		/// Something happened to a Node that invalidates the NodeCache. We have to make sure that the
		/// NodeCache is synchronized again.
		/// </summary>
		/// <param name="sender"></param>
		private void Node_NodeUpdated(object sender)
		{

		}
	}
}
