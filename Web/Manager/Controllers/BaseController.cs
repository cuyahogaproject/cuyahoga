using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Web.Mvc;
using Castle.Components.Validator;
using Castle.Core.Logging;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Manager.Filters;
using Cuyahoga.Web.Manager.Model.ViewModels;
using Cuyahoga.Web.Mvc;
using Cuyahoga.Web.Mvc.Partials;
using Resources.Cuyahoga.Web.Manager;
using UrlHelper = Cuyahoga.Web.Util.UrlHelper;

namespace Cuyahoga.Web.Manager.Controllers
{
	/// <summary>
	/// Base class for all controllers.
	/// </summary>
	[ExceptionFilter(ExceptionType = typeof(SecurityException))]
	public abstract class BaseController : Controller
	{
		private ICuyahogaContext _cuyahogaContext;
		private ISiteService _siteService;
		private ILogger _logger = NullLogger.Instance;
		private MessageViewData _messageViewData;
		private IModelValidator _modelValidator;

		/// <summary>
		/// Get or sets the Cuyahoga context.
		/// </summary>
		public ICuyahogaContext CuyahogaContext
		{
			get { return this._cuyahogaContext; }
			set { this._cuyahogaContext = value; }
		}

		/// <summary>
		/// Sets the SiteService.
		/// </summary>
		public ISiteService SiteService
		{
			protected get { return this._siteService; }
			set { this._siteService = value; }
		}
		
		/// <summary>
		/// Sets the ModelValidator.
		/// </summary>
		public IModelValidator ModelValidator
		{
			set { this._modelValidator = value; }
		}

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		public ILogger Logger
		{
			get { return this._logger; }
			set { this._logger = value; }
		}

		/// <summary>
		/// Validates the given object. If invalid, the errors are added to the ModelState.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <returns>True if the object is valid</returns>
		protected virtual bool ValidateModel(object objectToValidate)
		{
			return ValidateModel(objectToValidate, null);
		}

		/// <summary>
		/// Validates the given object. If invalid, the errors are added to the ModelState.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="includeProperties">Properties to check</param>
		/// <returns>True if the object is valid</returns>
		protected virtual bool ValidateModel(object objectToValidate, string[] includeProperties)
		{
			if (! this._modelValidator.IsValid(objectToValidate, includeProperties))
			{
				IDictionary<string, string[]> errorsForProperties = this._modelValidator.GetErrors();
				foreach (KeyValuePair<string, string[]> errorsForProperty in errorsForProperties)
				{
					string propertyName = errorsForProperty.Key;
					foreach (string errorMessage in errorsForProperty.Value)
					{
						ViewData.ModelState.AddModelError(propertyName, errorMessage);
					}
				}
				return false;
			}
			return true;
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			SetCurrentSite();
			InitMessageViewData();
			InitMenuViewData();
			base.OnActionExecuting(filterContext);
		}

		protected virtual void ShowMessage(string message)
		{
			ShowMessage(message, false);
		}

		protected virtual void ShowMessage(string message, bool persistForNextRequest)
		{
			RegisterMessage(MessageType.Message, message, persistForNextRequest);
		}

		protected virtual void ShowError(string error)
		{
			ShowError(error, false);
		}

		protected virtual void ShowError(string error, bool persistForNextRequest)
		{
			RegisterMessage(MessageType.Error, error, persistForNextRequest);
		}

		protected virtual void ShowException(Exception exception)
		{
			ShowException(exception, false);
		}

		protected virtual void ShowException(Exception exception, bool persistForNextRequest)
		{
			RegisterMessage(MessageType.Exception, exception.Message, persistForNextRequest);
			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
				RegisterMessage(MessageType.Exception, exception.Message, persistForNextRequest);
			}
		}

		private void SetCurrentSite()
		{
			if (this._siteService == null)
			{
				throw new InvalidOperationException("Unable to set the current site because the SiteService is unavailable");
			}
			this._cuyahogaContext.SetSite(this._siteService.GetSiteBySiteUrl(UrlHelper.GetSiteUrl()));
			// Also register partial request for the site chooser component.
			ViewData["SiteChooser"] = new PartialRequest(new
			{
				area ="Manager",
				controller = "Dashboard",
				action = "SiteChooser"
			});
		}

		private void InitMessageViewData()
		{
			if (TempData.ContainsKey("Messages"))
			{
				this._messageViewData = new MessageViewData((MessageViewData)TempData["Messages"]);
			}
			else
			{
				this._messageViewData = new MessageViewData();
			}
			ViewData["Messages"] = this._messageViewData;
		}

		private void RegisterMessage(string messageType, string message, bool persistForNextRequest)
		{
			string localizedMessage = GlobalResources.ResourceManager.GetString(message, Thread.CurrentThread.CurrentUICulture);
			this._messageViewData.AddMessage(messageType, localizedMessage ?? message, persistForNextRequest);
			if (persistForNextRequest)
			{
				// Just persist the messages directly.
				PersistMessages();
			}
		}

		private void PersistMessages()
		{
			if (this._messageViewData.HasFlashMessages)
			{
				TempData["Messages"] = this._messageViewData.GetFlashMessages();
			}
		}

		private void InitMenuViewData()
		{
			// TODO: extract this to some external component
			MainMenuViewData mainMenuViewData = new MainMenuViewData();
			User user = this._cuyahogaContext.CurrentUser;
			if (user != null && user.IsAuthenticated)
			{
				mainMenuViewData.AddStandardMenuItem(
					new MenuItem(this.Url.Action("Index", "Dashboard")
					, GlobalResources.ManagerMenuDashboard, CheckInPath("Dashboard")));
				if (user.HasRight(Rights.ManagePages, CuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddStandardMenuItem(
						new MenuItem(Url.Action("Index", "Pages")
						, GlobalResources.ManagerMenuPages, CheckInPath("Pages")));
				}
				if (user.HasRight(Rights.ManageFiles, CuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddStandardMenuItem(
						new MenuItem(Url.Action("Index", "Files")
						, GlobalResources.ManagerMenuFiles, CheckInPath("Files")));
				}
				if (user.HasRight(Rights.ManageUsers, CuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddOptionalMenuItem(
						new MenuItem(Url.Action("Index", "Users")
						, GlobalResources.ManagerMenuUsers, CheckInPath("Users")));
				}
				if (user.HasRight(Rights.ManageSite, CuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddOptionalMenuItem(
						new MenuItem(Url.Action("Index", "Site")
						, GlobalResources.ManagerMenuSite, CheckInPath("Site")));
				}
				if (user.HasRight(Rights.ManageServer, CuyahogaContext.CurrentSite))
				{
					mainMenuViewData.AddOptionalMenuItem(
						new MenuItem(Url.Action("Index", "Server")
						, GlobalResources.ManagerMenuServer, CheckInPath("Server")));
				}
			}
			ViewData.Add("MainMenuViewData", mainMenuViewData);
		}

		private bool CheckInPath(string controllerName)
		{
			return this.RouteData.Values["controller"].ToString().ToLower().Contains(controllerName.ToLower());
		}
	}
}