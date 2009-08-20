using System;
using EPocalipse.IFilter;
using log4net;

namespace Cuyahoga.Core.Service.Search
{
	/// <summary>
	/// Text extractor that leverages IFilters that are installed on machines. 
	/// Note that this is only supported on Windows due to COM dependencies of IFilters.
	/// </summary>
	public class IFilterTextExtractor : ITextExtractor
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (IFilterTextExtractor));

		/// <summary>
		/// Extract the contents of the given file as plain text.
		/// </summary>
		/// <param name="filePath">The physical path of the file that contains the text to be extracted.</param>
		/// <returns>The extracted text.</returns>
		public string ExtractTextFromFile(string filePath)
		{
			string extractedText = String.Empty;
			try
			{
				using (FilterReader filterReader = new FilterReader(filePath))
				{
					extractedText = filterReader.ReadToEnd();
				}
			}
			catch (ArgumentException ex)
			{
				// An argument exception usually happens when the IFilter for the file could not be found.
				// We just register a warning.
				if (Logger.IsWarnEnabled)
				{
					Logger.Warn(string.Format("Unable to extract text for {0}.", filePath), ex);
				}
			}
			return extractedText;
		}
	}
}