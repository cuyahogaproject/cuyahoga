using System;
using System.Collections;

using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality to manage nodes (pages).
	/// </summary>
	[Transactional]
	public class NodeService : INodeService
	{
		private ISiteStructureDao _siteStructureDao;
		private ICommonDao _commonDao;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="siteStructureDao"></param>
		/// <param name="commonDao"></param>
		public NodeService(ISiteStructureDao siteStructureDao, ICommonDao commonDao)
		{
			this._siteStructureDao = siteStructureDao;
			this._commonDao = commonDao;
		}

		#region INodeService Members

		public Node GetNodeById(int nodeId)
		{
			return (Node)this._commonDao.GetObjectById(typeof(Node), nodeId, true);
		}

		public IList GetRootNodes(Site site)
		{
			return this._siteStructureDao.GetRootNodes(site);
		}

		public Node GetRootNodeByCultureAndSite(string culture, Site site)
		{
			return this._siteStructureDao.GetRootNodeByCultureAndSite(culture, site);
		}

		public Node GetNodeByShortDescriptionAndSite(string shortDescription, Site site)
		{
			return this._siteStructureDao.GetNodeByShortDescriptionAndSite(shortDescription, site);
		}

		public IList GetNodesByTemplate(Template template)
		{
			return this._siteStructureDao.GetNodesByTemplate(template);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void SaveNode(Node node)
		{
			this._commonDao.SaveOrUpdateObject(node);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void UpdateNode(Node node, bool propagatePermissionsToChildNodes, bool propagatePermissionsToSections)
		{
			this._commonDao.SaveOrUpdateObject(node);
			if (propagatePermissionsToChildNodes)
			{
				PropagatePermissionsToChildNodes(node, propagatePermissionsToSections);
			}
			if (propagatePermissionsToSections)
			{
				PropagatePermissionsToSections(node);
			}
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void DeleteNode(Node node)
		{
			IList menus = this._siteStructureDao.GetMenusByParticipatingNode(node);
	
			foreach (CustomMenu menu in menus)
			{
				// HACK: due to a bug with proxies IList.Remove(object) always removes the first object in
				// the list. Also IList.IndexOf always returns 0. Therefore, we'll loop through the collection
				// and find the right index. Btw, when turning off proxies everything works fine.
				int positionFound = -1;
				for (int i = 0; i < menu.Nodes.Count; i++)
				{
					if (((Node)menu.Nodes[i]).Id == node.Id)
					{
						positionFound = i;
						break;
					}
				}
				if (positionFound > -1)
				{
					menu.Nodes.RemoveAt(positionFound);
				}
				this._commonDao.SaveOrUpdateObject(menu);
			}
			this._commonDao.DeleteObject(node);
		}

		public IList GetMenusByRootNode(Node rootNode)
		{
			return this._siteStructureDao.GetMenusByRootNode(rootNode);
		}

		#endregion

		private void PropagatePermissionsToChildNodes(Node parentNode, bool alsoPropagateToSections)
		{
			foreach (Node childNode in parentNode.ChildNodes)
			{
				childNode.NodePermissions.Clear();
				foreach (NodePermission pnp in parentNode.NodePermissions)
				{
					NodePermission childNodePermission = new NodePermission();
					childNodePermission.Node = childNode;
					childNodePermission.Role = pnp.Role;
					childNodePermission.ViewAllowed = pnp.ViewAllowed;
					childNodePermission.EditAllowed = pnp.EditAllowed;
					childNode.NodePermissions.Add(childNodePermission);
				}
				if (alsoPropagateToSections)
				{
					PropagatePermissionsToSections(childNode);
				}
				PropagatePermissionsToChildNodes(childNode, alsoPropagateToSections);
				this._commonDao.SaveOrUpdateObject(childNode);
			}
		}

		private void PropagatePermissionsToSections(Node node)
		{
			foreach (Section section in node.Sections)
			{
				section.SectionPermissions.Clear();
				foreach (NodePermission np in node.NodePermissions)
				{
					SectionPermission sp = new SectionPermission();
					sp.Section = section;
					sp.Role = np.Role;
					sp.ViewAllowed = np.ViewAllowed;
					sp.EditAllowed = np.EditAllowed;
					section.SectionPermissions.Add(sp);
				}
			}
			this._commonDao.SaveOrUpdateObject(node);
		}
	}
}
