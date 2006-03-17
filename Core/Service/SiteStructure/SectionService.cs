using System;
using System.Collections;

using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

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

		[Transaction(TransactionMode.RequiresNew)]
		public void DeleteSection(Section section)
		{
			this._commonDao.SaveOrUpdateObject(section);
		}

		#endregion
	}
}
