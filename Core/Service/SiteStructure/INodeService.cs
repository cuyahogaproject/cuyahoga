using System;
using System.Collections;

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
		IList GetRootNodes(Site site);

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
		/// Save a new node.
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
	}
}
