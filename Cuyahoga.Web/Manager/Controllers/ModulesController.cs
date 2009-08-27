using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Manager.Model.ViewModels;

namespace Cuyahoga.Web.Manager.Controllers
{
	[CuyahogaPermission(RequiredRights = Rights.ManageServer)]
	public class ModulesController : ManagerController
	{
		private readonly IModuleTypeService _moduleTypeService;
		private readonly ModuleLoader _moduleLoader;
		private readonly ISectionService _sectionService;

		public ModulesController(IModuleTypeService moduleTypeService, ModuleLoader moduleLoader, ISectionService sectionService)
		{
			_moduleTypeService = moduleTypeService;
			_sectionService = sectionService;
			_moduleLoader = moduleLoader;
		}

		public ActionResult Index()
		{
			IEnumerable<ModuleViewData> moduleViewData = GetModuleViewData();
			return View(moduleViewData);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Activate(string moduleName, bool autoActivate)
		{
			ModuleType moduleType = this._moduleTypeService.GetModuleByName(moduleName);
			try
			{
				moduleType.AutoActivate = autoActivate;
				this._moduleTypeService.SaveOrUpdateModuleType(moduleType);
				if (autoActivate)
				{
					this._moduleLoader.ActivateModule(moduleType);
					Messages.AddFlashMessageWithParams("ModuleActivatedMessage", moduleType.Name);
				}
				else
				{
					Messages.AddFlashMessageWithParams("ModuleDeactivatedMessage", moduleType.Name);
				}
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Index");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Install(string moduleName)
		{
			try
			{
				string moduleInstallDirectory = GetPhysicalModuleInstallDirectory(moduleName);
				DatabaseInstaller dbInstaller = new DatabaseInstaller(moduleInstallDirectory, null);
				dbInstaller.Install();
				ModuleType moduleType = this._moduleTypeService.GetModuleByName(moduleName);
				moduleType.AutoActivate = true;
				this._moduleTypeService.SaveOrUpdateModuleType(moduleType);
				this._moduleLoader.ActivateModule(moduleType);
				Messages.AddFlashMessageWithParams("ModuleInstalledAndActivatedMessage", moduleName);
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Index");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Upgrade(string moduleName)
		{
			ModuleType moduleType = this._moduleTypeService.GetModuleByName(moduleName);
			try
			{
				DatabaseInstaller dbInstaller = GetDbInstallerForModuleType(moduleType);
				dbInstaller.Upgrade();
				Messages.AddFlashMessageWithParams("ModuleUpgradedMessage", moduleName);
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Index");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Uninstall(string moduleName)
		{
			ModuleType moduleType = this._moduleTypeService.GetModuleByName(moduleName);
			// First check if there are no sections attached
			IList<Section> sectionsByModuleType = this._sectionService.GetSectionsByModuleType(moduleType);
			if (sectionsByModuleType.Count > 0)
			{
				StringBuilder messageBuilder = new StringBuilder();
				messageBuilder.Append(GetText("UninstallModuleForbiddenBecauseOfRelatedSectionsMessage"));
				messageBuilder.Append(":<br />");
				foreach (var section in sectionsByModuleType)
				{
					messageBuilder.AppendFormat("{0} ({1})<br />", section.Title, section.Id);
				}
				Messages.AddFlashMessage(messageBuilder.ToString());
			}
			else
			{
				try
				{
					DatabaseInstaller dbInstaller = GetDbInstallerForModuleType(moduleType);
					dbInstaller.Uninstall();
					Messages.AddFlashMessageWithParams("ModuleUninstalledMessage", moduleName);
				}
				catch (Exception ex)
				{
					Messages.AddFlashException(ex);
				}
			}
			return RedirectToAction("Index");
		}

		private IEnumerable<ModuleViewData> GetModuleViewData()
		{
			// Retrieve the available modules that are installed.
			IList<ModuleType> availableModules = this._moduleTypeService.GetAllModuleTypes();
			// Retrieve the available modules from the filesystem.
			string moduleRootDir = Server.MapPath("~/Modules");
			DirectoryInfo[] moduleDirectories = new DirectoryInfo(moduleRootDir).GetDirectories();
			// Go through the directories and check if there are missing ones. Those have to be added
			// as new ModuleType candidates.
			foreach (DirectoryInfo di in moduleDirectories)
			{
				// Skip hidden directories and shared directory.
				bool shouldAdd = (di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && di.Name.ToLowerInvariant() != "shared";

				foreach (ModuleType moduleType in availableModules)
				{
					if (moduleType.Name == di.Name)
					{
						shouldAdd = false;
						break;
					}
				}
				if (shouldAdd)
				{
					ModuleType newModuleType = new ModuleType();
					newModuleType.Name = di.Name;
					availableModules.Add(newModuleType);
				}
			}

			IList<ModuleViewData> moduleViewDataList = new List<ModuleViewData>();
			foreach (var availableModule in availableModules)
			{
				var moduleViewData = new ModuleViewData(availableModule);

				string physicalModuleInstallDirectory = GetPhysicalModuleInstallDirectory(availableModule.Name);
				Assembly moduleAssembly = null;
				if (availableModule.AssemblyName != null)
				{
					moduleAssembly = Assembly.Load(availableModule.AssemblyName);
				}
				DatabaseInstaller dbInstaller = new DatabaseInstaller(physicalModuleInstallDirectory, moduleAssembly);
				moduleViewData.CanInstall = dbInstaller.CanInstall;
				moduleViewData.CanUpgrade = dbInstaller.CanUpgrade;
				moduleViewData.CanUninstall = dbInstaller.CanUninstall;

				moduleViewData.ActivationStatus = this._moduleLoader.IsModuleActive(availableModule)
				                                  	? GetText("ActiveStatus")
				                                  	: GetText("InactiveStatus");
				if (dbInstaller.CurrentVersionInDatabase != null)
				{
					moduleViewData.ModuleVersion = dbInstaller.CurrentVersionInDatabase.ToString(3);
					if (dbInstaller.CanUpgrade)
					{
						moduleViewData.InstallationStatus = GetText("InstalledUpgradeStatus");
					}
					else
					{
						moduleViewData.InstallationStatus = GetText("InstalledStatus");						
					}
				}
				else
				{
					moduleViewData.InstallationStatus = GetText("UninstalledStatus");						
				}
				moduleViewDataList.Add(moduleViewData);
			}
			return moduleViewDataList;
		}

		private string GetPhysicalModuleInstallDirectory(string moduleName)
		{
			return Path.Combine(Server.MapPath("~/Modules/" + moduleName), "Install");
		}

		private DatabaseInstaller GetDbInstallerForModuleType(ModuleType moduleType)
		{
			Assembly moduleAssembly = Assembly.Load(moduleType.AssemblyName);
			string moduleInstallDirectory = GetPhysicalModuleInstallDirectory(moduleType.Name);
			return new DatabaseInstaller(moduleInstallDirectory, moduleAssembly);
		}
	}
}
