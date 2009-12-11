using System;
using System.Security;
using System.Web.Mvc;
using System.Web.Security;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Mvc.Security
{
	/// <summary>
	/// A custom version of the <see cref="AuthorizeAttribute"/> that supports working
	/// around a cookie/session bug in Flash.  
	/// </summary>
	/// <remarks>
	/// Details of the bug and workaround can be found on this blog:
	/// http://geekswithblogs.net/apopovsky/archive/2009/05/06/working-around-flash-cookie-bug-in-asp.net-mvc.aspx
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class FlashCompatibleAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly IUserService _userService;
		private readonly ICuyahogaContextProvider _contextProvider;
		/// <summary>
		/// The key to the authentication token that should be submitted somewhere in the request.
		/// </summary>
		private const string TOKEN_KEY = "token";

		public FlashCompatibleAuthorizeAttribute() : this(IoC.Resolve<IUserService>(), IoC.Resolve<ICuyahogaContextProvider>())
		{
		}

		public FlashCompatibleAuthorizeAttribute(IUserService userService, ICuyahogaContextProvider contextProvider)
		{
			_userService = userService;
			_contextProvider = contextProvider;
		}

		/// <summary>
		/// This changes the behavior of AuthorizeCore so that it will only authorize
		/// users if a valid token is submitted with the request.
		/// </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
		{
			string token = httpContext.Request.Params[TOKEN_KEY];

			if (token != null)
			{
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(token);

				if (ticket != null)
				{
					int userId;
					if (! Int32.TryParse(ticket.Name, out userId))
					{
						throw new SecurityException("Invalid authentication ticket.");
					}
					User currentUser = this._userService.GetUserById(userId);
					currentUser.IsAuthenticated = true;
					this._contextProvider.GetContext().SetUser(currentUser);
				}
			}

			return base.AuthorizeCore(httpContext);
		}
	}
}
