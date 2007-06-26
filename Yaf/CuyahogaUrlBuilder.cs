using System.Web;
using yaf;

namespace Cuyahoga.Modules.YetAnotherForum
{
	/// <summary>
	/// Summary description for CuyahogaUrlBuilder.
	/// </summary>
	public class CuyahogaUrlBuilder : IUrlBuilder
	{
		#region IUrlBuilder Members

		public string BuildUrl(string url)
		{
			string virtualUrl = HttpContext.Current.Items["VirtualUrl"].ToString();
			if (virtualUrl.IndexOf("?") > -1)
			{
				// Cut off the previous querystring.
				virtualUrl = virtualUrl.Substring(0, virtualUrl.IndexOf("?"));
			}
			return virtualUrl + "?" + url;
		}

		#endregion
	}
}
