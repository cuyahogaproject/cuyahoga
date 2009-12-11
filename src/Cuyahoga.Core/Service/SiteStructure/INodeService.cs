using System;
using System.Collections;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality to manage nodes (pages).
	/// </summary>
	public interface INodeService
	{
		/// <summary>
		/// Get a node by id.
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		Node GetNodeById(int nodeId);

		/// <summary>
		/// Get the root nodes by a given site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList<Node> GetRootNodes(Site site);

		/// <summary>
		/// Get a single root node by culture and site.
		/// </summary>
		/// <param name="culture"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		Node GetRootNodeByCultureAndSite(string culture, Site site);

		/// <summary>
		/// Get a single node by short description and site.
		/// </summary>
		/// <param name="shortDescription"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		Node GetNodeByShortDescriptionAndSite(string shortDescription, Site site);

		/// <summary>
		/// Get all nodes where a given template is attached to.
		/// </summary>
		/// <param name="template"></param>
		/// <returns></returns>
		IList GetNodesByTemplate(Template template);

		/// <summary>
		/// Save a node to the database.
		/// </summary>
		/// <param name="node"></param>
		void SaveNode(Node node);

		/// <summary>
		/// Update an exisiting node.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="propagatePermissionsToChildNodes"></param>
		/// <param name="propagatePermissionsToSections"></param>
		void UpdateNode(Node node, bool propagatePermissionsToChildNodes, bool propagatePermissionsToSections);

		/// <summary>
		/// Delete an exising node.
		/// </summary>
		/// <param name="node"></param>
		void DeleteNode(Node node);

		/// <summary>
		/// Get all menu items by a given root node.
		/// </summary>
		/// <param name="rootNode"></param>
		/// <returns></returns>
		IList GetMenusByRootNode(Node rootNode);

		/// <summary>
		/// Sort child nodes of a node with the given id.
		/// </summary>
		/// <param name="parentNodeId">The Id of the node to sort the children of.</param>
		/// <param name="orderedChildNodeIds">Ordered list of child node id's.</param>
		void SortNodes(int parentNodeId, int[] orderedChildNodeIds);

		/// <summary>
		/// Move a node to a different parent.
		/// </summary>
		/// <param name="nodeIdToMove">The Id of the node to move.</param>
		/// <param name="nodeIdToMoveTo">The Id of the new parent node.</param>
		void MoveNode(int nodeIdToMove, int nodeIdToMoveTo);

		/// <summary>
		/// Copy a node.
		/// </summary>
		/// <param name="nodeIdToCopy"></param>
		/// <param name="newParentNodeId"></param>
		/// <returns>The newly created node.</returns>
		Node CopyNode(int nodeIdToCopy, int newParentNodeId);

		/// <summary>
		/// Create a new root node for the given site and culture.
		/// </summary>
		/// <param name="site"></param>
		/// <param name="newNode"></param>
		/// <returns></returns>
		Node CreateRootNode(Site site, Node newNode);

		/// <summary>
		/// Create a new node under the given parent node.
		/// </summary>
		/// <param name="parentNode"></param>
		/// <param name="newNode"></param>
		/// <returns></returns>
		Node CreateNode(Node parentNode, Node newNode);

		/// <summary>
		/// Set node permissions.
		/// </summary>
		/// <param name="node">The node to set the permissions for.</param>
		/// <param name="viewRoleIds">Array of role id's that are allowed to view the node.</param>
		/// <param name="editRoleIds">Array of role id's that are allowed to edit the node.</param>
		/// <param name="propagateToChildPages">When true, all underlying nodes should get the same permissions.</param>
		/// <param name="propagateToChildSections">When true, all sections should get the same permissions.</param>
		void SetNodePermissions(Node node, int[] viewRoleIds, int[] editRoleIds, bool propagateToChildPages, bool propagateToChildSections);
	}
}
