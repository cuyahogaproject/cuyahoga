using System;
using System.Web;
using System.Web.Security;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Components;
using log4net;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.HttpModules
{
	/// <summary>
	/// HttpModule to extend Forms Authentication.
	/// </summary>
	public class AuthenticationModule : IHttpModule
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(AuthenticationModule));
		private IAuthenticationService authenticationService;
		private IUserService userService;

		public AuthenticationModule()
		{
		}

		public void Init(HttpApplication context)
		{
			this.authenticationService = IoC.Resolve<IAuthenticationService>();
			this.userService = IoC.Resolve<IUserService>();
			context.AuthenticateRequest += new EventHandler(Context_AuthenticateRequest);
		}

		public void Dispose()
		{
			// Nothing here	
		}

		private void Context_AuthenticateRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if (app.Context.User != null && app.Context.User.Identity.IsAuthenticated)
			{
				// There is a logged-in user with a standard Forms Identity. Replace it with
				// Cuyahoga identity (the User class implements IIdentity). 				
				int userId = Int32.Parse(app.Context.User.Identity.Name);
				User cuyahogaUser = userService.GetUserById(userId);
				cuyahogaUser.IsAuthenticated = true;
				CuyahogaContext.Current.SetUser(cuyahogaUser);
			}
		}
	}
}
