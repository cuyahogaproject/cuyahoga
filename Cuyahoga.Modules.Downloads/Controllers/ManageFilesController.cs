using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Service.Files;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Validation.ModelValidators;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Mvc.Controllers;

namespace Cuyahoga.Modules.Downloads.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	public class ManageFilesController : ModuleAdminController
	{
		private readonly IContentItemService<FileResource> _contentItemService;
		private readonly IFileResourceService _fileResourceService;
		private readonly IUserService _userService;
		private readonly ICategoryService _categoryService;
		private readonly ModuleLoader _moduleLoader;
		private DownloadsModule _module;

		/// <summary>
		/// Creates a new instance of the <see cref="ManageFilesController"/> class.
		/// </summary>
		/// <param name="contentItemService"></param>
		/// <param name="fileResourceService"></param>
		/// <param name="userService"></param>
		/// <param name="categoryService"></param>
		/// <param name="moduleLoader"></param>
		/// <param name="fileResourceModelValidator"></param>
		public ManageFilesController(IContentItemService<FileResource> contentItemService, IFileResourceService fileResourceService, IUserService userService, ICategoryService categoryService, ModuleLoader moduleLoader, FileResourceModelValidator fileResourceModelValidator)
		{
			_contentItemService = contentItemService;
			_fileResourceService = fileResourceService;
			_userService = userService;
			_categoryService = categoryService;
			_moduleLoader = moduleLoader;
			this.ModelValidator = fileResourceModelValidator;
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
			this._module = this._moduleLoader.GetModuleFromSection<DownloadsModule>(CurrentSection);
		}

		/// <summary>
		/// List files for the current section.
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			IEnumerable<FileResource> files = this._contentItemService.FindContentItemsBySection(CurrentSection,
																						 new ContentItemQuerySettings(
																							ContentItemSortBy.PublishedAt,
																							ContentItemSortDirection.DESC));
			return View(GetModuleAdminViewModel(files));
		}

		/// <summary>
		/// Show form for new file.
		/// </summary>
		/// <returns></returns>
		public ActionResult New()
		{
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			return View("New", GetModuleAdminViewModel(new FileResource()));
		}

		/// <summary>
		/// Show form for existing item.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Edit(long id)
		{
			FileResource fileResource = this._contentItemService.GetById(id);
			return View("Edit", GetModuleAdminViewModel(fileResource));
		}

		/// <summary>
		/// Save a new file.
		/// </summary>
		/// <param name="fileResource"></param>
		/// <param name="fileData"></param>
		/// <param name="roleIds"></param>
		/// <param name="categoryids"></param>
		/// <returns></returns>
		public ActionResult Create([Bind(Exclude = "Id")]FileResource fileResource, HttpPostedFileBase fileData, int[] roleIds, string categoryids)
		{
			if (fileData != null && fileData.ContentLength > 0)
			{	
				// TODO: handle Categories etc. more generic.
				// Categories
				fileResource.Categories.Clear();
				if (!String.IsNullOrEmpty(categoryids))
				{
					foreach (string categoryIdString in categoryids.Split(','))
					{
						fileResource.Categories.Add(this._categoryService.GetCategoryById(Convert.ToInt32(categoryIdString)));
					}
				}
				// TODO: handle Roles etc. more generic.
				fileResource.ContentItemPermissions.Clear();
				if (roleIds != null)
				{
					var roles = this._userService.GetRolesByIds(roleIds);
					foreach (Role role in roles)
					{
						fileResource.ContentItemPermissions.Add(new ContentItemPermission
						                                        	{ContentItem = fileResource, Role = role, ViewAllowed = true});
					}
				}

				fileResource.Section = CurrentSection;
				fileResource.FileName = fileData.FileName;
				fileResource.Length = fileData.ContentLength;
				fileResource.MimeType = fileData.ContentType;
				if (String.IsNullOrEmpty(fileResource.Title))
				{
					fileResource.Title = fileResource.FileName;
				}

				if (ValidateModel(fileResource, new[] { "Title", "Summary", "FileName", "PublishedAt", "PublishedUntil" }))
				{
					try
					{
						this._fileResourceService.SaveFileResource(fileResource, this._module.FileDir, fileData.InputStream);
						Messages.AddFlashMessage(GetText("FileSavedMessage"));
						return RedirectToAction("Index", "ManageFiles", GetNodeAndSectionParams());
					}
					catch (Exception ex)
					{
						Messages.AddException(ex);
					}
				}

			}
			else
			{
				Messages.AddErrorMessage(GetText("NoFileUploadedMessage"));
			}
			return New();
		}
	}
}
