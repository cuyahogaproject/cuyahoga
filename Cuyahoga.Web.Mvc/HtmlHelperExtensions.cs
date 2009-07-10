using System;
using System.Web.Mvc;

namespace Cuyahoga.Web.Mvc
{
	/// <summary>
	/// Contains HtmlHelper extensions, specific for Cuyahoga.
	/// </summary>
	public static class HtmlHelperExtensions
	{
		/// <summary>
		/// Include a reference to javascript file. 
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="scriptPath">The virtual path of the javascript file (supports ~/)</param>
		/// <returns>
		/// A <script> tag that references the given javascript file. 
		/// An empty string is returned when the reference has been returned earlier during the current request.
		/// </returns>
		/// <remarks>Tracks the path reference in HttpContext.Items to prevent duplicates.</remarks>
		public static string ScriptInclude(this HtmlHelper htmlHelper, string scriptPath)
		{
			if (IsAlreadyRegistered(htmlHelper, scriptPath))
			{
				return String.Empty;
			}
			string resolvedScriptPath = ResolveAndRegisterPath(htmlHelper, scriptPath);
			return string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", resolvedScriptPath);
		}

		/// <summary>
		/// Include a reference to a css file.
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="cssPath">The virtual path of the css file (supports ~/)</param>
		/// <returns>
		/// A <link> tag that references the given css file.
		/// An empty string is returned when the reference has been returned earlier during the current request.
		/// </returns>
		/// <remarks>Tracks the path reference in HttpContext.Items to prevent duplicates.</remarks>
		public static string CssLink(this HtmlHelper htmlHelper, string cssPath)
		{
			if (IsAlreadyRegistered(htmlHelper, cssPath))
			{
				return String.Empty;
			}
			string resolvedCssPath = ResolveAndRegisterPath(htmlHelper, cssPath);
			return string.Format("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\">", resolvedCssPath);
		}

		/// <summary>
		/// Include a reference to a css file.
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="cssPath">The virtual path of the css file (supports ~/)</param>
		/// <returns>
		/// A <style> block that has an @import reference to the given css file.
		/// An empty string is returned when the reference has been returned earlier during the current request.
		/// </returns>
		/// <remarks>Tracks the path reference in HttpContext.Items to prevent duplicates.</remarks>
		public static string CssImport(this HtmlHelper htmlHelper, string cssPath)
		{
			if (IsAlreadyRegistered(htmlHelper, cssPath))
			{
				return String.Empty;
			}
			string resolvedCssPath = ResolveAndRegisterPath(htmlHelper, cssPath);
			return string.Format("<style type=\"text/css\">@import url({0});</style>", resolvedCssPath);
		}

		private static bool IsAlreadyRegistered(HtmlHelper htmlHelper, string virtualPath)
		{
			return htmlHelper.ViewContext.HttpContext.Items.Contains(virtualPath);
		}

		private static string ResolveAndRegisterPath(HtmlHelper htmlHelper, string path)
		{
			var httpContext = htmlHelper.ViewContext.HttpContext;
			if (httpContext.Items.Contains(path))
			{
				throw new ArgumentException(string.Format("The path '{0}' is already registered.", path));
			}
			httpContext.Items.Add(path, String.Empty);
			UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
			return urlHelper.Content(path);
		}
	}
}
