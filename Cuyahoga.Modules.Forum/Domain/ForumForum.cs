using System;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.Forum.Domain
{
	/// <summary>
	/// Summary description for Forum.
	/// </summary>
	public class ForumForum
	{
		private int			_id;
		private DateTime	_dateCreated;
		private DateTime	_dateModified;
		private int			_categoryid;
		private string		_name;
		private string		_description;
		private DateTime	_lastposted;
		private int			_lastpostid;
		private int			_numtopics;
		private int			_numposts;
		private int			_sortorder;
		private int			_allowguestpost;
		private string		_lastpostusername;
		
		#region properties
		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
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

		public int CategoryId
		{
			get { return this._categoryid; }
			set { this._categoryid = value; }
		}

		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		public string Description
		{
			get { return this._description; }
			set { this._description = value; }
		}

		public int SortOrder
		{
			get { return this._sortorder; }
			set { this._sortorder = value; }
		}

		public DateTime LastPosted
		{
			get { return this._lastposted; }
			set { this._lastposted = value; }
		}

		public int LastPostId
		{
			get { return this._lastpostid; }
			set { this._lastpostid = value; }
		}

		public int NumTopics
		{
			get { return this._numtopics; }
			set { this._numtopics = value; }
		}

		public int NumPosts
		{
			get { return this._numposts; }
			set { this._numposts = value; }
		}

		public int AllowGuestPost
		{
			get { return this._allowguestpost; }
			set { this._allowguestpost = value; }
		}

		public string LastPostUserName
		{
			get { return this._lastpostusername; }
			set { this._lastpostusername = value; }
		}

		#endregion

		public ForumForum()
		{
			//
			// TODO: Add constructor logic here
			//
			this._id				= -1;
			this._dateCreated		= DateTime.Now;
			this._dateModified		= DateTime.Now;
			this._lastposted		= DateTime.Now;
			this._allowguestpost	= 0;
		}
	}
}
