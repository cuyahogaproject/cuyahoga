using System;
using System.Security.Principal;
using System.Collections;

using Cuyahoga.Core.DAL;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for Section.
	/// </summary>
	[Serializable]
	public class Section
	{
		private int _id;
		private DateTime _updateTimestamp;
		private int _nodeId; // TODO: get rid of this one
		private string _title;
		private string _placeholderId;
		private int _position;
		private int _cacheDuration;
		private bool _showTitle;
		private ModuleType _moduleType;
		private ModuleBase _module;
		private Node _node;
		private IList _sectionPermissions;

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
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
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
		/// Property Module
		/// </summary>
		public ModuleType ModuleType
		{
			get { return this._moduleType; }
			set { this._moduleType = value; }
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
		/// Property SectionPermissions (IList)
		/// </summary>
		public IList SectionPermissions
		{
			get { return this._sectionPermissions; }
			set { this._sectionPermissions = value; }
		}

		public bool AnonymousViewAllowed
		{
			get
			{
				foreach (Permission p in this._sectionPermissions)
				{
					if (p.ViewAllowed && Array.IndexOf(p.Role.Permissions, AccessLevel.Anonymous) > -1)
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

		private void InitSection()
		{
			this._nodeId = -1;
			this._showTitle = false;
			this._position = -1;
			this._cacheDuration = 0;
		}

		#endregion

		#region public methods

		/// <summary>
		/// Factory for the concrete module connected to this Section.
		/// </summary>
		/// <returns></returns>
		public ModuleBase CreateModule()
		{
			if (this._moduleType != null)
			{
				string assemblyQualifiedName = this._moduleType.ClassName + ", " + this._moduleType.AssemblyName;
				Type moduleType = Type.GetType(assemblyQualifiedName);
				ModuleBase concreteModule = (ModuleBase)Activator.CreateInstance(moduleType);
				concreteModule.Section = this;
				return concreteModule;
			}
			else
			{
				return null;
			}
		}

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
				foreach (Section section in this.Node.Sections)
				{
					if (section.PlaceholderId == oldPlaceholderId && oldPosition < section.Position)
					{
						section.Position--;
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
//			InitRoles();
//			if (this._node != null)
//			{
//				foreach (Role role in this._node.ViewRoles)
//				{
//					this._viewRoles.Add(role);
//				}
//				foreach (Role role in this._node.EditRoles)
//				{
//					this._editRoles.Add(role);
//				}
//			}
		}

		#endregion

		#region private methods


		#endregion
	}
}
