using System;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class ModuleViewData
	{
		private readonly ModuleType _moduleType;

		public int ModuleTypeId
		{
			get { return this._moduleType.ModuleTypeId; }
		}

		public string ModuleName
		{
			get { return this._moduleType.Name; }
		}

		public string AssemblyName
		{
			get { return this._moduleType.AssemblyName; }
		}

		public bool AutoActivate
		{
			get { return this._moduleType.AutoActivate; }
		}

		public string ModuleVersion
		{
			get; set;
		}

		public bool CanInstall
		{
			get; set;
		}

		public bool CanUpgrade
		{
			get; set;
		}

		public bool CanUninstall
		{
			get; set;
		}

		public bool CannotDoAnything
		{
			get { return AssemblyName == null && !CanInstall && !CanUpgrade && !CanUninstall; }
		}

		public string ActivationStatus
		{
			get; set;
		}

		public string InstallationStatus
		{
			get; set;
		}

		public ModuleViewData(ModuleType moduleType)
		{
			_moduleType = moduleType;
		}
	}
}
