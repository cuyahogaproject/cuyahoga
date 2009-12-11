using System.Collections;
using System.Globalization;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// Utilty class for Globalization stuff.
	/// </summary>
	public class Globalization
	{
		/// <summary>
		/// Get a sortedlist of all installed cultures, ordered by display name.
		/// </summary>
		/// <returns></returns>
		public static SortedList GetOrderedCultures()
		{
			SortedList orderedCultures = new SortedList();
			foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
			{
				orderedCultures.Add(ci.Name, ci.DisplayName);
			}
			return (orderedCultures);
		}

		/// <summary>
		/// Get the description of a language from a culture.
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public static string GetNativeLanguageTextFromCulture(string culture)
		{
			CultureInfo ci = new CultureInfo(culture);
			string languageAsText = ci.NativeName.Substring(0, ci.NativeName.IndexOf("(") - 1);

			return languageAsText;
		}

		/// <summary>
		/// Get the country part from the culture string.
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public static string GetCountryFromCulture(string culture)
		{
			return culture.Substring(3);
		}

		/// <summary>
		/// Get the two-letter ISO language name of the given culture.
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public static string GetLanguageFromCulture(string culture)
		{
			CultureInfo ci = new CultureInfo(culture);
			return ci.TwoLetterISOLanguageName;
		}
	}
}
