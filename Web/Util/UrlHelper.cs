using System;
using System.Web;

using Cuyahoga.Core;

namespace Cuyahoga.Web.Util
{
	/// <summary>
	/// The UrlHelper class contains methods for url creation and manipulation.
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
			string path = HttpContext.Current.Request.ApplicationPath;
			if (path.EndsWith("/"))
			{
				return path;
			}
			else
			{
				return path + "/";
			}
		}

		/// <summary>
		/// Returns a formatted url for a given node (/{ApplicationPath}/{Node.ShortDescription}.aspx.
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		public static string GetFriendlyUrlFromNode(Node node)
		{
			return GetApplicationPath() + node.ShortDescription + ".aspx";
		}

		/// <summary>
		/// Returns a formatted url for a given node (/{ApplicationPath}/{Node.Id}/view.aspx.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static string GetUrlFromNode(Node node)
		{
			return GetApplicationPath() + node.Id + "/view.aspx";
		}

		/// <summary>
		/// Returns a formatted url for a given section (/{ApplicationPath}/{Node.Id}/{Section.Id}/view.aspx.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public static string GetUrlFromSection(Section section)
		{
			return GetApplicationPath() + section.Node.Id + "/" + section.Id + "/view.aspx";
		}

		public static string[] GetModuleParamsFromPathInfo(string pathInfo)
		{
			if (pathInfo.Length > 0)
			{
				if (pathInfo.EndsWith("/"))
				{
					pathInfo = pathInfo.Substring(0, pathInfo.Length - 1);
				}
                pathInfo = pathInfo.Substring(1, pathInfo.Length -1);
				return pathInfo.Split(new char[] {'/'});
			}
			else
			{
				return null;
			}
		}
	}
}
