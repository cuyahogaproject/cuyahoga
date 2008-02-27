namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Represents a right that a related user is allowed to perform (for example 'CanAccessSiteManager')
	/// </summary>
	public class Right
	{
		private int _id;
		private string _name;
		private string _description;

		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// Name of the right.
		/// </summary>
		public virtual string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Description of the right.
		/// </summary>
		public virtual string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public Right()
		{
			this._id = -1;
		}
	}
}
