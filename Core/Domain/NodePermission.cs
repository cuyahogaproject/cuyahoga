using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Association class between Node and Role.
	/// </summary>
	public class NodePermission : Permission
	{
		private Node _node;

		/// <summary>
		/// Property Node (Node)
		/// </summary>
		public Node Node
		{
			get { return this._node; }
			set { this._node = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NodePermission() : base()
		{
		}
	}
}
