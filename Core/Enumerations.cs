using System;

namespace Cuyahoga.Core
{
	public enum NodePositionMovement
	{
		Up,
		Down,
		Left,
		Right
	}

	public enum AccessLevel
	{
		Administrator = 8,
		Editor = 4,
		Authenticated = 2,
		Anonymous = 1		
	}

	public enum Action
	{
		View,
		Edit,
		Add,
		Delete
	}
}
