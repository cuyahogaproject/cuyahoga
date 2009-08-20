using System;

namespace Cuyahoga.Core.Service.Search
{
	/// <summary>
	/// Text extractor that use 'safe' ways to extract the text (as in: not using COM or P/Invoke or other obscure techniques).
	/// </summary>
	public class NativeTextExtractor : ITextExtractor
	{
		/// <summary>
		/// Extract the contents of the given file as plain text.
		/// </summary>
		/// <param name="filePath">The physical path of the file that contains the text to be extracted.</param>
		/// <returns>The extracted text.</returns>
		public string ExtractTextFromFile(string filePath)
		{
			// Currently we have nothing here. Desperately looking for a decent solution.
			return String.Empty;
		}
	}
}