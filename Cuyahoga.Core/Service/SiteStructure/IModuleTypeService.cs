using System.Collections.Generic;
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
        IList<ModuleType> GetAllModuleTypesInUse();

        /// <summary>
        /// Get all modules, regardless of usage
        /// </summary>
        /// <returns></returns>
        IList<ModuleType> GetAllModuleTypes();

    	/// <summary>
        /// Get a ModuleType by its id
        /// </summary>
        /// <param name="moduleTypeId"></param>
        /// <returns></returns>
        ModuleType GetModuleById(int moduleTypeId);

    	/// <summary>
    	/// Get a ModuleType by its name.
    	/// </summary>
    	/// <param name="moduleName"></param>
    	/// <returns></returns>
    	ModuleType GetModuleByName(string moduleName);

    	/// <summary>
		/// Save a new ModuleType.
        /// </summary>
        /// <param name="moduleType"></param>
        void SaveModuleType(ModuleType moduleType);

    	/// <summary>
		/// Save or Update an exisiting ModuleType.
        /// </summary>
        /// <param name="moduleType"></param>
        void SaveOrUpdateModuleType(ModuleType moduleType);

    	/// <summary>
		/// Delete a ModuleType.
        /// </summary>
        /// <param name="moduleType"></param>
        void DeleteModuleType(ModuleType moduleType);
    }
}
