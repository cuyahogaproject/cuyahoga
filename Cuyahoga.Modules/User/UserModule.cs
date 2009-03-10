using System;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Communication;

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
	public class UserModule : ModuleBase, IActionProvider
	{
		private ModuleActionCollection _outboundModuleActions;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="section"></param>
		public UserModule()
		{
			InitOutboundActions();
		}

		private void InitOutboundActions()
		{
			this._outboundModuleActions = new ModuleActionCollection();
			this._outboundModuleActions.Add(new ModuleAction("ViewProfile", new string[1] {"UserId"}));
			this._outboundModuleActions.Add(new ModuleAction("EditProfile", null));
			this._outboundModuleActions.Add(new ModuleAction("Register", null));
			this._outboundModuleActions.Add(new ModuleAction("ResetPassword", null));
		}

		#region IActionProvider Members

		public ModuleActionCollection GetOutboundActions()
		{
			return this._outboundModuleActions;
		}

		#endregion
	}
}
