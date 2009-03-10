using System;
using System.Collections;

namespace Cuyahoga.Core.Communication
{
	/// <summary>
	/// Collection class for Actions.
	/// </summary>
	public class ModuleActionCollection : CollectionBase
	{
		/// <summary>
		/// Indexer property.
		/// </summary>
		public ModuleAction this[int index]
		{
			get { return (ModuleAction)this.List[index]; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ModuleActionCollection()
		{
		}

		/// <summary>
		/// Add an ModuleAction to the list.
		/// </summary>
		/// <param name="moduleAction"></param>
		public void Add(ModuleAction moduleAction)
		{
			this.List.Add(moduleAction);
		}

		/// <summary>
		/// Remove an ModuleAction from the list.
		/// </summary>
		/// <param name="moduleAction"></param>
		public void Remove(ModuleAction moduleAction)
		{
			this.List.Remove(moduleAction);
		}

		/// <summary>
		/// Does the list contain a given ModuleAction?
		/// </summary>
		/// <param name="moduleAction"></param>
		/// <returns></returns>
		public bool Contains(ModuleAction moduleAction)
		{
			return this.List.Contains(moduleAction);
		}

		/// <summary>
		/// Find a specific action in the list.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ModuleAction FindByName(string name)
		{
			foreach (ModuleAction action in this.List)
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
