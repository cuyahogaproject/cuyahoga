using System;
using System.Collections;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Membership
{
	/// <summary>
	/// Provides functionality for user management.
	/// </summary>
	public interface IUserService
	{
		/// <summary>
		/// Find users by username (starts with).
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		IList FindUsersByUsername(string searchString);

		/// <summary>
		/// Get a user by ID.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		User GetUserById(int userId);

		/// <summary>
		/// Get a user by username and email.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		User GetUserByUsernameAndEmail(string username, string email);

		/// <summary>
		/// Create a new user with the supplied username and email address for the given site.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="email"></param>
		/// <param name="site"></param>
		/// <returns>The generated password in clear text.</returns>
		string CreateUser(string username, string email, Site site);

		/// <summary>
		/// Update an existing user.
		/// </summary>
		/// <param name="user"></param>
		void UpdateUser(User user);

		/// <summary>
		/// Delete an existing user.
		/// </summary>
		/// <param name="user"></param>
		void DeleteUser(User user);

		/// <summary>
		/// Reset the password of the user with the given username and email address.
		/// </summary>
		/// <param name="user"></param>
		/// <returns>The new password</returns>
		string ResetPassword(string username, string email);

		/// <summary>
		/// Get all available roles.
		/// </summary>
		/// <returns></returns>
		IList GetAllRoles();

		/// <summary>
		/// Get a single role by id.
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		Role GetRoleById(int roleId);
	}
}
