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
	/// 
	/// MBO, 20041229: User functionality implemented in core. This module stays pretty empty.
	/// </remark>
	/// </summary>
	public class UserModule : ModuleBase
	{
		public override void DeleteModuleContent()
		{
			// No content, so nothing has to be done.
			return;
		}

	}
}
