using System;
using System.Collections.Specialized;
using System.Web.Security;
using Castle.MonoRail.Framework;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Components;
using log4net;

namespace Cuyahoga.Web.Manager.Filters
{
	/// <summary>
	/// MonoRail Authentication filter
	/// </summary>
	public class AuthenticationFilter : Filter
	{
		private static readonly ILog logger = LogManager.GetLogger(typeof (AuthenticationFilter));
		private IUserService _userService;
		private ICuyahogaContext _cuyahogaContext;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="userService">User Service (injected)</param>
		/// <param name="cuyahogaContext">Cuyahoga context</param>
		public AuthenticationFilter(IUserService userService, ICuyahogaContext cuyahogaContext)
		{
			this._userService = userService;
			this._cuyahogaContext = cuyahogaContext;
		}

		protected override bool OnBeforeAction(IRailsEngineContext context, Controller controller)
		{
			try
			{
				// Read Forms authentication cookie to see if there is a logged-in user.
				string cookieContent = context.Request.ReadCookie(FormsAuthentication.FormsCookieName);

				if (cookieContent == null)
				{
					// No user, redirect to login page.
					SendToLoginPage(controller);
					return false;
				}

				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieContent);

				// Read user and put it in the context.
				int userId = Convert.ToInt32(ticket.Name);
				User user = this._userService.GetUserById(userId);
				context.CurrentUser = user;
				this._cuyahogaContext.SetUser(user);

				return true;
			}
			catch (Exception ex)
			{
				logger.Error("Unexpected error performing authentication", ex);
			}

			SendToLoginPage(controller);
			return false;
		}

		private void SendToLoginPage(Controller controller)
		{
			NameValueCollection dictParams = new NameValueCollection();
			dictParams["returnurl"] = controller.Context.Request.RawUrl;

			controller.Redirect("login", "index", dictParams);
		}
	}
}
