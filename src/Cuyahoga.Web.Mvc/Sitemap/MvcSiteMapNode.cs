using System.Web;

namespace Cuyahoga.Web.Mvc.Sitemap
{
	/// <summary>
	/// MvcSiteMapNode
	/// </summary>
	public class MvcSiteMapNode : SiteMapNode
	{

		#region Properties

		public string Id { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public string Icon { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new MvcSiteMapNode instance
		/// </summary>
		public MvcSiteMapNode(SiteMapProvider provider, string key)
			: base(provider, key)
		{
			Id = key;
		}

		#endregion

	}
}