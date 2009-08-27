using System.Collections.Generic;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// The ModuleType class describes a module.
	/// </summary>
	public class ModuleType
	{
		private int _moduleTypeId;
		private string _name;
		private string _assemblyName;
		private string _className;
		private string _path;
		private string _editPath;
		private bool _autoActivate;
		private IList<ModuleSetting> _moduleSettings;
		private IList<ModuleService> _moduleServices;

		#region properties
		
		/// <summary>
		/// Property ModuleId (int)
		/// </summary>
		public virtual int ModuleTypeId
		{
			get { return this._moduleTypeId; }
			set { this._moduleTypeId = value; }
		}

		/// <summary>
		/// Property AssemblyName (string)
		/// </summary>
		public virtual string AssemblyName
		{
			get { return this._assemblyName; }
			set { this._assemblyName = value; }
		}

		/// <summary>
		/// Property ClassName (string)
		/// </summary>
		public virtual string ClassName
		{
			get { return this._className; }
			set { this._className = value; }
		}

		/// <summary>
		/// Property Name (string)
		/// </summary>
		public virtual string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property Path (string)
		/// </summary>
		public virtual string Path
		{
			get { return this._path; }
			set { this._path = value; }
		}

		/// <summary>
		/// Property EditPath (string)
		/// </summary>
		public virtual string EditPath
		{
			get { return this._editPath; }
			set { this._editPath = value; }
		}

		/// <summary>
		/// Indicates if the module is loaded at startup.
		/// </summary>
		public virtual bool AutoActivate
		{
			get { return _autoActivate; }
			set { _autoActivate = value; }
		}

		/// <summary>
		/// Property ModuleSettings (IList)
		/// </summary>
		public virtual IList<ModuleSetting> ModuleSettings
		{
			get { return this._moduleSettings; }
			set { this._moduleSettings = value; }
		}

		/// <summary>
		/// List of module-specific services.
		/// </summary>
		public virtual IList<ModuleService> ModuleServices
		{
			get { return this._moduleServices; }
			set { this._moduleServices = value; }
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public ModuleType()
		{
			this._moduleTypeId = -1;
			this._moduleSettings = new List<ModuleSetting>();
			this._moduleServices = new List<ModuleService>();
		}
	}
}
