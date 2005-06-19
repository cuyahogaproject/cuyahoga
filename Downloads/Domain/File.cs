using System;
using System.Collections;
using System.Security.Principal;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.Downloads.Domain
{
	/// <summary>
	/// The File class represents a file that is available for download.
	/// </summary>
	public class File
	{
		private int _id;
		private string _filePath;
		private string _title;
		private int _size;
		private int _nrOfDownloads;
		private string _contentType;
		private DateTime _datePublished;
		private Section _section;
		private User _publisher;
		private IList _allowedRoles;

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
		/// Property FilePath (string)
		/// </summary>
		public string FilePath
		{
			get { return this._filePath; }
			set { this._filePath = value; }
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
		/// The title that is shown.
		/// </summary>
		public string DisplayTitle
		{
			get 
			{
				if (this._title != null && this._title != String.Empty)
				{
					return this._title;
				}
				else
				{
					return this._filePath;
				}
			}
		}

		/// <summary>
		/// Property Size (int)
		/// </summary>
		public int Size
		{
			get { return this._size; }
			set { this._size = value; }
		}

		/// <summary>
		/// Property NrOfDownloads (int)
		/// </summary>
		public int NrOfDownloads
		{
			get { return this._nrOfDownloads; }
			set { this._nrOfDownloads = value; }
		}

		/// <summary>
		/// Property ContentType (string)
		/// </summary>
		public string ContentType
		{
			get { return this._contentType; }
			set { this._contentType = value; }
		}

		/// <summary>
		/// Property DatePublished (DateTime)
		/// </summary>
		public DateTime DatePublished
		{
			get { return this._datePublished; }
			set { this._datePublished = value; }
		}

		/// <summary>
		/// Property Section (Section)
		/// </summary>
		public Section Section
		{
			get { return this._section; }
			set { this._section = value; }
		}

		/// <summary>
		/// Property Publisher (User)
		/// </summary>
		public User Publisher
		{
			get { return this._publisher; }
			set { this._publisher = value; }
		}

		/// <summary>
		/// Property Roles (IList)
		/// </summary>
		public IList AllowedRoles
		{
			get { return this._allowedRoles; }
			set { this._allowedRoles = value; }
		}

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public File()
		{
			this._id = -1;
			this._nrOfDownloads = 0;
			this._allowedRoles = new ArrayList();
		}

		/// <summary>
		/// Check if download of the file is allowed for the given role.
		/// </summary>
		/// <param name="roleToCheck"></param>
		/// <returns></returns>
		public bool IsDownloadAllowed(Role roleToCheck)
		{
			foreach (Role role in this._allowedRoles)
			{
				if (role.Id == roleToCheck.Id && role.Name == roleToCheck.Name)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Check if download of the file is allowed for the given user.
		/// </summary>
		/// <param name="userToCheck"></param>
		/// <returns></returns>
		public bool IsDownloadAllowed(IIdentity userToCheck)
		{
			User cuyahogaUser = userToCheck as User;
			if (cuyahogaUser == null)
			{
				return this.IsAnonymousDownloadAllowed();
			}
			else
			{
				foreach (Role role in cuyahogaUser.Roles)
				{
					if (IsDownloadAllowed(role))
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Check if download of the file is allowed for anonymous users.
		/// </summary>
		/// <returns></returns>
		public bool IsAnonymousDownloadAllowed()
		{
			foreach (Role role in this._allowedRoles)
			{
				foreach (AccessLevel accessLevel in role.Permissions)
				{
					if (accessLevel == AccessLevel.Anonymous)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
