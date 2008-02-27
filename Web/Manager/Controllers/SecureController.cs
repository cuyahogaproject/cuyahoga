using System.Reflection;
using System.Security;
using Castle.MonoRail.Framework;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Manager.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	/// <summary>
	/// Base controller for all controllers that need some kind of authentication/authorization.
	/// </summary>
	[Filter(ExecuteEnum.BeforeAction, typeof(AuthenticationFilter), ExecutionOrder = 1)]
	[Rescue("SecurityError", typeof(SecurityException))]
	public abstract class SecureController : BaseController
	{
		private ICuyahogaContext _cuyahogaContext;

		/// <summary>
		/// Sets the Cuyahoga context.
		/// </summary>
		public ICuyahogaContext CuyahogaContext
		{
			set { this._cuyahogaContext = value; }
		}

		/// <summary>
		/// The Cuyahoga user that executes the request.
		/// </summary>
		public User CurrentUser
		{
			get
			{
				return this._cuyahogaContext.CurrentUser;
			}
		}

		protected override void InvokeMethod(MethodInfo method, IRequest request, System.Collections.IDictionary actionArgs)
		{
			string actionName = method.Name;
			object[] attributes = method.GetCustomAttributes(typeof(CuyahogaPermissionAttribute), true);
			if (attributes.Length > 0)
			{
				CuyahogaPermissionAttribute permissionAttribute = (CuyahogaPermissionAttribute)attributes[0];

				// Permission attribute found, check if the user has one of the required rights.
				CheckPermission(actionName, permissionAttribute);
			}
			else
			{
				// Permission not found, check if there is an attribute on controller level
				object[] classAttributes = this.GetType().GetCustomAttributes(typeof(CuyahogaPermissionAttribute), true);
				if (classAttributes.Length > 0)
				{
					CuyahogaPermissionAttribute permissionAttribute = (CuyahogaPermissionAttribute)classAttributes[0];
					CheckPermission(actionName, permissionAttribute);
				}
			}
			base.InvokeMethod(method, request, actionArgs);
		}

		private void CheckPermission(string actionName, CuyahogaPermissionAttribute permissionAttribute)
		{
			bool isAllowed = true;

			foreach (string right in permissionAttribute.RightsArray)
			{
				if (!CurrentUser.HasRight(right))
				{
					isAllowed = false;
				}
			}
			if (!isAllowed)
			{
				if (Logger.IsErrorEnabled)
				{
					Logger.ErrorFormat("User '{0}' is not allowed to perform the action '{1}' that requires right(s) '{2}'.", CurrentUser.UserName, actionName, permissionAttribute.RequiredRights);
				}
				throw new SecurityException("ActionNotAllowedException");
			}
		}
	}
}
