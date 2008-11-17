using System;
using System.Collections;
using System.Collections.Generic;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// Provides data access for user-related components.
	/// </summary>
	public interface IUserDao
	{
		/// <summary>
		/// Get a single user by username and password. Note that the supplied password 
		/// has to be hashed already.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		User GetUserByUsernameAndPassword(string username, string password);

		/// <summary>
		/// Get a single user by username and email.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		User GetUserByUsernameAndEmail(string username, string email);

		/// <summary>
		/// Find users by username (begins with).
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
		/// <param name="siteId">Filter on users for a given site</param>
		/// <param name="pageSize">Max number of users to return</param>
		/// <param name="pageNumber">Current page number</param>
		/// <param name="totalCount">The total number of users found</param>
		/// <returns>A list of users that match the given criteria</returns>
		IList<User> FindUsers(string username, int? roleId, bool? isActive, int? siteId, int pageSize, int pageNumber,
							  out int totalCount);

        /// <summary>
        /// Get all sections that the supplied user has read access to
        /// </summary>
        /// <param name="user"></param>
        IList<Section> GetViewableSectionsByUser(User user);

 
        // IList<Section> GetViewableSectionsByRoles(IList<Role> roles);

        /// <summary>
        /// Get all sections that the supplied access level grants read access for
        /// </summary>
        /// <param name="accessLevel"></param>
        /// <returns></returns>
        IList<Section> GetViewableSectionsByAccessLevel(AccessLevel accessLevel);


        /// <summary>
        /// Get all roles that are mapped to the given <see cref="AccessLevel"/>
        /// </summary>
        /// <param name="accessLevel"></param>
        /// <returns></returns>
        IList<Role> GetRolesByAccessLevel(AccessLevel accessLevel);

		/// <summary>
		/// Save or update a user.
		/// </summary>
		/// <param name="user"></param>
		void SaveOrUpdateUser(User user);

		/// <summary>
		/// Delete a user.
		/// </summary>
		/// <param name="user"></param>
		void DeleteUser(User user);

	}
}
