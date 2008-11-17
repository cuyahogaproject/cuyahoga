using System.Collections.Generic;
using System.Web.Mvc;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
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

		public ActionResult Index(int? page)
		{
			return Browse(null, null, null, page);
		}

		public ActionResult Browse(string username, int? roleId, bool? isActive, int? page)
		{
			ViewData["Title"] = GlobalResources.ManageUsersPageTitle;

			ViewData["username"] = username;
			ViewData["roles"] = new SelectList(_userService.GetAllRoles(), "Id", "Name", roleId);
			ViewData["roleid"] = roleId;
			IDictionary<bool, string> isActiveOptions = new Dictionary<bool, string>() { { true, GlobalResources.Yes }, { false, GlobalResources.No } };
			ViewData["isactiveoptions"] = new SelectList(isActiveOptions, "Key", "Value", isActive);
			ViewData["isactive"] = isActive;

			int totalCount;
			IList<User> users = _userService.FindUsers(username, roleId, isActive, CuyahogaContext.CurrentSite.Id, pageSize, page, out totalCount);
			return View("Index", new PagedList<User>(users, page.HasValue ? page.Value -1 : 0, pageSize, totalCount));
		}
	}
}
