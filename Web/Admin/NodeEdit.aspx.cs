using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using log4net;

using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for NodeEdit.
	/// </summary>
	public class NodeEdit : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(NodeEdit));

		protected System.Web.UI.WebControls.Label lblParentNode;
		protected System.Web.UI.WebControls.DropDownList ddlTemplates;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnNew;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.WebControls.Button btnCancel;
		protected System.Web.UI.WebControls.Repeater rptSections;
		protected System.Web.UI.WebControls.HyperLink hplNewSection;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTitle;
		protected System.Web.UI.WebControls.ImageButton btnUp;
		protected System.Web.UI.WebControls.ImageButton btnDown;
		protected System.Web.UI.WebControls.ImageButton btnLeft;
		protected System.Web.UI.WebControls.ImageButton btnRight;
		protected System.Web.UI.WebControls.TextBox txtShortDescription;
		protected System.Web.UI.WebControls.RegularExpressionValidator revShortDescription;
		protected System.Web.UI.WebControls.Repeater rptDeleteRoles;
		protected System.Web.UI.WebControls.Repeater rptRoles;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvShortDescription;
		protected System.Web.UI.WebControls.DropDownList ddlCultures;
		protected System.Web.UI.WebControls.CheckBox chkShowInNavigation;
		protected System.Web.UI.WebControls.Repeater rptMenus;
		protected System.Web.UI.WebControls.HyperLink hplNewMenu;
		protected System.Web.UI.WebControls.Panel pnlMenus;
		protected System.Web.UI.WebControls.CheckBox chkPropagateToSections;
		protected System.Web.UI.WebControls.CheckBox chkPropagateToChildNodes;
		protected System.Web.UI.WebControls.TextBox txtLinkUrl;
		protected System.Web.UI.WebControls.DropDownList ddlLinkTarget;
		protected System.Web.UI.WebControls.Panel pnlLink;
		protected System.Web.UI.WebControls.Panel pnlTemplate;
		protected System.Web.UI.WebControls.Panel pnlSections;
		protected System.Web.UI.WebControls.CheckBox chkLink;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.TextBox txtMetaDescription;
		protected System.Web.UI.WebControls.TextBox txtMetaKeywords;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Edit node";

			// Note: ActiveNode is handled primarily by the AdminBasePage because other pages use it.
			// ActiveNode is always freshly retrieved (also after postbacks), so it will be tracked by NHibernate.
			if (Context.Request.QueryString["NodeId"] != null 
				&& Int32.Parse(Context.Request.QueryString["NodeId"]) == -1)
			{
				// Create an empty new node if NodeId is set to -1
				this.ActiveNode = new Node();
				if (Context.Request.QueryString["ParentNodeId"] != null)
				{
					int parentNodeId = Int32.Parse(Context.Request.QueryString["ParentNodeId"]);
					this.ActiveNode.ParentNode = (Node)base.CoreRepository.GetObjectById(typeof(Node), parentNodeId);
					// Copy Site property from parent.
					this.ActiveNode.Site = this.ActiveNode.ParentNode.Site;

					if (! this.IsPostBack)
					{
						// Set defaults.
						this.ActiveNode.Template = this.ActiveNode.ParentNode.Template;
						this.ActiveNode.Culture = this.ActiveNode.ParentNode.Culture;
						// Copy security from parent.
						this.ActiveNode.CopyRolesFromParent();
					}
				}
				else if (Context.Request.QueryString["SiteId"] != null)
				{
					int siteId = Int32.Parse(Context.Request.QueryString["SiteId"]);
					this.ActiveNode.Site = (Site)base.CoreRepository.GetObjectById(typeof(Site), siteId);

					// Set defaults inheriting from site
					this.ActiveNode.Culture = this.ActiveNode.Site.DefaultCulture;
					this.ActiveNode.Template = this.ActiveNode.Site.DefaultTemplate;
				}
				// Short description is auto-generated, so we don't need the controls with new nodes.
				this.txtShortDescription.Visible = false;
				this.rfvShortDescription.Enabled = false;
				this.revShortDescription.Enabled = false;
			}
			if (! this.IsPostBack)
			{
				// There could be a section movement in the request. Check this and move sections if necessary.
				if (Context.Request.QueryString["SectionId"] != null && Context.Request.QueryString["Action"] != null)
				{
					MoveSections();
				}
				else
				{
					if (this.ActiveNode != null)
					{
						BindNodeControls();
						BindSections();
						if (this.ActiveNode.IsRootNode)
						{
							BindMenus();
						}
					}
					BindCultures();
					BindTemplates();
					BindRoles();
				}
			}
			if (this.ActiveNode != null)
			{
				BindPositionButtonsVisibility();
			}
		}

		private void BindNodeControls()
		{
			this.txtTitle.Text = this.ActiveNode.Title;
			this.txtShortDescription.Text = this.ActiveNode.ShortDescription;
			if (this.ActiveNode.ParentNode != null)
			{
				this.lblParentNode.Text = this.ActiveNode.ParentNode.Title;
			}
			this.chkShowInNavigation.Checked = this.ActiveNode.ShowInNavigation;
			this.txtMetaDescription.Text = this.ActiveNode.MetaDescription;
			this.txtMetaKeywords.Text = this.ActiveNode.MetaKeywords;

			this.chkLink.Enabled = this.ActiveNode.Sections.Count == 0;
			if (this.ActiveNode.IsExternalLink)
			{
				this.chkLink.Checked = true;
				this.pnlLink.Visible = true;
				this.pnlMenus.Visible = false;
				this.pnlTemplate.Visible = false;
				this.pnlSections.Visible = false;
				this.txtLinkUrl.Text = this.ActiveNode.LinkUrl;
				this.ddlLinkTarget.Items.FindByValue(this.ActiveNode.LinkTarget.ToString()).Selected = true;
			}
			// main buttons visibility
			btnNew.Visible = (this.ActiveNode.Id > 0);
			btnDelete.Visible = (this.ActiveNode.Id > 0);
			btnDelete.Attributes.Add("onclick", "return confirmDeleteNode();");
		}

		private void BindCultures()
		{
			this.ddlCultures.DataSource = Globalization.GetOrderedCultures();
			this.ddlCultures.DataValueField = "Key";
			this.ddlCultures.DataTextField = "Value";
			this.ddlCultures.DataBind();
			if (this.ActiveNode.Culture != null)
			{
				ddlCultures.Items.FindByValue(this.ActiveNode.Culture).Selected = true;
			}
		}

		private void BindPositionButtonsVisibility()
		{
			// node location buttons visibility
			btnUp.Visible = (this.ActiveNode.Position > 0);
			btnDown.Visible = ((this.ActiveNode.ParentNode != null) && (this.ActiveNode.Position != this.ActiveNode.ParentNode.ChildNodes.Count -1) && this.ActiveNode.Id != -1);
			btnLeft.Visible = (this.ActiveNode.Level > 0 && this.ActiveNode.Id != -1);
			btnRight.Visible = (this.ActiveNode.Position > 0);
		}

		private void BindTemplates()
		{
			IList templates = base.CoreRepository.GetAll(typeof(Template), "Name");
	
			// Bind
			this.ddlTemplates.DataSource = templates;
			this.ddlTemplates.DataValueField = "Id";
			this.ddlTemplates.DataTextField = "Name";
			this.ddlTemplates.DataBind();
			if (this.ActiveNode != null && this.ActiveNode.Template != null)
			{
				ListItem li = ddlTemplates.Items.FindByValue(this.ActiveNode.Template.Id.ToString());
				if (li != null)
				{
					li.Selected = true;
				}
			}
		}

		private void BindMenus()
		{
			this.pnlMenus.Visible = true;
			this.rptMenus.DataSource = base.CoreRepository.GetMenusByRootNode(this.ActiveNode);
			this.rptMenus.DataBind();
			this.hplNewMenu.NavigateUrl = String.Format("~/Admin/MenuEdit.aspx?MenuId=-1&NodeId={0}", this.ActiveNode.Id);
		}

		private void BindSections()
		{
			IList sortedSections = base.CoreRepository.GetSortedSectionsByNode(this.ActiveNode);
			// Synchronize sections, otherwise we'll have two collections with the same Sections
			this.ActiveNode.Sections = sortedSections;
			this.rptSections.DataSource = sortedSections;
			this.rptSections.DataBind();
			if (this.ActiveNode.Id > 0 && this.ActiveNode.Template != null)
			{
				// Also enable add section link
				this.hplNewSection.NavigateUrl = String.Format("~/Admin/SectionEdit.aspx?SectionId=-1&NodeId={0}", this.ActiveNode.Id.ToString());
				this.hplNewSection.Visible = true;
			}
		}

		private void BindRoles()
		{
			IList roles = base.CoreRepository.GetAll(typeof(Role), "PermissionLevel");
			this.rptRoles.ItemDataBound += new RepeaterItemEventHandler(rptRoles_ItemDataBound);
			this.rptRoles.DataSource = roles;
			this.rptRoles.DataBind();
		}

		private void SetTemplate()
		{
			if (this.ddlTemplates.Visible && this.ddlTemplates.SelectedValue != "-1")
			{
				int templateId = Int32.Parse(this.ddlTemplates.SelectedValue);
				this.ActiveNode.Template = (Template)base.CoreRepository.GetObjectById(typeof(Template), templateId);
			}
		}

		private void SetRoles()
		{
			this.ActiveNode.NodePermissions.Clear();
			foreach (RepeaterItem ri in rptRoles.Items)
			{	
				// HACK: RoleId is stored in the ViewState because the repeater doesn't have a DataKeys property.
				CheckBox chkView = (CheckBox)ri.FindControl("chkViewAllowed");
				CheckBox chkEdit = (CheckBox)ri.FindControl("chkEditAllowed");
				if (chkView.Checked || chkEdit.Checked)
				{
					NodePermission np = new NodePermission();
					np.Node = this.ActiveNode;
					np.Role = (Role)base.CoreRepository.GetObjectById(typeof(Role), (int)ViewState[ri.ClientID]);
					np.ViewAllowed = chkView.Checked;
					np.EditAllowed = chkEdit.Checked;
					this.ActiveNode.NodePermissions.Add(np);
				}
			}
		}

		private void SaveNode()
		{
			base.CoreRepository.ClearQueryCache("Nodes");

			if (this.ActiveNode.Id > 0)
			{
				base.CoreRepository.UpdateNode(this.ActiveNode
					, this.chkPropagateToChildNodes.Checked
					, this.chkPropagateToSections.Checked);
			}
			else
			{
				IList rootNodes = base.CoreRepository.GetRootNodes(this.ActiveNode.Site);
				this.ActiveNode.CalculateNewPosition(rootNodes);
				// Add node to the parent node's ChildNodes first
				if (this.ActiveNode.ParentNode != null)
				{
					this.ActiveNode.ParentNode.ChildNodes.Add(this.ActiveNode);
				}
				base.CoreRepository.SaveObject(this.ActiveNode);				
				Context.Response.Redirect(String.Format("NodeEdit.aspx?NodeId={0}&message=Node created sucessfully", this.ActiveNode.Id));
			}
		}

		private void MoveSections()
		{
			int sectionId = Int32.Parse(Context.Request.QueryString["SectionId"]);
			Section section = (Section)base.CoreRepository.GetObjectById(typeof(Section), sectionId);
			section.Node = this.ActiveNode;
			if (Context.Request.QueryString["Action"] == "MoveUp")
			{
				section.MoveUp();
				base.CoreRepository.FlushSession();
				// reset sections, so they will be refreshed from the database when required.
				this.ActiveNode.ResetSections();
			}
			else if (Context.Request.QueryString["Action"] == "MoveDown")
			{
				section.MoveDown();
				base.CoreRepository.FlushSession();
				// reset sections, so they will be refreshed from the database when required.
				this.ActiveNode.ResetSections();
			}
			// Redirect to the same page without the section movement parameters
			Context.Response.Redirect(Context.Request.Path + String.Format("?NodeId={0}", this.ActiveNode.Id));
		}

		private void SetShortDescription()
		{
			// TODO: check uniqueness. It's now handled by the database constraint but that is not
			// too descriptive.
			if (this.ActiveNode.Id > 0)
			{
				this.ActiveNode.ShortDescription = this.txtShortDescription.Text;
			}
			else
			{
				// Generate the short description for new nodes.
				this.ActiveNode.CreateShortDescription();
			}
		}

		private void MoveNode(NodePositionMovement npm)
		{
			base.CoreRepository.ClearQueryCache("Nodes");

			IList rootNodes = base.CoreRepository.GetRootNodes(this.ActiveNode.Site);
			this.ActiveNode.Move(rootNodes, npm);
			this.CoreRepository.FlushSession();
			Context.Response.Redirect(Context.Request.RawUrl);
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
			this.btnUp.Click += new System.Web.UI.ImageClickEventHandler(this.btnUp_Click);
			this.btnDown.Click += new System.Web.UI.ImageClickEventHandler(this.btnDown_Click);
			this.btnLeft.Click += new System.Web.UI.ImageClickEventHandler(this.btnLeft_Click);
			this.btnRight.Click += new System.Web.UI.ImageClickEventHandler(this.btnRight_Click);
			this.chkLink.CheckedChanged += new System.EventHandler(this.chkLink_CheckedChanged);
			this.ddlTemplates.SelectedIndexChanged += new System.EventHandler(this.ddlTemplates_SelectedIndexChanged);
			this.rptMenus.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptMenus_ItemDataBound);
			this.rptSections.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptSections_ItemDataBound);
			this.rptSections.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rptSections_ItemCommand);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				SetTemplate();				
				if (this.IsValid)
				{
					this.ActiveNode.Title = this.txtTitle.Text;
					this.ActiveNode.Culture = this.ddlCultures.SelectedValue;
					this.ActiveNode.ShowInNavigation = this.chkShowInNavigation.Checked;
					this.ActiveNode.MetaDescription = this.txtMetaDescription.Text.Trim().Length > 0
					? this.txtMetaDescription.Text.Trim()
					: null;
					this.ActiveNode.MetaKeywords = this.txtMetaKeywords.Text.Trim().Length > 0
						? this.txtMetaKeywords.Text.Trim()
						: null;
					if (this.chkLink.Checked)
					{
						this.ActiveNode.LinkUrl = this.txtLinkUrl.Text;
						this.ActiveNode.LinkTarget = (LinkTarget)Enum.Parse(typeof(LinkTarget), this.ddlLinkTarget.SelectedValue);
					}
					else  // rabol: [#CUY-51] - Clear the link in the database
					{
						this.ActiveNode.LinkUrl = null;
						this.ActiveNode.LinkTarget = LinkTarget.Self;
					}
					this.ActiveNode.Validate();
					SetShortDescription();
					SetRoles();
					SaveNode();
					ShowMessage("Node saved.");
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
				if (ex.InnerException != null)
				{
					msg += ", " + ex.InnerException.Message;
				}
				ShowError(msg);
				log.Error("Error saving Node", ex);
			}
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			// Create an url with NodeId -1 and the Id of the current node as ParentId
			string url = String.Format("NodeEdit.aspx?NodeId=-1&ParentNodeId={0}", this.ActiveNode.Id.ToString());
			// Redirect to the new url
			Context.Response.Redirect(url);
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveNode.Id == -1 && this.ActiveNode.ParentNode != null)
			{
				Context.Response.Redirect(String.Format("NodeEdit.aspx?NodeId={0}", this.ActiveNode.ParentNode.Id));
			}
			else
			{
				Context.Response.Redirect("Default.aspx");
			}
		}

		private void ddlTemplates_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				SetTemplate();	
				// Also save the current node (validate first)
				this.ActiveNode.Title = this.txtTitle.Text;
				this.ActiveNode.Culture = this.ddlCultures.SelectedValue;
				Validate();
				if (this.IsValid)
				{
					SetShortDescription();
					SetRoles();
					this.SaveNode();
					this.ShowMessage("Node saved while setting the template.");
					BindSections();
				}
			}
			catch (Exception ex)
			{
				this.ShowError(ex.Message);
				log.Error("Error while switching the Template.", ex);
			}
		}

		private void btnUp_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			MoveNode(NodePositionMovement.Up);
		}

		private void btnDown_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			MoveNode(NodePositionMovement.Down);		
		}

		private void btnLeft_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			MoveNode(NodePositionMovement.Left);
		}

		private void btnRight_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			MoveNode(NodePositionMovement.Right);
		}

		private void rptSections_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Section section = e.Item.DataItem as Section;
			if (section != null)
			{
				HyperLink hplEdit = (HyperLink)e.Item.FindControl("hplEdit");

				// HACK: as long as ~/ doesn't work properly in mono we have to use a relative path from the Controls
				// directory due to the template construction.
				hplEdit.NavigateUrl = String.Format("~/Admin/SectionEdit.aspx?SectionId={0}&NodeId={1}", section.Id, this.ActiveNode.Id);
				if (section.CanMoveUp())
				{
					HyperLink hplSectionUp = (HyperLink)e.Item.FindControl("hplSectionUp");
					hplSectionUp.NavigateUrl = Context.Request.RawUrl + String.Format("&SectionId={0}&Action=MoveUp", section.Id.ToString());
					hplSectionUp.Visible = true;
				}
				if (section.CanMoveDown())
				{
					HyperLink hplSectionDown = (HyperLink)e.Item.FindControl("hplSectionDown");
					hplSectionDown.NavigateUrl = Context.Request.RawUrl + String.Format("&SectionId={0}&Action=MoveDown", section.Id.ToString());
					hplSectionDown.Visible = true;
				}
				LinkButton lbtDelete = (LinkButton)e.Item.FindControl("lbtDelete");
				lbtDelete.Attributes.Add("onclick", "return confirm('Are you sure?')");

				// Check if the placeholder exists in the currently attached template
				BaseTemplate templateControl = (BaseTemplate)this.LoadControl(UrlHelper.GetApplicationPath() + this.ActiveNode.Template.Path);
				Label lblNotFound = (Label)e.Item.FindControl("lblNotFound");
				lblNotFound.Visible = (templateControl.Containers[section.PlaceholderId] == null);
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this.ActiveNode.Sections.Count > 0)
			{
				this.ShowError("Can't delete a node when there are sections attached. Please delete or detach all sections first.");
			}
			else if (this.ActiveNode.ChildNodes.Count > 0)
			{
				this.ShowError("Can't delete a node when there are child nodes attached. Please delete all childnodes first.");
			}
			else
			{
				try
				{	
					base.CoreRepository.ClearQueryCache("Nodes");

					bool hasParentNode = (this.ActiveNode.ParentNode != null);
					if (hasParentNode)
					{
						this.ActiveNode.ParentNode.ChildNodes.Remove(this.ActiveNode);
					}
					else
					{
						IList rootNodes = base.CoreRepository.GetRootNodes(this.ActiveNode.Site);
						rootNodes.Remove(this.ActiveNode);
					}
					base.CoreRepository.DeleteNode(this.ActiveNode);
					// Reset the position of the 'neighbour' nodes.
					if (this.ActiveNode.Level == 0)
					{						
						this.ActiveNode.ReOrderNodePositions(base.CoreRepository.GetRootNodes(this.ActiveNode.Site), this.ActiveNode.Position);
					}
					else
					{
						this.ActiveNode.ReOrderNodePositions(this.ActiveNode.ParentNode.ChildNodes, this.ActiveNode.Position);
					}
					base.CoreRepository.FlushSession();
					if (hasParentNode)
					{
						Context.Response.Redirect(String.Format("NodeEdit.aspx?NodeId={0}", this.ActiveNode.ParentNode.Id));
					}
					else
					{
						Context.Response.Redirect("Default.aspx");
					}
				}
				catch (Exception ex)
				{
					this.ShowError(ex.Message);
					log.Error(String.Format("Error deleting Node: {0}.", this.ActiveNode.Id), ex);
				}
			}
		}

		private void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Role role = e.Item.DataItem as Role;
			if (role != null)
			{
				CheckBox chkView = (CheckBox)e.Item.FindControl("chkViewAllowed");
				chkView.Checked = this.ActiveNode.ViewAllowed(role);
				CheckBox chkEdit = (CheckBox)e.Item.FindControl("chkEditAllowed");
				if (role.HasPermission(AccessLevel.Editor) || role.HasPermission(AccessLevel.Administrator))
				{
					chkEdit.Checked = this.ActiveNode.EditAllowed(role);
				}
				else
				{
					chkEdit.Visible = false;
				}
				// Add RoleId to the ViewState with the ClientID of the repeateritem as key.
				this.ViewState[e.Item.ClientID] = role.Id;
			}
		}

		private void rptMenus_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			CustomMenu menu = e.Item.DataItem as CustomMenu;
			if (menu != null)
			{
				HyperLink hplEdit = e.Item.FindControl("hplEditMenu") as HyperLink;
				hplEdit.NavigateUrl = String.Format("~/Admin/MenuEdit.aspx?MenuId={0}&NodeId={1}", menu.Id, this.ActiveNode.Id);
			}
		}

		private void rptSections_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete" || e.CommandName == "Detach")
			{
				int sectionId = Int32.Parse(e.CommandArgument.ToString());
				Section section = (Section)base.CoreRepository.GetObjectById(typeof(Section), sectionId);

				if (e.CommandName == "Delete")
				{
					section.Node = this.ActiveNode;
					try
					{
						// First tell the module to remove its content.
						ModuleBase module = ModuleLoader.GetModuleFromSection(section);
						module.DeleteModuleContent();
						// Make sure there is no gap in the section indexes. 
						// ABUSE: this method was not designed for this, but works fine.
						section.ChangeAndUpdatePositionsAfterPlaceholderChange(section.PlaceholderId, section.Position, false);
						// Now delete the Section.
						this.ActiveNode.Sections.Remove(section);
						base.CoreRepository.DeleteObject(section);
					}
					catch (Exception ex)
					{
						ShowError(ex.Message);
						log.Error(String.Format("Error deleting section : {0}.", section.Id.ToString()), ex);
					}
				}
				if (e.CommandName == "Detach")
				{
					try
					{
						// Make sure there is no gap in the section indexes. 
						// ABUSE: this method was not designed for this, but works fine.
						section.ChangeAndUpdatePositionsAfterPlaceholderChange(section.PlaceholderId, section.Position, false);
						// Now detach the Section.
						this.ActiveNode.Sections.Remove(section);
						section.Node = null;
						section.PlaceholderId = null;
						base.CoreRepository.UpdateObject(section);
						// Update search index to make sure the content of detached sections doesn't 
						// show up in a search.
						SearchHelper.UpdateIndexFromSection(section);
					}
					catch (Exception ex)
					{
						ShowError(ex.Message);
						log.Error(String.Format("Error detaching section : {0}.", section.Id.ToString()), ex);
					}
				}
				BindSections();
			}
		}

		private void chkLink_CheckedChanged(object sender, System.EventArgs e)
		{
			this.pnlLink.Visible = this.chkLink.Checked;
			this.pnlMenus.Visible = ! this.chkLink.Checked;
			this.pnlTemplate.Visible = ! this.chkLink.Checked;
			this.pnlSections.Visible = ! this.chkLink.Checked;
		}
	}
}
