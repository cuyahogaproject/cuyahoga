using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Support.FreeTextBox.Custom
{
	/// <summary>
	/// Popup link browser for FreeTextbox 2.0.x and Cuyahoga.
	/// </summary>
	public class LinkBrowser : System.Web.UI.Page
	{
		private CoreRepository _coreRepository;

		protected System.Web.UI.WebControls.TextBox txtUrl;
		protected System.Web.UI.WebControls.TextBox txtDescription;
		protected System.Web.UI.WebControls.PlaceHolder plhNodes;
		protected System.Web.UI.WebControls.DropDownList ddlTarget;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this._coreRepository = new CoreRepository(true);
			string descr = Context.Request.QueryString["descr"];
			this.txtDescription.Text = descr;
			BuildNodeTree();
		}

		private void BuildNodeTree()
		{
			// Build tree, starting with root nodes
			CoreRepository cr = new CoreRepository(true);
			IList nodes = cr.GetRootNodes();
			DisplayNodes(nodes);			
			cr.CloseSession();
		}

		private void DisplayNodes(IList nodes)
		{
			foreach (Node node in nodes)
			{
				if (node.ViewAllowed((Cuyahoga.Core.Domain.User)this.User.Identity))
				{
					this.plhNodes.Controls.Add(CreateDisplayNode(node));
					// Expand all nodes
					DisplayNodes(node.ChildNodes);
				}
			}
		}

		private Control CreateDisplayNode(Node node)
		{
			int indent = node.Level * 20;
			HtmlGenericControl container = new HtmlGenericControl("div");
			container.Attributes.Add("style", String.Format("padding-left: {0}px", indent.ToString()));
			LinkButton lbt = new LinkButton();
			lbt.Text = node.Title;
			lbt.CommandName = "select";
			lbt.CommandArgument = node.Id.ToString();
			lbt.Command += new CommandEventHandler(NodeLinkButton_Command);
			container.Controls.Add(lbt);

			return container;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Unload += new System.EventHandler(this.LinkBrowser_Unload);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void NodeLinkButton_Command(object sender, CommandEventArgs e)
		{
			int nodeId = Int32.Parse(e.CommandArgument.ToString());
			Node node = (Node)this._coreRepository.GetObjectById(typeof(Node), nodeId);
			this.txtUrl.Text = UrlHelper.GetFriendlyUrlFromNode(node);
			// We don't want to overwrite the description when the user already selected a text
			// for the description of the link.
			if (Context.Request.QueryString["descr"] == null || Context.Request.QueryString["descr"] == "")
			{
				this.txtDescription.Text = node.Title;
			}
		}

		private void LinkBrowser_Unload(object sender, System.EventArgs e)
		{
			this._coreRepository.CloseSession();
		}
	}
}
