using System;
using System.Threading;
using System.Web;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Components
{
	/// <summary>
	/// Provides a Cuyahoga-specific context, comparable to the HttpContext.
	/// </summary>
	public class CuyahogaContext : ICuyahogaContext
	{
		private Site _currentSite;
		private User _currentUser;

		/// <summary>
		/// The Cuyahoga user for the current request.
		/// </summary>
		public User CurrentUser
		{
			get { return this._currentUser; }
		}

		/// <summary>
		/// The current Cuyahoga site.
		/// </summary>
		public Site CurrentSite
		{
			get { return _currentSite; }
		}

		/// <summary>
		/// Gets the current ICuyahoga context.
		/// <remarks>
		/// This property is just for convenience. Only use it from places where the context can't be injected via IoC.
		/// TODO: We need to do something about the IoC dependency here.
		/// </remarks>
		/// </summary>
		public static ICuyahogaContext Current
		{
			get { return IoC.Resolve<ICuyahogaContext>(); }
		}

		/// <summary>
		/// Creates an instance of the CuyahogaContext class.
		/// </summary>
		public CuyahogaContext()
		{
		}

		/// <summary>
		/// Set the Cuyahoga user for the current context.
		/// </summary>
		/// <param name="user"></param>
		public void SetUser(User user)
		{
			this._currentUser = user;
			HttpContext.Current.User = user;
			Thread.CurrentPrincipal = user;
		}

		/// <summary>
		/// Set the Cuyahoga site for the current context.
		/// </summary>
		/// <param name="site"></param>
		public void SetSite(Site site)
		{
			this._currentSite = site;
		}
	}
}
