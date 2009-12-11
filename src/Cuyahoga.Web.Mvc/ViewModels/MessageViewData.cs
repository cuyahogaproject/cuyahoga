using System;
using System.Collections.Generic;

namespace Cuyahoga.Web.Mvc.ViewModels
{
	public class MessageViewData
	{
		private readonly IDictionary<string, IList<string>> _messages = new Dictionary<string, IList<string>>();
		private readonly IDictionary<string, IList<string>> _flashMessages = new Dictionary<string, IList<string>>();
		private readonly IDictionary<string, object[]> _messageParams = new Dictionary<string, object[]>();

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
		/// Gets a dictionary of message parameters. The corresponding message is the key.
		/// </summary>
		public IDictionary<string, object[]> MessageParams
		{
			get { return _messageParams; }
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
		/// Gets all messages merged with optional message parameters.
		/// </summary>
		/// <returns></returns>
		public IDictionary<string, IList<string>> GetDisplayMessages()
		{
			IDictionary<string, IList<string>> displayMessages = new Dictionary<string, IList<string>>();
			foreach (KeyValuePair<string, IList<string>> messagesForKey in _messages)
			{
				displayMessages.Add(messagesForKey.Key, new List<string>());
				foreach (string message in messagesForKey.Value)
				{
					if (this._messageParams.ContainsKey(message))
					{
						displayMessages[messagesForKey.Key].Add(String.Format(message, this._messageParams[message]));
					}
					else
					{
						displayMessages[messagesForKey.Key].Add(message);
					}
				}
			}
			return displayMessages;
		}

		/// <summary>
		/// Notifies when a flash message is added.
		/// </summary>
		public event EventHandler FlashMessageAdded;

		protected void OnFlashMessageAdded()
		{
			if (FlashMessageAdded != null)
			{
				FlashMessageAdded(this, EventArgs.Empty);
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
				foreach (KeyValuePair<string, object[]> messageParam in messagesFromPreviousRequest.MessageParams)
				{
					this._messageParams.Add(messageParam.Key, messageParam.Value);
				}
			}
		}

		/// <summary>
		/// Add a standard message.
		/// </summary>
		/// <param name="message"></param>
		public void AddMessage(string message)
		{
			AddMessage(message, MessageType.Message, false);
		}

		/// <summary>
		/// Add a standard message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="parameters"></param>
		public void AddMessageWithParams(string message, params object[] parameters)
		{
			AddMessage(message, MessageType.Message, false);
			this._messageParams[message] = parameters;
		}


		/// <summary>
		/// Add a standard message that will be shown after a redirect.
		/// </summary>
		/// <param name="message"></param>
		public void AddFlashMessage(string message)
		{
			AddMessage(message, MessageType.Message, true);
		}

		/// <summary>
		/// Add a standard message that will be shown after a redirect.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="parameters"></param>
		public void AddFlashMessageWithParams(string message, params object[] parameters)
		{
			AddMessage(message, MessageType.Message, true);
			this._messageParams[message] = parameters;
		}


		/// <summary>
		/// Add an error message.
		/// </summary>
		/// <param name="errorMessage"></param>
		public void AddErrorMessage(string errorMessage)
		{
			AddMessage(errorMessage, MessageType.Error);
		}

		/// <summary>
		/// Add an error message.
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="parameters"></param>
		public void AddErrorMessageWithParams(string errorMessage, params object[] parameters)
		{
			AddMessage(errorMessage, MessageType.Error);
			this._messageParams[errorMessage] = parameters;
		}

		/// <summary>
		/// Add an error message that will be shown after a redirect.
		/// </summary>
		/// <param name="errorMessage"></param>
		public void AddErrorFlashMessage(string errorMessage)
		{
			AddMessage(errorMessage, MessageType.Error, true);
		}

		/// <summary>
		/// Add an error message that will be shown after a redirect.
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="parameters"></param>
		public void AddErrorFlashMessageWithParams(string errorMessage, params object[] parameters)
		{
			AddMessage(errorMessage, MessageType.Error, true);
			this._messageParams[errorMessage] = parameters;
		}

		public void AddException(Exception exception)
		{
			AddException(exception, false);
		}

		/// <summary>
		/// Add an exception.
		/// </summary>
		/// <param name="exception"></param>
		public void AddFlashException(Exception exception)
		{
			AddException(exception, true);
		}

		private void AddException(Exception exception, bool persistForNextRequest)
		{
			AddMessage(exception.Message, MessageType.Exception, persistForNextRequest);
			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
				AddMessage(exception.Message, MessageType.Exception, persistForNextRequest);
			}
		}

		/// <summary>
		/// Add a message for the given <see cref="MessageType"></see>.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="messageType"></param>
		private void AddMessage(string message, string messageType)
		{
			AddMessage(message, messageType, false);
		}

		/// <summary>
		/// Add a message for the given <see cref="MessageType"></see>.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="messageType"></param>
		/// <param name="persistForNextRequest"></param>
		private void AddMessage(string message, string messageType, bool persistForNextRequest)
		{
			this._messages[messageType].Add(message);
			if (persistForNextRequest)
			{
				this._flashMessages[messageType].Add(message);
				OnFlashMessageAdded();
			}
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