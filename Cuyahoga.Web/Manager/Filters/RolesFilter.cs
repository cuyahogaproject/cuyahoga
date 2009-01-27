using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Manager.Filters
{
	/// <summary>
	/// Loads all roles for the current site into the view data and also puts a filtered list into the view data that with roles that 
	/// have editor rights. 
	/// </summary>
	public class RolesFilter : ActionFilterAttribute
	{
		private readonly IUserService _userService;
		private readonly ICuyahogaContext _cuyahogaContext;

		/// <summary>
		/// Creates a new instance of the <see cref="RolesFilter"></see> class.
		/// </summary>
		public RolesFilter() : this(IoC.Resolve<IUserService>(), IoC.Resolve<ICuyahogaContext>())
		{}

		/// <summary>
		/// Creates a new instance of the <see cref="RolesFilter"></see> class.
		/// </summary>
		/// <param name="userService"></param>
		/// <param name="cuyahogaContext"></param>
		public RolesFilter(IUserService userService, ICuyahogaContext cuyahogaContext)
		{
			_userService = userService;
			_cuyahogaContext = cuyahogaContext;
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			// Load available roles for the current site and put these into the view data
			IList<Role> rolesForSite = this._userService.GetAllRolesBySite(this._cuyahogaContext.CurrentSite);
			filterContext.Controller.ViewData["AllRoles"] = rolesForSite;
			// Filter the list of roles to a list of roles that have editor rights.
			IList<Role> editorRoles = rolesForSite.Where(r => r.HasRight(Rights.Editor)).ToList();
			filterContext.Controller.ViewData["EditorRoles"] = editorRoles;
		}
	}
}
