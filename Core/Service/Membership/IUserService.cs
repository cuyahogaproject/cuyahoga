using System.Collections;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;

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
		/// Find users and return a sliced list based on the paging parameters
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="roleId">Role to filter</param>
		/// <param name="isActive">Filter on active or inactive users</param>
		/// <param name="site">Filter on users for a given site</param>
		/// <param name="pageSize">Max number of users to return</param>
		/// <param name="pageNumber">Current page number</param>
		/// <param name="totalCount">The total number of users found</param>
		/// <returns>A list of users that match the given criteria</returns>
		IList<User> FindUsers(string username, int? roleId, bool? isActive, Site site, int pageSize, int? pageNumber,
		                      out int totalCount);

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
		/// Create a new user.
		/// </summary>
		/// <param name="user"></param>
		void CreateUser(User user);

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
		/// <param name="username"></param>
		/// <param name="email"></param>
		/// <returns>The new password</returns>
		string ResetPassword(string username, string email);

		/// <summary>
		/// Get all available roles.
		/// </summary>
		/// <returns></returns>
		IList GetAllRoles();

		/// <summary>
		/// Get all roles for a given site.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList<Role> GetAllRolesBySite(Site site);

		/// <summary>
		/// Get a single role by id.
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		Role GetRoleById(int roleId);

		/// <summary>
		/// Get roles for the given id's.
		/// </summary>
		/// <param name="roleIds"></param>
		/// <returns></returns>
		IList<Role> GetRolesByIds(int[] roleIds);

		/// <summary>
		/// Get all available rights.
		/// </summary>
		/// <returns></returns>
		IList<Right> GetAllRights();

		/// <summary>
		/// Get a Right by id.
		/// </summary>
		/// <param name="rightId"></param>
		/// <returns></returns>
		Right GetRightById(int rightId);

		/// <summary>
		/// Get rights for the give id's.
		/// </summary>
		/// <param name="rightIds"></param>
		/// <returns></returns>
		IList<Right> GetRightsByIds(int[] rightIds);

		/// <summary>
		/// Create a new role.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="currentSite"></param>
		void CreateRole(Role role, Site currentSite);

		/// <summary>
		/// Update a role.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="site"></param>
		void UpdateRole(Role role, Site site);

		/// <summary>
		/// Delete a role.
		/// </summary>
		/// <param name="role"></param>
		void DeleteRole(Role role);
	}
}
