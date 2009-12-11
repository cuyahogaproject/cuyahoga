using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Mvc.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.AccessAdmin)]
	public class CategoriesController : ManagerController
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService, IModelValidator<Category> modelValidator)
		{
			_categoryService = categoryService;
			this.ModelValidator = modelValidator;
		}

		public ActionResult Index()
		{
			return View(this._categoryService.GetAllCategories(CuyahogaContext.CurrentSite));
		}

		public ActionResult New()
		{
			SetupParentCategoriesViewData(null);
			return View(new Category());
		}

		public ActionResult Edit(int id)
		{
			Category currentCategory = this._categoryService.GetCategoryById(id);
			SetupParentCategoriesViewData(currentCategory);
			return View(currentCategory);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create([Bind(Exclude = "Id")]Category category, int? parentCategoryId)
		{
			category.Site = CuyahogaContext.CurrentSite;
			if (parentCategoryId.HasValue)
			{
				category.SetParentCategory(this._categoryService.GetCategoryById(parentCategoryId.Value));
			}
			else
			{
				category.SetParentCategory(null);
			}
			if (ValidateModel(category))
			{
				try
				{
					this._categoryService.CreateCategory(category);
					Messages.AddFlashMessageWithParams("CategoryCreatedMessage", category.Name);
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					Messages.AddException(ex);
				}
			}
			SetupParentCategoriesViewData(category);
			return View("New", category);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Update(int id, int? parentCategoryId)
		{
			Category category = this._categoryService.GetCategoryById(id);
			if (parentCategoryId.HasValue)
			{
				category.SetParentCategory(this._categoryService.GetCategoryById(parentCategoryId.Value));
			}
			else
			{
				category.SetParentCategory(null);
			}
			if (TryUpdateModel(category) && ValidateModel(category))
			{
				try
				{
					this._categoryService.UpdateCategory(category);
					Messages.AddFlashMessageWithParams("CategoryUpdatedMessage", category.Name);
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					Messages.AddException(ex);
				}
			}
			SetupParentCategoriesViewData(category);
			return View("Edit", category);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(int id)
		{
			Category category = this._categoryService.GetCategoryById(id);
			try
			{
				this._categoryService.DeleteCategory(category);
				Messages.AddFlashMessageWithParams("CategoryDeletedMessage", category.Name);
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Index");
		}

		private void SetupParentCategoriesViewData(Category currentCategory)
		{
			IList<Category> parentCategories = this._categoryService.GetAllCategories(CuyahogaContext.CurrentSite).ToList();
			if (currentCategory != null)
			{
				// Remove current category from available parent categories
				parentCategories.Remove(currentCategory);
			}
			ViewData["ParentCategories"] = parentCategories;
		}
	}
}
