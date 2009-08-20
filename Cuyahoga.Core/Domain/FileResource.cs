using System;
using System.Collections.Generic;
using Cuyahoga.Core.Service.Search;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// (Base) Class for Files
	/// </summary>
	public class FileResource : ContentItem, ISearchableContent
	{
		private string _fileName;
		private long _length;
		private string _mimeType;
		private int _downloadCount;
		private Func<string> _extractText;

        #region Properties

		/// <summary>
		/// The file name with extension. It's also the default title that can be overruled when setting the Title explicitly.
		/// </summary>
        public virtual string FileName
		{
			get{ return this._fileName; }
			set
			{
				this._fileName = value;
				if (String.IsNullOrEmpty(this.Title))
				{
					this.Title = this._fileName;
				}
			}
		}

		/// <summary>
		/// Length of File (Bytes)
		/// </summary>
        public virtual long Length
		{
			get{ return this._length; }
			set{ this._length = value; }
		}

		/// <summary>
		/// Mime Type
		/// </summary>
        public virtual string MimeType
		{
			get{ return this._mimeType; }
			set{ this._mimeType = value; }
		}

		/// <summary>
		/// Download Counter
		/// </summary>
        public virtual int DownloadCount
		{
			get{ return this._downloadCount; }
			set{ this._downloadCount = value; }
		}

		#endregion

		public override string GetContentUrl()
		{
			string defaultUrlFormat = "{0}/section.aspx/Download/{1}/{2}";
			if (this.Section == null)
			{
				throw new InvalidOperationException("Unable to get the url for the content because the associated section is missing.");
			}
			return String.Format(defaultUrlFormat, this.Section.Id, this.Id, this.FileName);
		}

        public override string ToString()
        {
            return this.FileName;
		}

		#region ISearchableContent members

		/// <summary>
		/// Get the full contents of this ContentItem for indexing
		/// </summary>
		/// <returns></returns>
		public virtual string ToSearchContent()
		{
			return this._extractText();
		}

		/// <summary>
		/// Get a list of <see cref="CustomSearchField"/>s, if any
		/// </summary>
		/// <returns></returns>
		public virtual IList<CustomSearchField> GetCustomSearchFields()
		{
			return new List<CustomSearchField>();
		}

		#endregion

		public virtual void SetTextExtractor(Func<string> extractMethod)
		{
			this._extractText = extractMethod;
		}
	}
}