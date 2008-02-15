using System;
using System.Security.Principal;
using System.Collections;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for Section.
	/// </summary>
	public class Section
	{
		private int _id;
		private DateTime _updateTimestamp;
		private string _title;
		private string _placeholderId;
		private int _position;
		private int _cacheDuration;
		private bool _showTitle;
		private ModuleType _moduleType;
		private Node _node;
		private IList _sectionPermissions;
		private IDictionary _settings;
		private IDictionary _connections;

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
		/// Property Title (string)
		/// </summary>
		public virtual string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		/// <summary>
		/// Property PlaceholderId (string). 
		/// </summary>
		public virtual string PlaceholderId
		{
			get { return this._placeholderId; }
			set {this._placeholderId = value; }
		}

		/// <summary>
		/// Property Position (int)
		/// </summary>
		public virtual int Position
		{
			get { return this._position; }
			set { this._position = value; }
		}

		/// <summary>
		/// Property CacheDuration (int)
		/// </summary>
		public virtual int CacheDuration
		{
			get { return this._cacheDuration; }
			set { this._cacheDuration = value; }
		}

		/// <summary>
		/// Property ShowTitle (bool)
		/// </summary>
		public virtual bool ShowTitle
		{
			get { return this._showTitle; }
			set { this._showTitle = value; }
		}

		/// <summary>
		/// Property Module
		/// </summary>
		public virtual ModuleType ModuleType
		{
			get { return this._moduleType; }
			set { this._moduleType = value; }
		}

		/// <summary>
		/// The Node where this Section belongs to. A Section can belong to either a Node or a Template or is
		/// 'left alone' (detached).
		/// </summary>
		public virtual Node Node
		{
			get { return this._node; }
			set { this._node = value; }
		}

		/// <summary>
		/// Property SectionPermissions (IList)
		/// </summary>
		public virtual IList SectionPermissions
		{
			get { return this._sectionPermissions; }
			set { this._sectionPermissions = value; }
		}

		/// <summary>
		/// Property Settings (IDictionary)
		/// </summary>
		public virtual IDictionary Settings
		{
			get { return this._settings; }
			set { this._settings = value; }
		}

		/// <summary>
		/// Connection points to other sections. The keys are actions and the values the other sections.
		/// </summary>
		public virtual IDictionary Connections
		{
			get { return this._connections; }
			set { this._connections = value; }
		}

		/// <summary>
		/// Can anonymous visitors view the content of this section?
		/// </summary>
		public virtual bool AnonymousViewAllowed
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

		/// <summary>
		/// The full display name of the section.
		/// </summary>
		public virtual string FullName
		{
			get 
			{
				string prefix = String.Empty;
				if (this.Node != null)
				{
					prefix = this.Node.Title;
				}
				return prefix + " - " + this._title + " - " + this.ModuleType.Name;
			}
		}

		#endregion

		#region events

		/// <summary>
		/// Notify the caller that the SessionFactory is rebuilt.
		/// </summary>
		public virtual event EventHandler SessionFactoryRebuilt;

		protected virtual void OnSessionFactoryRebuilt(EventArgs e)
		{
			if (SessionFactoryRebuilt != null)
			{
				SessionFactoryRebuilt(this, e);
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
			this._showTitle = false;
			this._position = -1;
			this._cacheDuration = 0;
			this._sectionPermissions = new ArrayList();
			this._settings = new Hashtable();
			this._connections = new Hashtable();
		}

		#endregion

		#region public methods

		/// <summary>
		/// Factory for the concrete module connected to this Section.
		/// </summary>
		/// <param name="sectionUrl">The url that indentifies the section. We need this because the section
		/// can't determine the url itself because it doesn't have a http context.</param>
		/// <returns></returns>
		//[Obsolete("Deprecated, use the ModuleService to obtain module instances")]
		//public virtual ModuleBase CreateModule(string sectionUrl)
		//{
		//    if (this._moduleType != null)
		//    {
		//        string assemblyQualifiedName = this._moduleType.ClassName + ", " + this._moduleType.AssemblyName;
		//        Type moduleType = Type.GetType(assemblyQualifiedName);
		//        if (moduleType == null)
		//        {
		//            throw new Exception("Could not find module: " + assemblyQualifiedName);
		//        }
		//        else
		//        {
		//            ModuleBase concreteModule = (ModuleBase)Activator.CreateInstance(moduleType, null);
		//            if (concreteModule.SessionFactoryRebuilt)
		//            {
		//                OnSessionFactoryRebuilt(EventArgs.Empty);
		//            }
		//            concreteModule.SectionUrl = sectionUrl;
		//            return concreteModule;
		//        }
		//    }
		//    else
		//    {
		//        return null;
		//    }
		//}

		/// <summary>
		/// Calculate the position of the new Section. If there are more sections with the same PlaceholderId
		/// the position will 1 higher than the top position.
		/// </summary>
		public virtual void CalculateNewPosition()
		{
			if (this.Node != null)
			{
				int maxPosition = -1;
				foreach (Section section in this.Node.Sections)
				{
					if (section.PlaceholderId == this.PlaceholderId 
						&& section.Position > maxPosition
						&& section.Id != this._id)
					{
						maxPosition = section.Position;
					}
				}
				this._position = maxPosition + 1;
			}
			else
			{
				this._position = 0;
			}
		}

		/// <summary>
		/// Section can move down when there's another section below this section with the same Node property and
		/// the same placeholder.
		/// </summary>
		/// <returns></returns>
		public virtual bool CanMoveDown()
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
		public virtual bool CanMoveUp()
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
		public virtual void MoveDown()		
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
				this.Node.Sections.Remove(this);
				this.Node.Sections.Insert(this.Position, this);
			}
		}

		/// <summary>
		/// Decrease the position of the section and increase the position of the following section.
		/// </summary>
		public virtual void MoveUp()
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
				this.Node.Sections.Remove(this);
				this.Node.Sections.Insert(this.Position, this);
			}
		}

		/// <summary>
		/// Update section positions of the sections that have the same Node and PlaceholderId
		//  when the PlaceholderId is changed or deleted (close the gap).
		/// </summary>
		/// <param name="oldPlaceholderId"></param>
		/// <param name="oldPosition"></param>
		/// <param name="resetSections"></param>
		public virtual void ChangeAndUpdatePositionsAfterPlaceholderChange(string oldPlaceholderId, int oldPosition, bool resetSections)
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
				if (resetSections)
				{
					// reset sections, so they will be refreshed from the database when required.
					this.Node.ResetSections();
				}
			}       
		}

		/// <summary>
		/// Indicates if viewing of the section is allowed. Anonymous users get a special treatment because we
		/// can't check their rights because they are no full-blown Cuyahoga users (not authenticated).
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public virtual bool ViewAllowed(IIdentity user)
		{
			User cuyahogaUser = user as User;
			if (this.AnonymousViewAllowed)
			{
				return true;
			}
			else if (cuyahogaUser != null)
			{
				return cuyahogaUser.CanView(this);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Does the specified role have view rights to this Section?
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public virtual bool ViewAllowed(Role role)
		{
			foreach (SectionPermission sp in this.SectionPermissions)
			{
				if (sp.Role == role && sp.ViewAllowed)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Does the specified role have edit rights to this Section?
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public virtual bool EditAllowed(Role role)
		{
			foreach (SectionPermission sp in this.SectionPermissions)
			{
				if (sp.Role == role && sp.EditAllowed)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Copy permissions from the parent Node
		/// </summary>
		public virtual void CopyRolesFromNode()
		{
			if (this.Node != null)
			{
				foreach (NodePermission np in this.Node.NodePermissions)
				{
					SectionPermission sp = new SectionPermission();
					sp.Section = this;
					sp.Role = np.Role;
					sp.ViewAllowed = np.ViewAllowed;
					sp.EditAllowed = np.EditAllowed;
					this.SectionPermissions.Add(sp);
				}
			}
		}
		#endregion

		#region private methods


		#endregion
	}
}
