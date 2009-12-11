using System.Globalization;

namespace Cuyahoga.Web.Mvc.Localization
{
	public interface ILocalizer
	{
		/// <summary>
		/// Returns a text string for the given key or the key itself when nothing was found. Uses the default baseName.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		string GetString(string key);

		/// <summary>
		/// Returns a text string for the given key or the key itself when nothing was found.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="baseName"></param>
		/// <returns></returns>
		string GetString(string key, string baseName);

		/// <summary>
		/// Returns a text string for the given key or the key itself when nothing was found.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="baseName"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		string GetString(string key, string baseName, CultureInfo culture);
	}
}