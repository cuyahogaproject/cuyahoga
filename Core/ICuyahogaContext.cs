using System.Web;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core
{
	public interface ICuyahogaContext
	{
		/// <summary>
		/// The Cuyahoga user for the current request.
		/// </summary>
		User CurrentUser { get; }

		/// <summary>
		/// The current Cuyahoga site.
		/// </summary>
		Site CurrentSite { get; }

		/// <summary>
		/// Set the Cuyahoga user for the current context.
		/// </summary>
		/// <param name="user"></param>
		void SetUser(User user);

		/// <summary>
		/// Set the Cuyahoga site for the current context.
		/// </summary>
		/// <param name="site"></param>
		void SetSite(Site site);
	}
}