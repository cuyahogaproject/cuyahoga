using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Mvc.Localization;
using Cuyahoga.Web.Mvc.ViewModels;

namespace Cuyahoga.Web.Mvc.Filters
{
	public class MessagesFilter : ActionFilterAttribute
	{
		private MessageViewData _messageViewData;
		private TempDataDictionary _tempData;
		private ILocalizer _localizer;

		public MessagesFilter() : this(IoC.Resolve<ILocalizer>())
		{
		}

		public MessagesFilter(ILocalizer localizer)
		{
			this._localizer = localizer;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			InitMessageViewData(filterContext);
			base.OnActionExecuting(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			DisplayModelStateErrors(filterContext.Controller);
			base.OnActionExecuted(filterContext);
		}

		private void InitMessageViewData(ActionExecutingContext filterContext)
		{
			var currentController = filterContext.Controller;
			this._tempData = currentController.TempData;
			if (this._tempData.ContainsKey("Messages"))
			{
				this._messageViewData = new MessageViewData((MessageViewData)this._tempData["Messages"]);
			}
			else
			{
				this._messageViewData = new MessageViewData();
			}
			this._messageViewData.FlashMessageAdded += new EventHandler(MessageViewData_FlashMessageAdded);
			currentController.ViewData["Messages"] = this._messageViewData;
		}

		private void MessageViewData_FlashMessageAdded(object sender, EventArgs e)
		{
			// HACK: Immediately store MessageViewData in TempData when a flash message is added. We can't do this
			// in OnActionExecuted because it is not fired in case of exceptions.
			this._tempData["Messages"] = this._messageViewData;
		}

		private void DisplayModelStateErrors(ControllerBase currentController)
		{
			// Show the ModelState errors in the standard Cuyahoga errorbox.
			if (!currentController.ViewData.ModelState.IsValid)
			{
				string generalMessage = this._localizer.GetString("ModelValidationErrorMessage");
				TagBuilder errorList = new TagBuilder("ul");
				StringBuilder errorSummary = new StringBuilder();
				foreach (KeyValuePair<string, ModelState> modelStateKvp in currentController.ViewData.ModelState)
				{
					foreach (ModelError modelError in modelStateKvp.Value.Errors)
					{
						TagBuilder listItem = new TagBuilder("li");
						string baseName = String.Format("{0}.globalresources"
							, currentController.GetType().Namespace.Replace(".Controllers", String.Empty).ToLowerInvariant());
						listItem.SetInnerText(this._localizer.GetString(modelError.ErrorMessage, baseName));
						errorSummary.AppendLine(listItem.ToString(TagRenderMode.Normal));
					}
				}
				errorList.InnerHtml = errorSummary.ToString();
				this._messageViewData.AddErrorMessage(generalMessage + errorList.ToString(TagRenderMode.Normal));
			}
			this._messageViewData.FlashMessageAdded -= new EventHandler(MessageViewData_FlashMessageAdded);
		}
	}
}
