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
			FileResource fileResource = new FileResource();
			// For new items, inherit the permissions of the section
			foreach (SectionPermission sectionPermission in CurrentSection.SectionPermissions)
			{
				fileResource.ContentItemPermissions.Add(new ContentItemPermission() { ContentItem = fileResource, Role = sectionPermission.Role, ViewAllowed = sectionPermission.ViewAllowed });
			}
			return View("New", GetModuleAdminViewModel(fileResource));
		}

		/// <summary>
		/// Show form for existing item.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Edit(long id)
		{
			FileResource fileResource = this._contentItemService.GetById(id);
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
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
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create([Bind(Exclude = "Id")]FileResource fileResource, HttpPostedFileBase fileData, int[] roleIds, string categoryids)
		{
			BindCategoriesAndRoles(fileResource, categoryids, roleIds);

			if (fileData != null && fileData.ContentLength > 0)
			{	
				fileResource.Section = CurrentSection;
				fileResource.FileName = fileData.FileName;
				fileResource.Length = fileData.ContentLength;
				fileResource.MimeType = fileData.ContentType;

				if (ValidateModel(fileResource, new[] { "Title", "Summary", "FileName", "PublishedAt", "PublishedUntil" }))
				{
					try
					{
						this._fileResourceService.SaveFileResource(fileResource, this._module.FileDir, fileData.InputStream);
						Messages.AddFlashMessage("FileSavedMessage");
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
				Messages.AddErrorMessage("NoFileUploadedMessage");
			}
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			return View("New", GetModuleAdminViewModel(fileResource));
		}

		/// <summary>
		/// Update an existing file.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="fileData"></param>
		/// <param name="roleIds"></param>
		/// <param name="categoryids"></param>
		/// <returns></returns>
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Update(int id, HttpPostedFileBase fileData, int[] roleIds, string categoryids)
		{
			FileResource fileResource = this._contentItemService.GetById(id);
			
			BindCategoriesAndRoles(fileResource, categoryids, roleIds);

			if (TryUpdateModel(fileResource, new[] { "Title", "Summary", "PublishedAt", "PublishedUntil" })
				&& ValidateModel(fileResource, new[] { "Title", "Summary", "PublishedAt", "PublishedUntil" }))
			{
				try
				{
					if (fileData != null && fileData.ContentLength > 0)
					{
						fileResource.FileName = fileData.FileName;
						fileResource.Length = fileData.ContentLength;
						fileResource.MimeType = fileData.ContentType;
						this._fileResourceService.SaveFileResource(fileResource, this._module.FileDir, fileData.InputStream);
					}
					else
					{
						this._fileResourceService.UpdateFileResource(fileResource);
					}
					Messages.AddFlashMessage("FileSavedMessage");
					return RedirectToAction("Index", "ManageFiles", GetNodeAndSectionParams());
				}
				catch (Exception ex)
				{
					Messages.AddException(ex);
				}
			}

			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			return View("Edit", GetModuleAdminViewModel(fileResource));
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(int id)
		{
			FileResource fileResource = this._contentItemService.GetById(id);
			try
			{
				this._fileResourceService.DeleteFileResource(fileResource, this._module.FileDir);
				Messages.AddFlashMessage("FileDeletedMessage");
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Index", "ManageFiles", GetNodeAndSectionParams());
		}

		private void BindCategoriesAndRoles(FileResource fileResource, string categoryids, int[] roleIds)
		{
			// TODO: handle more generic (Binder? base Controller?)
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
					fileResource.ContentItemPermissions.Add(new ContentItemPermission { ContentItem = fileResource, Role = role, ViewAllowed = true });
				}
			}
		}
	}
}
