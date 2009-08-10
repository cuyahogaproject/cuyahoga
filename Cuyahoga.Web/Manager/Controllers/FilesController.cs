using System;
using System.IO;
using System.Web.Mvc;
using Cuyahoga.Core;
using Cuyahoga.Core.Service.Files;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageFiles)]
	public class FilesController : ManagerController
	{
		private readonly IFileService _fileService;

		public FilesController(IFileService fileService)
		{
			_fileService = fileService;
		}

		public ActionResult Index(string path)
		{
			if (String.IsNullOrEmpty(path))
			{
				path = Url.Content(CuyahogaContext.CurrentSite.SiteDataDirectory + "UserFiles/");
			}
			return View(GetDirectoryViewDataForPath(path));
		}

		public ActionResult List(string path)
		{
			VerifyAccessRights(path);
			return PartialView(GetDirectoryViewDataForPath(path));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[PermissionFilter(RequiredRights = Rights.CreateDirectory)]
		public ActionResult CreateDirectory(string name, string parentPath)
		{
			VerifyAccessRights(parentPath);

			string physicalDirectory = Server.MapPath(parentPath + name);
			try
			{
				this._fileService.CreateDirectory(physicalDirectory);
				return RedirectToAction("List", new { Path = parentPath + name + "/" });
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
				return RedirectToAction("List", new {Path = parentPath});
			}
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
			return Url.Content(this._fileService.GetRootDataPath());
		}
	}
}
