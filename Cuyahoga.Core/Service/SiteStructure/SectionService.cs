using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using NHibernate.Criterion;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Provides functionality for section management.
	/// </summary>
	[Transactional]
	public class SectionService : ISectionService
	{
		private ISiteStructureDao _siteStructureDao;
		private ICommonDao _commonDao;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="siteStructureDao"></param>
		/// <param name="commonDao"></param>
		public SectionService(ISiteStructureDao siteStructureDao, ICommonDao commonDao)
		{
			this._siteStructureDao = siteStructureDao;
			this._commonDao = commonDao;
		}

		#region ISectionService Members

		public Section GetSectionById(int sectionId)
		{
			return (Section)this._commonDao.GetObjectById(typeof(Section), sectionId);
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

		public IList GetSectionsByModuleTypes(IList moduleTypes)
		{
			return this._siteStructureDao.GetSectionsByModuleTypes(moduleTypes);
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
