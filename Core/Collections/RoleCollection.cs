using System;
using System.Collections;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Collections
{
	/// <summary>
	/// Summary description for RoleCollection.
	/// </summary>
	public class RoleCollection : CollectionBase
	{
		public RoleCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public Role this[int index]
		{
			get { return (Role)this.List[index]; }

		}

		public void Add (Role role)
		{
			this.List.Add(role);
		}

		public void Remove (Role role)
		{
			this.List.Remove(role);
		}

		/// <summary>
		/// Check if a the RoleCollection contains a specific role. Comparison is done by Id. 
		/// Little dirty though.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool Contains(Role role)
		{
			foreach (Role innerRole in this)
			{
				if (role.Id == innerRole.Id)
				{
					return true;
				}
			}
			return false;
		}

		public int IndexOf(Role role)
		{
			return this.List.IndexOf(role);
		}
	}
}
