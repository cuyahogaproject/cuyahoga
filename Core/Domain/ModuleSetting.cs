using System;
using System.Reflection;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for ModuleSetting.
	/// </summary>
	public class ModuleSetting
	{
		private string _name;
		private string _friendlyName;
		private string _settingDataType;
		private bool _isCustomType;
		private bool _isRequired;
		private ModuleType _moduleType;

		/// <summary>
		/// Property Name (string)
		/// </summary>
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property FriendlyName (string)
		/// </summary>
		public string FriendlyName
		{
			get { return this._friendlyName; }
			set { this._friendlyName = value; }
		}

		/// <summary>
		/// Property _settingDataType (string)
		/// </summary>
		public string SettingDataType
		{
			get { return this._settingDataType; }
			set { this._settingDataType = value; }
		}

		/// <summary>
		/// Property IsCustomType (bool)
		/// </summary>
		public bool IsCustomType
		{
			get { return this._isCustomType; }
			set { this._isCustomType = value; }
		}

		/// <summary>
		/// Property IsRequired (bool)
		/// </summary>
		public bool IsRequired
		{
			get { return this._isRequired; }
			set { this._isRequired = value; }
		}

		/// <summary>
		/// Property ModuleType (ModuleType)
		/// </summary>
		public ModuleType ModuleType
		{
			get { return this._moduleType; }
			set { this._moduleType = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ModuleSetting()
		{
		}

		/// <summary>
		/// Gets the CLR Type of the ModuleSetting
		/// </summary>
		/// <returns></returns>
		public Type GetRealType()
		{
			if (this._settingDataType != null)
			{
				if (this._isCustomType)
				{
					// Use the assemblyname of the ModuleType to find the custom type
					string fullName = this._settingDataType + ", " + this._moduleType.AssemblyName;
					return Type.GetType(fullName);
				}
				else
				{
					return Type.GetType(this._settingDataType, true, true);
				}
			}
			else
			{
				throw new NullReferenceException("Unable to get the type of the module setting");
			}
		}
	}
}
