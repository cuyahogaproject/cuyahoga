using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Util
{
	/// <summary>
	/// Adapter for UrlUtil for backward compatibility.
	/// </summary>
	public class UrlHelper
	{
		private UrlHelper()
		{
		}

		/// <summary>
		/// GetApplicationPath returns the base application path and ensures that it allways ends with a "/".
		/// </summary>
		/// <returns></returns>
		public static string GetApplicationPath()
		{
			return UrlUtil.GetApplicationPath();
		}

		/// <summary>
		/// Get the (lowercase) url of the site without any trailing slashes.
		/// </summary>
		/// <returns></returns>
		public static string GetSiteUrl()
		{
			return UrlUtil.GetSiteUrl();
		}

		/// <summary>
		/// Returns a formatted url for a given node (/{ApplicationPath}/{Node.ShortDescription}.aspx.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static string GetFriendlyUrlFromNode(Node node)
		{
			return UrlUtil.GetFriendlyUrlFromNode(node);
		}

		/// <summary>
		/// Returns a formatted url for a given node (/{ApplicationPath}/{Node.Id}/view.aspx
		/// or /{ApplicationPath}/{Node.ShortDescription}.aspx if the site has friendly urls
		/// turned on).
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static string GetUrlFromNode(Node node)
		{
			return UrlUtil.GetUrlFromNode(node);
		}

		/// <summary>
		/// Get the full url of a Node with the host url resolved via the Site property.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static string GetFullUrlFromNodeViaSite(Node node)
		{
			return UrlUtil.GetFullUrlFromNodeViaSite(node);
		}

		/// <summary>
		/// Returns a formatted url for a given section (/{ApplicationPath}/{Section.Id}/section.aspx).
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public static string GetUrlFromSection(Section section)
		{
			return UrlUtil.GetUrlFromSection(section);
		}

		/// <summary>
		/// Returns a formatted url for a given section (http://{hostname}/{ApplicationPath}/{Section.Id}/section.aspx).
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public static string GetFullUrlFromSection(Section section)
		{
			return UrlUtil.GetFullUrlFromSection(section);
		}

		/// <summary>
		/// Get the full url of a Section with the host url resolved via the Site property.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public static string GetFullUrlFromSectionViaSite(Section section)
		{
			return UrlUtil.GetFullUrlFromSectionViaSite(section);
		}

		/// <summary>
		/// Returns a formatted url for a rss feed for a given section 
		/// (http://{hostname}/{ApplicationPath}/{Section.Id}/rss.aspx).
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public static string GetRssUrlFromSection(Section section)
		{
			return UrlUtil.GetRssUrlFromSection(section);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pathInfo"></param>
		/// <returns></returns>
		public static string[] GetParamsFromPathInfo(string pathInfo)
		{
			return UrlUtil.GetParamsFromPathInfo(pathInfo);
		}

		/// <summary>
		/// Set the target window for the node.
		/// </summary>
		/// <param name="hpl"></param>
		/// <param name="node"></param>
		public static void SetHyperLinkTarget(HyperLink hpl, Node node)
		{
			UrlUtil.SetHyperLinkTarget(hpl, node);
		}
	}
}
