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
using Cuyahoga.Core.Service;
using Cuyahoga.Web.Admin.UI;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for MenuEdit.
	/// </summary>
	public class MenuEdit : AdminBasePage
	{
		private CustomMenu _activeMenu;

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.DropDownList ddlPlaceholder;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.Button btnBack;
		protected System.Web.UI.WebControls.ListBox lbxAvailableNodes;
		protected System.Web.UI.WebControls.ListBox lbxSelectedNodes;
		protected System.Web.UI.WebControls.Button btnAdd;
		protected System.Web.UI.WebControls.Button btnRemove;
		protected System.Web.UI.WebControls.Button btnUp;
		protected System.Web.UI.WebControls.Button btnDown;
		protected System.Web.UI.WebControls.Button btnDelete;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.Title = "Edit custom menu";
			if (Context.Request.QueryString["MenuId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["MenuId"]) == -1)
				{
					// Create a new CustomMenu instance
					this._activeMenu = new CustomMenu();
					this._activeMenu.RootNode = base.ActiveNode;
					this.btnDelete.Visible = false;
				}
				else
				{
					// Get Menu data
					this._activeMenu = (CustomMenu)base.CoreRepository.GetObjectById(typeof(CustomMenu), 
						Int32.Parse(Context.Request.QueryString["MenuId"]));
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onclick", "return confirm('Are you sure?');");
				}
			}
			if (! this.IsPostBack)
			{
				this.txtName.Text = this._activeMenu.Name;
				BindPlaceholders();
				BindAvailableNodes();
				BindSelectedNodes();
			}
		}

		private void BindPlaceholders()
		{
			string templatePath = Util.UrlHelper.GetApplicationPath() + this.ActiveNode.Template.Path;
			BaseTemplate template = (BaseTemplate)this.LoadControl(templatePath);
			this.ddlPlaceholder.DataSource = template.Containers;
			this.ddlPlaceholder.DataValueField = "Key";
			this.ddlPlaceholder.DataTextField = "Key";
			this.ddlPlaceholder.DataBind();
			if (this._activeMenu.Id != -1)
			{
				ListItem li = this.ddlPlaceholder.Items.FindByValue(this._activeMenu.Placeholder);
				if (li != null)
				{
					li.Selected = true;
				}
			}
		}

		private void BindAvailableNodes()
		{
			IList rootNodes = this.ActiveNode.Site.RootNodes;
			AddAvailableNodes(rootNodes);
		}

		private void BindSelectedNodes()
		{
			foreach (Node node in this._activeMenu.Nodes)
			{
				this.lbxSelectedNodes.Items.Add(new ListItem(node.Title, node.Id.ToString()));
				// also remove from available nodes
				ListItem item = this.lbxAvailableNodes.Items.FindByValue(node.Id.ToString());
				if (item != null)
				{
					this.lbxAvailableNodes.Items.Remove(item);
				}
			}
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
				this.lbxAvailableNodes.Items.Add(li);
				if (node.ChildNodes.Count > 0)
				{
					AddAvailableNodes(node.ChildNodes);
				}
			}
		}

		private void AttachSelectedNodes()
		{
			this._activeMenu.Nodes.Clear();
			foreach (ListItem item in this.lbxSelectedNodes.Items)
			{
				Node node = (Node)base.CoreRepository.GetObjectById(typeof(Node), Int32.Parse(item.Value));
				this._activeMenu.Nodes.Add(node);
			}
		}

		private void SaveMenu()
		{
			base.CoreRepository.ClearQueryCache("Menus");

			if (this._activeMenu.Id > 0)
			{
				base.CoreRepository.UpdateObject(this._activeMenu);
			}
			else
			{
				base.CoreRepository.SaveObject(this._activeMenu);
				Context.Response.Redirect(String.Format("NodeEdit.aspx?NodeId={0}", this.ActiveNode.Id));
			}
		}

		private void SynchronizeNodeListBoxes()
		{
			// First fetch a fresh list of available nodes because everything has to be
			// nice in place.
			this.lbxAvailableNodes.Items.Clear();
			BindAvailableNodes();
			// make sure the selected nodes are not in the available nodes list.
			int itemCount = this.lbxAvailableNodes.Items.Count;
			for (int i = itemCount - 1; i >= 0; i--)
			{
				ListItem item = this.lbxAvailableNodes.Items[i];
				if (this.lbxSelectedNodes.Items.FindByValue(item.Value) != null)
				{
					this.lbxAvailableNodes.Items.RemoveAt(i);
				}
			}
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
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("NodeEdit.aspx?NodeId=" + this.ActiveNode.Id.ToString());
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				this._activeMenu.Name = this.txtName.Text;
				this._activeMenu.Placeholder = this.ddlPlaceholder.SelectedValue;
				try
				{
					AttachSelectedNodes();
					SaveMenu();
					ShowMessage("Menu saved");
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._activeMenu != null)
			{
				base.CoreRepository.ClearQueryCache("Menus");

				try
				{
					base.CoreRepository.DeleteObject(this._activeMenu);
					Context.Response.Redirect("NodeEdit.aspx?NodeId=" + this.ActiveNode.Id.ToString());
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			ListItem item = this.lbxAvailableNodes.SelectedItem;
			if (item != null)
			{
				this.lbxSelectedNodes.Items.Add(item);
				item.Selected = false;
			}
			SynchronizeNodeListBoxes();
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			ListItem item = this.lbxSelectedNodes.SelectedItem;
			if (item != null)
			{
				this.lbxSelectedNodes.Items.Remove(this.lbxSelectedNodes.SelectedItem);
				item.Selected = false;
			}
			SynchronizeNodeListBoxes();
		}

		private void btnUp_Click(object sender, System.EventArgs e)
		{
			ListItem item = this.lbxSelectedNodes.SelectedItem;
			if (item != null)
			{
				int origPosition = this.lbxSelectedNodes.SelectedIndex;
				if (origPosition > 0)
				{
					this.lbxSelectedNodes.Items.Remove(item);
					this.lbxSelectedNodes.Items.Insert(origPosition - 1, item);
				}
			}
		}

		private void btnDown_Click(object sender, System.EventArgs e)
		{
			ListItem item = this.lbxSelectedNodes.SelectedItem;
			if (item != null)
			{
				int origPosition = this.lbxSelectedNodes.SelectedIndex;
				if (origPosition < this.lbxSelectedNodes.Items.Count -1)
				{
					this.lbxSelectedNodes.Items.Remove(item);
					this.lbxSelectedNodes.Items.Insert(origPosition + 1, item);
				}
			}
		}
	}
}
