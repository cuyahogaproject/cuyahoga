using System;

using Cuyahoga.Core.Collections;

namespace Cuyahoga.Core
{
	/// <summary>
	/// This interface contains a fixed set of properties for personalization.
	/// </summary>
	public interface IPersonalizable
	{
		/// <summary>
		/// Database keyvalue.
		/// </summary>
		int Id
		{ 
			get;
			set;
		}
		/// <summary>
		/// All roles that can view the object.
		/// </summary>
		RoleCollection ViewRoles
		{ 
			get;
			set;
		}
		/// <summary>
		/// All roles that can edit the object.
		/// </summary>
		RoleCollection EditRoles
		{ 
			get;
			set;
		}

		/// <summary>
		/// Indicates if the object can be viewed by anonymous visitors.
		/// </summary>
		bool AnonymousViewAllowed
		{
			get;
		}
	}
}
