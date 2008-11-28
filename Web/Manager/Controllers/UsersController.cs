using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Validation.ModelValidators;
using Cuyahoga.Web.Manager.Filters;
using Cuyahoga.Web.Mvc.Paging;
using Resources.Cuyahoga.Web.Manager;
using CuyahogaUser = Cuyahoga.Core.Domain.User;
using CuyahogaSite = Cuyahoga.Core.Domain.Site;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageUsers)]
	public class UsersController : SecureController
	{
		private readonly IUserService _userService;
		private readonly RoleModelValidator _roleModelValidator;
		private const int pageSize = 20;

		public UsersController(IUserService userService, UserModelValidator userModelValidator, RoleModelValidator roleModelValidator)
		{
			this._userService = userService;
			this.ModelValidator = userModelValidator;
			this._roleModelValidator = roleModelValidator;
		}

		public ActionResult Index(int? page)
		{
			return Browse(null, null, null, null, page);
		}

		public ActionResult Browse(string username, int? roleId, bool? isActive, bool? globalSearch, int? page)
		{
			ViewData["Title"] = GlobalResources.ManageUsersPageTitle;

			ViewData["username"] = username;
			ViewData["roles"] = new SelectList(this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite), "Id", "Name", roleId);
			ViewData["roleid"] = roleId;
			IDictionary<bool, string> isActiveOptions = new Dictionary<bool, string>() { { true, GlobalResources.Yes }, { false, GlobalResources.No } };
			ViewData["isactiveoptions"] = new SelectList(isActiveOptions, "Key", "Value", isActive);
			ViewData["isactive"] = isActive;
			ViewData["globalsearchallowed"] = CuyahogaContext.CurrentUser.HasRight(Rights.GlobalPermissions);
			ViewData["globalsearch"] = globalSearch;

			int totalCount;
			CuyahogaSite siteToFilter = globalSearch.HasValue && globalSearch.Value == true ? null : CuyahogaContext.CurrentSite;
			IList<User> users = _userService.FindUsers(username, roleId, isActive, siteToFilter, pageSize, page, out totalCount);
			return View("Index", new PagedList<User>(users, page.HasValue ? page.Value -1 : 0, pageSize, totalCount));
		}

		public ActionResult New()
		{
			ViewData["Title"] = GlobalResources.NewUserPageTitle;
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			User user = new User();
			ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
			return View("NewUser", user);
		}

		public ActionResult Edit(int id)
		{
			ViewData["Title"] = GlobalResources.EditUserPageTitle;
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			User user = this._userService.GetUserById(id);
			ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
			return View("EditUser", user);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(int[] roleIds)
		{
			User newUser = new User();
			try
			{
				UpdateModel(newUser, new []{ "UserName", "FirstName", "LastName", "Email", "Website", "IsActive", "TimeZone"});
				newUser.Password = CuyahogaUser.HashPassword(Request.Form["Password"]);
				newUser.PasswordConfirmation = CuyahogaUser.HashPassword(Request.Form["PasswordConfirmation"]);
				if (roleIds.Length > 0)
				{
					IList<Role> roles = this._userService.GetRolesByIds(roleIds);
					foreach (Role role in roles)
					{
						newUser.Roles.Add(role);
					}
				}

				if (ValidateModel(newUser))
				{
					this._userService.CreateUser(newUser);
					ShowMessage(String.Format(GlobalResources.UserCreatedMessage, newUser.UserName), true);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			ViewData["Title"] = GlobalResources.NewUserPageTitle;
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", newUser.TimeZone);
			return View("NewUser", newUser);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Update(int id, int[] roleIds)
		{
			User user = this._userService.GetUserById(id);
			user.Roles.Clear();
			try
			{
				UpdateModel(user, new[] { "UserName", "FirstName", "LastName", "Email", "Website", "IsActive", "TimeZone" });
				
				if (roleIds.Length > 0)
				{
					IList<Role> roles = this._userService.GetRolesByIds(roleIds);
					foreach (Role role in roles)
					{
						user.Roles.Add(role);
					}
				}

				if (ValidateModel(user, new[] { "FirstName", "LastName", "Email", "Website", "Roles" }))
				{
					this._userService.UpdateUser(user);
					ShowMessage(String.Format(GlobalResources.UserUpdatedMessage, user.UserName), true);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			ViewData["Title"] = GlobalResources.EditUserPageTitle;
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
			return View("EditUser", user);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ChangePassword(int id, string password, string passwordConfirmation)
		{
			User user = this._userService.GetUserById(id);
			try
			{
				user.Password = CuyahogaUser.HashPassword(password);
				user.PasswordConfirmation = CuyahogaUser.HashPassword(passwordConfirmation);

				if (ValidateModel(user, new[] { "Password", "PasswordConfirmation" }))
				{
					this._userService.UpdateUser(user);
					ShowMessage(GlobalResources.PasswordChangedMessage);
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			ViewData["Title"] = GlobalResources.EditUserPageTitle;
			ViewData["Roles"] = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			ViewData["TimeZones"] = new SelectList(TimeZoneUtil.GetTimeZones(), "Key", "Value", user.TimeZone);
			return View("EditUser", user);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(int id)
		{
			User user = this._userService.GetUserById(id);
			try
			{
				this._userService.DeleteUser(user);
				ShowMessage(String.Format(GlobalResources.UserDeletedMessage, user.UserName), true);
			}
			catch(Exception ex)
			{
				ShowException(ex, true);
			}
			return RedirectToAction("Index");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CheckUsernameAvailability(string userName)
		{
			bool userExists = false;
			if (! String.IsNullOrEmpty(userName) && userName.Length > 3)
			{
				User user = this._userService.GetUserByUserName(userName);
				userExists = user != null;
			}
			return Json(!userExists);
		}

		public ActionResult Roles()
		{
			ViewData["Title"] = GlobalResources.ManageRolesPageTitle;
			IList<Role> roles = this._userService.GetAllRolesBySite(CuyahogaContext.CurrentSite);
			return View("Roles", roles);
		}

		public ActionResult NewRole()
		{
			ViewData["Title"] = GlobalResources.NewRolePageTitle;
			ViewData["Rights"] = this._userService.GetAllRights();
			Role role = new Role();
			return View(role);
		}

		public ActionResult EditRole(int id)
		{
			ViewData["Title"] = GlobalResources.EditRolePageTitle;
			ViewData["Rights"] = this._userService.GetAllRights();
			Role role = this._userService.GetRoleById(id);
			return View(role);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CreateRole(int[] rightIds)
		{
			Role newRole = new Role();
			try
			{
				UpdateModel(newRole, new[] { "Name", "IsGlobal" });
				if (rightIds.Length > 0)
				{
					IList<Right> rights = this._userService.GetRightsByIds(rightIds);
					foreach (Right right in rights)
					{
						newRole.Rights.Add(right);
					}
				}

				if (ValidateModel(newRole, this._roleModelValidator, new [] { "Name" }))
				{
					this._userService.CreateRole(newRole, CuyahogaContext.CurrentSite);
					ShowMessage(String.Format(GlobalResources.RoleCreatedMessage, newRole.Name), true);
					return RedirectToAction("roles");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			ViewData["Title"] = GlobalResources.NewRolePageTitle;
			ViewData["Rights"] = this._userService.GetAllRights();
			return View("NewRole", newRole);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UpdateRole(int id, int[] rightIds)
		{
			Role role = this._userService.GetRoleById(id);
			role.Rights.Clear();
			try
			{
				UpdateModel(role, new[] { "Name", "IsGlobal" });

				if (rightIds.Length > 0)
				{
					IList<Right> rights = this._userService.GetRightsByIds(rightIds);
					foreach (Right right in rights)
					{
						role.Rights.Add(right);
					}
				}

				if (ValidateModel(role, this._roleModelValidator, new[] { "Name" }))
				{
					this._userService.UpdateRole(role, CuyahogaContext.CurrentSite);
					ShowMessage(String.Format(GlobalResources.RoleUpdatedMessage, role.Name), true);
					return RedirectToAction("Roles");
				}
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
			ViewData["Title"] = GlobalResources.EditRolePageTitle;
			ViewData["Rights"] = this._userService.GetAllRights();
			return View("EditRole", role);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DeleteRole(int id)
		{
			Role role = this._userService.GetRoleById(id);
			try
			{
				this._userService.DeleteRole(role);
				ShowMessage(String.Format(GlobalResources.RoleDeletedMessage, role.Name), true);
			}
			catch (Exception ex)
			{
				ShowException(ex, true);
			}
			return RedirectToAction("Roles");
		}
	}
}
