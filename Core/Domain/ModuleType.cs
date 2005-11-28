using System;
using System.Collections;

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
		private IList _moduleSettings;

		#region properties
		
		/// <summary>
		/// Property ModuleId (int)
		/// </summary>
		public int ModuleTypeId
		{
			get { return this._moduleTypeId; }
			set { this._moduleTypeId = value; }
		}

		/// <summary>
		/// Property AssemblyName (string)
		/// </summary>
		public string AssemblyName
		{
			get { return this._assemblyName; }
			set { this._assemblyName = value; }
		}

		/// <summary>
		/// Property ClassName (string)
		/// </summary>
		public string ClassName
		{
			get { return this._className; }
			set { this._className = value; }
		}

		/// <summary>
		/// Property Name (string)
		/// </summary>
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property Path (string)
		/// </summary>
		public string Path
		{
			get { return this._path; }
			set { this._path = value; }
		}

		/// <summary>
		/// Property EditPath (string)
		/// </summary>
		public string EditPath
		{
			get { return this._editPath; }
			set { this._editPath = value; }
		}

		/// <summary>
		/// Property ModuleSettings (IList)
		/// </summary>
		public IList ModuleSettings
		{
			get { return this._moduleSettings; }
			set { this._moduleSettings = value; }
		}

		#endregion

		public ModuleType()
		{
			this._moduleTypeId = -1;
			this._moduleSettings = new ArrayList();
		}
	}
}
