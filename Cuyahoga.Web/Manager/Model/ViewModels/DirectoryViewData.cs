using System;
using System.Collections.Generic;
using System.IO;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	/// <summary>
	/// View model class that contains data for the file manager.
	/// </summary>
	public class DirectoryViewData
	{
		private IList<string> _trail;

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
		/// The parent directory.
		/// </summary>
		public string ParentDirectory
		{
			get
			{
				if (this.Trail.Count > 1)
				{
					return this.Trail[1];
				}
				return null;
			}
		}

		/// <summary>
		/// Gets all virtual directories from the current directory up to the root directory.
		/// </summary>
		public IList<string> Trail
		{
			get { return this._trail; }
		}

		/// <summary>
		/// Creates a new instance of the <see cref="DirectoryViewData"/> class.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="rootPath"></param>
		/// <param name="subDirectories"></param>
		/// <param name="files"></param>
		public DirectoryViewData(string path, string rootPath, IList<DirectoryInfo> subDirectories, IList<FileInfo> files)
		{
			Path = path;
			RootPath = rootPath;
			SubDirectories = subDirectories ?? new List<DirectoryInfo>();
			Files = files ?? new List<FileInfo>();
			BuildTrail();
		}

		private void BuildTrail()
		{
			this._trail = new List<string>();
			this._trail.Add(this.RootPath);
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
						this._trail.Add(trailPath);
					}
				}
			}
		}
	}
}
