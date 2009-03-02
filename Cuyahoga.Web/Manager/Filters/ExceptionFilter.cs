using System;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Mvc.Localization;
using Cuyahoga.Web.Mvc.ViewModels;

namespace Cuyahoga.Web.Manager.Filters
{
	/// <summary>
	/// Filter attribute that allows for more graceful error handling, including localized exceptions.
	/// </summary>
	public class ExceptionFilter : HandleErrorAttribute
	{
		private readonly ILocalizer _localizer;
		private readonly ILogger _logger = NullLogger.Instance;

		public ExceptionFilter() : this(IoC.Resolve<ILocalizer>(), IoC.Resolve<ILogger>())
		{
		}

		public ExceptionFilter(ILocalizer localizer, ILogger logger)
		{
			this._localizer = localizer;
			this._logger = logger;
		}
		public override void OnException(ExceptionContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}

			// If custom errors are disabled, we need to let the normal ASP.NET exception handler
			// execute so that the user can see useful debugging information.
			if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
			{
				return;
			}

			Exception exception = filterContext.Exception;

			// If this is not an HTTP 500 (for example, if somebody throws an HTTP 404 from an action method),
			// ignore it.
			if (new HttpException(null, exception).GetHttpCode() != 500)
			{
				return;
			}

			if (!ExceptionType.IsInstanceOfType(exception))
			{
				return;
			}

			string controllerName = (string)filterContext.RouteData.Values["controller"];
			string actionName = (string)filterContext.RouteData.Values["action"];
			HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

			if (this._logger.IsErrorEnabled)
			{
				this._logger.Error(string.Format("An unexpected error occured while executing {0} in {1}.", actionName, controllerName), exception);
			}

			MessageViewData messageViewData = new MessageViewData();

			while (exception != null)
			{
				messageViewData.AddErrorMessage(this._localizer.GetString(exception.Message));
				exception = exception.InnerException;
			}
			var viewData = new ViewDataDictionary<HandleErrorInfo>(model);
			viewData["Messages"] = messageViewData;

			// Render error view
			filterContext.Result = new ViewResult
			                       	{
			                       		ViewName = View,
			                       		MasterName = Master,
			                       		ViewData = viewData,
			                       		TempData = filterContext.Controller.TempData
			                       	};

			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = 500;
		}
	}
}