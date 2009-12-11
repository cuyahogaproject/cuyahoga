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
		private ArrayList _upgradeScriptVersions;
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
		/// The version of the assembly that is to be installed or upgraded.
		/// </summary>
		public Version NewAssemblyVersion
		{
			get { return this._assembly.GetName().Version; }
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
		public bool CanUninstall
		{
			get { return CheckCanUninstall(); }
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
			string databaseSubDirectory = Path.Combine("Database", this._databaseType.ToString().ToLower());
			this._databaseScriptsDirectory = Path.Combine(installRootDirectory, databaseSubDirectory);

			this._upgradeScriptVersions = new ArrayList();
			CheckDatabaseScripts();
			// Sort the versions in ascending order. This way it's easy to iterate through the scripts
			// when upgrading.
			this._upgradeScriptVersions.Sort();

			if (this._assembly != null)
			{
				CheckCurrentVersionInDatabase();
			}
		}

		/// <summary>
		/// Check if the database connection is valid.
		/// </summary>
		/// <returns></returns>
		public bool TestDatabaseConnection()
		{
			return DatabaseUtil.TestDatabaseConnection();
		}

		/// <summary>
		/// Install the database part of a Cuyaghoga component.
		/// </summary>
		public void Install()
		{
			if (CanInstall)
			{
				log.Info("Installing module with " + this._installScriptFile);
				DatabaseUtil.ExecuteSqlScript(this._installScriptFile);
			}
			else
			{
				throw new InvalidOperationException("Can't install assembly from: " + this._installRootDirectory);
			}
		}

		/// <summary>
		/// Upgrade the database part of a Cuyahoga component to higher version.
		/// </summary>
		public void Upgrade()
		{
			if (CanUpgrade)
			{
				log.Info("Upgrading " + this._assembly.GetName().Name);
				// Iterate through the sorted versions that are extracted from the upgrade script names.
				foreach (Version version in this._upgradeScriptVersions)
				{
					// Only run the script if the version is higher than the current database version
					if (version > this._currentVersionInDatabase)
					{
						string upgradeScriptPath = Path.Combine(this._databaseScriptsDirectory, version.ToString(3) + ".sql");
						log.Info("Running upgrade script " + upgradeScriptPath);
						DatabaseUtil.ExecuteSqlScript(upgradeScriptPath);
						this._currentVersionInDatabase = version;
					}
				}
			}
			else
			{
				throw new InvalidOperationException("Can't upgrade assembly from: " + this._installRootDirectory);
			}
		}

		/// <summary>
		/// Uninstall the database part of a Cuyaghoga component.
		/// </summary>
		public void Uninstall()
		{
			if (CanUninstall)
			{
				log.Info("Uninstalling module with " + this._installScriptFile);
				DatabaseUtil.ExecuteSqlScript(this._uninstallScriptFile);
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
							this._upgradeScriptVersions.Add(version);
						}
						else
						{
							log.Warn(String.Format("Invalid SQL script file found in {0}: {1}", this._databaseScriptsDirectory, file.Name));
						}
					}
				}
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
			return this._currentVersionInDatabase == null && this._installScriptFile != null;
		}

		private bool CheckCanUpgrade()
		{
			if (this._assembly != null)
			{
				if (this._currentVersionInDatabase != null && this._upgradeScriptVersions.Count > 0)
				{
					// Upgrade is possible if the script with the highest version number
					// has a number higher than the current database version AND when the
					// assembly version number is equal or higher than the script with
					// the highest version number.
					Version highestScriptVersion = (Version)this._upgradeScriptVersions[this._upgradeScriptVersions.Count - 1];

					if (this._currentVersionInDatabase < highestScriptVersion
						&& this._assembly.GetName().Version >= highestScriptVersion)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool CheckCanUninstall()
		{
			return (this._assembly != null && this._uninstallScriptFile != null);
		}
	}
}
