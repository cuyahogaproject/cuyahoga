using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Cuyahoga.Web.Mvc.HtmlEditor
{
	public static class HtmlEditorExtensions
	{
		private const string SupportDir = "~/Support/";

		public static string HtmlEditor(this HtmlHelper htmlHelper, string name, string value, string contentCss)
		{
			return HtmlEditor(htmlHelper, name, value, contentCss, new RouteValueDictionary());
		}

		public static string HtmlEditor(this HtmlHelper htmlHelper, string name, string value, string contentCss, object htmlAttributes)
		{
			return HtmlEditor(htmlHelper, name, value, contentCss, new RouteValueDictionary(htmlAttributes));
		}

		public static string HtmlEditor(this HtmlHelper htmlHelper, string name, string value, string contentCss, IDictionary<string, object> htmlAttributes)
		{
			if (!htmlAttributes.ContainsKey("class"))
			{
				htmlAttributes.Add("class", "htmleditor");
			}
			StringBuilder sb = new StringBuilder();
			if (! String.IsNullOrEmpty(contentCss))
			{
				sb.Append(htmlHelper.Hidden("contentcssfield", contentCss, new { @class = "contentcss" }));
			}
			IHtmlEditorScriptProvider htmlEditorScriptProvider = new TinyMceScriptProvider(); // Hardcoded to tinymce for the time being
			foreach (string supportScriptFile in htmlEditorScriptProvider.GetSupportScriptFiles())
			{
				sb.AppendLine(htmlHelper.ScriptInclude(VirtualPathUtility.Combine(SupportDir, supportScriptFile)));
			}
			sb.AppendLine(htmlHelper.TextArea(name, value, htmlAttributes));
			return sb.ToString();
		}
	}
}
