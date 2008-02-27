using System;
using System.Threading;
using System.Web;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Components
{
	/// <summary>
	/// Provides a Cuyahoga-specific context, comparable to the HttpContext.
	/// </summary>
	public class CuyahogaContext : ICuyahogaContext
	{
		private HttpContext _httpContext;

		/// <summary>
		/// The underlying ASP.NET context.
		/// </summary>
		public HttpContext HttpContext
		{
			get
			{
				if (this._httpContext == null)
				{
					throw new InvalidOperationException("The underlying HttpContext is null. Make sure to call Initialize() before doing things with CuyahogaContext.");
				}
				return _httpContext;
			}
		}

		/// <summary>
		/// The Cuyahoga user for the current request.
		/// </summary>
		public User CurrentUser
		{
			get { return this.HttpContext.User as User; }
		}

		/// <summary>
		/// Creates an instance of the CuyahogaContext class.
		/// </summary>
		public CuyahogaContext()
		{
		}

		/// <summary>
		/// Initialize the CuyahogaContext.
		/// </summary>
		/// <param name="underlyingContext"></param>
		public void Initialize(HttpContext underlyingContext)
		{
			this._httpContext = underlyingContext;
		}

		/// <summary>
		/// Set the Cuyahoga user for the current context.
		/// </summary>
		/// <param name="user"></param>
		public void SetUser(User user)
		{
			this.HttpContext.User = user;
			Thread.CurrentPrincipal = user;
		}
	}
}
