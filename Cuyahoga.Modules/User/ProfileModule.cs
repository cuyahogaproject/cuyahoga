using System;
using System.Web;
using System.Collections.Generic;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Communication;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.Email;
using User = Cuyahoga.Core.Domain.User;

namespace Cuyahoga.Modules.User
{
	/// <summary>
	/// The controller class for the Profile module.
	/// </summary>
	public class ProfileModule : ModuleBase, IActionConsumer
	{
		private static readonly string virtualEmailTemplateDir = "~/Modules/User/EmailTemplates";
		private static readonly string userCacheKeyPrefix = "User_";
		private ProfileModuleAction _currentAction;
		private ModuleActionCollection _inboundModuleActions;
		private int _currentUserId;
		private IUserService _userService;
		private IEmailService _emailService;
		private string _emailTemplateDir;

		/// <summary>
		/// The ID of the current user (when the action is ViewProfile).
		/// </summary>
		public int CurrentUserId
		{
			get { return this._currentUserId; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="emailService"></param>
		/// <param name="userService"></param>
		public ProfileModule(IUserService userService, IEmailService emailService)
		{
			this._userService = userService;
			this._emailService = emailService;
			this._emailService.TemplateDir = HttpContext.Current.Request.MapPath(virtualEmailTemplateDir);

			// Set default action.
			this._currentAction = ProfileModuleAction.EditProfile;
			InitInboundActions();
		}

		/// <summary>
		/// Check if a user with the given username already exists.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public bool CheckIfUserExists(string username)
		{
			return this._userService.FindUsersByUsername(username).Count > 0;
		}

		/// <summary>
		/// Register a new user and send an email with the login information.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="emailAddress"></param>
		public void RegisterUser(string username, string emailAddress)
		{
			Site currentSite = this.Section.Node.Site;
			// Create user
			string newPassword = this._userService.CreateUser(username, emailAddress, currentSite);
			// Send email
			this._emailService.Language = base.Section.Node.Culture.Substring(0, 2);
			Dictionary<string, string> subjectParams = new Dictionary<string,string>(1);
			subjectParams.Add("$site", currentSite.Name);
			Dictionary<string, string> bodyParams = new Dictionary<string,string>(3);
			bodyParams.Add("$site", currentSite.Name);
			bodyParams.Add("$username", username);
			bodyParams.Add("$password", newPassword);

			try
			{
				this._emailService.ProcessEmail(currentSite.WebmasterEmail, emailAddress, "Register", subjectParams, bodyParams);
			}
			catch
			{
				// Delete newly created user when emailing account info fails.
				Cuyahoga.Core.Domain.User newUser = this._userService.GetUserByUsernameAndEmail(username, emailAddress);
				if (newUser != null)
				{
					this._userService.DeleteUser(newUser);
				}
				throw;
			}
		}

		/// <summary>
		/// Update an exising user.
		/// </summary>
		/// <param name="user"></param>
		public void UpdateUser(Cuyahoga.Core.Domain.User user)
		{
			// Save to database
			this._userService.UpdateUser(user);
			// Remove old user instance from the cache
			HttpContext.Current.Cache.Remove(userCacheKeyPrefix + user.Id.ToString());
		}

		/// <summary>
		/// Reset the password of the user with the given username and email address.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="emailAddress"></param>
		public void ResetPassword(string username, string emailAddress)
		{
			string newPassword = this._userService.ResetPassword(username, emailAddress);
			// Send email
			this._emailService.Language = base.Section.Node.Culture.Substring(0, 2);
			Dictionary<string, string> subjectParams = new Dictionary<string,string>(1);
			subjectParams.Add("$site", this.Section.Node.Site.Name);
			Dictionary<string, string> bodyParams = new Dictionary<string,string>(1);
			bodyParams.Add("$password", newPassword);

			this._emailService.ProcessEmail(this.Section.Node.Site.WebmasterEmail, emailAddress, "ResetPassword", subjectParams, bodyParams);
		}

		/// <summary>
		/// Get a user by id.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public Cuyahoga.Core.Domain.User GetUserById(int userId)
		{
			return this._userService.GetUserById(userId);
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
					if (this._currentAction == ProfileModuleAction.ViewProfile)
					{
						// The second paramter is the UserId
						this._currentUserId = Int32.Parse(base.ModuleParams[1]);
					}
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
		public override string CurrentViewControlPath
		{
			get
			{
				string basePath = "Modules/User/";
				return basePath + this._currentAction.ToString() + ".ascx";
			}
		}

		private void InitInboundActions()
		{
			this._inboundModuleActions = new ModuleActionCollection();
			this._inboundModuleActions.Add(new ModuleAction("ViewProfile", new string[1] {"UserId"}));
			this._inboundModuleActions.Add(new ModuleAction("EditProfile", null));
			this._inboundModuleActions.Add(new ModuleAction("Register", null));
			this._inboundModuleActions.Add(new ModuleAction("ResetPassword", null));
		}

		#region IActionConsumer Members

		public ModuleActionCollection GetInboundActions()
		{
			return this._inboundModuleActions;
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
