using System;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.User
{
	/// <summary>
	/// The UserModule provides services for user management, specifically non-admin users.
	/// <remark>
	/// Note that the core user functionality is not in this module but in Cuyahoga.Core and
	/// Cuyahoga.Web.Util.AuthenticationModule. This class is needed because every module requires 
	/// a corresponding class. Perhaps functionality like 'forgot password?' will be added here in the future.
	/// </remark>
	/// </summary>
	public class UserModule : Module
	{
		public UserModule()
		{
		}

		public override void LoadContent()
		{
			// Nothing
		}

		public override void DeleteContent()
		{
			// Nothing
		}
	}
}
