using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Caching;

using Cuyahoga.Web.Util;
namespace Cuyahoga.Modules.Forum.Domain
{
	/// <summary>
	/// The Emoticon class matches the text representation of an icon (i.e., ":)" for a smile) with its file name.
	/// </summary>
	public class ForumFile
	{
		private int			_id;
		private string		_origfilename;
		private string		_forumfilename;
		private int			_filesize;
		private int			_dlcount;
		private string		_contenttype;
		private DateTime	_dateCreated;
		private DateTime	_dateModified;

		#region Properties
		/// <summary>
		/// Corresponds to the primary key value of EmoticonID in the Emoticons table.
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set	{ this._id =  value; }
		}

		/// <summary>
		/// The text representation of the emoticon, i.e., ";)" for a wink.
		/// </summary>
		public string OrigFileName
		{
			get	{ return this._origfilename; }
			set { this._origfilename = value; }
		}

		/// <summary>
		/// The file name of the Emoticon.
		/// </summary>
		public string ForumFileName
		{
			get { return this._forumfilename; }
			set { this._forumfilename = value; }
		}

		public int FileSize
		{
			get { return this._filesize; }
			set { this._filesize = value; }
		}

		public int DlCount
		{
			get { return this._dlcount; }
			set { this._dlcount = value; }
		}

		public string ContentType
		{
			get { return this._contenttype; }
			set { this._contenttype = value; }
		}

		public DateTime DateModified
		{
			get { return this._dateModified; }
			set { this._dateModified = value; }
		}

		public DateTime DateCreated
		{
			get { return this._dateCreated; }
			set { this._dateCreated = value; }
		}

		#endregion

		/// <summary>
		/// Creates a default Emoticon object.
		/// </summary>
		public ForumFile()
		{
			this._id				= -1;
			this._origfilename		= "";
			this._forumfilename		= "";
			this._filesize			= 0;
			this._dlcount			= 0;
			this._contenttype		= "";
			this._dateCreated		= DateTime.Now;
			this._dateModified		= DateTime.Now;

		}
	}
}
