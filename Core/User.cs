using System;
using System.Collections;
using System.Security.Principal;

using Cuyahoga.Core.DAL;
using Cuyahoga.Core.Collections;

namespace Cuyahoga.Core
{
	/// <summary>
	/// Summary description for User.
	/// </summary>
	public class User : IIdentity
	{
		private int _id;
		private string _userName;
		private string _password;
		private string _firstName;
		private string _lastName;
		private string _email;
		private DateTime _lastLogin;
		private string _lastIp;
		private bool _isAuthenticated;
		private RoleCollection _roles;
		private AccessLevel[] _permissions;

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
        /// Property UserName (string)
        /// </summary>
        public string UserName
        {
        	get { return this._userName; }
        	set { this._userName = value; }
        }

		/// <summary>
		/// Property Password (string)
		/// </summary>
		public string Password
		{
			get { return this._password; }
			set { this._password = value; }
		}

		/// <summary>
		/// Property FirstName (string)
		/// </summary>
		public string FirstName
		{
			get { return this._firstName; }
			set { this._firstName = value; }
		}

		/// <summary>
		/// Property LastName (string)
		/// </summary>
		public string LastName
		{
			get { return this._lastName; }
			set { this._lastName = value; }
		}

		/// <summary>
		/// Property Email (string)
		/// </summary>
		public string Email
		{
			get { return this._email; }
			set { this._email = value; }
		}

		/// <summary>
		/// Property LastLogin (DateTime)
		/// </summary>
		public DateTime LastLogin
		{
			get { return this._lastLogin; }
			set { this._lastLogin = value; }
		}

		/// <summary>
		/// Property LastIp (string)
		/// </summary>
		public string LastIp
		{
			get { return this._lastIp; }
			set { this._lastIp = value; }
		}

		/// <summary>
		/// Property Roles (RoleCollection)
		/// </summary>
		public RoleCollection Roles
		{
			get { return this._roles; }
		}

		/// <summary>
		/// IIdentity property <see cref="System.Security.Principal.IIdentity" />.
		/// </summary>
		public bool IsAuthenticated
		{
			get { return this._isAuthenticated; }
		}

		/// <summary>
		/// IIdentity property. 
		/// <remark>Returns a string with the Id of the user and not the username</remark>
		/// </summary>
		public string Name
		{
			get
			{ 
				if (this._isAuthenticated)
					return this._id.ToString();
				else
					return "";
			}
		}

		/// <summary>
		/// IIdentity property <see cref="System.Security.Principal.IIdentity" />.
		/// </summary>
		public string AuthenticationType
		{
			get	{ return "CuyahogaAuthentication"; }
		}

		public AccessLevel[] Permissions
		{
			get 
			{
				if (this._permissions.Length == 0)
				{
					ArrayList permissions = new ArrayList();
					foreach (Role role in this.Roles)
					{
						foreach (AccessLevel permission in role.Permissions)
						{
							if (permissions.IndexOf(permission) == -1)
								permissions.Add(permission);
						}
					}
					this._permissions = (AccessLevel[])permissions.ToArray(typeof(AccessLevel));
				}
				return this._permissions;
			}
		}

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public User()
		{
			this._id = -1;
			this._isAuthenticated = false;
			this._roles = new RoleCollection();
			this._permissions = new AccessLevel[0];
		}

		/// <summary>
		/// Constructor that accepts a userId as parameter and tries to load the user from the database.
		/// </summary>
		/// <param name="userId"></param>
		public User(int userId)
		{
			CmsDataFactory.GetInstance().GetUserById(userId, this);
		}

		/// <summary>
		/// Try to log-in the user with the username and password. 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool Login(string username, string password)
		{
            ICmsDataProvider dp = CmsDataFactory.GetInstance();
            dp.GetUserByUsernameAndPassword(username, password, this);
			this._isAuthenticated = (this._id > 0);
			return this._isAuthenticated;
		}

		/// <summary>
		/// Check if the user has the requested access rights.
		/// </summary>
		/// <param name="accessLevel"></param>
		/// <returns></returns>
		public bool HasPermission(AccessLevel permission)
		{
			return Array.IndexOf(this.Permissions, permission) > -1;
		}

		public bool CanView(Node node)
		{
			foreach (Role nodeViewRole in node.ViewRoles)
			{
				if (this.Roles.Contains(nodeViewRole))
				{
					return true;
				}
			}
			return false;
		}

		public bool CanView(Section section)
		{
			foreach (Role sectionViewRole in section.ViewRoles)
			{
				if (this.Roles.Contains(sectionViewRole))
				{
					return true;
				}
			}
			return false;
		}

		public bool CanEdit(Section section)
		{
			foreach (Role sectionEditRole in section.EditRoles)
			{
				if (this.Roles.Contains(sectionEditRole))
				{
					return true;
				}
			}
			return false;
		}
	}
}
