using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Cuyahoga.Core;
using Cuyahoga.Core.Service.Membership;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	/// <summary>
	/// View model class that contains data for the file manager.
	/// </summary>
	public class DirectoryViewData
	{
		private IDictionary<string, string> _trail;

		/// <summary>
		/// The current virtual path.
		/// </summary>
		public string Path { get; private set; }

		/// <summary>
		/// The root virtual path for the current user and site.
		/// </summary>
		public string RootPath { get; private set; }

		/// <summary>
		/// All subdirectories that are below the current virtual path.
		/// </summary>
		public IList<DirectoryInfo> SubDirectories { get; private set; }

		/// <summary>
		/// All files that are in the current virtual path.
		/// </summary>
		public IList<FileInfo> Files { get; private set; }

		/// <summary>
		/// Indicates if the current user is allowed to create directories.
		/// </summary>
		public bool CanCreateDirectory { get; private set; }

		/// <summary>
		/// Indicates if the current user is allowed to copy files or directories.
		/// </summary>
		public bool CanCopy { get; private set; }

		/// <summary>
		/// Indicates if the current user is allowed to move files or directories.
		/// </summary>
		public bool CanMove { get; private set; }

		/// <summary>
		/// Indicates if the current user is allowed to delete files or directories.
		/// </summary>
		public bool CanDelete { get; private set; }

		/// <summary>
		/// The parent directory.
		/// </summary>
		public string ParentDirectory
		{
			get
			{
				if (this._trail.Count > 1)
				{
					return this._trail.Keys.ToList()[this._trail.Count - 2];
				}
				return null;
			}
		}

		/// <summary>
		/// Gets all virtual directories from the current directory up to the root directory.
		/// Keys are the virtual directories and the values are the directory names.
		/// </summary>
		public IDictionary<string, string> Trail
		{
			get { return this._trail; }
		}

		/// <summary>
		/// Creates a new instance of the <see cref="DirectoryViewData"/> class.
		/// </summary>
		/// <param name="cuyahogaContext"></param>
		/// <param name="path"></param>
		/// <param name="rootPath"></param>
		/// <param name="subDirectories"></param>
		/// <param name="files"></param>
		public DirectoryViewData(ICuyahogaContext cuyahogaContext, string path, string rootPath, IList<DirectoryInfo> subDirectories, IList<FileInfo> files)
		{
			var user = cuyahogaContext.CurrentUser;
			CanCreateDirectory = user.HasRight(Rights.CreateDirectory);
			CanCopy = user.HasRight(Rights.CopyFiles);
			CanMove = user.HasRight(Rights.MoveFiles);
			CanDelete = user.HasRight(Rights.DeleteFiles);
			Path = path;
			RootPath = rootPath;
			SubDirectories = subDirectories ?? new List<DirectoryInfo>();
			Files = files ?? new List<FileInfo>();
			BuildTrail();
		}

		private void BuildTrail()
		{
			this._trail = new Dictionary<string, string>();
			this._trail.Add(this.RootPath, GetDirectoryNameFromPath(this.RootPath));
			if (this.Path.Length > this.RootPath.Length)
			{
				string pathFragmentWithoutRoot = this.Path.Substring(this.RootPath.Length);
				string[] subdirectories = pathFragmentWithoutRoot.Split('/');
				string trailPath = this.RootPath;
				foreach (string subdirectory in subdirectories)
				{
					if (!String.IsNullOrEmpty(subdirectory))
					{
						trailPath += subdirectory + "/";
						this._trail.Add(trailPath, GetDirectoryNameFromPath(trailPath));
					}
				}
			}
		}

		private string GetDirectoryNameFromPath(string path)
		{
			string pathWithoutSlash = VirtualPathUtility.RemoveTrailingSlash(path);
			return pathWithoutSlash.Substring(pathWithoutSlash.LastIndexOf("/") + 1);
		}
	}
}
