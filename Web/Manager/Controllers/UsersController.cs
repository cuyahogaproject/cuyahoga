using System.Collections.Generic;
using System.Web.Mvc;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Manager.Filters;
using Cuyahoga.Web.Mvc.Paging;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageUsers)]
	public class UsersController : SecureController
	{
		private readonly ICommonDao _commonDao;
		private readonly IUserService _userService;
		private const int pageSize = 20;

		public UsersController(ICommonDao commonDao, IUserService userService)
		{
			this._commonDao = commonDao;
			this._userService = userService;
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Index(int? page)
		{
			return Browse(null, null, null, null, page);
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Browse(string username, int? roleId, bool? isActive, bool? globalSearch, int? page)
		{
			ViewData["Title"] = GlobalResources.ManageUsersPageTitle;

			ViewData["username"] = username;
			ViewData["roles"] = new SelectList(_userService.GetAllRoles(), "Id", "Name", roleId);
			ViewData["roleid"] = roleId;
			IDictionary<bool, string> isActiveOptions = new Dictionary<bool, string>() { { true, GlobalResources.Yes }, { false, GlobalResources.No } };
			ViewData["isactiveoptions"] = new SelectList(isActiveOptions, "Key", "Value", isActive);
			ViewData["isactive"] = isActive;
			ViewData["globalsearchallowed"] = CuyahogaContext.CurrentUser.HasRight(Rights.GlobalPermissions);
			ViewData["globalsearch"] = globalSearch;

			int totalCount;
			Site siteToFilter = globalSearch.HasValue && globalSearch.Value == true ? null : CuyahogaContext.CurrentSite;
			IList<User> users = _userService.FindUsers(username, roleId, isActive, siteToFilter, pageSize, page, out totalCount);
			return View("Index", new PagedList<User>(users, page.HasValue ? page.Value -1 : 0, pageSize, totalCount));
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult New()
		{
			ViewData["Title"] = GlobalResources.NewUserPageTitle;
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			User user = new User();
			ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
			return View("Edit", user);
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Edit(int id)
		{
			ViewData["Title"] = GlobalResources.EditUserPageTitle;
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			User user = this._userService.GetUserById(id);
			ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
			return View("Edit", user);
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Roles()
		{
			ViewData["Title"] = GlobalResources.ManageRolesPageTitle;
			return View("Roles");
		}
	}
}
