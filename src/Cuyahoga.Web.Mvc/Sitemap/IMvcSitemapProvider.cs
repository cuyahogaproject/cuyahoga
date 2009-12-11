using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Mvc.Sitemap
{
	public interface IMvcSitemapProvider
	{
		/// <summary>
		/// Initializes the sitemap provider and gets the attributes that are set in the config
		/// that enable us to customise the behaviour of this provider.
		/// </summary>
		/// <param name="name">Name of the provider</param>
		/// <param name="attributes">Provider attributes</param>
		void Initialize(string name, NameValueCollection attributes);

		/// <summary>
		/// Builds the sitemap, firstly reads in the XML file, and grabs the outer rootNode element and 
		/// maps this to become our main out rootNode SiteMap node.
		/// </summary>
		/// <returns>The rootNode SiteMapNode.</returns>
		SiteMapNode BuildSiteMap();

		/// <summary>
		/// Determine if a node is accessible for a user
		/// </summary>
		/// <param name="context">Current HttpContext</param>
		/// <param name="node">Sitemap node</param>
		/// <param name="site">Cuyahoga site (optional)</param>
		/// <returns>True/false if the node is accessible</returns>
		bool IsAccessibleToUser(HttpContextBase context, SiteMapNode node, Site site);

		/// <summary>
		/// Gets all MvcSiteMapNodes that are under the given SiteMapNode.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		IEnumerable<MvcSiteMapNode> GetMvcChildNodes(SiteMapNode node);

		/// <summary>
		/// Gets the MvcSiteMapNode that is the parent of the given node.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		MvcSiteMapNode GetMvcParentNode(MvcSiteMapNode node);

		SiteMapNode CurrentNode { get; }
		SiteMapNode RootNode { get; }
		bool SecurityTrimmingEnabled { get; }
	}
}