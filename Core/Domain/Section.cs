using System;
using System.Security.Principal;

using Cuyahoga.Core.Collections;
using Cuyahoga.Core.DAL;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for Section.
	/// </summary>
	public class Section : IPersonalizable
	{
		private int _id;
		private int _nodeId;
		private string _title;
		private string _placeholderId;
		private int _position;
		private int _cacheDuration;
		private bool _showTitle;
		private Module _module;
		private Node _node;
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
		/// Property NodeId (int). Use this property only for quick and dirty access to the Id of the node.
		/// In most cases the Section will be created within the context of a Node (Node.Sections).
		/// </summary>
		public int NodeId
		{
			get { return this._nodeId; }
			set { this._nodeId = value; }
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
		/// Property PlaceholderId (string). Setting this property also changes the position of this section
		/// and sections that have the same Node and PlaceholderId.
		/// </summary>
		public string PlaceholderId
		{
			get { return this._placeholderId; }
			set 
			{
				this._placeholderId = value;
			}
		}

		/// <summary>
		/// Property Position (int)
		/// </summary>
		public int Position
		{
			get { return this._position; }
			set { this._position = value; }
		}

		/// <summary>
		/// Property CacheDuration (int)
		/// </summary>
		public int CacheDuration
		{
			get { return this._cacheDuration; }
			set { this._cacheDuration = value; }
		}

		/// <summary>
		/// Property ShowTitle (bool)
		/// </summary>
		public bool ShowTitle
		{
			get { return this._showTitle; }
			set { this._showTitle = value; }
		}

		/// <summary>
		/// Property Module (abstract Module)
		/// Note: Modules are not lazy loaded.
		/// </summary>
		public Module Module
		{
			get { return this._module; }
			set { this._module = value; }
		}

		/// <summary>
		/// Property Node (Node)
		/// </summary>
		public Node Node
		{
			get { return this._node; }
			set 
			{ 
				this._nodeId = value.Id;
				this._node = value; 
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
					CmsDataFactory.GetInstance().GetRolesBySection(this);
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
					CmsDataFactory.GetInstance().GetRolesBySection(this);
				}
				return this._editRoles; 
			}
			set { this._editRoles = value; }
		}

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

		#region constructor and initialization

		/// <summary>
		/// Default constructor. Creates a new empty instance.
		/// </summary>
		public Section()
		{
			this._id = -1;
			InitSection();
		}

		/// <summary>
		/// Constructor that loads the section from the database, including the Module and the Node (if available).
		/// </summary>
		/// <param name="Id"></param>
		public Section(int id)
		{
			this._id = id;
			InitSection();
			ICmsDataProvider dp = CmsDataFactory.GetInstance();
			dp.GetSectionById(id, this);
		}

		private void InitSection()
		{
			this._nodeId = -1;
			this._module = null;
			this._node = null;
			this._title = null;
			this._showTitle = false;
			this._position = -1;
			this._placeholderId = null;
			this._cacheDuration = 0;
		}

		#endregion

		#region public methods

		public void CalculateNewPosition()
		{
			if (this.Node != null)
			{
				int maxPosition = -1;
				// Iterate other sections. If there are more sections with the same PlaceholderId
				// the position will 1 higher than the top position.
				foreach (Section section in this.Node.Sections)
				{
					if (section.PlaceholderId == this.PlaceholderId && section.Position > maxPosition)
						maxPosition = section.Position;
				}
				this.Position = maxPosition + 1;
			}
			else
				this._position = 0;
		}

		/// <summary>
		/// Section can move down when there's another section below this section with the same Node property and
		/// the same placeholder.
		/// </summary>
		/// <returns></returns>
		public bool CanMoveDown()
		{
			if (this.Node != null)
			{
				foreach (Section section in this.Node.Sections)
				{
					if (section.PlaceholderId == this.PlaceholderId && section.Position > this.Position)
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Section can move up when there's another section above this section with the same Node property and
		/// the same placeholder.
		/// </summary>
		/// <returns></returns>
		public bool CanMoveUp()
		{
			if (this.Node != null)
			{
				foreach (Section section in this.Node.Sections)
				{
					if (section.PlaceholderId == this.PlaceholderId && section.Position < this.Position)
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Increase the position of the section and decrease the position of the previous section.
		/// </summary>
		public void MoveDown()		
		{
			if (this.Node != null)
			{
				foreach (Section section in this.Node.Sections)
				{
					if (section.PlaceholderId == this.PlaceholderId && section.Position == this.Position + 1)
					{
						// switch positions
						this.Position++;
						section.Position--;
						ICmsDataProvider dp = CmsDataFactory.GetInstance();
						dp.UpdateSection(this);
						dp.UpdateSection(section);
						// reset sections, so they will be refreshed from the database when required.
						this.Node.ResetSections();
						break;
					}
				}
			}
		}

		/// <summary>
		/// Decrease the position of the section and increase the position of the following section.
		/// </summary>
		public void MoveUp()
		{
			if (this.Node != null)
			{
				foreach (Section section in this.Node.Sections)
				{
					if (section.PlaceholderId == this.PlaceholderId && section.Position == this.Position - 1)
					{
						// switch positions
						this.Position--;
						section.Position++;
						ICmsDataProvider dp = CmsDataFactory.GetInstance();
						dp.UpdateSection(this);
						dp.UpdateSection(section);
						// reset sections, so they will be refreshed from the database when required.
						this.Node.ResetSections();
						break;
					}
				}
			}
		}

		/// <summary>
		/// Remove a section. This includes deleting all the content of any associated module and
		/// rearranging the 'neighbour' section positions.
		/// </summary>
		public void Remove()
		{
            // First delete the module content
			this.Module.DeleteContent();
			// Then delete section
			ICmsDataProvider dp = CmsDataFactory.GetInstance();
			dp.DeleteSection(this);
			// Then rearrange positions of neighbour sections (kind of abusing an already existing method :-))
			this.ChangeAndUpdatePositionsAfterPlaceholderChange(this.PlaceholderId, this.Position);
		}

		/// <summary>
		/// Update section positions of the sections that have the same Node and PlaceholderId
		//  when the PlaceholderId is changed or deleted (close the gap).
		/// </summary>
		/// <param name="oldPlaceholderId"></param>
		/// <param name="oldPosition"></param>
		public void ChangeAndUpdatePositionsAfterPlaceholderChange(string oldPlaceholderId, int oldPosition)
		{
			if (this.Node != null)
			{
				ICmsDataProvider dp = CmsDataFactory.GetInstance();
				foreach (Section section in this.Node.Sections)
				{
					if (section.PlaceholderId == oldPlaceholderId && oldPosition < section.Position)
					{
						section.Position--;
						dp.UpdateSection(section);
					}
				}
				// reset sections, so they will be refreshed from the database when required.
				this.Node.ResetSections();
			}       
		}

		/// <summary>
		/// Indicates if viewing of the section is allowed. Anonymous users get a special treatment because we
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

		public void CopyRolesFromNode()
		{
			InitRoles();
			if (this._node != null)
			{
				foreach (Role role in this._node.ViewRoles)
				{
					this._viewRoles.Add(role);
				}
				foreach (Role role in this._node.EditRoles)
				{
					this._editRoles.Add(role);
				}
			}
		}

		#endregion

		#region private methods

		private void InitRoles()
		{
			this._viewRoles = new RoleCollection();
			this._editRoles = new RoleCollection();
		}

		#endregion
	}
}
