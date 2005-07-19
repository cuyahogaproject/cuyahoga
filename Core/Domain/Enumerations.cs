using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// 
	/// </summary>
	public enum NodePositionMovement
	{
		Up,
		Down,
		Left,
		Right
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum AccessLevel
	{
		Anonymous = 1,
		Authenticated = 2,
		Editor = 4,
		Administrator = 8
	}

	/// <summary>
	/// 
	/// </summary>
	public enum Action
	{
		View,
		Edit,
		Add,
		Delete
	}

	/// <summary>
	/// The target window of a link.
	/// </summary>
	public enum LinkTarget
	{
		/// <summary>
		/// Link opens in the same window.
		/// </summary>
		Self,
		/// <summary>
		/// Link opens in new window.
		/// </summary>
		New
	}
}
