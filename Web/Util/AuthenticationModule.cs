using System;
using System.Web;
using System.Web.Security;
using System.Web.Caching;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DAL;
using Cuyahoga.Core.Security;

namespace Cuyahoga.Web.Util
{
	/// <summary>
	/// HttpModule to extend Forms Authentication. When a user logs in, the profile is loaded and put 
	/// in the cache with a simple key containing 'USER_' + userId.
	/// TODO: move non-httpmodule methods to another class?
	/// </summary>
	public class AuthenticationModule : IHttpModule
	{
		private const string USER_CACHE_PREFIX = "User_";
		private const int AUTHENTICATION_TIMEOUT = 20;

		public AuthenticationModule()
		{
		}

		public void Init(HttpApplication context)
		{
			context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
		}

		public void Dispose()
		{
			// Nothing here	
		}

		/// <summary>
		/// Try to authenticate the user. If authentication succeeds, an instance of the Cuyahoga user is 
		/// cached for future usage.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool AuthenticateUser(string username, string password)
		{
			string currentIp = HttpContext.Current.Request.UserHostAddress;
			User user = new User();
			// Set the user properties we already know.
			user.UserName = username;
			user.Password = password;
			user.LastLogin = DateTime.Now;
			user.LastIp = currentIp;

			if (user.Login())
			{
				// We also need the roles, so get them
				CmsDataFactory.GetInstance().GetRolesByUser(user);
				// Create the authentication ticket
				HttpContext.Current.User = new CuyahogaPrincipal(user);
				FormsAuthenticationTicket ticket = 
					new FormsAuthenticationTicket(1, user.Name, DateTime.Now, DateTime.Now.AddMinutes(AUTHENTICATION_TIMEOUT), false, "");
				string cookiestr = FormsAuthentication.Encrypt(ticket);
				HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);                
				HttpContext.Current.Response.Cookies.Add(cookie);
				// Finally cache the user
				CacheUser(HttpContext.Current, user);
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Log out the current user and remove the instance from the cache.
		/// </summary>
		public void Logout()
		{
			if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
			{
				string cacheIdentifier = USER_CACHE_PREFIX + HttpContext.Current.User.Identity.Name;
				HttpContext.Current.Cache.Remove(cacheIdentifier);
				FormsAuthentication.SignOut();
			}
		}

		private void CacheUser(HttpContext context, User user)
		{
			string cacheIdentifier = USER_CACHE_PREFIX + user.Id.ToString();
			context.Cache.Insert(cacheIdentifier, user, null, DateTime.MaxValue, new TimeSpan(0, AUTHENTICATION_TIMEOUT, 0));
		}

		private void context_AuthenticateRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if (app.Context.User != null && app.Context.User.Identity.IsAuthenticated)
			{
				// There is a logged-in user with a standard Forms Identity. Replace it with
				// the cached Cuyahoga identity (the User class implements IIdentity). 
				string cacheIdentifier = USER_CACHE_PREFIX + app.Context.User.Identity.Name;
				if (app.Context.Cache[cacheIdentifier] == null)
				{
					// For some reason the user is still logged-in but the cuyahoga User instance was removed 
					// from the cache. Fetch a new instance and cache it.
					int userId = Int32.Parse(app.Context.User.Identity.Name);
					User user = new User(userId);
					CacheUser(app.Context, user);
				}
				User cuyahogaUser = (User)app.Context.Cache[cacheIdentifier];
				app.Context.User = new CuyahogaPrincipal(cuyahogaUser);
			}
		}
	}
}
