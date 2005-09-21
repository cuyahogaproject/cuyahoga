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

using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Admin.UI;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for SiteAliasEdit.
	/// </summary>
	public class SiteAliasEdit : AdminBasePage
	{
		protected System.Web.UI.WebControls.TextBox txtUrl;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnCancel;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.WebControls.DropDownList ddlEntryNodes;
		private SiteAlias _activeSiteAlias;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.Title = "Edit site alias";

			if (Context.Request.QueryString["SiteAliasId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["SiteAliasId"]) == -1)
				{
					// Create a new site alias instance
					this._activeSiteAlias = new SiteAlias();
					if (Context.Request.QueryString["SiteId"] != null)
					{
						this._activeSiteAlias.Site = (Site)base.CoreRepository.GetObjectById(typeof(Cuyahoga.Core.Domain.Site)
							, Int32.Parse(Context.Request.QueryString["SiteId"]));
					}
					else
					{
						throw new Exception("No site specified for the new alias.");
					}
					this.btnDelete.Visible = false;
				}
				else
				{
					// Get site alias data
					this._activeSiteAlias = (SiteAlias)base.CoreRepository.GetObjectById(typeof(Cuyahoga.Core.Domain.SiteAlias)
						, Int32.Parse(Context.Request.QueryString["SiteAliasId"]));
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?')");
				}
				if (! this.IsPostBack)
				{
					BindSiteAliasControls();
					BindAvailableNodes();
				}
			}
		}

		private void BindSiteAliasControls()
		{
			this.txtUrl.Text = this._activeSiteAlias.Url;
		}

		private void BindAvailableNodes()
		{
			// First create an option for the default root node of the site.
			ListItem li = new ListItem("-- inherit from site --", "-1");
			this.ddlEntryNodes.Items.Add(li);
			IList rootNodes = this._activeSiteAlias.Site.RootNodes;
			AddAvailableNodes(rootNodes);
		}

		private void AddAvailableNodes(IList nodes)
		{
			foreach (Node node in nodes)
			{
				int indentSpaces = node.Level * 5;
				string itemIndentSpaces = String.Empty;
				for (int i = 0; i < indentSpaces; i++)
				{
					itemIndentSpaces += "&nbsp;";
				}
				ListItem li = new ListItem(Context.Server.HtmlDecode(itemIndentSpaces) + node.Title, node.Id.ToString());
				li.Selected = (this._activeSiteAlias.EntryNode != null && this._activeSiteAlias.EntryNode.Id == node.Id);
				this.ddlEntryNodes.Items.Add(li);
				if (node.ChildNodes.Count > 0)
				{
					AddAvailableNodes(node.ChildNodes);
				}
			}
		}

		private void SaveSiteAlias()
		{
			base.CoreRepository.ClearQueryCache("Sites");

			if (this._activeSiteAlias.Id > 0)
			{
				base.CoreRepository.UpdateObject(this._activeSiteAlias);
			}
			else
			{
				base.CoreRepository.SaveObject(this._activeSiteAlias);
			}
			Context.Response.Redirect("SiteEdit.aspx?SiteId=" + this._activeSiteAlias.Site.Id.ToString());
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("SiteEdit.aspx?SiteId=" + this._activeSiteAlias.Site.Id.ToString());
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				this._activeSiteAlias.Url = this.txtUrl.Text;
				if (this.ddlEntryNodes.SelectedIndex > 0)
				{
					int entryNodeId = Int32.Parse(this.ddlEntryNodes.SelectedValue);
					this._activeSiteAlias.EntryNode = (Node)base.CoreRepository.GetObjectById(typeof(Node), entryNodeId);
				}
				else
				{
					this._activeSiteAlias.EntryNode = null;
				}
				SaveSiteAlias();
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			base.CoreRepository.ClearQueryCache("Sites");

			if (this._activeSiteAlias != null)
			{
				try
				{
					base.CoreRepository.DeleteObject(this._activeSiteAlias);
					Context.Response.Redirect("SiteEdit.aspx?SiteId=" + this._activeSiteAlias.Site.Id.ToString());
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
		}
	}
}
