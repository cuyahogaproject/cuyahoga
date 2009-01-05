using System;
using System.Collections;

namespace Cuyahoga.Core.Communication
{
	/// <summary>
	/// Collection class for Actions.
	/// </summary>
	public class ActionCollection : CollectionBase
	{
		/// <summary>
		/// Indexer property.
		/// </summary>
		public Action this[int index]
		{
			get { return (Action)this.List[index]; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ActionCollection()
		{
		}

		/// <summary>
		/// Add an Action to the list.
		/// </summary>
		/// <param name="action"></param>
		public void Add(Action action)
		{
			this.List.Add(action);
		}

		/// <summary>
		/// Remove an Action from the list.
		/// </summary>
		/// <param name="action"></param>
		public void Remove(Action action)
		{
			this.List.Remove(action);
		}

		/// <summary>
		/// Does the list contain a given Action?
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public bool Contains(Action action)
		{
			return this.List.Contains(action);
		}

		/// <summary>
		/// Find a specific action in the list.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Action FindByName(string name)
		{
			foreach (Action action in this.List)
			{
				if (action.Name == name)
				{
					return action;
				}
			}
			return null;
		}
	}
}
