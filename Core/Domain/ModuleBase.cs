using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// The base class for all Cuyahoga modules
	/// </summary>
	public abstract class ModuleBase
	{
		private Section _section;
		private ModuleType _moduleType;

		/// <summary>
		/// Property Section (Section)
		/// </summary>
		public Section Section
		{
			get { return this._section; }
			set { this._section = value; }
		}

		/// <summary>
		/// Property ModuleType (ModuleType)
		/// </summary>
		public ModuleType ModuleType
		{
			get { return this._moduleType; }
			set { this._moduleType = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ModuleBase()
		{
		}
	}
}
