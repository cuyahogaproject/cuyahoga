using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Caching;

namespace Cuyahoga.Modules.Forum.Domain
{
	public class ForumTag
	{
		private int			_id;
		private string		_forumCodeStart;
		private string		_forumCodeEnd;
		private string		_htmlCodeStart;
		private string		_htmlCodeEnd;
		private DateTime	_dateCreated;
		private DateTime	_dateModified;

		#region Properties
		public int Id
		{
			get { return this._id; }
			set	{ this._id =  value; }
		}

		public string ForumCodeStart
		{
			get	{ return this._forumCodeStart; }
			set { this._forumCodeStart = value; }
		}

		public string ForumCodeEnd
		{
			get { return this._forumCodeEnd; }
			set { this._forumCodeEnd = value; }
		}

		public string HtmlCodeStart
		{
			get	{ return this._htmlCodeStart; }
			set { this._htmlCodeStart = value; }
		}

		public string HtmlCodeEnd
		{
			get { return this._htmlCodeEnd; }
			set { this._htmlCodeEnd = value; }
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

		public ForumTag()
		{
			this._id				= -1;
			this._forumCodeStart	= "";
			this._forumCodeEnd		= "";
			this._htmlCodeEnd		= "";
			this._htmlCodeStart		= "";
			this._dateCreated		= DateTime.Now;
			this._dateModified		= DateTime.Now;
		}
	}
}
