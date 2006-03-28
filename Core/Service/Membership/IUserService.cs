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
		/// Get a user by username and email.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		[Obsolete("This method is deprecated, use ResetPassword instead.")]
		User GetUserByUsernameAndEmail(string username, string email);

		/// <summary>
		/// Create a new user.
		/// </summary>
		/// <param name="user"></param>
		void CreateUser(User user);

		/// <summary>
		/// Reset the password of the user with the given username and email address.
		/// </summary>
		/// <param name="user"></param>
		void ResetPassword(string username, string email);

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
