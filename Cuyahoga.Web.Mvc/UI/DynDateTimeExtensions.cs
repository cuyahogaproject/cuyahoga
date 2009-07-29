using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Cuyahoga.Web.Mvc.UI
{
	/// <summary>
	/// HtmlHelper extensions for the dynDateTime jquery datetime plugin (http://code.google.com/p/dyndatetime/)
	/// </summary>
	public static class DynDateTimeExtensions
	{
		public static string DateInput(this HtmlHelper htmlHelper, string name, object value)
		{
			return DateInput(htmlHelper, name, value, null);
		}

		public static string DateInput(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(GetDateInputScripts(htmlHelper));
			sb.Append(htmlHelper.TextBox(name, value, htmlAttributes));
			sb.Append("<script type=\"text/javascript\">");
			sb.Append(@"
				$(document).ready(function() {
					$('#" + name + @"').dynDateTime();
				})");
			sb.Append("</script>");
			return sb.ToString();
		}

		public static string DateTimeInput(this HtmlHelper htmlHelper, string name, object value)
		{
			return DateTimeInput(htmlHelper, name, value, null);
		}

		public static string DateTimeInput(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(GetDateInputScripts(htmlHelper));
			sb.Append(htmlHelper.TextBox(name, value, htmlAttributes));
			sb.Append("<script type=\"text/javascript\">");
			sb.Append(@"
				$(document).ready(function() {
					$('#" + name + @"').dynDateTime({
						showsTime: 'true'
					});
				})");
			sb.Append("</script>");
			return sb.ToString();
		}

		private static string GetDateInputScripts(HtmlHelper htmlHelper)
		{
			// Gets the required script and css
			StringBuilder sb = new StringBuilder();
			sb.Append(htmlHelper.ScriptInclude("~/Support/dynDateTime/jquery.dynDateTime.min.js"));
			sb.Append(htmlHelper.ScriptInclude("~/Support/dynDateTime/lang/calendar-en.js"));
			sb.Append(htmlHelper.CssImport("~/Support/dynDateTime/css/calendar-system.css"));
			return sb.ToString();
		}

	}
}
