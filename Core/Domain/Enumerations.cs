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
}
