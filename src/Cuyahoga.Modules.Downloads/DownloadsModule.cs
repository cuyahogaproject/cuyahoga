using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Service.Files;
using Cuyahoga.Web.Mvc;
using Cuyahoga.Web.Mvc.Areas;

namespace Cuyahoga.Modules.Downloads
{
	/// <summary>
	/// The Downloads Module provides file downloads for users.
	/// </summary>
	public class DownloadsModule : ModuleBase, IMvcModule
	{
		private string _physicalDir;
		private bool _showPublisher;
		private bool _showDateModified;
		private bool _showNumberOfDownloads;
		private long _currentFileId;
		private DownloadsModuleActions _currentAction;
		private readonly IFileResourceService _fileResourceService;
		private readonly IContentItemService<FileResource> _contentItemService;
		private readonly ICuyahogaContext _cuyahogaContext;

		#region properties

		/// <summary>
		/// The physical directory where the files are located.
		/// </summary>
		/// <remarks>
		/// If there is no setting for the physical directory, the default of
		/// Application_Root/SiteData/site_id/Downloads/section_id is used.
		///	</remarks>
		public string FileDir
		{
			get
			{
				if (this._physicalDir == null)
				{
					this._physicalDir = Path.Combine(this._cuyahogaContext.PhysicalSiteDataDirectory
						, "UserFiles" + Path.DirectorySeparatorChar + "Downloads" + Path.DirectorySeparatorChar + Section.Id);
					this._fileResourceService.CheckPhysicalDirectory(this._physicalDir);
				}
				return this._physicalDir;
			}
			set
			{
				this._physicalDir = value;
				this._fileResourceService.CheckPhysicalDirectory(this._physicalDir);
			}
		}

		/// <summary>
		/// The Id of the file that is to be downloaded.
		/// </summary>
		public long CurrentFileId
		{
			get { return this._currentFileId; }
		}

		/// <summary>
		/// A specific action that has to be done by the module.
		/// </summary>
		public DownloadsModuleActions CurrentAction
		{
			get { return this._currentAction; }
		}

		/// <summary>
		/// Show the name of the user who published the file?
		/// </summary>
		public bool ShowPublisher
		{
			get { return this._showPublisher; }
		}

		/// <summary>
		/// Show the date and time when the file was last updated?
		/// </summary>
		public bool ShowDateModified
		{
			get { return this._showDateModified; }
		}

		/// <summary>
		/// Show the number of downloads?
		/// </summary>
		public bool ShowNumberOfDownloads
		{
			get { return this._showNumberOfDownloads; }
		}

		#endregion

		/// <summary>
		/// DownloadsModule constructor.
		/// </summary>
		/// <param name="fileResourceService"></param>
		/// <param name="cuyahogaContext"></param>
		/// <param name="contentItemService"></param>
		public DownloadsModule(IFileResourceService fileResourceService, ICuyahogaContext cuyahogaContext, IContentItemService<FileResource> contentItemService)
		{
			this._fileResourceService = fileResourceService;
			this._contentItemService = contentItemService;
			this._cuyahogaContext = cuyahogaContext;
		}

		public override void ReadSectionSettings()
		{
			base.ReadSectionSettings();
			// Set dynamic module settings
			string physicalDir = Convert.ToString(base.Section.Settings["PHYSICAL_DIR"]);
			if (physicalDir != String.Empty)
			{
				this._physicalDir = physicalDir;
			}
			this._showPublisher = Convert.ToBoolean(base.Section.Settings["SHOW_PUBLISHER"]);
			this._showDateModified = Convert.ToBoolean(base.Section.Settings["SHOW_DATE"]);
			this._showNumberOfDownloads = Convert.ToBoolean(base.Section.Settings["SHOW_NUMBER_OF_DOWNLOADS"]);
		}

		/// <summary>
		/// Validate module settings.
		/// </summary>
		public override void ValidateSectionSettings()
		{
			// Check if the virtual directory exists.
			// We need to do this based on the section setting because it might be possible that the related section 
			// isn't saved yet.
			if (base.Section != null)
			{
				string physicalDir = Convert.ToString(base.Section.Settings["PHYSICAL_DIR"]);
				if (!String.IsNullOrEmpty(physicalDir))
				{
					this._fileResourceService.CheckPhysicalDirectory(physicalDir);
				}
			}
			base.ValidateSectionSettings();
		}

		/// <summary>
		/// Get the a fileResource as stream.
		/// </summary>
		/// <returns></returns>
		public Stream GetFileAsStream(FileResource fileResource)
		{
			if (fileResource.IsViewAllowed(this._cuyahogaContext.CurrentUser ?? HttpContext.Current.User))
			{
				string physicalFilePath = Path.Combine(this.FileDir, fileResource.FileName);				
				return this._fileResourceService.ReadFile(physicalFilePath);
			}
			throw new AccessForbiddenException("The current user has no permission to download the fileResource.");
		}

		/// <summary>
		/// Retrieve the meta-information of all files that belong to this module.
		/// </summary>
		/// <returns></returns>
		public virtual IList<FileResource> GetAllFiles()
		{
			return this._contentItemService.FindVisibleContentItemsBySection(this.Section, new ContentItemQuerySettings(ContentItemSortBy.PublishedAt, ContentItemSortDirection.DESC));
		}

		/// <summary>
		/// Get the meta-information of a single file.
		/// </summary>
		/// <param name="fileId"></param>
		/// <returns></returns>
		public virtual FileResource GetFileById(long fileId)
		{
			return this._contentItemService.GetById(fileId);
		}

		/// <summary>
		/// Increase the number of downloads for the given file.
		/// </summary>
		/// <param name="fileResource"></param>
		public virtual void IncreaseNrOfDownloads(FileResource fileResource)
		{
			this._fileResourceService.IncreaseDownloadCount(fileResource);
		}

		/// <summary>
		/// Parse the pathinfo.
		/// </summary>
		protected override void ParsePathInfo()
		{
			base.ParsePathInfo();
			if (base.ModuleParams != null)
			{
				if (base.ModuleParams.Length >= 2)
				{
					// First argument is the module action and the second is the Id of the file. The rest we don't care about for now.
					try
					{
						this._currentAction = (DownloadsModuleActions)Enum.Parse(typeof(DownloadsModuleActions)
							, base.ModuleParams[0], true);
						this._currentFileId = Int32.Parse(base.ModuleParams[1]);
					}
					catch (ArgumentException ex)
					{
						throw new Exception("Error when parsing module action: " + base.ModuleParams[0], ex);
					}
					catch (Exception ex)
					{
						throw new Exception("Error when parsing module parameters: " + base.ModulePathInfo, ex);
					}
				}
				else
				{
					this._currentAction = DownloadsModuleActions.ViewFiles;
				}
			}
		}

		#region IMvcModule members

		public void RegisterRoutes(RouteCollection routes)
		{
			routes.CreateArea("Modules/Downloads", "Cuyahoga.Modules.Downloads.Controllers",
				routes.MapRoute("DownloadsRoute", "Modules/Downloads/{controller}/{action}/{id}", new { action = "Index", controller = "", id = "" })
			);
		}

		#endregion
	}	

	public enum DownloadsModuleActions
	{
		ViewFiles,
		Download
	}
}
