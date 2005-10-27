using System;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Communication;

namespace Cuyahoga.Modules.User
{
	/// <summary>
	/// The controller class for the Profile module.
	/// </summary>
	public class ProfileModule : ModuleBase, IActionConsumer
	{
		private ProfileModuleAction _currentAction;
		private ActionCollection _inboundActions;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="section"></param>
		public ProfileModule(Section section) : base(section)
		{
			// Set default action.
			this._currentAction = ProfileModuleAction.ViewProfile;
			InitInboundActions();
		}

		/// <summary>
		/// Override ParsePathInfo to determine action and optional parameters.
		/// </summary>
		protected override void ParsePathInfo()
		{
			base.ParsePathInfo();
			if (base.ModuleParams.Length > 0)
			{
				// First pathinfo parameter is the module action.
				try
				{
					this._currentAction = (ProfileModuleAction)Enum.Parse(typeof(ProfileModuleAction)
						, base.ModuleParams[0], true);
				}
				catch (ArgumentException ex)
				{
					throw new Exception("Error when parsing module action: " + base.ModuleParams[0], ex);
				}
				catch (Exception ex)
				{
					throw new Exception("Error when parsing module parameters: " + base.ModulePathInfo, ex);
				}
			}
		}

		/// <summary>
		/// The current view user control based on the action that was set while parsing the pathinfo.
		/// </summary>
		public override string CurrentViewControl
		{
			get
			{
				string basePath = "Modules/User/";
				switch (this._currentAction)
				{
					case ProfileModuleAction.ViewProfile:
						return basePath + "ViewProfile.ascx";
					case ProfileModuleAction.EditProfile:
						return basePath + "EditProfile.ascx";
					case ProfileModuleAction.Register:
						return basePath + "Register.ascx";
					case ProfileModuleAction.ResetPassword:
						return basePath + "ResetPassword.ascx";
					default:
						return base.CurrentViewControl;
				}
			}
		}

		private void InitInboundActions()
		{
			this._inboundActions = new ActionCollection();
			this._inboundActions.Add(new Action("ViewProfile", new string[1] {"UserId"}));
			this._inboundActions.Add(new Action("EditProfile", null));
			this._inboundActions.Add(new Action("Register", null));
			this._inboundActions.Add(new Action("ResetPassword", null));
		}

		#region IActionConsumer Members

		public ActionCollection GetInboundActions()
		{
			return this._inboundActions;
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public enum ProfileModuleAction
	{
		/// <summary>
		/// 
		/// </summary>
		ViewProfile,
		/// <summary>
		/// 
		/// </summary>
		EditProfile,
		/// <summary>
		/// 
		/// </summary>
		Register,
		/// <summary>
		/// 
		/// </summary>
		ResetPassword
	}
}
