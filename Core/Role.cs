using System;
using System.Collections;

namespace Cuyahoga.Core
{
	/// <summary>
	/// Summary description for Role.
	/// </summary>
	public class Role
	{
		private int _id;
		private string _name;
		private int _permissionLevel;
		private AccessLevel[] _permissions;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
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
		/// Property PermissionLevel (int). When set, the integer value is translated to a list of 
		/// AccessLevel enums (Permissions).
		/// </summary>
		public int PermissionLevel
		{
			get { return this._permissionLevel; }
			set 
			{ 
				this._permissionLevel = value; 
				TranslatePermissionLevelToAccessLevels();
			}
		}

		/// <summary>
		/// Gets a list of translated AccessLevel enums of the Role.
		/// </summary>
		public AccessLevel[] Permissions
		{
			get { return this._permissions; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Role()
		{
			this._id = -1;
			this._name = null;
			this._permissionLevel = -1;
		}

		/// <summary>
		/// Check if the role has the requested access rights.
		/// </summary>
		/// <param name="accessLevel"></param>
		/// <returns></returns>
		public bool HasPermission(AccessLevel permission)
		{
			return Array.IndexOf(this.Permissions, permission) > -1;
		}

		private void TranslatePermissionLevelToAccessLevels()
		{
			ArrayList permissions = new ArrayList();
			int tempPermissionLevel = this._permissionLevel;
			AccessLevel[] accessLevels = (AccessLevel[])Enum.GetValues(typeof(AccessLevel));
			Array.Reverse(accessLevels);
			foreach (AccessLevel accesLevel in accessLevels)
			{
				if (tempPermissionLevel >= (int)accesLevel)
				{
					permissions.Add(accesLevel);
					tempPermissionLevel -= (int)accesLevel;
				}
			}
			this._permissions = (AccessLevel[])permissions.ToArray(typeof(AccessLevel));
		}
	}
}
