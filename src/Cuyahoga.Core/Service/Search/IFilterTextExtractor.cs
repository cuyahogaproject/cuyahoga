using System;
using System.IO;
using System.Linq;
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
		private string _allowedExtensions;
		private const string DefaultAllowedExtensions = ".pdf,.doc,.docx,.ppt,.pptx,.xls,.xlsx";

		/// <summary>
		/// A comma separated string with the allowed extensions for the IFilterTextExtractor.
		/// </summary>
		public string AllowedExtensions
		{
			get { return this._allowedExtensions ?? DefaultAllowedExtensions; }
			set { this._allowedExtensions = value; }
		}

		/// <summary>
		/// Extract the contents of the given file as plain text.
		/// </summary>
		/// <param name="filePath">The physical path of the file that contains the text to be extracted.</param>
		/// <returns>The extracted text.</returns>
		public string ExtractTextFromFile(string filePath)
		{
			string extractedText = String.Empty;
			string[] allowedExtensionsArray = this.AllowedExtensions.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
			if (allowedExtensionsArray.Contains(Path.GetExtension(filePath)))
			{
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
					// This is a non-critical error, so we're just logging it.
					Logger.Error(string.Format("Unable to extract text for {0}.", filePath), ex);
				}
			}
			return extractedText;
		}
	}
}