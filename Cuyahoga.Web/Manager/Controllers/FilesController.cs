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
		private IFileService _fileService;

		public FilesController(IFileService fileService)
		{
			_fileService = fileService;
		}

		public ActionResult Index(string path)
		{
			if (String.IsNullOrEmpty(path))
			{
				path = GetRootDataPath();
			}
			return View(GetDirectoryViewDataForPath(path));
		}

		public ActionResult List(string path)
		{
			VerifyAccessRights(path);
			return PartialView(GetDirectoryViewDataForPath(path));
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
			var directoryViewData = new DirectoryViewData(virtualPath, GetRootDataPath(), theDirectory.GetDirectories(), theDirectory.GetFiles());
			return directoryViewData;
		}

		private string GetRootDataPath()
		{
			if (CuyahogaContext.CurrentUser.HasRight(Rights.AccessRootDataFolder))
			{
				return Url.Content("~/SiteData/");
			}
			return CuyahogaContext.CurrentSite.SiteDataDirectory + "UserFiles/";
		}
	}
}
