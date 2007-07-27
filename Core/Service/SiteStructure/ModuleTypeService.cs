using System;
using System.Collections;

using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.SiteStructure
{
    [Transactional]
    public class ModuleTypeService : IModuleTypeService
    {
        private ISiteStructureDao _siteStructureDao;
        private ICommonDao _commonDao;

        public ModuleTypeService(ISiteStructureDao siteStructureDao, ICommonDao commonDao)
        {
            this._siteStructureDao = siteStructureDao;
            this._commonDao = commonDao;
        }

        #region IModuleService Members

        public IList GetAllModuleTypesInUse()
        {
            return this._siteStructureDao.GetAllModuleTypesInUse();
        }

        public IList GetAllModuleTypes()
        {
            return this._commonDao.GetAll(typeof(ModuleType));
        }

        public ModuleType GetModuleById(int moduleTypeId)
        {
            return this._commonDao.GetObjectById(typeof(ModuleType), moduleTypeId) as ModuleType;
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
