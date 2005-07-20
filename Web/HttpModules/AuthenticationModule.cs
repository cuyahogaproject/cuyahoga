using System;
using System.Web;
using System.Web.Security;

using log4net;

using Cuyahoga.Core;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Security;

namespace Cuyahoga.Web.HttpModules
{
	/// <summary>
	/// HttpModule to extend Forms Authentication.
	/// </summary>
	public class AuthenticationModule : IHttpModule
	{
		private const int AUTHENTICATION_TIMEOUT = 20;
		private static readonly ILog log = LogManager.GetLogger(typeof(AuthenticationModule));

		public AuthenticationModule()
		{
		}

		public void Init(HttpApplication context)
		{
			context.AuthenticateRequest += new EventHandler(Context_AuthenticateRequest);
		}

		public void Dispose()
		{
			// Nothing here	
		}

		/// <summary>
		/// Try to authenticate the user.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool AuthenticateUser(string username, string password, bool persistLogin)
		{
			CoreRepository cr = (CoreRepository)HttpContext.Current.Items["CoreRepository"];
			string hashedPassword = Encryption.StringToMD5Hash(password);
			try
			{
				User user = cr.GetUserByUsernameAndPassword(username, hashedPassword);
				if (user != null)
				{
					if (! user.IsActive)
					{
						log.Warn(String.Format("Inactive user {0} tried to login.", user.UserName));
						throw new AccessForbiddenException("The account is disabled.");
					}
					user.IsAuthenticated = true;
					string currentIp = HttpContext.Current.Request.UserHostAddress;
					user.LastLogin = DateTime.Now;
					user.LastIp = currentIp;
					// Save login date and IP
					cr.UpdateObject(user);
					// Create the authentication ticket
					HttpContext.Current.User = new CuyahogaPrincipal(user);
					FormsAuthentication.SetAuthCookie(user.Name, persistLogin);

					return true;
				}
				else
				{
					log.Warn(String.Format("Invalid username-password combination: {0}:{1}.", username, password));
					return false;
				}
			}
			catch (Exception ex)
			{
				log.Error(String.Format("An error occured while logging in user {0}.", username));
				throw new Exception(String.Format("Unable to log in user '{0}': " + ex.Message, username), ex);
			}
		}

		/// <summary>
		/// Log out the current user.
		/// </summary>
		public void Logout()
		{
			if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
			{
				FormsAuthentication.SignOut();
			}
		}

		private void Context_AuthenticateRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if (app.Context.User != null && app.Context.User.Identity.IsAuthenticated)
			{
				CoreRepository cr = (CoreRepository)HttpContext.Current.Items["CoreRepository"];
				// There is a logged-in user with a standard Forms Identity. Replace it with
				// the cached Cuyahoga identity (the User class implements IIdentity). 				
				int userId = Int32.Parse(app.Context.User.Identity.Name);
				User cuyahogaUser = (User)cr.GetObjectById(typeof(User), userId);
				cuyahogaUser.IsAuthenticated = true;
				app.Context.User = new CuyahogaPrincipal(cuyahogaUser);
			}
		}
	}
}
