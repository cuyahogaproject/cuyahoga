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
		/// The current site url (url of the site, or the alias url when visiting the site via the alias).
		/// </summary>
		string CurrentSiteUrl { get; }

		/// <summary>
		/// Gets or sets the physical site data directory.
		/// </summary>
		string PhysicalSiteDataDirectory { get; set; }

		/// <summary>
		/// Set the Cuyahoga user for the current context.
		/// </summary>
		/// <param name="user">The current user</param>
		void SetUser(User user);

		/// <summary>
		/// Set the Cuyahoga site for the current context.
		/// </summary>
		/// <param name="site">The current site</param>
		/// <param name="siteUrl">The url of the current site. If this differs from the SiteUrl of the given site, we're dealing with an alias.</param>
		void SetSite(Site site, string siteUrl);
	}
}