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

using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Admin.UI;

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
		protected System.Web.UI.WebControls.Repeater rptCustomSettings;
		protected System.Web.UI.WebControls.PlaceHolder plcCustomSettings;
		protected System.Web.UI.WebControls.Button btnCancel;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Edit section";

			if (this._activeSection != null && ! this.IsPostBack)
			{
				BindSectionControls();
				BindModules();
				BindPlaceholders();
				BindCustomSettings();
				BindRoles();
			}		
		}

		/// <summary>
		/// Loads aan existing Section from the database or creates a new one if the SectionId = -1
		/// </summary>
		private void LoadSection()
		{
			// Note: Called from OnInit
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
			}
			// Create the controls for the ModuleType-specific settings.
			if (this._activeSection.ModuleType != null)
			{
				CreateCustomSettings();
			}
		}

		private void CreateCustomSettings()
		{
			if (this._activeSection.ModuleType.ModuleSettings.Count > 0)
			{
				foreach (ModuleSetting ms in this._activeSection.ModuleType.ModuleSettings)
				{
					HtmlTableRow settingRow = new HtmlTableRow();
					HtmlTableCell labelCell = new HtmlTableCell();
					labelCell.InnerText = ms.FriendlyName;
					HtmlTableCell controlCell = new HtmlTableCell();
					controlCell.Controls.Add(SettingControlHelper.CreateSettingControl(ms.Name, ms.GetRealType(), null));
					settingRow.Cells.Add(labelCell);
					settingRow.Cells.Add(controlCell);
					this.plcCustomSettings.Controls.Add(settingRow);
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
					this.hplLookup.Attributes.Add("onClick"
						, String.Format("window.open(\"TemplatePreview.aspx?TemplateId={0}&Control={1}\", \"Preview\", \"width=760 height=400\")"
						, this.ActiveNode.Template.Id
						, this.ddlPlaceholder.ClientID)
					);
				}
				catch (Exception ex)
				{
					this.ShowError(ex.Message);
				}
			}
		}

		private void BindCustomSettings()
		{
			if (this._activeSection.Settings.Count > 0)
			{
				foreach (ModuleSetting ms in this._activeSection.ModuleType.ModuleSettings)
				{
					Control ctrl = this.TemplateControl.FindControl(ms.Name);
					if (this._activeSection.Settings[ms.Name] != null)
					{
						string settingValue = this._activeSection.Settings[ms.Name].ToString();
						if (ctrl is TextBox)
						{
							((TextBox)ctrl).Text = settingValue;
						}
						else if (ctrl is CheckBox)
						{
							((CheckBox)ctrl).Checked = Boolean.Parse(settingValue);
						}
						else if (ctrl is DropDownList)
						{
							DropDownList ddl = (DropDownList)ctrl;
							ListItem li = ddl.Items.FindByValue(settingValue);
							if (li != null)
							{
								li.Selected = true;
							}
						}
					}
				}
			}
		}

		private void BindRoles()
		{
			IList roles = base.CoreRepository.GetAll(typeof(Role), "PermissionLevel");
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

		private void SetCustomSettings()
		{
			foreach(ModuleSetting ms in this._activeSection.ModuleType.ModuleSettings)
			{
				Control ctrl = this.TemplateControl.FindControl(ms.Name);
				object val = null;
				if (ctrl is TextBox)
				{
					string text = ((TextBox)ctrl).Text;
					if (ms.IsRequired && text == String.Empty)
					{
						throw new Exception(String.Format("The value for {0} is required.", ms.FriendlyName));
					}
					val = text;
				}
				else if (ctrl is CheckBox)
				{
					val = ((CheckBox)ctrl).Checked;
				}
				else if (ctrl is DropDownList)
				{
					val = ((DropDownList)ctrl).SelectedValue;
				}
				try
				{
					// Check if the datatype is correct -> brute force casting :)
					Type type = ms.GetRealType();
					if (type.IsEnum && val is string)
					{
						val = Enum.Parse(type, val.ToString());
					}
					else
					{
						if (val.ToString().Length > 0)
						{
							object testObj = Convert.ChangeType(val, type);
						}
					}
				}
				catch (InvalidCastException ex)
				{
					throw new Exception(String.Format("Invalid value entered for {0}: {1}", ms.FriendlyName, val.ToString()), ex);
				}
				this._activeSection.Settings[ms.Name] = val.ToString();
			}
		}

		private void SetRoles()
		{
			this._activeSection.SectionPermissions.Clear();
			foreach (RepeaterItem ri in rptRoles.Items)
			{	
				// HACK: RoleId is stored in the ViewState because the repeater doesn't have a DataKeys property.
				CheckBox chkView = (CheckBox)ri.FindControl("chkViewAllowed");
				CheckBox chkEdit = (CheckBox)ri.FindControl("chkEditAllowed");
				if (chkView.Checked || chkEdit.Checked)
				{
					SectionPermission sp = new SectionPermission();
					sp.Section = this._activeSection;
					sp.Role = (Role)base.CoreRepository.GetObjectById(typeof(Role), (int)ViewState[ri.ClientID]);
					sp.ViewAllowed = chkView.Checked;
					sp.EditAllowed = chkEdit.Checked;
					this._activeSection.SectionPermissions.Add(sp);
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
			// Load the section here because we need to create dynamic controls based on the 
			// ModuleType of the section. This method comes after base.OnInit because there,
			// the ActiveNode is set and we need that to attach that one to the section.
			LoadSection();
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

					// Custom settings
					if (this._activeSection.Id > -1)
					{
						SetCustomSettings();
					}

					// Roles
					SetRoles();

					// Save the active section
					SaveSection();

					// Detect a placeholderId change and change positions of adjacent sections if necessary.					
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
				CheckBox chkView = (CheckBox)e.Item.FindControl("chkViewAllowed");
				chkView.Checked = this._activeSection.ViewAllowed(role);
				CheckBox chkEdit = (CheckBox)e.Item.FindControl("chkEditAllowed");
				if (role.HasPermission(AccessLevel.Editor) || role.HasPermission(AccessLevel.Administrator))
				{
					chkEdit.Checked = this._activeSection.EditAllowed(role);
				}
				else
				{
					chkEdit.Visible = false;
				}
				// Add RoleId to the ViewState with the ClientID of the repeateritem as key.
				this.ViewState[e.Item.ClientID] = role.Id;
			}
		}
	}
}
