using System;
using System.Collections;

using Cuyahoga.Core.Collections;

namespace Cuyahoga.Core.DAL
{
	/// <summary>
	/// Summary description for ICmsDataProvider.
	/// </summary>
	public interface ICmsDataProvider
	{
		#region Node related

		/// <summary>
		/// Gets Node data. Since we're calling the database, we can fetch the related template as well :).
		/// Don't get the ParentNode though, because that one is lazy loaded because of the recursive relationship.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="node"></param>
		void GetNodeById(int id, Node node);
		
		/// <summary>
		/// Gets the nodes belonging to a given parentnode. 
		/// </summary>
		/// <param name="parentNode"></param>
		/// <param name="nodes"></param>
		void GetNodesByParent(Node parentNode, NodeCollection nodes);

		void GetNodeByParentIdAndPosition(int parentId, int position, Node node);

		void UpdateVerticalNodePosition(Node node, NodePositionMovement movement);

		void UpdateNodePositions(Node parentNode, int positionFrom, int amount);

		void InsertNode(Node node);

		void UpdateNode(Node node);

		void DeleteNode(Node node);

		int GetMaxNodePositionAtRootLevel();

		#endregion

		#region Section related

		void GetSectionById(int id, Section section);

		void GetSectionsByNode(Node node, SectionCollection sections);

		void InsertSection(Section section);

		void UpdateSection(Section section);

		void DeleteSection(Section section);

		#endregion

		#region Template related

		void GetAllTemplates(TemplateCollection templates);

		#endregion

		#region Module related

		void ReadAndCacheAllModules();

		Hashtable GetAllModules();

		#endregion

		#region User and role related

		void GetUserByUsernameAndPassword(string userName, string password, User user);

		void GetUserById(int id, User user);

		void FindUsersByName(string userName, UserCollection users);

		void InsertUser(User user);

		void UpdateUser(User user);

		void DeleteUser(User user);

		void GetAllRoles(RoleCollection roles);

		void GetRolesByUser(User user);

		void GetRolesByNode(Node node);

		void GetRolesBySection(Section section);

		#endregion
	}
}
