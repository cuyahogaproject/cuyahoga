using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// The SiteAlias class enables mapping of an alternative url to an existing Cuyahoga site.
	/// Optionally you can specify a Node to where the alias should point.
	/// </summary>
	public class SiteAlias
	{
		private int _id;
		private Site _site;
		private Node _entryNode;
		private string _url;
		private DateTime _updateTimestamp;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property Site (Site)
		/// </summary>
		public virtual Site Site
		{
			get { return this._site; }
			set { this._site = value; }
		}

		/// <summary>
		/// Optional entry point.
		/// </summary>
		public virtual Node EntryNode
		{
			get { return this._entryNode; }
			set { this._entryNode = value; }
		}

		/// <summary>
		/// Property Url (string)
		/// </summary>
		public virtual string Url
		{
			get { return this._url; }
			set { this._url = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public virtual DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SiteAlias()
		{
			this._id = -1;
		}
	}
}
