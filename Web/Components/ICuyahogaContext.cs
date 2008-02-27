using System.Web;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Components
{
	public interface ICuyahogaContext
	{
		/// <summary>
		/// The underlying ASP.NET context.
		/// </summary>
		HttpContext HttpContext { get; }

		/// <summary>
		/// The Cuyahoga user for the current request.
		/// </summary>
		User CurrentUser { get; }

		/// <summary>
		/// Initialize the CuyahogaContext.
		/// </summary>
		/// <param name="underlyingContext"></param>
		void Initialize(HttpContext underlyingContext);

		/// <summary>
		/// Set the Cuyahoga user for the current context.
		/// </summary>
		/// <param name="user"></param>
		void SetUser(User user);
	}
}