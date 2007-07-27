using System;
using System.IO;
using System.Collections;
using System.Security.Principal;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// (Base) Class for Files
	/// </summary>
	public class FileResource : ContentItem
	{
		private string name;
		private string extension;
		private string physicalPath;
		private long length;
		private string mimeType;
		private int downloadCount;
		private IList downloadRoles;
		private IDictionary userAttributes;

        #region Properties

        /// <summary>
		/// Filename (with extension)
		/// </summary>
		public virtual string Name
		{
			get{ return this.name; }
			set{ this.name = value; }
		}

		/// <summary>
		/// Extension of File (.png, .doc, etc.)
		/// </summary>
		public virtual string Extension
		{
			get{ return this.extension; }
			set{ this.extension = value; }
		}

		/// <summary>
		/// Absolute Path on Disc (C:\Files\..)
		/// </summary>
        public virtual string PhysicalPath
		{
			get{ return this.physicalPath; }
			set{ this.physicalPath = value; }
		}

		/// <summary>
		/// Length of File (Bytes)
		/// </summary>
        public virtual long Length
		{
			get{ return this.length; }
			set{ this.length = value; }
		}

		/// <summary>
		/// Mime Type
		/// </summary>
        public virtual string MimeType
		{
			get{ return this.mimeType; }
			set{ this.mimeType = value; }
		}

		/// <summary>
		/// Download Counter
		/// </summary>
        public virtual int DownloadCount
		{
			get{ return this.downloadCount; }
			set{ this.downloadCount = value; }
		}

		/// <summary>
		/// Predefined Values of Allowed Roles for Download
		/// </summary>
        public virtual IList DownloadRoles
		{
			get{ return this.downloadRoles; }
			set{ this.downloadRoles = value; }
		}

		/// <summary>
		/// Extra Attributes
		/// </summary>
        public virtual IDictionary UserAttributes
		{
			get{ return this.userAttributes; }
			set{ this.userAttributes = value; }
		}

		#endregion

		public FileResource(): base()
		{
			this.downloadCount = 1;
			this.length = 1;
			this.downloadRoles = new ArrayList();
		}
	
		#region Methods

		public virtual void SetFileInformation(FileInfo fi)
		{
			//set default type
			this.SetFileInformation(fi, "application/octet-stream");
		}
		
		public virtual void SetFileInformation(FileInfo fi, string mimeType)
		{
			this.name = fi.Name;
			this.extension = fi.Extension;
			this.physicalPath = fi.FullName;
			this.mimeType = mimeType;
			this.length = fi.Length;
		}

		public virtual void SetFileInformation(System.Web.HttpPostedFile hpf)
		{
			FileInfo fi = new FileInfo(hpf.FileName);
			this.SetFileInformation(fi, hpf.ContentType);
		}

        public override string ToString()
        {
            return this.name;
        }

		#endregion
	}
}