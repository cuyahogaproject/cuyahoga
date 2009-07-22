using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Web.Mvc.Controllers;

namespace Cuyahoga.Web.Modules.Shared.Controllers
{
	public class CategoriesController : BaseController
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public ActionResult GetAll()
		{
			IEnumerable<Category> categories = this._categoryService.GetAllCategories(CuyahogaContext.CurrentSite);
			var displayCategories = from category in categories
			                        select new
			                               	{
			                               		category.Id,
			                               		category.Name,
			                               		category.Description,
			                               		category.Level
			                               	};
			return Json(displayCategories);
		}
	}
}
