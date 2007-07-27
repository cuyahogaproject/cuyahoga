using System;
using System.Collections;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.SiteStructure
{
    public interface IModuleTypeService
    {

        /// <summary>
        /// Get all module types that are currently in use (that have related sections) 
        /// in the Cuyahoga installation.
        /// </summary>
        /// <returns></returns>
        IList GetAllModuleTypesInUse();

        /// <summary>
        /// Get all modules, regardless of usage
        /// </summary>
        /// <returns></returns>
        IList GetAllModuleTypes();

        /// <summary>
        /// Get a moduleType by its id
        /// </summary>
        /// <param name="moduleTypeId"></param>
        /// <returns></returns>
        ModuleType GetModuleById(int moduleTypeId);

        /// <summary>
        /// Save a new moduleType.
        /// </summary>
        /// <param name="moduleType"></param>
        void SaveModuleType(ModuleType moduleType);

        /// <summary>
        /// Save or Update an exisiting moduleType.
        /// </summary>
        /// <param name="moduleType"></param>
        void SaveOrUpdateModuleType(ModuleType moduleType);

        /// <summary>
        /// Delete a moduleType.
        /// </summary>
        /// <param name="moduleType"></param>
        void DeleteModuleType(ModuleType moduleType);

    }
}
