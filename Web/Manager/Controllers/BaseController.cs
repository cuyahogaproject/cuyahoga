using System;
using System.Security;
using System.Threading;
using System.Web.Mvc;
using Castle.Components.Validator;
using Castle.Core.Logging;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
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
		private IValidatorRunner _validatorRunner;
		private ILogger _logger = NullLogger.Instance;
		private MessageViewData _messageViewData;

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
		/// Sets the Validator registry.
		/// </summary>
		public IValidatorRegistry ValidatorRegistry
		{
			set { this._validatorRunner = new ValidatorRunner(value); }
		}

		/// <summary>
		/// The validator runner.
		/// </summary>
		public IValidatorRunner ValidatorRunner
		{
			get { return this._validatorRunner; }
		}

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		public ILogger Logger
		{
			get { return this._logger; }
			set { this._logger = value; }
		}

		protected virtual bool ValidateModel(object objectToValidate)
		{
			if (! ValidatorRunner.IsValid(objectToValidate))
			{
				ErrorSummary errorSummary = ValidatorRunner.GetErrorSummary(objectToValidate);
				if (errorSummary.HasError)
				{
					foreach (string property in errorSummary.InvalidProperties)
					{
						var errorsForProperty = errorSummary.GetErrorsForProperty(property);
						foreach (var error in errorsForProperty)
						{
							ViewData.ModelState.AddModelError(property, error);
						}
					}
				}
				return false;
			}
			return true;
		}

		protected virtual void ShowMessage(string message)
		{
			RegisterMessage(MessageType.Message, message);
		}

		protected virtual void ShowError(string error)
		{
			RegisterMessage(MessageType.Error, error);
		}

		protected virtual void ShowException(Exception exception)
		{
			RegisterMessage(MessageType.Exception, exception.Message);
			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
				RegisterMessage(MessageType.Exception, exception.Message);
			}
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			SetCurrentSite();
			InitMessageViewData();
			InitMenuViewData();
			base.OnActionExecuting(filterContext);
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
			this._messageViewData = new MessageViewData();
			ViewData["Messages"] = this._messageViewData;
		}

		private void RegisterMessage(string messageType, string message)
		{
			string localizedMessage = GlobalResources.ResourceManager.GetString(message, Thread.CurrentThread.CurrentUICulture);
			this._messageViewData.AddMessage(messageType, localizedMessage ?? message);
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