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
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Web.Admin.UI;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for SectionAttach.
	/// </summary>
	public class SectionAttachNode : AdminBasePage
	{
		private Section _activeSection;
		private Site _selectedSite;
		private Node _selectedNode;

		protected System.Web.UI.WebControls.Label lblSection;
		protected System.Web.UI.WebControls.Label lblModuleType;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.ListBox lbxAvailableNodes;
		protected System.Web.UI.WebControls.DropDownList ddlSites;
		protected System.Web.UI.WebControls.DropDownList ddlPlaceholder;
		protected System.Web.UI.WebControls.HyperLink hplLookup;
		protected System.Web.UI.WebControls.Button btnBack;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Attach section";
			if (Context.Request.QueryString["SectionId"] != null)
			{
				// Get section data
				this._activeSection = (Section)base.CoreRepository.GetObjectById(typeof(Section), 
					Int32.Parse(Context.Request.QueryString["SectionId"]));
			}
			if (! this.IsPostBack)
			{
				BindSectionControls();
				BindSites();
			}
			else
			{
				if (this.ddlSites.SelectedIndex > -1)
				{
					this._selectedSite = (Site)base.CoreRepository.GetObjectById(typeof(Site)
						, Int32.Parse(this.ddlSites.SelectedValue));
				}
				if (this.lbxAvailableNodes.SelectedIndex > -1)
				{
					this._selectedNode = (Node)base.CoreRepository.GetObjectById(typeof(Node)
						, Int32.Parse(this.lbxAvailableNodes.SelectedValue));
				}
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

		private void BindSites()
		{
			IList sites = base.CoreRepository.GetAll(typeof(Site));
			foreach (Site site in sites)
			{
				if (this._selectedSite == null)
				{
					this._selectedSite = site;
				}
				ListItem li = new ListItem(site.Name + " (" + site.SiteUrl + ")", site.Id.ToString());
				this.ddlSites.Items.Add(li);
			}
			BindNodes();
		}

		private void BindNodes()
		{
			if (this._selectedSite != null)
			{
				this.lbxAvailableNodes.Visible = true;
				this.lbxAvailableNodes.Items.Clear();
				IList rootNodes = base.CoreRepository.GetRootNodes(this._selectedSite);
				AddAvailableNodes(rootNodes);
			}
			this.btnSave.Enabled = false;
			this.ddlPlaceholder.Visible = false;
			this.hplLookup.Visible = false;
		}

		private void BindPlaceholders()
		{
			if (this._selectedNode != null)
			{
				if (this._selectedNode.Template != null)
				{
					try
					{
						this.ddlPlaceholder.Visible = true;
						// Read template control and get the containers (placeholders)
						string templatePath = Util.UrlHelper.GetApplicationPath() + this._selectedNode.Template.Path;
						BaseTemplate template = (BaseTemplate)this.LoadControl(templatePath);
						this.ddlPlaceholder.DataSource = template.Containers;
						this.ddlPlaceholder.DataValueField = "Key";
						this.ddlPlaceholder.DataTextField = "Key";
						this.ddlPlaceholder.DataBind();
						// Create url for lookup
						this.hplLookup.Visible = true;
						this.hplLookup.NavigateUrl = "javascript:;";
						this.hplLookup.Attributes.Add("onclick"
							, String.Format("window.open(\"TemplatePreview.aspx?TemplateId={0}&Control={1}\", \"Preview\", \"width=760 height=400\")"
							, this._selectedNode.Template.Id
							, this.ddlPlaceholder.ClientID)
							);
						this.btnSave.Enabled = true;
					}
					catch (Exception ex)
					{
						ShowError(ex.Message);
					}
				}
				else
				{
					this.ddlPlaceholder.Visible = false;
					this.btnSave.Enabled = false;
					this.hplLookup.Visible = false;
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
			this.ddlSites.SelectedIndexChanged += new System.EventHandler(this.ddlSites_SelectedIndexChanged);
			this.lbxAvailableNodes.SelectedIndexChanged += new System.EventHandler(this.lbxAvailableNodes_SelectedIndexChanged);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("~/Admin/Sections.aspx");
		}

		private void ddlSites_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindNodes();
		}

		private void lbxAvailableNodes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindPlaceholders();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			this._activeSection.Node = this._selectedNode;
			this._activeSection.PlaceholderId = this.ddlPlaceholder.SelectedValue;
			this._selectedNode.Sections.Add(this._activeSection);
			this._activeSection.CalculateNewPosition();

			try
			{
				base.CoreRepository.UpdateObject(this._activeSection);
				// Update the full text index to make sure that the content can be found.
				SearchHelper.UpdateIndexFromSection(this._activeSection);

				Context.Response.Redirect("~/Admin/NodeEdit.aspx?NodeId=" + this._selectedNode.Id.ToString());
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}
	}
}
