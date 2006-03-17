using System;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Membership
{
	/// <summary>
	/// Provides functionality for authenticating users.
	/// </summary>
	public interface IAuthenticationService
	{
		/// <summary>
		/// Authenticate a user by a given username and password.
		/// </summary>
		/// <param name="username">The username</param>
		/// <param name="password">The password as entered by the user.</param>
		/// <param name="ipAddress">The remote IP address of the user that tries to authenticate.</param>
		/// <returns>A User object if authentication succeeds, otherwise null.</returns>
		User AuthenticateUser(string username, string password, string ipAddress);
	}
}
