using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Base class for permission related association objects.
	/// </summary>
	public abstract class Permission
	{
		private int _id;
		private bool _viewAllowed;
		private bool _editAllowed;
		private Role _role;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property ViewAllowed (bool)
		/// </summary>
		public bool ViewAllowed
		{
			get { return this._viewAllowed; }
			set { this._viewAllowed = value; }
		}

		/// <summary>
		/// Property EditAllowed (bool)
		/// </summary>
		public bool EditAllowed
		{
			get { return this._editAllowed; }
			set { this._editAllowed = value; }
		}

		/// <summary>
		/// Property Role (Role)
		/// </summary>
		public Role Role
		{
			get { return this._role; }
			set { this._role = value; }
		}

		/// <summary>
		/// Protected constructor
		/// </summary>
		protected Permission()
		{
			this._id = -1;
			this._viewAllowed = false;
			this._editAllowed = false;
		}
	}
}
