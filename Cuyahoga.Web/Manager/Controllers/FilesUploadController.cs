using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Core.Service.Files;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc.Controllers;
using Cuyahoga.Web.Mvc.Filters;
using Cuyahoga.Web.Mvc.Security;

namespace Cuyahoga.Web.Manager.Controllers
{
	/// <summary>
	/// Special controller for uploading files with the  Flash upload control (swfupload) that doesn't use 
	/// the credentials of the current user.
	/// </summary>
	[FlashCompatibleAuthorize]
	public class FilesUploadController : BaseController
	{
		private readonly IFileManagerService _fileManagerService;

		/// <summary>
		/// Creates a new instance of the <see cref="FilesUploadController"/> class.
		/// </summary>
		/// <param name="fileManagerService"></param>
		public FilesUploadController(IFileManagerService fileManagerService)
		{
			_fileManagerService = fileManagerService;
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[PermissionFilter(RequiredRights = Rights.ManageFiles)]
		public ActionResult UploadAjax(string uploadpath, HttpPostedFileBase filedata)
		{
			AjaxMessageViewData ajaxResult = new AjaxMessageViewData();
			if (filedata != null)
			{
				try
				{
					string physicalDataDirectory = Server.MapPath(uploadpath);
					string physicalFilePath = Path.Combine(physicalDataDirectory, filedata.FileName);
					string fileNameAfterUpload = this._fileManagerService.UploadFile(physicalFilePath, filedata.InputStream);
					ajaxResult.Message = String.Format(GetText("FileUploadSuccessMessage"), fileNameAfterUpload);
				}
				catch (Exception ex)
				{
					ajaxResult.Error = ex.Message;
				}
			}
			return Json(ajaxResult);
		}

	}
}
