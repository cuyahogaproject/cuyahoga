using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Web.Mvc;
using Resources.Cuyahoga.Web.Manager;
using MessageViewData=Cuyahoga.Web.Manager.Model.ViewModels.MessageViewData;

namespace Cuyahoga.Web.Manager.Filters
{
	/// <summary>
	/// Filter attribute that allows for more graceful error handling, including localized exceptions.
	/// </summary>
	public class ExceptionFilter : HandleErrorAttribute
	{
		private readonly MessageViewData _messageViewData = new MessageViewData();

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

			RegisterMessage(MessageType.Exception, exception.Message);
			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
				RegisterMessage(MessageType.Exception, exception.Message);
			}

			var viewData = new ViewDataDictionary<HandleErrorInfo>(model);
			viewData["Messages"] = this._messageViewData;

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

		private void RegisterMessage(string messageType, string message)
		{
			string localizedMessage = GlobalResources.ResourceManager.GetString(message, Thread.CurrentThread.CurrentUICulture);
			this._messageViewData.AddMessage(messageType, localizedMessage ?? message);
		}
	}
}