using System;
using Castle.MonoRail.Framework;
using Cuyahoga.Web.Components.MonoRail;

namespace Cuyahoga.Web.Manager.ViewComponents
{
	public class MessagesComponent : ViewComponent
	{
		public override void Render()
		{
			foreach (string messageType in MessageType.GetTypes())
			{
				if (PropertyBag[messageType] != null)
				{
					if (messageType == MessageType.Exception)
					{
						IResource globalTexts = RailsContext.CurrentController.Resources["globaltext"];
						Exception exception = (Exception) PropertyBag[messageType];
						PropertyBag["exceptionmessage"] = globalTexts.GetString(exception.Message);
						if (exception.InnerException != null)
						{
							PropertyBag["innerexceptionmessage"] = globalTexts.GetString(exception.InnerException.Message);
						}
						PropertyBag["exceptiondetails"] = exception.ToString();
					}
					RenderView(messageType);
				}
			}
		}
	}
}
