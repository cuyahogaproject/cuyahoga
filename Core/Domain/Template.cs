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
		private string _css;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
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
		/// Property Name (string)
		/// </summary>
		public virtual string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property Path (string)
		/// </summary>
		public virtual string Path
		{
			get { return this._path; }
			set { this._path = value; }
		}

		/// <summary>
		/// Property Css (string)
		/// </summary>
		public virtual string Css
		{
			get { return this._css; }
			set { this._css = value; }
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
