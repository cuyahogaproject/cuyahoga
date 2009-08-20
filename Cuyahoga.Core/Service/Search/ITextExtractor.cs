namespace Cuyahoga.Core.Service.Search
{
	/// <summary>
	/// Functionality for text extraction.
	/// </summary>
	public interface ITextExtractor
	{
		/// <summary>
		/// Extract the contents of the given file as plain text.
		/// </summary>
		/// <param name="filePath">The physical path of the file that contains the text to be extracted.</param>
		/// <returns>The extracted text.</returns>
		string ExtractTextFromFile(string filePath);
	}
}
