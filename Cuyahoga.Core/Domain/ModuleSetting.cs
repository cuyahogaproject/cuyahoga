using System;

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
					Type realType = Type.GetType(fullName);
					if (realType == null)
					{
						throw new NullReferenceException(String.Format("The custom type {0} was not found in assembly {1}."
							, this._settingDataType, this._moduleType.AssemblyName));
					}
					else
					{
						return realType;
					}
				}
				else
				{
					Type realType = Type.GetType(this._settingDataType, true, true);
					if (realType == null)
					{
						throw new NullReferenceException(String.Format("The CLR type {0} is invalid."
							, this._settingDataType));
					}
					else
					{
						return realType;
					}
				}
			}
			else
			{
				throw new NullReferenceException("Unable to get the type of the module setting");
			}
		}
	}
}
