using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Configuration;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Admin.UI
{
	/// <summary>
	/// The base class for the Cuyahoga admin pages. Provides generic functionality.
	/// </summary>
	public class AdminBasePage : GenericBasePage
	{
		private Node _activeNode;

		public Node ActiveNode
		{
			get { return this._activeNode; }
			set { this._activeNode = value; }
		}

		public HtmlGenericControl MessageBox
		{
			get
			{
				return this.TemplateControl.FindControl("MessageBox") as HtmlGenericControl;
			}
		}

		/// <summary>
		/// Get the application root directory with trailing "/"
		/// TODO: replace with Util.UrlHelper.GetApplicationPath where used
		/// </summary>
		public string ApplicationRoot
		{
			get
			{
				if (Context.Request.ApplicationPath.Length != 1)
					return Context.Request.ApplicationPath + "/";
				else
					return Context.Request.ApplicationPath;
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public AdminBasePage()
		{
			this._activeNode = null;
		}

		protected override void OnInit(EventArgs e)
		{
			// Set template dir, template filename and css.
			base.TemplateDir = ConfigurationSettings.AppSettings["TemplateDir"];
			base.TemplateFilename = ConfigurationSettings.AppSettings["DefaultTemplate"];
			base.Css = ConfigurationSettings.AppSettings["DefaultCss"];

			// Try to set active Node.
			if (Context.Request.QueryString["NodeId"] != null)
			{
				int nodeId = Int32.Parse(Context.Request.QueryString["NodeId"]);
				if (nodeId > 0)
				{
					this._activeNode = (Node)base.CoreRepository.GetObjectById(typeof(Node), nodeId);
				}
			}
			// Now, we're here we could check authorization as well.
			if (! ((User)this.User.Identity).HasPermission(AccessLevel.Administrator))
			{
				throw new ActionForbiddenException("The logged-in user has insufficient rights to access the site administration");
			}

			base.OnInit (e);
		}

		/// <summary>
		/// Try to find the MessageBox control, insert the errortext and set visibility to true.
		/// </summary>
		/// <param name="errorText"></param>
		public void ShowError(string errorText)
		{
			if (this.MessageBox != null)
			{
				this.MessageBox.InnerText = "An error occured: " + errorText;
				this.MessageBox.Attributes["class"] = "errorbox";
				this.MessageBox.Visible = true;
			}
			else
			{
				// Throw an Exception and hope it will be handled by the global application exception handler.
				throw new Exception(errorText);
			}
		}

		/// <summary>
		/// Try to find the MessageBox control, insert the message and set visibility to true.
		/// </summary>
		/// <param name="message"></param>
		public void ShowMessage(string message)
		{
			if (this.MessageBox != null)
			{
				this.MessageBox.InnerText = message;
				this.MessageBox.Attributes["class"] = "messagebox";
				this.MessageBox.Visible = true;
			}
			// TODO: change the class attribute to make a difference with the error (nice background image?)
		}
	}
}
