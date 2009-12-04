using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Principal;
using Castle.Components.Validator;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for User.
	/// </summary>
	public class User : IPrincipal, IIdentity
	{
		private int _id;
		private string _userName;
		private string _password;
		private string _passwordConfirmation;
		private string _firstName;
		private string _lastName;
		private string _email;
		private string _website;
		private int _timeZone;
		private bool _isActive;
		private DateTime? _lastLogin;
		private string _lastIp;
		private bool _isAuthenticated;
		private IList<Role> _roles;
		private IList<Right> _rights;
		private DateTime _insertTimestamp;
		private DateTime _updateTimestamp;

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
        /// Property UserName (string)
        /// </summary>
		[ValidateNonEmpty("UserNameValidatorNonEmpty")]
		[ValidateLength(1, 50, "UserNameValidatorLength")]
        public virtual string UserName
        {
        	get { return this._userName; }
        	set { this._userName = value; }
        }

		/// <summary>
		/// Property Password (string). Internally the MD5 hash of the password is used.
		/// </summary>
		[ValidateNonEmpty("PasswordValidatorNonEmpty")]
		[ValidateLength(5, 50, "PasswordValidatorLength")]
		public virtual string Password
		{
			get { return this._password; }
			set { this._password = value; }
		}

		/// <summary>
		/// Password confirmation.
		/// </summary>
		[ValidateNonEmpty("PasswordConfirmationValidatorNonEmpty")]
		[ValidateLength(5, 50, "PasswordValidatorLength")]
		[ValidateSameAs("Password", "PasswordValidatorSameAs")]
		public virtual string PasswordConfirmation
		{
			get { return _passwordConfirmation; }
			set { _passwordConfirmation = value; }
		}

		/// <summary>
		/// Property FirstName (string)
		/// </summary>
		[ValidateLength(1, 100, "FirstNameValidatorLength")]
		public virtual string FirstName
		{
			get { return this._firstName; }
			set { this._firstName = value; }
		}

		/// <summary>
		/// Property LastName (string)
		/// </summary>
		[ValidateLength(1, 100, "LastNameValidatorLength")]
		public virtual string LastName
		{
			get { return this._lastName; }
			set { this._lastName = value; }
		}

		/// <summary>
		/// The full name of the user. This can be used for display purposes. If there is no firstname
		/// and lastname, the username will be returned.
		/// </summary>
		public virtual string FullName
		{
			get 
			{ 
				if (!string.IsNullOrEmpty(this._firstName) 
					&& !string.IsNullOrEmpty(this._lastName))
				{
					return this._firstName + " " + this._lastName; 
				}
				else
				{
					return this._userName;
				}
			}
		}

		/// <summary>
		/// Property Email (string)
		/// </summary>
		[ValidateNonEmpty("EmailValidatorNonEmpty")]
		[ValidateLength(1, 100, "EmailValidatorLength")]
		[ValidateEmail("EmailValidatorEmail")]
		public virtual string Email
		{
			get { return this._email; }
			set { this._email = value; }
		}

		/// <summary>
		/// Property Website (string)
		/// </summary>
		[ValidateLength(1, 100, "WebsiteValidatorLength")]
		public virtual string Website
		{
			get { return this._website; }
			set { this._website = value; }
		}

		/// <summary>
		/// The timezone offset of the user in minutes.
		/// </summary>
		public virtual int TimeZone
		{
			get { return this._timeZone; }
			set { this._timeZone = value; }
		}

		/// <summary>
		/// Property IsActive (bool)
		/// </summary>
		public virtual bool IsActive
		{
			get { return this._isActive; }
			set { this._isActive = value; }
		}

		/// <summary>
		/// Property LastLogin (DateTime)
		/// </summary>
		public virtual DateTime? LastLogin
		{
			get { return this._lastLogin; }
			set { this._lastLogin = value; }
		}

		/// <summary>
		/// Property LastIp (string)
		/// </summary>
		public virtual string LastIp
		{
			get { return this._lastIp; }
			set { this._lastIp = value; }
		}

		/// <summary>
		/// Property Roles (IList)
		/// </summary>
		[ValidateCollectionNotEmpty("RolesValidatorNotEmpty")]
		public virtual IList<Role> Roles
		{
			get { return this._roles; }
			set { this._roles = value; }
		}

		/// <summary>
		/// IIdentity property <see cref="System.Security.Principal.IIdentity" />.
		/// </summary>
		public virtual bool IsAuthenticated
		{
			get { return this._isAuthenticated; }
			set { this._isAuthenticated = value; }
		}

		/// <summary>
		/// IIdentity property. 
		/// <remark>Returns a string with the Id of the user and not the username</remark>
		/// </summary>
		public virtual string Name
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
		public virtual string AuthenticationType
		{
			get	{ return "CuyahogaAuthentication"; }
		}

		/// <summary>
		/// Property InsertTimestamp (DateTime)
		/// </summary>
		public virtual DateTime InsertTimestamp
		{
			get { return this._insertTimestamp; }
			set { this._insertTimestamp = value; }
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
		/// All rights that this user has.
		/// </summary>
		public virtual IList<Right> Rights
		{
			get
			{
				if (this._rights == null)
				{
					this._rights = new List<Right>();
					foreach (Role role in this._roles)
					{
						foreach (Right right in role.Rights)
						{
							if (! this._rights.Contains(right))
							{
								this._rights.Add(right);
							}
						}
					}
				}
				return this._rights;
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
			this._isActive = true;
			this._timeZone = 0;
			this._rights = null;
			this._roles = new List<Role>();
			// Default to now, otherwise NHibernate tries to insert a NULL.
			this._insertTimestamp = DateTime.Now;
		}

		/// <summary>
		/// Check if the user has the requested access rights.
		/// </summary>
		/// <param name="permission"></param>
		/// <returns></returns>
		[Obsolete("Replaced by HasRight().")]
		public virtual bool HasPermission(AccessLevel permission)
		{
			return HasRight(permission.ToString());
		}

		/// <summary>
		/// Check if the user has the requested access right.
		/// </summary>
		/// <param name="rightName"></param>
		/// <returns></returns>
		public virtual bool HasRight(string rightName)
		{
			foreach (Right right in this.Rights)
			{
				if (right.Name.Equals(rightName, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Check if the user has the requested access right for a given site.
		/// </summary>
		/// <param name="rightName"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		public virtual bool HasRight(string rightName, Site site)
		{
			foreach (Role role in site.Roles)
			{
				if (IsInRole(role) && role.HasRight(rightName))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Indicates if the user has view permissions for a certain Node.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public virtual bool CanView(Node node)
		{
			foreach (Permission p in node.NodePermissions)
			{
				if (p.ViewAllowed && IsInRole(p.Role))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Indicates if the user has edit permissions for a certain Node.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public virtual bool CanEdit(Node node)
		{
			foreach (Permission p in node.NodePermissions)
			{
				if (p.EditAllowed && IsInRole(p.Role))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Indicates if the user has view permissions for a certain Section.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual bool CanView(Section section)
		{
			foreach (Permission p in section.SectionPermissions)
			{
				if (p.ViewAllowed && IsInRole(p.Role))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Indicates if the user has edit permissions for a certain Section.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual bool CanEdit(Section section)
		{
			foreach (Permission p in section.SectionPermissions)
			{
				if (p.EditAllowed && IsInRole(p.Role))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Create a MD5 hash of the password.
		/// </summary>
		/// <param name="password">The password in clear text</param>
		/// <returns>The MD5 hash of the password</returns>
		public static string HashPassword(string password)
		{
			if (ValidatePassword(password))
			{
				return Util.Encryption.StringToMD5Hash(password);
			}
			else
			{
				throw new SecurityException("InvalidPassword");
			}
		}

		/// <summary>
		/// Check if the password is valid.
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public static bool ValidatePassword(string password)
		{
			// Very simple password rule. Extend here when required.
			return (password.Length >= 5);
		}

		/// <summary>
		/// Generates a new password and stores a hashed password in the User instance.
		/// </summary>
		/// <returns>The newly created password.</returns>
		public virtual string GeneratePassword()
		{
			int length = 8;
			string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			string pwd = String.Empty;
			Random rnd = new Random();
			for (int i = 0; i < length; i++)
			{
				pwd += chars[rnd.Next(chars.Length)];
			}
			this._password = User.HashPassword(pwd);
			return pwd;
		}

		/// <summary>
		/// Determine if the user is in a give Role.
		/// </summary>
		/// <param name="roleToCheck"></param>
		/// <returns></returns>
		public virtual bool IsInRole(Role roleToCheck)
		{
			foreach (Role role in this.Roles)
			{
				if (role.Id == roleToCheck.Id && role.Name == roleToCheck.Name)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determine if the user is in a give Role.
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public virtual bool IsInRole(string roleName)
		{
			foreach (Role role in this.Roles)
			{
				if (role.Name == roleName)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Identity of the 
		/// </summary>
		public virtual IIdentity Identity
		{
			get { return this; }
		}
	}
}
