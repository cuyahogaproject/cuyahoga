using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for Template.
	/// </summary>
	[Serializable]
	public class Template
	{
		private int _id;
		private DateTime _updateTimestamp;
		private string _name;
		private string _path;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
		}

		/// <summary>
		/// Property Name (string)
		/// </summary>
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property Path (string)
		/// </summary>
		public string Path
		{
			get { return this._path; }
			set { this._path = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Template()
		{
			this._id = -1;
		}
	}
}
