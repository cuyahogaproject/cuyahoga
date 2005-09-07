using System;
using System.Reflection;
using System.IO;
using System.Collections;

using log4net;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Core.Service
{
	/// <summary>
	/// The DatabaseInstaller class is responsible for installing, upgrading and uninstalling
	/// database tables and records. 
	/// </summary>
	public class DatabaseInstaller
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseInstaller));

		private string _installRootDirectory;
		private string _databaseScriptsDirectory;
		private Assembly _assembly;
		private DatabaseType _databaseType;
		private string _installScriptFile;
		private string _uninstallScriptFile;
		private ArrayList _scriptVersions;
		private Version _currentVersionInDatabase;

		/// <summary>
		/// The database type of the current database;
		/// </summary>
		public DatabaseType DatabaseType
		{
			get { return this._databaseType; }
		}

		/// <summary>
		/// The current version of the module/assembly in the database.
		/// </summary>
		public Version CurrentVersionInDatabase
		{
			get { return this._currentVersionInDatabase; }
		}

		/// <summary>
		/// Indicates if a module or assembly can be installed from the given location.
		/// </summary>
		public bool CanInstall
		{
			get { return CheckCanInstall(); }
		}

		/// <summary>
		/// Indicates if a module or assembly can be upgraded from the given location.
		/// </summary>
		public bool CanUpgrade
		{
			get { return CheckCanUpgrade(); }
		}

		/// <summary>
		/// Indicates if a module or assembly can be uninstalled from the given location.
		/// </summary>
		public bool CanUnInstall
		{
			get { return CheckCanUnInstall(); }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="installRootDirectory">The physical path of the directory where the install
		/// scripts are located. This is the root install directory without 'Database/DatabaseType'.</param>
		/// <param name="assembly">The (optional) assembly that has to be upgraded or uninstalled.</param>
		public DatabaseInstaller(string installRootDirectory, Assembly assembly)
		{
			this._installRootDirectory = installRootDirectory;
			this._assembly = assembly;
			this._databaseType = DatabaseUtil.GetCurrentDatabaseType();
			string databaseSubDirectory = Path.Combine("Database", this._databaseType.ToString());
			this._databaseScriptsDirectory = Path.Combine(installRootDirectory, databaseSubDirectory);

			this._scriptVersions = new ArrayList();
			CheckDatabaseScripts();
			if (this._assembly != null)
			{
				CheckCurrentVersionInDatabase();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Install()
		{
			if (CanInstall)
			{
			}
			else
			{
				throw new InvalidOperationException("Can't install assembly from: " + this._installRootDirectory);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Upgrade()
		{
			if (CanUpgrade)
			{
			}
			else
			{
				throw new InvalidOperationException("Can't upgrade assembly from: " + this._installRootDirectory);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void UnInstall()
		{
			if (CanUnInstall)
			{
			}
			else
			{
				throw new InvalidOperationException("Can't uninstall assembly from: " + this._installRootDirectory);
			}
		}

		private void CheckDatabaseScripts()
		{
			DirectoryInfo directory = new DirectoryInfo(this._databaseScriptsDirectory);
			if (directory.Exists)
			{
				foreach (FileInfo file in directory.GetFiles("*.sql"))
				{
					if (file.Name.ToLower() == "install.sql")
					{
						this._installScriptFile = file.FullName;
					}
					else if (file.Name.ToLower() == "uninstall.sql")
					{
						this._uninstallScriptFile = file.FullName;
					}
					else
					{
						// Extract the version from the script filename.
						// NOTE: these filenames have to be in the major.minor.patch.sql format
						string[] extractedVersion = file.Name.Split('.');
						if (extractedVersion.Length == 4)
						{
							Version version = new Version(
								Int32.Parse(extractedVersion[0]),
								Int32.Parse(extractedVersion[1]),
								Int32.Parse(extractedVersion[2]));
							this._scriptVersions.Add(version);
						}
						else
						{
							log.Warn(String.Format("Invalid SQL script file found in {0}: {1}", this._databaseScriptsDirectory, file.Name));
						}
					}
				}
				// Sort the versions in ascending order. This way it's easy to iterate through the scripts
				// when upgrading.
				this._scriptVersions.Sort();
			}
		}

		private void CheckCurrentVersionInDatabase()
		{
			if (this._assembly != null)
			{
				this._currentVersionInDatabase = DatabaseUtil.GetAssemblyVersion(this._assembly.GetName().Name);
			}
		}

		private bool CheckCanInstall()
		{
			if (this._assembly != null)
			{
				return false;
			}
			else
			{
				return this._installScriptFile != null;
			}
		}

		private bool CheckCanUpgrade()
		{
			if (this._assembly != null)
			{
				if (this._scriptVersions.Count > 0)
				{
					// Upgrade is possible if the script with the highest version number
					// has a number higher than the current database version AND when the
					// assembly version number is equal or higher than the script with
					// the highest version number.
					Version highestScriptVersion = (Version)this._scriptVersions[this._scriptVersions.Count - 1];

					if (this._currentVersionInDatabase < highestScriptVersion
						&& this._assembly.GetName().Version >= highestScriptVersion)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool CheckCanUnInstall()
		{
			return (this._assembly != null && this._uninstallScriptFile != null);
		}
	}
}
