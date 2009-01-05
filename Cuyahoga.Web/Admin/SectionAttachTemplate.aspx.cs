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
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for SectionAttachTemplate.
	/// </summary>
	public class SectionAttachTemplate : AdminBasePage
	{
		private Section _activeSection;

		protected System.Web.UI.WebControls.Label lblSection;
		protected System.Web.UI.WebControls.Label lblModuleType;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Repeater rptTemplates;
		protected System.Web.UI.WebControls.Button btnBack;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.Title = "Attach section to template(s)";
			if (Context.Request.QueryString["SectionId"] != null)
			{
				// Get section data
				this._activeSection = (Section)base.CoreRepository.GetObjectById(typeof(Section), 
					Int32.Parse(Context.Request.QueryString["SectionId"]));
			}
			if (! this.IsPostBack)
			{
				BindSectionControls();
				BindTemplates();
			}
			else
			{
			}
		}

		private void BindSectionControls()
		{
			if (this._activeSection != null)
			{
				this.lblSection.Text = this._activeSection.Title;
				this.lblModuleType.Text = this._activeSection.ModuleType.Name;
			}
		}

		private void BindTemplates()
		{
			this.rptTemplates.DataSource = base.CoreRepository.GetAll(typeof(Template), "Name");
			this.rptTemplates.DataBind();
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
			this.rptTemplates.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptTemplates_ItemDataBound);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptTemplates_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			Template template = e.Item.DataItem as Template;
			if (template != null)
			{
				DropDownList ddlPlaceHolders = e.Item.FindControl("ddlPlaceHolders") as DropDownList;
				HyperLink hplLookup = e.Item.FindControl("hplLookup") as HyperLink;

				// Read template control and get the containers (placeholders)
				string templatePath = Util.UrlHelper.GetApplicationPath() + template.Path;
				BaseTemplate templateControl = (BaseTemplate)this.LoadControl(templatePath);
				foreach (DictionaryEntry entry in templateControl.Containers)
				{
					// Check if the placeholder isn't taken by another section
					bool isTaken = false;
					Section section = template.Sections[entry.Key] as Section;
					if (section != null)
					{
						if (section == this._activeSection)
						{
							// it's already connected to the current section -> check checkbox
							CheckBox chkAttached = e.Item.FindControl("chkAttached") as CheckBox;
							chkAttached.Checked = true;
						}
						else
						{
							isTaken = true;
						}
					}
					if (! isTaken)
					{
						string placeHolderId = entry.Key.ToString();
						ddlPlaceHolders.Items.Add(placeHolderId);
						if (section == this._activeSection)
						{
							ddlPlaceHolders.SelectedValue = placeHolderId;
						}
					}
				}
				// Set lookup link
				hplLookup.NavigateUrl = "javascript:;";
				hplLookup.Attributes.Add("onclick"
					, String.Format("window.open(\"TemplatePreview.aspx?TemplateId={0}&Control={1}\", \"Preview\", \"width=760 height=400\")"
					, template.Id
					, ddlPlaceHolders.ClientID)
				);

				// Add TemplateId to the ViewState with the ClientID of the repeateritem as key.
				this.ViewState[e.Item.ClientID] = template.Id;
			}
		}

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("~/Admin/Sections.aspx");
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				foreach (RepeaterItem ri in this.rptTemplates.Items)
				{
					Template template = (Template)base.CoreRepository.GetObjectById(typeof(Template), (int)this.ViewState[ri.ClientID]);
					CheckBox chkAttached = ri.FindControl("chkAttached") as CheckBox;
					DropDownList ddlPlaceHolders = ri.FindControl("ddlPlaceHolders") as DropDownList;
					string selectedPlaceholderId = ddlPlaceHolders.SelectedValue;
					// Find to find if the current section is already attached to the template
					string attachedPlaceholderId = null;
					foreach (DictionaryEntry entry in template.Sections)
					{
						if (entry.Value == this._activeSection)
						{
							attachedPlaceholderId = entry.Key.ToString();
							break;
						}
					}
					// Attach
					if (chkAttached.Checked)
					{
						if (attachedPlaceholderId != null && attachedPlaceholderId != selectedPlaceholderId)
						{
							template.Sections.Remove(attachedPlaceholderId);
						}
						template.Sections[selectedPlaceholderId] = this._activeSection;
					}
					else
					{
						// Remove a possible attached section
						if (attachedPlaceholderId != null)
						{
							template.Sections.Remove(attachedPlaceholderId);
						}
					}
					base.CoreRepository.UpdateObject(template);
				}
				Context.Response.Redirect("~/Admin/Sections.aspx");
			}
			catch (Exception ex)
			{
				ShowException(ex);
			}
		}
	}
}
