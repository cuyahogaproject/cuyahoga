using System;
using Castle.MonoRail.Framework.Helpers;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Controllers
{
	public class UsersController : SecureController
	{
		private readonly ICommonDao _commonDao;

		public UsersController(ICommonDao commonDao)
		{
			this._commonDao = commonDao;
		}

		public void Index()
		{
			PropertyBag["roles"] = this._commonDao.GetAll<Role>("Name");
			PropertyBag["users"] = PaginationHelper.CreatePagination(this, this._commonDao.GetAll<User>("UserName"), 2);
		}

		public void Filter(string userName, int roleId, string isActive)
		{
			PropertyBag["roles"] = this._commonDao.GetAll<Role>("Name");
			PropertyBag["users"] = PaginationHelper.CreatePagination(this, this._commonDao.GetAll<User>("UserName"), 2);
			RenderView("index");
		}
	}
}