using System;
using System.Web;

using Cuyahoga.Core;

namespace Cuyahoga.Web.Util
{
	/// <summary>
	/// Summary description for UrlHelper.
	/// </summary>
	public class UrlHelper
	{
		private UrlHelper()
		{
		}

		public static string GetApplicationPath()
		{
			string path = HttpContext.Current.Request.ApplicationPath;
			if (path.EndsWith("/"))
				return path;
			else
				return path + "/";
		}

		/// <summary>
		/// Returns a formatted url for a given node (/{ApplicationPath}/{Node.ShortDescription}.aspx.
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		public static string GetUrlFromNode(Node node)
		{
			return GetApplicationPath() + node.ShortDescription + ".aspx";
		}
	}
}
