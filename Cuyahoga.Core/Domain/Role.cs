using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Castle.Components.Validator;

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
		private bool _isGlobal;
		private AccessLevel[] _permissions;
		private IList<Right> _rights;
		private IList<Site> _sites;
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
		[ValidateNonEmpty("RoleNameValidatorNonEmpty")]
		[ValidateLength(1, 50, "RoleNameValidatorLength")]
		public virtual string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property PermissionLevel (int). When set, the integer value is translated to a list of 
		/// AccessLevel enums (Permissions).
		/// </summary>
		[Obsolete("PermissionLevel is deprecated and replaced by the Rights collection.")]
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
		/// Indicates if the role is global for all sites within the Cuyahoga installation.
		/// </summary>
		public virtual bool IsGlobal
		{
			get { return _isGlobal; }
			set { _isGlobal = value; }
		}

		/// <summary>
		/// Gets a list of translated AccessLevel enums of the Role.
		/// </summary>
		[Obsolete("AccessLevel is deprecated and replaced by the Rights collection.")]
		public virtual AccessLevel[] Permissions
		{
			get { return this._permissions; }
		}

		/// <summary>
		/// Gets or sets a list of access rights.
		/// </summary>
		public virtual IList<Right> Rights
		{
			get { return _rights; }
			set { _rights = value; }
		}

		/// <summary>
		/// Gets or sets a list of related sites.
		/// </summary>
		public virtual IList<Site> Sites
		{
			get { return _sites; }
			set { _sites = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual string RightsString
		{
			get { return GetRightsAsString(); }
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
			this._rights = new List<Right>();
			this._sites = new List<Site>();
		}

		/// <summary>
		/// Check if the role has the requested access rights.
		/// </summary>
		/// <param name="permission"></param>
		/// <returns></returns>
		[Obsolete("Replaced by HasRight().")]
		public virtual bool HasPermission(AccessLevel permission)
		{
			return HasRight(permission.ToString());
		}

		/// <summary>
		/// Check if the role has the requested access right.
		/// </summary>
		/// <param name="rightName"></param>
		/// <returns></returns>
		public virtual bool HasRight(string rightName)
		{
			foreach (Right right in _rights)
			{
				if (right.Name.Equals(rightName, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
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

		private string GetRightsAsString()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < this._rights.Count; i++)
			{
				Right right = this._rights[i];
				sb.Append(right.Name);
				if (i < this._rights.Count - 1)
				{
					sb.Append(", ");
				}
			}

			return sb.ToString();
		}
	}
}
