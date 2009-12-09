using System;
using System.Web;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Web.Components;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.HttpModules
{
	/// <summary>
	/// HttpModule to extend Forms Authentication.
	/// </summary>
	public class AuthenticationModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
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
				IUserService userService = IoC.Resolve<IUserService>();

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
