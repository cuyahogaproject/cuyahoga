using System;
using System.Collections;
using System.Security.Principal;

using Cuyahoga.Core.Collections;
using Cuyahoga.Core.DAL;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for Node.
	/// </summary>
	[Serializable]
	public class Node : IPersonalizable
	{
		private int _id;
		private DateTime _updateTimestamp;
		private int _parentId;
		private string _title;
		private string _shortDescription;
		private int _position;
		private Node _parentNode;
		private IList _childNodes;
		private IList _sections;
		private Template _template;
		private int[] _trail;
		// flag to prevent lazy loading of the parent node when set to null
		private RoleCollection _viewRoles;
		private RoleCollection _editRoles;

		#region properties

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
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
		/// Property ParentId (int)
		/// </summary>
		public virtual int ParentId
		{
			get { return this._parentId; }
			set { this._parentId = value; }
		}

		/// <summary>
		/// Property Title (string)
		/// </summary>
		public virtual string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// Property ShortDescription (string)
		/// </summary>
		public virtual string ShortDescription
		{
			get { return this._shortDescription; }
			set 
			{ 
				if (value.Trim().Length == 0)
					this._shortDescription = null;
				else
					this._shortDescription = value; 
			}
		}

		/// <summary>
		/// Property Order (int)
		/// </summary>
		public virtual int Position
		{
			get { return this._position; }
			set { this._position = value; }
		}

		/// <summary>
		/// Property Level (int)
		/// </summary>
		public virtual int Level
		{
			get 
			{ 
				int level = 0;
				Node parentNode = this.ParentNode;
				while (parentNode != null)
				{
					parentNode = parentNode.ParentNode;
					level++;
				}
				return level;
			}
		}

		/// <summary>
		/// Property ParentNode (Node). Lazy loaded.
		/// </summary>
		public virtual Node ParentNode
		{
			get { return this._parentNode; }
			set { this._parentNode = value; }
		}

		/// <summary>
		/// Property ChildNodes (NodeCollection). Lazy loaded.
		/// </summary>
		public virtual IList ChildNodes
		{
			get 
			{ 
				return this._childNodes; 
			}
			set 
			{ 
				// TODO?
				// Notify that the ChildNodes are loaded. I really want to do this only when the 
				// ChildNodes are loaded (lazy) from the database but I don't know if this happens right now.
				// Implement IInterceptor?
				//OnChildrenLoaded();
				this._childNodes = value; 
			}
		}

		/// <summary>
		/// Property Sections (SectionCollection). Lazy loaded.
		/// </summary>
		public virtual IList Sections
		{
			get { return this._sections; }
			set { this._sections = value; }
		}

		/// <summary>
		/// Property Template (Template)
		/// </summary>
		public virtual Template Template
		{
			get { return this._template; }
			set { this._template = value; }
		}

		/// <summary>
		/// Array with all NodeId's from the current node to the root node.
		/// </summary>
		public virtual int[] Trail
		{
			get
			{
				if (this._trail == null && this.Level > -1)
				{
                    this._trail = new int[this.Level + 1];
					this._trail[this.Level] = this._id;
					Node tmpParentNode = this.ParentNode;
					while (tmpParentNode != null)
					{
						this._trail[tmpParentNode.Level] = tmpParentNode.Id;
						tmpParentNode = tmpParentNode.ParentNode;
					}                
				}
				return this._trail;
			}
		}

		/// <summary>
		/// Property ViewRoles (RoleCollection), semi-lazy loaded.
		/// </summary>
		public virtual RoleCollection ViewRoles
		{
			get 
			{ 
				if (this._viewRoles == null)
				{
					// Load the roles from the database. All roles will be loaded at once (view, edit).
					InitRoles();
					CmsDataFactory.GetInstance().GetRolesByNode(this);
				}
				return this._viewRoles; 
			}
			set { this._viewRoles = value; }
		}

		/// <summary>
		/// Property EditRoles (RoleCollection), semi-lazy loaded.
		/// </summary>
		public virtual RoleCollection EditRoles
		{
			get 
			{ 
				if (this._editRoles == null)
				{
					InitRoles();
					CmsDataFactory.GetInstance().GetRolesByNode(this);
				}
				return this._editRoles; 
			}
			set { this._editRoles = value; }
		}

		/// <summary>
		/// Can the node be viewed by anonymous users?
		/// </summary>
		public virtual bool AnonymousViewAllowed
		{
			get
			{
				foreach (Role role in this.ViewRoles)
				{
					if (Array.IndexOf(role.Permissions, AccessLevel.Anonymous) > -1)
					{
						return true;
					}
				}
				return false;
			}
		}

		#endregion

		#region events

		public delegate void LoadChildrenHandler(object sender);
		public delegate void UpdateHandler(object sender);

		/// <summary>
		/// This event is raised when ChildNodes are loaded from the database.
		/// </summary>
		public event LoadChildrenHandler ChildrenLoaded;

		/// <summary>
		/// This event is raised when the state of the node changes.
		/// </summary>
		public event UpdateHandler NodeUpdated;

		protected void OnChildrenLoaded()
		{
			if (ChildrenLoaded != null)
				ChildrenLoaded(this);
		}

		protected void OnUpdate()
		{
			if (NodeUpdated != null)
				NodeUpdated(this);
		}

		#endregion

		#region constructors and initialization

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Node()
		{
			this._id = -1;
			InitNode();			
		}

		private void InitNode()
		{
			this._parentId = -1;
			this._shortDescription = null;
			this._parentNode = null;
			this._template = null;
			this._childNodes = null;
			this._sections = null;
			this._position = -1;
			this._trail = null;
		}

		#endregion

		#region methods

		/// <summary>
		/// Move the node one position upwards and move the node above this one one position downwards.
		/// </summary>
		/// <param name="rootNodes">We need these when the node has no ParentNode.</param>
		public virtual void MoveUp(IList rootNodes)
		{
			if (this._position > 0)
			{
				// HACK: Assume that the node indexes are the same as the value of the positions.
				this._position--;
				if (this.ParentNode == null)
				{
					((Node)rootNodes[this._position]).Position++;
				}
				else
				{
					((Node)this.ParentNode.ChildNodes[this._position]).Position++;
				}
			}
		}

		/// <summary>
		/// Move the node one position downwards and move the node above this one one position upwards.
		/// </summary>
		/// <param name="rootNodes">We need these when the node has no ParentNode.</param>
		public virtual void MoveDown(IList rootNodes)
		{
			if (this._position < this.ParentNode.ChildNodes.Count - 1)
			{
				// HACK: Assume that the node indexes are the same as the value of the positions.
				this._position++;
				if (this.ParentNode == null)
				{
					((Node)rootNodes[this._position]).Position--;
				}
				else
				{
					((Node)this.ParentNode.ChildNodes[this._position]).Position--;
				}
			}
		}

		/// <summary>
		/// Move node to the same level as the parentnode at the position just beneath the parent node.
		/// </summary>
		/// <param name="rootNodes">The root nodes. We need these when a node is moved to the
		/// root level because the nodes that come after this one ahve to be moved and can't be reached
		/// anymore by traversing related nodes.</param>
		public virtual void MoveLeft(IList rootNodes)
		{
			int newPosition = this.ParentNode.Position + 1;
			if (this.ParentNode.Level == 0)
			{
				for (int i = newPosition; i < rootNodes.Count; i++)
				{
					Node nodeAlsoToBeMoved = (Node)rootNodes[i];
					nodeAlsoToBeMoved.Position++;
				}
			}
			else
			{
				for (int i = newPosition; i < this.ParentNode.ParentNode.ChildNodes.Count; i++)
				{
					Node nodeAlsoToBeMoved = (Node)this.ParentNode.ParentNode.ChildNodes[i];
					nodeAlsoToBeMoved.Position++;
				}
			}
			this.ParentNode.ChildNodes.Remove(this);
			ReOrderNodePositions(this.ParentNode.ChildNodes, this.Position);
			this.ParentNode = this.ParentNode.ParentNode;
			if (this.ParentNode != null)
			{
				this.ParentNode.ChildNodes.Add(this);
			}
			this.Position = newPosition;
		}

		/// <summary>
		/// Add node to the children of the previous node in the list.
		/// </summary>
		/// <param name="rootNodes"></param>
		public virtual void MoveRight(IList rootNodes)
		{
			if (this._position > 0)
			{
				Node previousSibling;
				if (this.ParentNode != null)
				{
					previousSibling = (Node)this.ParentNode.ChildNodes[this._position - 1];
					this.ParentNode.ChildNodes.Remove(this);
					ReOrderNodePositions(this.ParentNode.ChildNodes, this.Position);
				}
				else
				{
					previousSibling = (Node)rootNodes[this._position - 1];
					ReOrderNodePositions(rootNodes, this.Position);
				}

				this.Position = previousSibling.ChildNodes.Count;
				previousSibling.ChildNodes.Add(this);
				this.ParentNode = previousSibling;
			}
		}

		/// <summary>
		/// Calculate the position of a new node.
		/// </summary>
		/// <param name="rootNodes">The root nodes for the case an item as added at root level.</param>
		public virtual void CalculateNewPosition(IList rootNodes)
		{
			if (this.ParentNode != null)
			{
                this._position = this.ParentNode.ChildNodes.Count;				
			}
			else
			{
				this._position = rootNodes.Count;
			}
		}

		/// <summary>
		/// Ensure that there is no gap between the positions of nodes
		/// </summary>
		/// <param name="parentNode"></param>
		/// <param name="gapPosition"></param>
		public void ReOrderNodePositions(IList nodeListWithGap, int gapPosition)
		{
			foreach (Node node in nodeListWithGap)
			{
				if (node.Position > gapPosition)
				{
					node.Position--;
				}
			}
		}

		/// <summary>
		/// Set the sections to null, so they will be loaded from the database next time.
		/// </summary>
		public void ResetSections()
		{
			this._sections = null;
			// Notify
			OnUpdate();
		}

		/// <summary>
		/// Indicates if viewing of the node is allowed. Anonymous users get a special treatment because we
		/// can't check their rights because they are no full-blown Cuyahoga users (not authenticated).
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public bool ViewAllowed(IIdentity user)
		{
			User cuyahogaUser = user as User;
			if (this.AnonymousViewAllowed)
				return true;
			else if (cuyahogaUser != null)
				return cuyahogaUser.CanView(this);
			else
				return false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void CopyRolesFromParent()
		{
			InitRoles();
			if (this._parentNode != null)
			{
				foreach (Role role in this._parentNode.ViewRoles)
				{
					this._viewRoles.Add(role);
				}
				foreach (Role role in this._parentNode.EditRoles)
				{
					this._editRoles.Add(role);
				}
			}
		}

		/// <summary>
		/// Generate a short description based on the parent short description and the title.
		/// If the short description already exists, a number is added until it is unique.
		/// </summary>
		public void CreateShortDescription()
		{
			string prefix = "";
			if (this._parentNode != null)
			{
				prefix += this._parentNode.ShortDescription + "/";
			}
			this._shortDescription = prefix + this._title.Replace(" ", "").ToLower();
			int suffix = 1;
			while (! CheckUniqueShortDescription())
			{
				string tmpShortDescription = this._shortDescription.Substring(0, this._shortDescription.Length - 2);
				this._shortDescription = tmpShortDescription + "_" + suffix.ToString();
				suffix++;
			}
		}

		public bool CheckUniqueShortDescription()
		{
			if (this._shortDescription != null)
			{
				// ABUSE: use GetNodeByShortDescription to check if the short description is unique.
				Node dummyNode = new Node();
				CmsDataFactory.GetInstance().GetNodeByShortDescription(this._shortDescription, dummyNode);
				return (dummyNode.Id == this.Id);
			}
			else
			{
				throw new NullReferenceException("The Short Description may not be null");
			}
		}

		private void InitRoles()
		{
			this._viewRoles = new RoleCollection();
			this._editRoles = new RoleCollection();
		}

		#endregion
	}
}
