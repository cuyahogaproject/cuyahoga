using System;

namespace Cuyahoga.Core.Domain
{
	public enum NodePositionMovement
	{
		Up,
		Down,
		Left,
		Right
	}

	[Flags]
	public enum AccessLevel
	{
		Anonymous = 1,
		Authenticated = 2,
		Editor = 4,
		Administrator = 8
	}

	public enum Action
	{
		View,
		Edit,
		Add,
		Delete
	}
}
