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
using System.Globalization;

using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Admin.UI;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for SiteEdit.
	/// </summary>
	public class SiteEdit : AdminBasePage
	{
		private Site _activeSite;

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.TextBox txtSiteUrl;
		protected System.Web.UI.WebControls.DropDownList ddlTemplates;
		protected System.Web.UI.WebControls.DropDownList ddlCultures;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnCancel;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvSiteUrl;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.Title = "Edit site";
			
			if (Context.Request.QueryString["SiteId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["SiteId"]) == -1)
				{
					// Create a new site instance
					this._activeSite = new Site();
					this.btnDelete.Visible = false;
				}
				else
				{
					// Get site data
					this._activeSite = (Site)base.CoreRepository.GetObjectById(typeof(Cuyahoga.Core.Domain.Site)
						, Int32.Parse(Context.Request.QueryString["SiteId"]));
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?')");
				}
				if (! this.IsPostBack)
				{
					BindSiteControls();
					BindTemplates();
					BindCultures();
				}
			}
		}

		private void BindSiteControls()
		{
			this.txtName.Text = this._activeSite.Name;
			this.txtSiteUrl.Text = this._activeSite.SiteUrl;
		}

		private void BindTemplates()
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
			if (this._activeSite.DefaultTemplate != null)
			{
				ddlTemplates.Items.FindByValue(this._activeSite.DefaultTemplate.Id.ToString()).Selected = true;
			}
			this.ddlTemplates.Visible = true;
		}

		private void BindCultures()
		{
			this.ddlCultures.DataSource = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			this.ddlCultures.DataValueField = "Name";
			this.ddlCultures.DataTextField = "DisplayName";
			this.ddlCultures.DataBind();
			if (this._activeSite.DefaultCulture != null)
			{
				ddlCultures.Items.FindByValue(this._activeSite.DefaultCulture).Selected = true;
			}
		}

		private void SaveSite()
		{
			try
			{
				if (this._activeSite.Id == -1)
				{
					base.CoreRepository.SaveObject(this._activeSite);
					Context.Response.Redirect("Default.aspx");
				}
				else
				{
					base.CoreRepository.UpdateObject(this._activeSite);
					ShowMessage("Site saved");
				}
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
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
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				this._activeSite.Name = txtName.Text;
				this._activeSite.SiteUrl = txtSiteUrl.Text;
				if (this.ddlTemplates.SelectedValue != "-1")
				{
					int templateId = Int32.Parse(this.ddlTemplates.SelectedValue);
					Template template = (Template) base.CoreRepository.GetObjectById(typeof (Template), templateId);
					this._activeSite.DefaultTemplate = template;
				}
				else if (this.ddlTemplates.SelectedValue == "-1")
				{
					this._activeSite.DefaultTemplate = null;
				}
				this._activeSite.DefaultCulture = this.ddlCultures.SelectedValue;
				SaveSite();
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("Default.aspx");
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this._activeSite.RootNodes.Count > 0)
				{
					ShowError("Can't delete a site when there are still related nodes. Please delete all nodes before deleting an entire site.");
				}
				else
				{
					base.CoreRepository.DeleteObject(this._activeSite);
					Context.Response.Redirect("Default.aspx");
				}
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}

	}
}
