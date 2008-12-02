using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Caching;
using System.Collections.Specialized;
using System.Reflection;
using System.Collections;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Mvc.Filters;

namespace Cuyahoga.Web.Mvc.Sitemap
{
	/// <summary>
	/// Provides an XML based sitemap provider for the ASP.NET MVC framework.
	/// Based on the work done by Maarten Balliauw: http://blog.maartenballiauw.be/post/2008/08/29/Building-an-ASPNET-MVC-sitemap-provider-with-security-trimming.aspx
	/// </summary>
	/// <remarks>
	/// Changes from the original version: 
	/// - changed security check to Cuyahoga permissions instead of role based security.
	/// - permission caching.
	/// </remarks>
	public class MvcSitemapProvider : StaticSiteMapProvider, IMvcSitemapProvider
	{

		#region Private

		private SiteMapNode rootNode;
		private readonly XNamespace ns = "";
		private const string rootName = "siteMap";
		private const string nodeName = "siteMapNode";
		private const string mvcNodeName = "mvcSiteMapNode";
		private int cacheDuration = 1;
		private string siteMapFile = string.Empty;
		private string cacheKey = "122EF2B1-F0A4-4507-B011-94669840F79C";
		private bool canCache = true;
		private readonly object padlock = new object();
		private readonly ICollection<string> ignoreAttributes = new [] { "title", "description", "resourceKey", "id", "paramid", "system", "icon" };
		private IDictionary<string, string[]> permissionCache = new Dictionary<string, string[]>();

		#endregion

		#region Properties

		/// <summary>
		/// Calls the BuildSiteMap method.
		/// </summary>
		/// <returns>Root node of the site map</returns>
		protected override SiteMapNode GetRootNodeCore()
		{
			return this.BuildSiteMap();
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Creates a new instance of the <see cref="MvcSitemapProvider"/> class. 
		/// </summary>
		public MvcSitemapProvider() : this(null)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="MvcSitemapProvider"/> class. 
		/// </summary>
		/// <param name="providerName">The name of the sitemap provider in Web.config (optional). When left empty, the default configuration is used.</param>
		/// <remarks>
		/// Normally, the sitemap provider is initialized by calling the static SiteMap class, but we're creating the providers
		/// with Windsor, therefore we have to explicitly configure the provider by reading web.config.
		/// </remarks>
		public MvcSitemapProvider(string providerName)
		{
			// Read the sitemap configuration from web.config
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.config");
			SiteMapSection siteMapSection = (SiteMapSection)config.GetSection("system.web/siteMap");
			if (siteMapSection == null)
			{
				throw new NullReferenceException("Unable to configure the MvcSitemap provider because there was no siteMap section found in web.config.");
			}
			if (providerName == null)
			{
				providerName = siteMapSection.DefaultProvider;
			}
			Initialize(providerName, siteMapSection.Providers[providerName].Parameters);
		}

		/// <summary>
		/// Initializes the sitemap provider and gets the attributes that are set in the config
		/// that enable us to customise the behaviour of this provider.
		/// </summary>
		/// <param name="name">Name of the provider</param>
		/// <param name="attributes">Provider attributes</param>
		public override void Initialize(string name, NameValueCollection attributes)
		{
			base.Initialize(name, attributes);

			// Get the siteMapFile from the attributes.
			this.siteMapFile = attributes["siteMapFile"];

			// If a cacheDuration was passed, set it. Otherwise default to 1 minute.
			if (!string.IsNullOrEmpty(attributes["cacheDuration"]))
			{
				this.cacheDuration = int.Parse(attributes["cacheDuration"]);
			}

			// If a cache key was set in config, set it. 
			// Otherwise it will use the default which is a GUID.
			if (!string.IsNullOrEmpty(attributes["cacheKey"]))
			{
				this.cacheKey = attributes["cacheKey"];
			}

		}

		#endregion

		#region Sitemap Building/XML Parsing

		/// <summary>
		/// Builds the sitemap, firstly reads in the XML file, and grabs the outer rootNode element and 
		/// maps this to become our main out rootNode SiteMap node.
		/// </summary>
		/// <returns>The rootNode SiteMapNode.</returns>
		public override SiteMapNode BuildSiteMap()
		{
			if (rootNode != null && HttpContext.Current.Cache[this.cacheKey] != null)
			{
				// If sitemap already loaded and our cache key is still set,
				// checking a cache item enables us to invalidate the sitemap
				// after a given time period.
				return rootNode;
			}

			lock (this.padlock)
			{
				XDocument siteMapXML;

				// Clear the current sitemap.
				this.Clear();
				this.permissionCache.Clear();

				try
				{
					// Load the XML document.
					siteMapXML = XDocument.Load(HttpContext.Current.Server.MapPath(this.siteMapFile));

					// Get the rootNode siteMapNode element, and map this to a .NET SiteMapNode,
					// this becomes our rootNode node.
					var rootElement = siteMapXML.Element(ns + rootName).Element(ns + nodeName);
					this.rootNode = this.GetSiteMapNodeFromXMLElement(rootElement);

					// Process our XML file, passing in the main rootNode sitemap node and xml element.
					this.ProcessXMLNodes(this.rootNode, rootElement);

					// Add our main rootNode node.
					AddNode(this.rootNode);

					// Create a cache item, this is used for the sole purpose of being able to invalidate our sitemap
					// after a given time period, it also adds a dependancy on the sitemap file,
					// so that once changed it will refresh your sitemap, unfortunately at this stage
					// there is no dependancy for dynamic data, this could be implemented by clearing the cache item,
					// by setting a custom cacheKey, then use this in your administration console for example to
					// clear the cache item when the structure requires refreshing.
					if (canCache)
					{
						HttpContext.Current.Cache.Insert(this.cacheKey,
						                                 "",
						                                 new CacheDependency(HttpContext.Current.Server.MapPath(this.siteMapFile)),
						                                 DateTime.Now.AddMinutes(this.cacheDuration),
						                                 Cache.NoSlidingExpiration
							);
					}

				}
				catch (Exception ex)
				{
					// If there was ANY error loading or parsing the sitemap XML file, throw an exception.
					throw new Exception("An error occured while parsing the sitemap XML.", ex);
				}
				finally
				{
					siteMapXML = null;
				}
			}

			// Finally return our rootNode SiteMapNode.
			return rootNode;
		}

		/// <summary>
		/// Recursively processes our XML document, parsing our siteMapNodes and dynamicNode(s).
		/// </summary>
		/// <param name="siteMapRootNode">The main rootNode sitemap node.</param>
		/// <param name="rootElement">The main siteMapRootNode XML element.</param>
		protected void ProcessXMLNodes(SiteMapNode siteMapRootNode, XElement rootElement)
		{
			SiteMapNode childNode = siteMapRootNode;

			// Loop through each element below the current siteMapRootNode element.
			foreach (XElement node in rootElement.Elements())
			{
				if (node.Name == ns + nodeName)
				{
					// If this is a normal siteMapNode then map the xml element
					// to a SiteMapNode, and add the node to the current siteMapRootNode.
					childNode = this.GetSiteMapNodeFromXMLElement(node);
					AddNode(childNode, siteMapRootNode);
				}
				else if (node.Name == ns + mvcNodeName)
				{
					// If this is an mvcSiteMapNode then map the xml element
					// to a MvcSiteMapNode, and add the node to the current siteMapRootNode.
					childNode = this.GetMvcSiteMapNodeFromXMLElement(node);
					AddNode(childNode, siteMapRootNode);
				}
				else
				{
					// If the current node is not one of the known node types throw and exception
					throw new Exception("An invalid element was found in the sitemap.");
				}

				// Continue recursively processing the XML file.
				ProcessXMLNodes(childNode, node);
			}
		}

		/// <summary>
		/// Clears the current sitemap.
		/// </summary>
		protected override void Clear()
		{
			this.rootNode = null;
			base.Clear();
		}

		#endregion

		#region Mappers

		/// <summary>
		/// Determine if a node is accessible for a user
		/// </summary>
		/// <param name="context">Current HttpContext</param>
		/// <param name="node">Sitemap node</param>
		/// <returns>True/false if the node is accessible</returns>
		public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
		{
			return IsAccessibleToUser(new HttpContextWrapper(context), node, null);
		}
		/// <summary>
		/// Determine if a node is accessible for a user
		/// </summary>
		/// <param name="context">Current HttpContext</param>
		/// <param name="node">Sitemap node</param>
		/// <param name="site">Cuyahoga site (optional)</param>
		/// <returns>True/false if the node is accessible</returns>
		public bool IsAccessibleToUser(HttpContextBase context, SiteMapNode node, Site site)
		{
			// Is security trimming enabled?
			if (!this.SecurityTrimmingEnabled)
				return true;

			// Is it a regular node? No need for more things to do!
			MvcSiteMapNode mvcNode = node as MvcSiteMapNode;
			if (mvcNode == null)
				return base.IsAccessibleToUser(HttpContext.Current, node); // dirty, but the base sitemap provider requires an HttpContext. 

			// Find current handler
			MvcHandler handler = context.Handler as MvcHandler;

			if (handler != null)
			{
				User cuyahogaUser = handler.RequestContext.HttpContext.User as User;
				if (cuyahogaUser == null)
				{
					return false;
				}

				string permissionCacheKey = mvcNode.Key + "_" + mvcNode.Action;
				if (! permissionCache.ContainsKey(permissionCacheKey))
				{
					CacheRightsForNode(mvcNode, handler, permissionCacheKey);
				}

				// Determine if the user has the required rights
				foreach (string requiredRight in this.permissionCache[permissionCacheKey])
				{
					if (site == null && ! cuyahogaUser.HasRight(requiredRight)
						|| site != null && ! cuyahogaUser.HasRight(requiredRight, site))
					{
						return false;
					}
				}
				return true; // MVC handler and all required rights are OK.
			}

			return false;
		}

		private void CacheRightsForNode(MvcSiteMapNode mvcNode, MvcHandler handler, string permissionCacheKey)
		{
			lock (this.padlock)
			{
				// double check
				if (! permissionCache.ContainsKey(permissionCacheKey))
				{
					// check permission attributes and store required rights in the permission cache.
					// It's an MvcSiteMapNode, try to figure out the controller class
					IController controller = ControllerBuilder.Current.GetControllerFactory().CreateController(handler.RequestContext, mvcNode.Controller);
					Type controllerType = controller.GetType();

					// Find all AuthorizeAttributes on the controller class and action method
					ArrayList controllerAttributes = new ArrayList(controllerType.GetCustomAttributes(typeof(PermissionFilterAttribute), true));
					ArrayList actionAttributes = new ArrayList();
					MethodInfo[] methods = controllerType.GetType().GetMethods(BindingFlags.Public);
					foreach (MethodInfo method in methods)
					{
						object[] attributes = method.GetCustomAttributes(typeof(ActionNameAttribute), true);
						if (
							(attributes.Length == 0 && method.Name == mvcNode.Action)
							|| (attributes.Length > 0 && ((ActionNameAttribute)attributes[0]).Name == mvcNode.Action)
							)
						{
							actionAttributes.AddRange(method.GetCustomAttributes(typeof(PermissionFilterAttribute), true));
						}
					}

					ICollection<string> rights = new List<string>();

					// Attributes found?
					if (controllerAttributes.Count > 0)
					{
						PermissionFilterAttribute attribute = controllerAttributes[0] as PermissionFilterAttribute;
						foreach (string right in attribute.RightsArray)
						{
							if (! rights.Contains(right))
							{
								rights.Add(right);
							}
						}
					}
					if (actionAttributes.Count > 0)
					{
						PermissionFilterAttribute attribute = actionAttributes[0] as PermissionFilterAttribute;
						foreach (string right in attribute.RightsArray)
						{
							if (!rights.Contains(right))
							{
								rights.Add(right);
							}
						}
					}
					
					permissionCache.Add(permissionCacheKey, rights.ToArray());
				}
			}
		}

		public IEnumerable<MvcSiteMapNode> GetMvcChildNodes(SiteMapNode node)
		{
			return node.ChildNodes.OfType<MvcSiteMapNode>();
		}

		public MvcSiteMapNode GetMvcParentNode(MvcSiteMapNode node)
		{
			return node.ParentNode as MvcSiteMapNode;
		}

		/// <summary>
		/// Finds the current sitemap node based on context
		/// </summary>
		/// <param name="context">Current HttpContext</param>
		/// <returns>Current sitemap node</returns>
		public override SiteMapNode FindSiteMapNode(HttpContext context)
		{
			// Node
			SiteMapNode node = null;

			// Fetch route data
			HttpContextWrapper httpContext = new HttpContextWrapper(HttpContext.Current);
			RouteData routeData = RouteTable.Routes.GetRouteData(httpContext);
			if (routeData != null)
			{
				IDictionary<string, object> routeValues = routeData.Values;
				string controller = (string)routeValues["controller"];
				string action = (string)routeValues["action"];
				node = FindControllerActionNode(this.RootNode, controller, action, routeData.Values)
				       ?? FindControllerActionNode(this.RootNode, controller, "Index", routeData.Values)
				          ?? FindControllerActionNode(this.RootNode, "Home", "Index", routeData.Values);
			}

			// Try base class
			if (node == null)
			{
				node = base.FindSiteMapNode(context);
			}

			return node;
		}

		/// <summary>
		/// Maps a controller + action from the XML file to a SiteMapNode.
		/// </summary>
		/// <param name="rootNode">Root node</param>
		/// <param name="controller">Controller</param>
		/// <param name="action">Action</param>
		/// <param name="values">Values</param>
		/// <returns>A SiteMapNode which represents the controller + action.</returns>
		private SiteMapNode FindControllerActionNode(SiteMapNode rootNode, string controller, string action, IDictionary<string, object> values)
		{
			SiteMapNode siteMapNode = null;

			if (rootNode != null)
			{
				// Search current level
				foreach (SiteMapNode node in this.GetChildNodes(rootNode))
				{
					string nodeController = node["controller"] ?? "";
					string nodeAction = node["action"] ?? "";

					// Find node in sitemap based on controller + action
					if (nodeController == controller && nodeAction == action)
					{
						siteMapNode = node;
						break;
					}
				}

				// Search one deeper level
				if (siteMapNode == null)
				{
					foreach (SiteMapNode node in this.GetChildNodes(rootNode))
					{
						siteMapNode = FindControllerActionNode(node, controller, action, values);
						if (siteMapNode != null)
						{
							break;
						}
					}
				}
			}

			return siteMapNode;
		}

		/// <summary>
		/// Maps an XMLElement from the XML file to a SiteMapNode.
		/// </summary>
		/// <param name="node">The element to map.</param>
		/// <returns>A SiteMapNode which represents the XMLElement.</returns>
		protected SiteMapNode GetSiteMapNodeFromXMLElement(XElement node)
		{
			// Get the URL attribute, need this so we can get the key.
			string url = GetAttributeValue(node.Attribute("url"));

			// Create a new sitemapnode, setting the key and url
			var smNode = new SiteMapNode(this, url)
			             	{
			             		Url = url
			             	};

			// Add each attribute to our attributes collection on the sitemapnode.
			foreach (XAttribute attribute in node.Attributes())
			{
				smNode[attribute.Name.ToString()] = attribute.Value;
			}

			// Set the other properties on the sitemapnode, 
			// these are for title and description, these come
			// from the nodes attrbutes are we populated all attributes
			// from the xml to the node.
			smNode.Title = smNode["title"];
			smNode.Description = smNode["description"];

			return smNode;
		}


		/// <summary>
		/// Maps an XMLElement from the XML file to a SiteMapNode.
		/// </summary>
		/// <param name="node">The element to map.</param>
		/// <returns>A SiteMapNode which represents the XMLElement.</returns>
		protected SiteMapNode GetMvcSiteMapNodeFromXMLElement(XElement node)
		{
			// Get the ID attribute, need this so we can get the key.
			string id = GetAttributeValue(node.Attribute("id"));

			// Create a new sitemapnode, setting the key and url
			var smNode = new MvcSiteMapNode(this, id);

			// Create a route data dictionary
			IDictionary<string, object> routeValues = new Dictionary<string, object>();

			// Add each attribute to our attributes collection on the sitemapnode
			// and to a route data dictionary.
			foreach (XAttribute attribute in node.Attributes())
			{
				string attributeName = attribute.Name.ToString();
				string attributeValue = attribute.Value;

				smNode[attributeName] = attributeValue;

				if (! this.ignoreAttributes.Contains(attributeName))
				{
					routeValues.Add(attributeName, attributeValue);
				}
				else if (attributeName == "paramid")
				{
					routeValues.Add("id", attributeValue);
				}
			}

			// Set the other properties on the sitemapnode, 
			// these are for title and description, these come
			// from the nodes attrbutes are we populated all attributes
			// from the xml to the node.
			smNode.Title = smNode["title"];
			smNode.Description = smNode["description"]; 
			smNode.ResourceKey = smNode["resourceKey"];
			smNode.Controller = smNode["controller"];
			smNode.Action = smNode["action"] ?? "Index";
			smNode.Icon = smNode["icon"];

			// Verify route values
			if (!routeValues.ContainsKey("controller")) routeValues.Add("controller", "Home");
			if (!routeValues.ContainsKey("action")) routeValues.Add("action", "Index");

			// Build URL
			HttpContextWrapper httpContext = new HttpContextWrapper(HttpContext.Current);
			RouteData routeData = RouteTable.Routes.GetRouteData(httpContext);
			if (routeData != null)
			{
				VirtualPathData virtualPath = routeData.Route.GetVirtualPath(new RequestContext(httpContext, routeData), new RouteValueDictionary(routeValues));

				if (virtualPath != null)
				{
					smNode.Url = "~/" + virtualPath.VirtualPath;
				}
				else
				{
					canCache = false;
				}
			}

			return smNode;
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Given an XAttribute, will either return an empty string if its value is
		/// null or the actual value.
		/// </summary>
		/// <param name="attribute">The attribe to get the value for.</param>
		/// <returns></returns>
		public string GetAttributeValue(XAttribute attribute)
		{
			return attribute != null ? attribute.Value : string.Empty;
		}

		#endregion
	}

	#region Classes

	#endregion
}