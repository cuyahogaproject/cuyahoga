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

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for NodeEdit.
	/// </summary>
	public class NodeEdit : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		protected System.Web.UI.WebControls.Label lblParentNode;
		protected System.Web.UI.WebControls.DropDownList ddlTemplates;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnNew;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.WebControls.Button btnCancel;
		protected System.Web.UI.WebControls.Repeater rptSections;
		protected System.Web.UI.WebControls.Label lblTemplates;
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
		protected System.Web.UI.WebControls.TextBox txtTitle;
	
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
			// main buttons visibility
			btnNew.Visible = (this.ActiveNode.Id > 0);
			btnDelete.Visible = (this.ActiveNode.Id > 0);
			btnDelete.Attributes.Add("onClick", "return confirmDeleteNode();");
		}

		private void BindCultures()
		{
			this.ddlCultures.DataSource = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			this.ddlCultures.DataValueField = "Name";
			this.ddlCultures.DataTextField = "DisplayName";
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
			// Only show a dropdownlist for templates if no sections are attached.
			if (this.ActiveNode.Sections == null || this.ActiveNode.Sections.Count == 0)
			{
				IList templates = base.CoreRepository.GetAll(typeof(Template), "Name");
				// Insert option for no template
				Template emptyTemplate = new Template();
				emptyTemplate.Id = -1;
				emptyTemplate.Name = "No template";
				templates.Insert(0, emptyTemplate);

				// Bind
				this.ddlTemplates.DataSource = templates;
				this.ddlTemplates.DataValueField = "Id";
				this.ddlTemplates.DataTextField = "Name";
				this.ddlTemplates.DataBind();
				if (this.ActiveNode != null && this.ActiveNode.Template != null)
				{
					ddlTemplates.Items.FindByValue(this.ActiveNode.Template.Id.ToString()).Selected = true;
				}
				this.ddlTemplates.Visible = true;
			}
			else
			{
				this.lblTemplates.Text = this.ActiveNode.Template.Name;
				this.lblTemplates.Visible = true;
			}
		}

		private void BindSections()
		{
			this.rptSections.DataSource = base.CoreRepository.GetSortedSectionsByNode(this.ActiveNode);
			this.rptSections.DataBind();
			if (this.ActiveNode.Template != null)
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
				Template template = new Template();
				template.Id = Int32.Parse(this.ddlTemplates.SelectedValue);
				template.Name = this.ddlTemplates.SelectedItem.Text;
				this.ActiveNode.Template = template;
			}
			else if (this.ddlTemplates.SelectedValue == "-1")
			{
				this.ActiveNode.Template = null;
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
			if (this.ActiveNode.Id > 0)
			{
				base.CoreRepository.UpdateObject(this.ActiveNode);
			}
			else
			{
				IList rootNodes = base.CoreRepository.GetRootNodes(this.ActiveNode.Site);
				this.ActiveNode.CalculateNewPosition(rootNodes);
				base.CoreRepository.SaveObject(this.ActiveNode);
				Context.Response.Redirect(String.Format("NodeEdit.aspx?NodeId={0}", this.ActiveNode.Id));
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
			else if (Context.Request.QueryString["Action"] == "Delete")
			{
				// We consider deleting a section also a 'Movement' :-)
				try
				{
					base.CoreRepository.DeleteObject(section);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
			// Redirect to the same page without the section movement parameters
			Context.Response.Redirect(Context.Request.Path + String.Format("?NodeId={0}", this.ActiveNode.Id));
		}

		private void SetShortDescription()
		{
			if (this.ActiveNode.Id > 0)
			{
				this.ActiveNode.ShortDescription = this.txtShortDescription.Text;
// TODO:	how to check the uniqueness? Searching for a node will implicitly flush the session and throw an 
//			exception when the shortdescription already exists.

//				Node node = base.CoreRepository.GetNodeByShortDescription(this.ActiveNode.ShortDescription);
//				if (node != null && node.Id != this.ActiveNode.Id)
//				{
//					throw new Exception("The friendly url of the node already exists");
//				}
			}
			else
			{
				// Generate the short description for new nodes.
				this.ActiveNode.CreateShortDescription();
				// Check the uniqueness of the shortdescription and regenerate it when it already exists.
//				bool isUnique = false;
//				int suffix = 1;
//				while (! isUnique)
//				{				
//					Node node = base.CoreRepository.GetNodeByShortDescription(this.ActiveNode.ShortDescription);
//					if (node != null)
//					{
//						// Not unique, regenerate
//						this.ActiveNode.RecreateShortDescription(suffix);
//						suffix++;
//					}
//					else
//					{
//						isUnique = true;
//					}
//				}
			}
		}

		private void MoveNode(NodePositionMovement npm)
		{
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
			this.ddlTemplates.SelectedIndexChanged += new System.EventHandler(this.ddlTemplates_SelectedIndexChanged);
			this.rptSections.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptSections_ItemDataBound);
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
					// TODO: ensure a unique culture when the node is a root node
					this.ActiveNode.Culture = this.ddlCultures.SelectedValue;
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
				Validate();
				if (this.IsValid)
				{
					SetShortDescription();
					SetRoles();
					this.SaveNode();
					this.ShowMessage("Node saved while setting the template.");
				}
			}
			catch (Exception ex)
			{
				this.ShowError(ex.Message);
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
				HyperLink hplDelete = (HyperLink)e.Item.FindControl("hplDelete");

				// HACK: as long as ~/ doesn't work properly in mono we have to use a relative path from the Controls
				// directory due to the template construction.
				hplEdit.NavigateUrl = String.Format("../SectionEdit.aspx?SectionId={0}&NodeId={1}", section.Id, this.ActiveNode.Id);
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

				hplDelete.NavigateUrl = String.Format("javascript:confirmDeleteSection({0}, {1});", section.Id.ToString(), this.ActiveNode.Id.ToString());
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
					base.CoreRepository.DeleteObject(this.ActiveNode);
					// Reset the position of the 'neighbour' nodes.
					if (this.ActiveNode.Level == 0)
					{						
						this.ActiveNode.ReOrderNodePositions(base.CoreRepository.GetRootNodes(this.ActiveNode.Site), this.ActiveNode.Position);
					}
					else
					{
						this.ActiveNode.ReOrderNodePositions(this.ActiveNode.ParentNode.ChildNodes, this.ActiveNode.Position);
					}
					if (this.ActiveNode.ParentNode != null)
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
	}
}
