using System;
using System.Text;
using System.Collections;

namespace Cuyahoga.Core.Domain
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
		private DateTime _updateTimestamp;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
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
		/// Property PermissionLevel (int). When set, the integer value is translated to a list of 
		/// AccessLevel enums (Permissions).
		/// </summary>
		public virtual int PermissionLevel
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
		public virtual AccessLevel[] Permissions
		{
			get { return this._permissions; }
		}

		public virtual string PermissionsString
		{
			get { return GetPermissionsAsString(); }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public virtual DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
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
		public virtual bool HasPermission(AccessLevel permission)
		{
			return Array.IndexOf(this.Permissions, permission) > -1;
		}

		private void TranslatePermissionLevelToAccessLevels()
		{
			ArrayList permissions = new ArrayList();
			AccessLevel[] accessLevels = (AccessLevel[])Enum.GetValues(typeof(AccessLevel));

			foreach (AccessLevel accesLevel in accessLevels)
			{
				if ((this.PermissionLevel & (int)accesLevel) == (int)accesLevel)
				{
					permissions.Add(accesLevel);
				}
			}
			this._permissions = (AccessLevel[])permissions.ToArray(typeof(AccessLevel));
		}

		private string GetPermissionsAsString()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < this._permissions.Length; i++)
			{
				AccessLevel accessLevel = this._permissions[i];
				sb.Append(accessLevel.ToString());
				if (i < this._permissions.Length - 1)
				{
					sb.Append(", ");
				}
			}

			return sb.ToString();
		}
	}
}
