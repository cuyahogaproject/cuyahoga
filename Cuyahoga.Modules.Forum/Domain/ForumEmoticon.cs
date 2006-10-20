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
	public class ForumEmoticon
	{
		private int _id;
		private string _textversion;
		private string _imagename;
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
		public string TextVersion
		{
			get	{ return this._textversion; }
			set { this._textversion = value; }
		}

		/// <summary>
		/// The file name of the Emoticon.
		/// </summary>
		public string ImageName
		{
			get { return this._imagename; }
			set { this._imagename = value; }
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
		public ForumEmoticon()
		{
			this._id	= -1;
			this._textversion	= "";
			this._imagename	= "";
			this._dateCreated		= DateTime.Now;
			this._dateModified		= DateTime.Now;

		}
	}
}
