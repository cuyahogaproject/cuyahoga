using System;
using System.Collections;
using System.Web;

using NHibernate;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Modules.Downloads.Domain;

namespace Cuyahoga.Modules.Downloads
{
	/// <summary>
	/// The Downloads Module provides file downloads for users.
	/// </summary>
	public class DownloadsModule : ModuleBase
	{
		private string _physicalDir;
		private bool _showPublisher;
		private bool _showDateModified;

		/// <summary>
		/// The physical directory where the files are located.
		/// </summary>
		/// <remarks>
		/// If there is no setting for the physical directory, the default of
		/// Application_Root/files is used.
		///	</remarks>
		public string FileDir
		{
			get 
			{ 
				if (this._physicalDir == null)
				{
					this._physicalDir = HttpContext.Current.Server.MapPath("~/files");
					CheckPhysicalDirectory();
				}
				return this._physicalDir;
			}
			set 
			{ 
				this._physicalDir = value;
				CheckPhysicalDirectory();
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="section"></param>
		public DownloadsModule(Section section) : base(section)
		{
			SessionFactory sf = SessionFactory.GetInstance();
			// Register classes that are used by the DownloadsModule
			sf.RegisterPersistentClass(typeof(Cuyahoga.Modules.Downloads.Domain.File));

			base.SessionFactoryRebuilt = sf.Rebuild();

			// Set dynamic module settings
			string physicalDir = Convert.ToString(section.Settings["PHYSICAL_DIR"]);
			if (physicalDir != String.Empty)
			{
				this._physicalDir = physicalDir;
				CheckPhysicalDirectory();
			}
			this._showPublisher = Convert.ToBoolean(section.Settings["SHOW_PUBLISHER"]);
			this._showDateModified = Convert.ToBoolean(section.Settings["SHOW_DATE"]);
		}

		/// <summary>
		/// Retrieve the meta-information of all files that belong to this module.
		/// </summary>
		/// <returns></returns>
		public IList GetAllFiles()
		{
			string hql = "from File f where f.Section.Id = :sectionId";
			IQuery q = base.NHSession.CreateQuery(hql);
			q.SetInt32("sectionId", base.Section.Id);

			try
			{
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Files for section: " + base.Section.Title, ex);
			}
		}

		/// <summary>
		/// Get the meta-information of a single file.
		/// </summary>
		/// <param name="fileId"></param>
		/// <returns></returns>
		public File GetFileById(int fileId)
		{
			try
			{
				return (File)base.NHSession.Load(typeof(File), fileId);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get File with identifier: " + fileId.ToString(), ex);
			}
		}

		/// <summary>
		/// Insert or update the meta-information of a file.
		/// </summary>
		public void SaveFile(File file)
		{
			if (file.Id == -1)
			{
				file.DateModified = DateTime.Now;
			}
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.SaveOrUpdate(file);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save File", ex);
			}
		}

		/// <summary>
		/// Delete the meta-information of a file
		/// </summary>
		public void DeleteFile(File file)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(file);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete File", ex);
			}
		}

		private void CheckPhysicalDirectory()
		{
			// Check existence
			if (! System.IO.Directory.Exists(this._physicalDir))
			{
				throw new Exception(String.Format("The Downloads module didn't find the physical directory {0} on the server.", this._physicalDir));
			}
		}
	}
}
