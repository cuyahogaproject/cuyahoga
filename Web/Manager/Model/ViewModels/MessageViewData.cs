using System.Collections.Generic;
using Cuyahoga.Web.Mvc;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class MessageViewData
	{
		private IDictionary<string, IList<string>> _messages = new Dictionary<string, IList<string>>();
		private IDictionary<string, IList<string>> _flashMessages = new Dictionary<string, IList<string>>();

		/// <summary>
		/// Gets a dictionary of all messages that should be displayed to the user, including the ones that come 
		/// from the previous request (TempData).
		/// </summary>
		public IDictionary<string, IList<string>> Messages
		{
			get { return _messages; }
		}

		/// <summary>
		/// Gets a dictionary of messages that are added during the current request.
		/// </summary>
		public IDictionary<string, IList<string>> FlashMessages
		{
			get { return _flashMessages; }
		}

		/// <summary>
		/// Indicates if there are any messages added that need to be persited for the next request.
		/// </summary>
		public bool HasFlashMessages
		{
			get 
			{
				return (this._flashMessages[MessageType.Message].Count
					+ this._flashMessages[MessageType.Error].Count
					+ this._flashMessages[MessageType.Exception].Count) > 0;
			}
		}

		/// <summary>
		/// Creates a new instance of the <see cref="MessageViewData"></see> class.
		/// </summary>
		public MessageViewData()
		{
			Initialize();
		}

		/// <summary>
		/// Creates a new instance of the <see cref="MessageViewData"></see> class.
		/// </summary>
		/// <param name="messagesFromPreviousRequest">Messages from the previous request (TempData)</param>
		public MessageViewData(MessageViewData messagesFromPreviousRequest)
		{
			Initialize();

			if (messagesFromPreviousRequest != null)
			{
				foreach (KeyValuePair<string, IList<string>> messages in messagesFromPreviousRequest.Messages)
				{
					this._messages[messages.Key] = messages.Value;
				}
			}
		}

		/// <summary>
		/// Add a message for the given <see cref="MessageType"></see>.
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="message"></param>
		public void AddMessage(string messageType, string message)
		{
			AddMessage(messageType, message, false);
		}

		/// <summary>
		/// Add a message for the given <see cref="MessageType"></see>.
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="message"></param>
		/// <param name="persistForNextRequest"></param>
		public void AddMessage(string messageType, string message, bool persistForNextRequest)
		{
			this._messages[messageType].Add(message);
			if (persistForNextRequest)
			{
				this._flashMessages[messageType].Add(message);
			}
		}

		/// <summary>
		/// Gets a <see cref="MessageViewData"></see> object for the messages that were added 
		/// during the current request.
		/// </summary>
		/// <returns></returns>
		public MessageViewData GetFlashMessages()
		{
			MessageViewData flashMessageViewData = new MessageViewData();
			foreach (KeyValuePair<string, IList<string>> flashMessagesForKey in this._flashMessages)
			{
				flashMessageViewData.Messages[flashMessagesForKey.Key] = flashMessagesForKey.Value;
			}
			return flashMessageViewData;
		}

		private void Initialize()
		{
			this._messages[MessageType.Message] = new List<string>();
			this._messages[MessageType.Error] = new List<string>();
			this._messages[MessageType.Exception] = new List<string>();
			this._flashMessages[MessageType.Message] = new List<string>();
			this._flashMessages[MessageType.Error] = new List<string>();
			this._flashMessages[MessageType.Exception] = new List<string>();
		}
	}
}