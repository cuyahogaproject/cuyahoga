using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Mvc.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.AccessAdmin)]
	public class CategoriesController : ManagerController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
