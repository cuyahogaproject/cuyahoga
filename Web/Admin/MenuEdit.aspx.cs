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
		private Menu _activeMenu;

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.DropDownList ddlPlaceholder;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.Button btnBack;
		protected System.Web.UI.WebControls.Button btnDelete;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.Title = "Edit custom menu";
			if (Context.Request.QueryString["MenuId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["MenuId"]) == -1)
				{
					// Create a new Menu instance
					this._activeMenu = new Menu();
					this._activeMenu.RootNode = base.ActiveNode;
					this.btnDelete.Visible = false;
				}
				else
				{
					// Get Menu data
					this._activeMenu = (Menu)base.CoreRepository.GetObjectById(typeof(Menu), 
						Int32.Parse(Context.Request.QueryString["MenuId"]));
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?');");
				}
			}
			if (! this.IsPostBack)
			{
				this.txtName.Text = this._activeMenu.Name;
				BindPlaceholders();
			}
		}



		private void BindPlaceholders()
		{
			string templatePath = this.ApplicationRoot + this.ActiveNode.Template.Path;
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

		private void SaveMenu()
		{
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
	}
}
