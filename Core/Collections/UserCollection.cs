using System;
using System.Collections;

namespace Cuyahoga.Core.Collections
{
	/// <summary>
	/// Summary description for UserCollection.
	/// </summary>
	public class UserCollection : CollectionBase
	{
		public UserCollection()
		{
		}

		public User this[int index]
		{
			get { return (User)this[index]; }
		}

		public void Add (User user)
		{
			this.List.Add(user);
		}

		public void Remove (User user)
		{
			this.List.Remove(user);
		}
	}
}
