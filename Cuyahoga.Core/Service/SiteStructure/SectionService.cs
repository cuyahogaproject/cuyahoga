using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Service.Content;
using NHibernate.Criterion;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality for section management.
	/// </summary>
	[Transactional]
	public class SectionService : ISectionService
	{
		private readonly ISiteStructureDao _siteStructureDao;
		private readonly ICommonDao _commonDao;
		private readonly IContentItemService<ContentItem> _contentItemService;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="siteStructureDao"></param>
		/// <param name="commonDao"></param>
		/// <param name="contentItemService"></param>
		public SectionService(ISiteStructureDao siteStructureDao, ICommonDao commonDao, IContentItemService<ContentItem> contentItemService)
		{
			this._siteStructureDao = siteStructureDao;
			this._commonDao = commonDao;
			this._contentItemService = contentItemService;
		}

		#region ISectionService Members

		public Section GetSectionById(int sectionId)
		{
			return this._commonDao.GetObjectById<Section>(sectionId);
		}

		public IList GetSortedSectionsByNode(Node node)
		{
			return this._siteStructureDao.GetSortedSectionsByNode(node);
		}

		public IList GetUnconnectedSections()
		{
			return this._siteStructureDao.GetUnconnectedSections();
		}

		public IList GetTemplatesBySection(Section section)
		{
			return this._siteStructureDao.GetTemplatesBySection(section);
		}

		public IList<Section> GetSectionsByModuleType(ModuleType moduleType)
		{
			return this._siteStructureDao.GetSectionsByModuleType(moduleType);
		}

		public IList<ModuleType> GetSortedActiveModuleTypes()
		{
			DetachedCriteria crit = DetachedCriteria.For<ModuleType>()
				.Add(Expression.Eq("AutoActivate", true))
				.AddOrder(Order.Asc("Name"));
			return this._commonDao.GetAllByCriteria<ModuleType>(crit);
		}

		public ModuleType GetModuleTypeById(int id)
		{
			return (ModuleType) this._commonDao.GetObjectById(typeof(ModuleType), id);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void ArrangeSections(string placeholder, int[] orderedSectionIds)
		{
			for (int i = 0; i < orderedSectionIds.Length; i++)
			{
				Section section = GetSectionById(orderedSectionIds[i]);
				section.PlaceholderId = placeholder;
				section.Position = i;
			}
			// Invalidate cache
			this._commonDao.RemoveCollectionFromCache("Cuyahoga.Core.Domain.Node.Sections");
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void SetSectionPermissions(Section section, int[] viewRoleIds, int[] editRoleIds)
		{
			section.SectionPermissions.Clear();
			IList<Role> viewRoles = this._commonDao.GetByIds<Role>(viewRoleIds);
			foreach (Role role in viewRoles)
			{
				section.SectionPermissions.Add(new SectionPermission() { Section = section, Role = role, ViewAllowed = true });
			}
			IList<Role> editRoles = this._commonDao.GetByIds<Role>(editRoleIds);
			foreach (Role role in editRoles)
			{
				if (viewRoles.Contains(role))
				{
					section.SectionPermissions.Single(sp => sp.Role == role).EditAllowed = true;
				}
				else
				{
					section.SectionPermissions.Add(new SectionPermission() { Section = section, Role = role, EditAllowed = true });
				}
			}
			this._commonDao.UpdateObject(section);

			// Also update related content items that inherit their security settings from the section.
			// This is for example required for full text-indexing where the permissions are also stored in the index.
			IEnumerable<ContentItem> contentItemsThatInheritPermissions = this._contentItemService.FindContentItemsBySection(section).Where(ci => ci.SupportsItemLevelPermissions == false);
			foreach (ContentItem contentItem in contentItemsThatInheritPermissions)
			{
				this._contentItemService.Save(contentItem);
			}
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void SaveSection(Section section)
		{
			this._commonDao.SaveOrUpdateObject(section);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void UpdateSection(Section section)
		{
			this._commonDao.SaveOrUpdateObject(section);
		}

		[Transaction(TransactionMode.Requires)]
		public void DeleteSection(Section section, ModuleBase module)
		{
			// Delete module content if the connected module allows this
			module.DeleteModuleContent();

			// Remove connections
			this._commonDao.DeleteObject(section);
			// Invalidate cache
			this._commonDao.RemoveCollectionFromCache("Cuyahoga.Core.Domain.Node.Sections");
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void DeleteSection(Section section, int nodeId, ModuleBase module)
		{
			Node node = (Node) this._commonDao.GetObjectById(typeof(Node), nodeId);
			node.RemoveSection(section);
			DeleteSection(section, module);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void DetachSection(Section section, int nodeId)
		{
			Node node = (Node)this._commonDao.GetObjectById(typeof(Node), nodeId);
			node.RemoveSection(section);
		}

		#endregion
	}
}
