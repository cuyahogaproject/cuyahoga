using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Mvc.Localization;
using Cuyahoga.Web.Mvc.ViewModels;

namespace Cuyahoga.Web.Mvc.Filters
{
	public class LocalizationFilter : ActionFilterAttribute
	{
		private readonly ILocalizer _localizer;

		public LocalizationFilter() : this(IoC.Resolve<ILocalizer>())
		{
		}

		public LocalizationFilter(ILocalizer localizer)
		{
			this._localizer = localizer;
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			TranslateMessages(filterContext);
		}

		private void TranslateMessages(ActionExecutedContext filterContext)
		{
			if (filterContext.Controller.ViewData.ContainsKey("Messages"))
			{
				MessageViewData messageViewData = (MessageViewData) filterContext.Controller.ViewData["Messages"];
				foreach (KeyValuePair<string, IList<string>> messagesForType in messageViewData.Messages)
				{
					for (int i = 0; i < messagesForType.Value.Count; i++)
					{
						string baseName = String.Format("resources.{0}.globalresources"
							, filterContext.Controller.GetType().Namespace.Replace(".Controllers", String.Empty).ToLowerInvariant());
						string originalMessage = messagesForType.Value[i];
						string translatedMessage = this._localizer.GetString(originalMessage, baseName);
						if (translatedMessage != originalMessage)
						{
							messagesForType.Value[i] = translatedMessage;
							// Change the key of the messageParams if there were params for the original key
							if (messageViewData.MessageParams.ContainsKey(originalMessage))
							{
								messageViewData.MessageParams.Add(translatedMessage, messageViewData.MessageParams[originalMessage]);
								messageViewData.MessageParams.Remove(originalMessage);
							}
						}
					}
				}
			}
		}
	}
}
