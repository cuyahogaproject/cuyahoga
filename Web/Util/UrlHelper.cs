using System;
using System.Web;

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
		/// Returns a formatted url for a given nodeId (/{ApplicationPath}/{NodeId}/Show.aspx.
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		public static string GetUrlFromNodeId(int nodeId)
		{
			return GetApplicationPath() + nodeId.ToString() + "/Show.aspx";
		}
	}
}
