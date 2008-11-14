using System;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Security;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Manager.Controllers;
using Resources.Cuyahoga.Web.Manager;
using LoginViewData=Cuyahoga.Web.Manager.Model.ViewModels.LoginViewData;

namespace Cuyahoga.Web.Manager.Controllers
{
	public class LoginController : BaseController
	{
		private readonly IAuthenticationService _authenticationService;

		/// <summary>
		/// Create and initialize an instance of the LoginController class.
		/// </summary>
		/// <param name="authenticationService"></param>
		public LoginController(IAuthenticationService authenticationService)
		{
			this._authenticationService = authenticationService;
		}

		[AcceptVerbs("GET")]
		public ActionResult Index(string returnUrl)
		{
			ViewData["Title"] = GlobalResources.LoginPageTitle;
			ViewData["ReturnUrl"] = returnUrl;
			return View(new LoginViewData());
		}

		[AcceptVerbs("POST")]
		public ActionResult Login(string returnUrl)
		{
			var loginUser = new LoginViewData();
			try
			{
				if (TryUpdateModel(loginUser, new[] { "Username", "Password" }) && ValidateModel(loginUser))
				{
					User user = this._authenticationService.AuthenticateUser(loginUser.Username, loginUser.Password, Request.UserHostAddress);

					FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
					if (!String.IsNullOrEmpty(returnUrl))
					{
						return Redirect(returnUrl);
					}
					return RedirectToAction("Index", "Dashboard");
				}
			}
			catch (AuthenticationException ex)
			{
				Logger.WarnFormat("User {0} unsuccesfully logged in with password {1}.", loginUser.Username, loginUser.Password);
				ShowException(ex);
			}
			catch (Exception ex)
			{
				Logger.Error("Unexpected error while logging in", ex);
				ShowException(ex);
			}
			ViewData["Title"] = GlobalResources.LoginPageTitle;
			return View("Index", loginUser);
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index");
		}
	}
}