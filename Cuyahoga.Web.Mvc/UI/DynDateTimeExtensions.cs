using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Cuyahoga.Web.Mvc.UI
{
	/// <summary>
	/// HtmlHelper extensions for the dynDateTime jquery datetime plugin (http://code.google.com/p/dyndatetime/)
	/// </summary>
	public static class DynDateTimeExtensions
	{
		private static readonly string[] AvailableLanguages = 
			{"af","al","bg","br","ca","da","de","du","el","en","es","fi","fr","hu","hr","it","jp","ko","lt","lv","nl","no","pl","pt","ro","ru","si","sk","sp","sv","tr","zh"};
		
		public static string DateInput(this HtmlHelper htmlHelper, string name, object value)
		{
			return DateInput(htmlHelper, name, value, null);
		}

		public static string DateInput(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
		{
			string displayValue = null;
			if (value != null)
			{
				displayValue = Convert.ToDateTime(value).ToShortDateString();
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(GetDateInputScripts(htmlHelper));
			sb.Append(htmlHelper.TextBox(name, displayValue, htmlAttributes));
			sb.Append("<script type=\"text/javascript\">");
			sb.Append(@"
				$(document).ready(function() {
					$('#" + name + @"').dynDateTime({
						ifFormat : '" + ConvertDateFormat(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) + @"'
					});
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
			string displayValue = null;
			if (value != null)
			{
				DateTime theDateTime = Convert.ToDateTime(value);
				displayValue = theDateTime.ToShortDateString() + " " + theDateTime.ToShortTimeString();
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(GetDateInputScripts(htmlHelper));
			sb.Append(htmlHelper.TextBox(name, displayValue, htmlAttributes));
			sb.Append("<script type=\"text/javascript\">");
			sb.Append(@"
				$(document).ready(function() {
					$('#" + name + @"').dynDateTime({
						showsTime: 'true',
						ifFormat : '" + ConvertDateFormat(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) + " " + ConvertTimeFormat(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern) + @"'
					});
				})");
			sb.Append("</script>");
			return sb.ToString();
		}

		private static string GetDateInputScripts(HtmlHelper htmlHelper)
		{
			// Determine name of language file
			string language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
			if (! AvailableLanguages.Contains(language))
			{
				// default to 'en' when no language is found.
				language = "en";
			}

			// Gets the required script and css
			StringBuilder sb = new StringBuilder();
			sb.Append(htmlHelper.ScriptInclude("~/Support/dynDateTime/jquery.dynDateTime.min.js"));
			sb.Append(htmlHelper.ScriptInclude(String.Format("~/Support/dynDateTime/lang/calendar-{0}.js", language)));
			sb.Append(htmlHelper.CssImport("~/Support/dynDateTime/css/calendar-system.css"));
			return sb.ToString();
		}

		private static string ConvertDateFormat(string dotNetShortDateFormat)
		{
			string tempFormat = ReplaceFormatCharacter(dotNetShortDateFormat, "y", "%Y");
			tempFormat = ReplaceFormatCharacter(tempFormat, "M", "%m");
			tempFormat = ReplaceFormatCharacter(tempFormat, "d", "%d");
			return tempFormat;
		}

		private static string ConvertTimeFormat(string dotNetShortTimeFormat)
		{
			string tempFormat = ReplaceFormatCharacter(dotNetShortTimeFormat, "H", "%H");
			tempFormat = ReplaceFormatCharacter(tempFormat, "m", "%M");
			tempFormat = ReplaceFormatCharacter(tempFormat, "h", "%I");
			tempFormat = tempFormat.Replace("tt", "%p");
			return tempFormat;
		}

		private static string ReplaceFormatCharacter(string shortDateFormat, string from, string to)
		{
			// This method replaces 1 to 4 occurences of the given 'from' string to the 'to'
			// string.
			string pattern = from + "{1,4}";
			return Regex.Replace(shortDateFormat, pattern, to, RegexOptions.Compiled);
		}
	}
}
