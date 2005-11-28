using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Association class between Section and Role.
	/// </summary>
	public class SectionPermission : Permission
	{
		private Section _section;

		/// <summary>
		/// Property Section (Section)
		/// </summary>
		public Section Section
		{
			get { return this._section; }
			set { this._section = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SectionPermission() : base()
		{
		}
	}
}
