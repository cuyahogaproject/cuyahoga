using System;
using System.Collections;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Admin.UI;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for SiteEdit.
	/// </summary>
	public class SiteEdit : AdminBasePage
	{
		private Site _activeSite;
		private ITemplateService _templateService;
		private IUserService _userService;

		protected TextBox txtName;
		protected RequiredFieldValidator rfvName;
		protected TextBox txtSiteUrl;
		protected DropDownList ddlTemplates;
		protected DropDownList ddlCultures;
		protected Button btnSave;
		protected Button btnCancel;
		protected Button btnDelete;
		protected DropDownList ddlPlaceholders;
		protected DropDownList ddlRoles;
		protected TextBox txtWebmasterEmail;
		protected RequiredFieldValidator rfvWebmasterEmail;
		protected HyperLink hplNewAlias;
		protected Panel pnlAliases;
		protected Repeater rptAliases;
		protected CheckBox chkUseFriendlyUrls;
		protected RequiredFieldValidator rfvSiteUrl;
		protected TextBox txtMetaDescription;
		protected TextBox txtMetaKeywords;

		/// <summary>
		/// Constructor.
		/// </summary>
		public SiteEdit()
		{
			this._templateService = Container.Resolve<ITemplateService>();
			this._userService = Container.Resolve<IUserService>();
		}
	
		private void Page_Load(object sender, EventArgs e)
		{
			base.Title = "Edit site";
			
			if (Context.Request.QueryString["SiteId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["SiteId"]) == -1)
				{
					// Create a new site instance
					this._activeSite = new Site();
					this.btnDelete.Visible = false;
					this.hplNewAlias.Visible = false;
				}
				else
				{
					// Get site data
					this._activeSite = base.SiteService.GetSiteById(Int32.Parse(Context.Request.QueryString["SiteId"]));
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onclick", "return confirm('Are you sure?')");
				}
				if (! this.IsPostBack)
				{
					BindSiteControls();
					BindTemplates();
					BindCultures();
					BindRoles();
					if (this._activeSite.Id > 0)
					{
						BindAliases();
					}
				}
			}
		}

		private void BindSiteControls()
		{
			this.txtName.Text = this._activeSite.Name;
			this.txtSiteUrl.Text = this._activeSite.SiteUrl;
			this.txtWebmasterEmail.Text = this._activeSite.WebmasterEmail;
			this.chkUseFriendlyUrls.Checked = this._activeSite.UseFriendlyUrls;
			this.txtMetaDescription.Text = this._activeSite.MetaDescription;
			this.txtMetaKeywords.Text = this._activeSite.MetaKeywords;
		}

		private void BindTemplates()
		{
			IList templates = this._templateService.GetAllTemplates();
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
			if (this._activeSite.DefaultTemplate != null)
			{
				ddlTemplates.Items.FindByValue(this._activeSite.DefaultTemplate.Id.ToString()).Selected = true;
				BindPlaceholders();
			}
			this.ddlTemplates.Visible = true;
		}

		private void BindPlaceholders()
		{
			// Try to find the placeholder in the selected template.
			if (this.ddlTemplates.SelectedIndex > 0)
			{
				try
				{
					Template template = this._templateService.GetTemplateById(Int32.Parse(this.ddlTemplates.SelectedValue));
					// Read template control and get the containers (placeholders)
					string templatePath = UrlHelper.GetApplicationPath() + template.Path;
					BaseTemplate templateControl = (BaseTemplate)this.LoadControl(templatePath);
					this.ddlPlaceholders.DataSource = templateControl.Containers;
					this.ddlPlaceholders.DataValueField = "Key";
					this.ddlPlaceholders.DataTextField = "Key";
					this.ddlPlaceholders.DataBind();
					if (this._activeSite.DefaultPlaceholder != null && this._activeSite.DefaultPlaceholder != String.Empty)
					{
						this.ddlPlaceholders.Items.FindByValue(this._activeSite.DefaultPlaceholder).Selected = true;
					}
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
			else
			{
				this.ddlPlaceholders.Items.Clear();
			}
		}

		private void BindCultures()
		{
			this.ddlCultures.DataSource = Globalization.GetOrderedCultures();
			this.ddlCultures.DataValueField = "Key";
			this.ddlCultures.DataTextField = "Value";
			this.ddlCultures.DataBind();
			if (this._activeSite.DefaultCulture != null)
			{
				ListItem li = ddlCultures.Items.FindByValue(this._activeSite.DefaultCulture);
				if (li != null)
				{
					ddlCultures.Items.FindByValue(this._activeSite.DefaultCulture).Selected = true;
				}
			}
		}

		private void BindRoles()
		{
			this.ddlRoles.DataSource = this._userService.GetAllRoles();
			this.ddlRoles.DataValueField = "Id";
			this.ddlRoles.DataTextField = "Name";
			this.ddlRoles.DataBind();
			if (this._activeSite.DefaultRole != null)
			{
				ddlRoles.Items.FindByValue(this._activeSite.DefaultRole.Id.ToString()).Selected = true;
			}
		}

		private void BindAliases()
		{
			this.rptAliases.DataSource = base.SiteService.GetSiteAliasesBySite(this._activeSite);
			this.rptAliases.DataBind();
			this.hplNewAlias.NavigateUrl = String.Format("~/Admin/SiteAliasEdit.aspx?SiteId={0}&SiteAliasId=-1", this._activeSite.Id);
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
			this.ddlTemplates.SelectedIndexChanged += new System.EventHandler(this.ddlTemplates_SelectedIndexChanged);
			this.rptAliases.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptAliases_ItemDataBound);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (this.IsValid)
			{
				this._activeSite.Name = txtName.Text;
				this._activeSite.SiteUrl = txtSiteUrl.Text;
				this._activeSite.WebmasterEmail = txtWebmasterEmail.Text;
				this._activeSite.UseFriendlyUrls = this.chkUseFriendlyUrls.Checked;

				if (this.ddlTemplates.SelectedValue != "-1")
				{
					int templateId = Int32.Parse(this.ddlTemplates.SelectedValue);
					Template template = this._templateService.GetTemplateById(templateId);
					this._activeSite.DefaultTemplate = template;
					if (this.ddlPlaceholders.SelectedIndex > -1)
					{
						this._activeSite.DefaultPlaceholder = this.ddlPlaceholders.SelectedValue;
					}
				}
				else if (this.ddlTemplates.SelectedValue == "-1")
				{
					this._activeSite.DefaultTemplate = null;
					this._activeSite.DefaultPlaceholder = null;
				}
				this._activeSite.DefaultCulture = this.ddlCultures.SelectedValue;
				int defaultRoleId = Int32.Parse(this.ddlRoles.SelectedValue);
				this._activeSite.DefaultRole = this._userService.GetRoleById(defaultRoleId);
				this._activeSite.MetaDescription = this.txtMetaDescription.Text.Trim().Length > 0
					? this.txtMetaDescription.Text.Trim()
					: null;
				this._activeSite.MetaKeywords = this.txtMetaKeywords.Text.Trim().Length > 0
					? this.txtMetaKeywords.Text.Trim()
					: null;

				try
				{
					base.SiteService.SaveSite(this._activeSite);
					ShowMessage("Site saved.");
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("Default.aspx");
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
				base.SiteService.DeleteSite(this._activeSite);
				Response.Redirect("Default.aspx");
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}

		private void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindPlaceholders();
		}

		private void rptAliases_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			SiteAlias sa = (SiteAlias)e.Item.DataItem;
			HyperLink hplEdit = e.Item.FindControl("hplEdit") as HyperLink;
			if (hplEdit != null)
			{
				hplEdit.NavigateUrl = String.Format("~/Admin/SiteAliasEdit.aspx?SiteId={0}&SiteAliasId={1}", this._activeSite.Id, sa.Id);
			}
			Label lblEntryNode = e.Item.FindControl("lblEntryNode") as Label;
			if (lblEntryNode != null)
			{
				if (sa.EntryNode == null)
				{
					lblEntryNode.Text = "Inherited from site";
				}
				else
				{
					lblEntryNode.Text = sa.EntryNode.Title + " (" + sa.EntryNode.Culture + ")";
				}
			}
		}
	}
}
