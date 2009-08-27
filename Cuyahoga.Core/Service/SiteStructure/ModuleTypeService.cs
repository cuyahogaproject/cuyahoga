using System;
using System.Collections.Generic;
using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.SiteStructure
{
    [Transactional]
    public class ModuleTypeService : IModuleTypeService
    {
        private readonly ISiteStructureDao _siteStructureDao;
        private readonly ICommonDao _commonDao;

        public ModuleTypeService(ISiteStructureDao siteStructureDao, ICommonDao commonDao)
        {
            this._siteStructureDao = siteStructureDao;
            this._commonDao = commonDao;
        }

        #region IModuleService Members

        public IList<ModuleType> GetAllModuleTypesInUse()
        {
            return this._siteStructureDao.GetAllModuleTypesInUse();
        }

        public IList<ModuleType> GetAllModuleTypes()
        {
        	return this._commonDao.GetAll<ModuleType>();
        }

        public ModuleType GetModuleById(int moduleTypeId)
        {
            return this._commonDao.GetObjectById(typeof(ModuleType), moduleTypeId) as ModuleType;
        }

    	public ModuleType GetModuleByName(string moduleName)
    	{
    		return (ModuleType) this._commonDao.GetObjectByDescription(typeof (ModuleType), "Name", moduleName);
    	}

    	[Transaction(TransactionMode.RequiresNew)]
        public void SaveModuleType(ModuleType moduleType)
        {
            this._commonDao.SaveObject(moduleType);
        }

        [Transaction(TransactionMode.RequiresNew)]
        public void SaveOrUpdateModuleType(ModuleType moduleType)
        {
            this._commonDao.SaveOrUpdateObject(moduleType);
        }

        [Transaction(TransactionMode.RequiresNew)]
        public void DeleteModuleType(ModuleType moduleType)
        {
            this._commonDao.DeleteObject(moduleType);
        }

        #endregion
    }
}
