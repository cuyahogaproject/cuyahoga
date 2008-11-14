using System.Collections.Generic;
using Cuyahoga.Web.Mvc;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class MessageViewData
	{
		private IDictionary<string, IList<string>> _messages = new Dictionary<string, IList<string>>();

		public IDictionary<string, IList<string>> Messages
		{
			get { return _messages; }
		}

		public MessageViewData()
		{
			this._messages[MessageType.Message] = new List<string>();
			this._messages[MessageType.Error] = new List<string>();
			this._messages[MessageType.Exception] = new List<string>();
		}

		public void AddMessage(string messageType, string message)
		{
			this._messages[messageType].Add(message);
		}
	}
}