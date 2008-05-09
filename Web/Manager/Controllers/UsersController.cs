using System;
using System.Collections;
using System.Collections.Specialized;
using Castle.MonoRail.Framework.Helpers;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Manager.Controllers
{
	public class UsersController : SecureController
	{
		private readonly ICommonDao _commonDao;
		private readonly IUserService _userService;
		private const int pageSize = 2;

		public UsersController(ICommonDao commonDao, IUserService userService)
		{
			this._commonDao = commonDao;
			this._userService = userService;
		}

		public void Index(string userName, int? roleId, bool? isActive, int? page)
		{
			int currentPage = page.HasValue ? page.Value : 1;
			PropertyBag["roles"] = this._commonDao.GetAll<Role>("Name");
			PagedResultSet<User> usersResult = this._userService.FindUsers(userName, roleId, isActive, pageSize, page);
			PropertyBag["users"] =
				PaginationHelper.CreateCustomPage(usersResult.Results, pageSize, currentPage, usersResult.TotalCount);

			NameValueCollection parameters = new NameValueCollection();
			if (!String.IsNullOrEmpty(userName)) parameters.Add("username", userName);
			if (roleId.HasValue) parameters.Add("roleid", roleId.ToString());
			if (isActive.HasValue) parameters.Add("isactive", isActive.ToString().ToLower());

			string pageUrl = UrlBuilder.BuildUrl(Context.UrlInfo, "users", "index", parameters);
			if (pageUrl.EndsWith("&")) pageUrl = pageUrl.Substring(0, pageUrl.Length - 1);
			PropertyBag["pageurl"] = pageUrl;
		}
	}
}