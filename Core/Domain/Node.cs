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
	public class Node : IPersonalizable
	{
		private int _id;
		private int _parentId;
		private string _title;
		private string _shortDescription;
		private int _position;
		private Node _parentNode;
		private NodeCollection _childNodes;
		private SectionCollection _sections;
		private Template _template;
		private int[] _trail;
		// flag to prevent lazy loading of the parent node when set to null
		private bool _parentSetToNull = false;
		private RoleCollection _viewRoles;
		private RoleCollection _editRoles;

		#region properties

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property ParentId (int)
		/// </summary>
		public int ParentId
		{
			get { return this._parentId; }
			set { this._parentId = value; }
		}

		/// <summary>
		/// Property Title (string)
		/// </summary>
		public string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// Property ShortDescription (string)
		/// </summary>
		public string ShortDescription
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
		public int Position
		{
			get { return this._position; }
			set { this._position = value; }
		}

		/// <summary>
		/// Property Level (int)
		/// </summary>
		public int Level
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
		public Node ParentNode
		{
			get 
			{
				if (! this._parentSetToNull && this._parentNode == null && this._parentId > 0)
				{
					// Load parent
					this.ParentNode = new Node(this._parentId);
					// Notify
					OnUpdate();
				}
				return this._parentNode; 
			}
			set 
			{ 
				this._parentNode = value; 
				if (value != null)
				{
					this._parentId = value.Id;
				}
				OnUpdate();
			}
		}

		/// <summary>
		/// Property ChildNodes (NodeCollection). Lazy loaded.
		/// </summary>
		public NodeCollection ChildNodes
		{
			get 
			{
				if (this._childNodes == null && this._id > 0)
				{
					// ChildNodes are not initialized, so we visit the database for these
					this._childNodes = new NodeCollection();
					ICmsDataProvider dp = CmsDataFactory.GetInstance();
					dp.GetNodesByParent(this, this._childNodes);
					// Notify
					OnChildrenLoaded();
				}
				return this._childNodes; 
			}
		}

		/// <summary>
		/// Property Sections (SectionCollection). Lazy loaded.
		/// </summary>
		public SectionCollection Sections
		{
			get 
			{ 
				if (this._sections == null && this._id > 0)
				{
					this._sections = new SectionCollection();
					ICmsDataProvider dp = CmsDataFactory.GetInstance();
					dp.GetSectionsByNode(this, this._sections);
					// Notify
					OnUpdate();
				}
				return this._sections; 
			}
		}

		/// <summary>
		/// Property Module (Module)
		/// </summary>
		public Template Template
		{
			get 
			{ 
				return this._template; 
			}
			set 
			{ 
				this._template = value; 
			}
		}

		/// <summary>
		/// Array with all NodeId's from the current node to the root node.
		/// </summary>
		public int[] Trail
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
		/// 
		/// </summary>
		public Node PreviousSibling
		{
			get
			{
				if (this.Position > 0)
				{
					if (this.ParentNode != null)
						return this.ParentNode.ChildNodes[this.Position - 1];
					else
					{
						ICmsDataProvider dp = CmsDataFactory.GetInstance();
						Node node = new Node();
						dp.GetNodeByParentIdAndPosition(-1, this.Position - 1, node);
						return node;
					}
				}
				else
					return null;
			}
		}

		/// <summary>
		/// Property ViewRoles (RoleCollection), semi-lazy loaded.
		/// </summary>
		public RoleCollection ViewRoles
		{
			get 
			{ 
				if (this._viewRoles == null)
				{
					// Load the roles from the database. All roles will be loaded at once (view, edit, add, remove).
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
		public RoleCollection EditRoles
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
		public bool AnonymousViewAllowed
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

		/// <summary>
		/// Constructor that accepts an id as parameter. It loads the node from the database.
		/// </summary>
		/// <param name="id"></param>
		public Node(int id) : this()
		{
			CmsDataFactory.GetInstance().GetNodeById(id, this);
		}

		public Node (string shortDescription) : this()
		{
			CmsDataFactory.GetInstance().GetNodeByShortDescription(shortDescription, this);
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

		public void MoveUp()
		{
			if (this._position > 0)
			{
				this._position--;
				ICmsDataProvider dp = CmsDataFactory.GetInstance();
				dp.UpdateVerticalNodePosition(this, NodePositionMovement.Up);
			}
		}

		public void MoveDown()
		{
            this._position++;
			ICmsDataProvider dp = CmsDataFactory.GetInstance();
			dp.UpdateVerticalNodePosition(this, NodePositionMovement.Down);
		}

		/// <summary>
		/// Move node to the same level as the parentnode at the position just beneath the parent node.
		/// </summary>
		public void MoveLeft()
		{
			ICmsDataProvider dp = CmsDataFactory.GetInstance();
			int newPosition = this.ParentNode.Position + 1;
            dp.UpdateNodePositions(this.ParentNode.ParentNode, newPosition, 1);
			this.ParentNode.ChildNodes.Remove(this);
			ReOrderNodePositions(this.ParentNode, this.Position);
			this.ParentNode = this.ParentNode.ParentNode;
			this._parentSetToNull = (this._parentNode == null);
			this.Position = newPosition;
			dp.UpdateNode(this);
		}

		/// <summary>
		/// Add node to the children of the previous node in the list.
		/// </summary>
		public void MoveRight()
		{
			int newPosition = this.PreviousSibling.ChildNodes.Count;
			if (this.ParentNode != null)
				this.ParentNode.ChildNodes.Remove(this);
            ReOrderNodePositions(this.ParentNode, this.Position);
            this.ParentNode = this.PreviousSibling;
			this._position = newPosition;
			ICmsDataProvider dp = CmsDataFactory.GetInstance();
			dp.UpdateNode(this);
		}

		/// <summary>
		/// Calculate the position of a new node.
		/// </summary>
		public void CalculateNewPosition()
		{
			if (this.ParentNode != null)
			{
                this._position = this.ParentNode.ChildNodes.Count;				
			}
			else
			{
                // Root level, we have to visit the database for this one
                ICmsDataProvider dp = CmsDataFactory.GetInstance();
				this._position = dp.GetMaxNodePositionAtRootLevel() + 1;
			}
		}

		/// <summary>
		/// Ensure that there is no gap between the positions of nodes
		/// </summary>
		/// <param name="parentNode"></param>
		/// <param name="gapPosition"></param>
		public void ReOrderNodePositions(Node parentNode, int gapPosition)
		{
			ICmsDataProvider dp = CmsDataFactory.GetInstance();
			dp.UpdateNodePositions(parentNode, gapPosition, -1);
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
