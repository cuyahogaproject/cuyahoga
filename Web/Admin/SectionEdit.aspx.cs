using System;
using System.Collections;
using System.ComponentModel;
// using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.DAL;
using Cuyahoga.Core.Collections;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for SectionEdit.
	/// </summary>
	public class SectionEdit : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		private Section _activeSection = null;

		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.CheckBox chkShowTitle;
		protected System.Web.UI.WebControls.DropDownList ddlModule;
		protected System.Web.UI.WebControls.Label lblModule;
		protected System.Web.UI.WebControls.DropDownList ddlPlaceholder;
		protected System.Web.UI.WebControls.TextBox txtCacheDuration;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.HyperLink hplLookup;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTitle;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvCache;
		protected System.Web.UI.WebControls.CompareValidator cpvCache;
		protected System.Web.UI.WebControls.Repeater rptRoles;
		protected System.Web.UI.WebControls.Button btnCancel;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Edit section";

			if (Context.Request.QueryString["SectionId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["SectionId"]) == -1)
				{
					// Create a new section instance
					this._activeSection = new Section();
					this._activeSection.Node = this.ActiveNode;
					if (! this.IsPostBack)
					{
						this._activeSection.CopyRolesFromNode();
					}
				}
				else
				{
					// Get section data
					this._activeSection = (Section)base.CoreRepository.GetObjectById(typeof(Section), 
						Int32.Parse(Context.Request.QueryString["SectionId"]));
				}

				if (! this.IsPostBack)
				{
					BindSectionControls();
					BindModules();
					BindPlaceholders();
					BindRoles();
				}
			}		
		}

		private void BindSectionControls()
		{
			this.txtTitle.Text = this._activeSection.Title;
			this.chkShowTitle.Checked = this._activeSection.ShowTitle;
			this.txtCacheDuration.Text = this._activeSection.CacheDuration.ToString();
		}

		private void BindModules()
		{
			if (this._activeSection.ModuleType != null)
			{
				// A module is attached, there could be data already in it, so we don't give the option to change it
				this.lblModule.Text = this._activeSection.ModuleType.Name;
				this.ddlModule.Visible = false;
				this.lblModule.Visible = true;
			}
			else
			{
				IList availableModules = base.CoreRepository.GetAll(typeof(ModuleType), "Name");
				foreach (ModuleType moduleType in availableModules)
				{
					this.ddlModule.Items.Add(new ListItem(moduleType.Name, moduleType.ModuleTypeId.ToString()));
				}
				if (this._activeSection.ModuleType != null)
				{
					this.ddlModule.Items.FindByValue(this._activeSection.ModuleType.ModuleTypeId.ToString()).Selected = true;
				}
				this.ddlModule.Visible = true;
				this.lblModule.Visible = false;
			}
		}

		private void BindPlaceholders()
		{
			if (this.ActiveNode.Template != null)
			{
				try
				{
					// Read template control and get the containers (placeholders)
					string templatePath = this.ApplicationRoot + this.ActiveNode.Template.Path;
					BaseTemplate template = (BaseTemplate)this.LoadControl(templatePath);
					this.ddlPlaceholder.DataSource = template.Containers;
					this.ddlPlaceholder.DataValueField = "Key";
					this.ddlPlaceholder.DataTextField = "Key";
					this.ddlPlaceholder.DataBind();
					if (this._activeSection.PlaceholderId != null && this._activeSection.PlaceholderId != "")
						this.ddlPlaceholder.Items.FindByValue(this._activeSection.PlaceholderId).Selected = true;
					// Create url for lookup
					this.hplLookup.NavigateUrl = "javascript:;";
					this.hplLookup.Attributes.Add("onClick", String.Format("window.open(\"TemplatePreview.aspx?Template={0}&Control={1}\", \"Preview\", \"width=760 height=400\")", templatePath, this.ddlPlaceholder.ClientID));
				}
				catch (Exception ex)
				{
					this.ShowError(ex.Message);
				}
			}
		}

		private void BindRoles()
		{
			RoleCollection roles = new RoleCollection();
			CmsDataFactory.GetInstance().GetAllRoles(roles);
			this.rptRoles.ItemDataBound += new RepeaterItemEventHandler(rptRoles_ItemDataBound);
			this.rptRoles.DataSource = roles;
			this.rptRoles.DataBind();
		}

		private void SaveSection()
		{
			if (this._activeSection.Id > 0)
			{
				base.CoreRepository.UpdateObject(this._activeSection);
			}
			else
			{
				base.CoreRepository.SaveObject(this._activeSection);
				Context.Response.Redirect(String.Format("SectionEdit.aspx?SectionId={0}&NodeId={1}", this._activeSection.Id, this.ActiveNode.Id));
			}
		}

		private void SetRoles()
		{
//			this._activeSection.ViewRoles.Clear();
//			this._activeSection.EditRoles.Clear();
//
//			foreach (RepeaterItem ri in rptRoles.Items)
//			{	
//				// HACK: RoleId is stored in the ViewState because the repeater doesn't have a DataKeys property.
//				// Another HACK: we're only using the role id's to save database roundtrips.
//				Role role = new Role();
//				role.Id = (int)this.ViewState[ri.ClientID];
//				CheckBox chkView = (CheckBox)ri.FindControl("chkViewAllowed");
//				if (chkView.Checked)
//					this._activeSection.ViewRoles.Add(role);
//				CheckBox chkEdit = (CheckBox)ri.FindControl("chkEditAllowed");
//				if (chkEdit.Checked)
//					this._activeSection.EditRoles.Add(role);
//			}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect(String.Format("NodeEdit.aspx?NodeId={0}", this.ActiveNode.Id));
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				// Remember the previous PlaceholderId and Position to detect changes
				string oldPlaceholderId = this._activeSection.PlaceholderId;
				int oldPosition = this._activeSection.Position;

				try
				{
					this._activeSection.Title = this.txtTitle.Text;
					this._activeSection.ShowTitle = this.chkShowTitle.Checked;
					this._activeSection.Node = this.ActiveNode;
					if (this.ddlModule.Visible)
					{
						this._activeSection.ModuleType = (ModuleType)CoreRepository.GetObjectById(
							typeof(ModuleType), Int32.Parse(this.ddlModule.SelectedValue));
					}
					this._activeSection.PlaceholderId = this.ddlPlaceholder.SelectedValue;
					this._activeSection.CacheDuration = Int32.Parse(this.txtCacheDuration.Text);

					// Calculate new position if the section is new or when the PlaceholderId has changed
					if (this._activeSection.Id == -1 || this._activeSection.PlaceholderId != oldPlaceholderId)
						this._activeSection.CalculateNewPosition();

					// Roles
					SetRoles();

					// Save the active section
					SaveSection();

					// Detect a placeholderId change and change positions of neighbour sections if necessary.					
					if (oldPosition != -1 && oldPlaceholderId != this._activeSection.PlaceholderId)
						this._activeSection.ChangeAndUpdatePositionsAfterPlaceholderChange(oldPlaceholderId, oldPosition);

					ShowMessage("Section saved.");
				}
				catch (Exception ex)
				{
					this.ShowError(ex.Message);
				}

			}
		}

		private void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Role role = e.Item.DataItem as Role;
			if (role != null)
			{
//				CheckBox chkView = (CheckBox)e.Item.FindControl("chkViewAllowed");
//				chkView.Checked = this._activeSection.ViewRoles.Contains(role);
//				CheckBox chkEdit = (CheckBox)e.Item.FindControl("chkEditAllowed");
//				if (role.HasPermission(AccessLevel.Editor) || role.HasPermission(AccessLevel.Administrator))
//				{
//					chkEdit.Checked = this._activeSection.EditRoles.Contains(role);
//				}
//				else
//				{
//					chkEdit.Visible = false;
//				}
//				// Add RoleId to the ViewState with the ClientID of the repeateritem as key.
//				this.ViewState[e.Item.ClientID] = role.Id;
			}
		}
	}
}
