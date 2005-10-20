using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// 
	/// </summary>
	public enum NodePositionMovement
	{
		/// <summary>
		/// 
		/// </summary>
		Up,
		/// <summary>
		/// 
		/// </summary>
		Down,
		/// <summary>
		/// 
		/// </summary>
		Left,
		/// <summary>
		/// 
		/// </summary>
		Right
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum AccessLevel
	{
		/// <summary>
		/// 
		/// </summary>
		Anonymous = 1,
		/// <summary>
		/// 
		/// </summary>
		Authenticated = 2,
		/// <summary>
		/// 
		/// </summary>
		Editor = 4,
		/// <summary>
		/// 
		/// </summary>
		Administrator = 8
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

	/// <summary>
	/// The type of the current database.
	/// </summary>
	public enum DatabaseType
	{
		/// <summary>
		/// Microsoft SQL Server 2000 and up.
		/// </summary>
		MsSql2000,
		/// <summary>
		/// PostgreSQL 7.4 and up.
		/// </summary>
		PostgreSQL,
		/// <summary>
		/// MySQL 4.0 and up.
		/// </summary>
		MySQL
	}
}
