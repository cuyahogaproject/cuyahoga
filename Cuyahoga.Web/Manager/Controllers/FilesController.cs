using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Cuyahoga.Core;
using Cuyahoga.Core.Service.Files;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Manager.Filters;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageFiles)]
	public class FilesController : ManagerController
	{
		private readonly IFileManagerService _fileManagerService;

		/// <summary>
		/// Creates a new instance of the <see cref="FilesController"/> class.
		/// </summary>
		/// <param name="fileManagerService"></param>
		public FilesController(IFileManagerService fileManagerService)
		{
			_fileManagerService = fileManagerService;
		}

		/// <summary>
		/// Display the file manager admin view page.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public ActionResult Index(string path)
		{
			if (String.IsNullOrEmpty(path))
			{
				path = Url.Content(CuyahogaContext.CurrentSite.SiteDataDirectory + "UserFiles/");
			}
			return View(GetDirectoryViewDataForPath(path));
		}

		/// <summary>
		/// Display a list of files and directories for the given path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		[PartialMessagesFilter]
		public ActionResult List(string path)
		{
			VerifyAccessRights(path);
			return PartialView(GetDirectoryViewDataForPath(path));
		}

		/// <summary>
		/// Get a JSON array with all the available sitedata directories for the current user.
		/// </summary>
		/// <returns></returns>
		public ActionResult GetAllDirectories()
		{
			IList directories = new ArrayList();
			string rootDataPath = GetRootDataPath();
			directories.Add(new {Path = rootDataPath, Name = rootDataPath, Level = 0});
			AddDirectoriesForParentPath(directories, rootDataPath , 1);
			return Json(directories);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[PermissionFilter(RequiredRights = Rights.CreateDirectory)]
		public ActionResult CreateDirectory(string name, string parentPath)
		{
			VerifyAccessRights(parentPath);

			string physicalDirectory = Server.MapPath(parentPath + name);
			try
			{
				this._fileManagerService.CreateDirectory(physicalDirectory);
				Messages.AddFlashMessageWithParams("Directory {0} was created successfully.", name);
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("List", new { Path = parentPath });
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[PermissionFilter(RequiredRights = Rights.DeleteFiles)]
		public ActionResult Delete(string path, string[] directories, string[] files)
		{
			var directoriesToDelete = directories != null ? directories.Select(dir => Server.MapPath(dir)).ToArray() : new string[0];
			var filesToDelete = files != null ? files.Select(file => Server.MapPath(file)).ToArray() : new string[0];
			try
			{
				this._fileManagerService.DeleteFilesAndDirectories(filesToDelete, directoriesToDelete);
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("List", new {Path = path});
		}

		private void VerifyAccessRights(string path)
		{
			if (!path.StartsWith(GetRootDataPath()))
			{
				throw new AccessForbiddenException(string.Format("Access forbidden to {0}!", path));
			}
		}

		private DirectoryViewData GetDirectoryViewDataForPath(string virtualPath)
		{
			DirectoryInfo theDirectory = new DirectoryInfo(Server.MapPath(virtualPath));
			var directoryViewData = new DirectoryViewData(CuyahogaContext, virtualPath, GetRootDataPath(), theDirectory.GetDirectories(), theDirectory.GetFiles());
			return directoryViewData;
		}

		private string GetRootDataPath()
		{
			return Url.Content(this._fileManagerService.GetRootDataPath());
		}

		private void AddDirectoriesForParentPath(IList directories, string parentPath, int level)
		{
			string physicalPath = Server.MapPath(parentPath);
			var subDirectories = new DirectoryInfo(physicalPath).GetDirectories();
			foreach (DirectoryInfo subDirectory in subDirectories)
			{
				string virtualPath = parentPath + subDirectory.Name + "/";
				directories.Add(new {Path = virtualPath, Name = subDirectory.Name, Level = level});
				AddDirectoriesForParentPath(directories, virtualPath, level + 1);
			}
		}
	}
}
