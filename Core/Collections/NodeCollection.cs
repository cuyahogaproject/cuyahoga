using System;
using System.Collections;

namespace Cuyahoga.Core.Collections
{
	/// <summary>
	/// Summary description for NodeCollection.
	/// </summary>
	public class NodeCollection : CollectionBase
	{
		public NodeCollection()
		{
		}

		public void Add(Node node)
		{
			this.List.Add(node);
		}

		public void Remove (Node node)
		{
			this.List.Remove(node);
		}

		public Node this[int index]
		{
			get { return (Node)this.List[index]; }
		}
	}
}
