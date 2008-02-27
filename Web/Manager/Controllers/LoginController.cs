using System;
using System.Security.Authentication;
using System.Web.Security;
using Castle.MonoRail.Framework;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Components.MonoRail;

namespace Cuyahoga.Web.Manager.Controllers
{
	/// <summary>
	/// Controller for login-related actions.
	/// </summary>
	[Layout("Simple")]
	public class LoginController : BaseController
	{
		private IAuthenticationService authenticationService;

		/// <summary>
		/// Create and initialize an instance of the LoginController class.
		/// </summary>
		/// <param name="authenticationService"></param>
		public LoginController(IAuthenticationService authenticationService)
		{
			this.authenticationService = authenticationService;
		}

		public void Index()
		{
		}

		public void Login(string username, string password, string returnUrl)
		{
			try
			{
				User user = this.authenticationService.AuthenticateUser(username, password, Request.UserHostAddress);
				
				FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
				if (returnUrl != null)
				{
					Redirect(returnUrl);
				}
				else
				{
					Redirect("dashboard", "index");
				}
				
				return;
			}
			catch (AuthenticationException ex)
			{
				Flash[MessageType.Exception] = ex;
				RedirectToAction("index", "returnUrl=" + returnUrl);
			}
			catch (Exception ex)
			{
				if (Logger.IsErrorEnabled)
				{
					Logger.Error("Error performing log in", ex);
				}

				Flash[MessageType.Exception] = ex;
				RedirectToAction("index", "returnUrl=" + returnUrl);
			}
		}

		public void Logout()
		{
			FormsAuthentication.SignOut();
			Redirect("dashboard", "index");
		}
	}
}
