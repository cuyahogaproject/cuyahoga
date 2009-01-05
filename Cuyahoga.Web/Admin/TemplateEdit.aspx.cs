using System;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Admin.UI;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for TemplateEdit.
	/// </summary>
	public class TemplateEdit : AdminBasePage
	{
		private Template _activeTemplate;

		protected TextBox txtName;
		protected RequiredFieldValidator rfvName;
		protected TextBox txtPath;
		protected RequiredFieldValidator rfvPath;
		protected Button btnSave;
		protected DropDownList ddlCss;
		protected TextBox txtBasePath;
		protected RequiredFieldValidator rfvBasePath;
		protected DropDownList ddlTemplateControls;
		protected Button btnBack;
		protected Button btnVerifyBasePath;
		protected Label lblTemplateControlWarning;
		protected Label lblCssWarning;
		protected Panel pnlPlaceholders;
		protected Repeater rptPlaceholders;
		protected Button btnDelete;
	
		private void Page_Load(object sender, EventArgs e)
		{
			this.Title = "Edit template";

			if (Context.Request.QueryString["TemplateId"] != null)
			{
				if (Int32.Parse(Context.Request.QueryString["TemplateId"]) == -1)
				{
					this._activeTemplate = new Template();
				}
				else
				{
					this._activeTemplate = (Template)base.CoreRepository.GetObjectById(typeof(Template)
						, Int32.Parse(Context.Request.QueryString["TemplateId"]));
					this.pnlPlaceholders.Visible = true;
				}

				if (! this.IsPostBack)
				{
					BindTemplateControls();
					BindTemplateUserControls();
					BindCss();
					if (this._activeTemplate.Id != -1)
					{
						BindPlaceholders();
					}
				}
			}	
		}

		private void BindTemplateControls()
		{
			this.txtName.Text = this._activeTemplate.Name;
			this.txtBasePath.Text = this._activeTemplate.BasePath;
			this.btnDelete.Visible = (this._activeTemplate.Id > 0 && base.CoreRepository.GetNodesByTemplate(this._activeTemplate).Count <= 0);
			this.btnDelete.Attributes.Add("onclick", "return confirm('Are you sure?')");
		}

		private void BindTemplateUserControls()
		{
			this.ddlTemplateControls.Items.Clear();

			string physicalTemplateDir = Context.Server.MapPath(
				UrlHelper.GetApplicationPath() + this._activeTemplate.BasePath);
			DirectoryInfo dir = new DirectoryInfo(physicalTemplateDir);
			if (dir.Exists)
			{
				FileInfo[] templateControls = dir.GetFiles("*.ascx");
				if (templateControls.Length == 0 && this.IsPostBack)
				{
					this.lblTemplateControlWarning.Visible = true;
					this.lblTemplateControlWarning.Text = "No template user controls found at the [base path] location.";
				}
				else
				{
					foreach (FileInfo templateControlFile in templateControls)
					{
						this.ddlTemplateControls.Items.Add(templateControlFile.Name);
					}
					ListItem li = this.ddlTemplateControls.Items.FindByValue(this._activeTemplate.TemplateControl);
					if (li != null)
					{
						li.Selected = true;
					}
				}
			}
		}

		private void BindCss()
		{
			this.ddlCss.Items.Clear();

			string physicalCssDir = Context.Server.MapPath(
				UrlHelper.GetApplicationPath() + this._activeTemplate.BasePath + "/Css");
			DirectoryInfo dir = new DirectoryInfo(physicalCssDir);
			if (dir.Exists)
			{
				FileInfo[] cssSheets = dir.GetFiles("*.css");
				if (cssSheets.Length == 0 && this.IsPostBack)
				{
					this.lblCssWarning.Visible = true;
					this.lblCssWarning.Text = "No stylesheet files found at the [base path]/Css location.";
				}
				else
				{
					foreach (FileInfo css in cssSheets)
					{
						this.ddlCss.Items.Add(css.Name);
					}
					ListItem li = this.ddlCss.Items.FindByValue(this._activeTemplate.Css);
					if (li != null)
					{
						li.Selected = true;
					}
				}
			}
			else
			{
				this.lblCssWarning.Visible = true;
				this.lblCssWarning.Text = "The location for the stylesheets ([base path]/Css) could not be found.";
			}
		}

		private void BindPlaceholders()
		{
			// Load template control first.
			string templateControlPath = UrlHelper.GetApplicationPath() + this._activeTemplate.Path;
			if (File.Exists(Server.MapPath(templateControlPath)))
			{
				BaseTemplate templateControl = (BaseTemplate) this.Page.LoadControl(templateControlPath);
				this.rptPlaceholders.DataSource = templateControl.Containers;
				this.rptPlaceholders.DataBind();
			}
			else
			{
				ShowError("Unable to load the template control " + templateControlPath);
			}
		}

		private void CheckBasePath()
		{
			if (this._activeTemplate.BasePath.Trim() == String.Empty)
			{
				ShowError("The base path can not be empty.");
			}
			else
			{
				string physicalBasePath = Context.Server.MapPath(
					UrlHelper.GetApplicationPath() + this._activeTemplate.BasePath);
				if (! Directory.Exists(physicalBasePath))
				{
					ShowError("The base path you entered could not be found on the server.");
				}
				else
				{
					BindTemplateUserControls();
					BindCss();
				}
			}
		}

		private void SaveTemplate()
		{
			try
			{
				if (this._activeTemplate.Id == -1)
				{
					base.CoreRepository.SaveObject(this._activeTemplate);
					Context.Response.Redirect("Templates.aspx");
				}
				else
				{
					base.CoreRepository.UpdateObject(this._activeTemplate);
					ShowMessage("Template saved");
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
			this.btnVerifyBasePath.Click += new System.EventHandler(this.btnVerifyBasePath_Click);
			this.rptPlaceholders.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptPlaceholders_ItemDataBound);
			this.rptPlaceholders.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rptPlaceholders_ItemCommand);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnBack.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (this.IsValid)
			{
				if (this.ddlTemplateControls.SelectedIndex == -1 || this.ddlCss.SelectedIndex == -1)
				{
					ShowError("No template control or css selected.");
				}
				else
				{
					this._activeTemplate.Name = this.txtName.Text;
					this._activeTemplate.BasePath = this.txtBasePath.Text;
					this._activeTemplate.TemplateControl = this.ddlTemplateControls.SelectedValue;
					this._activeTemplate.Css = this.ddlCss.SelectedValue;
					SaveTemplate();
				}
			}	
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (this._activeTemplate.Id > 0)
			{
				try
				{
					base.CoreRepository.DeleteObject(this._activeTemplate);
					Context.Response.Redirect("Templates.aspx");
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Context.Response.Redirect("Templates.aspx");
		}

		private void btnVerifyBasePath_Click(object sender, EventArgs e)
		{
			this._activeTemplate.BasePath = this.txtBasePath.Text;
			CheckBasePath();
		}

		private void rptPlaceholders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DictionaryEntry entry = (DictionaryEntry)e.Item.DataItem;
				
				string placeholder = entry.Key.ToString();
				Label lblPlaceholder = e.Item.FindControl("lblPlaceholder") as Label;
				HyperLink hplSection = e.Item.FindControl("hplSection") as HyperLink;
				HyperLink hplAttachSection = e.Item.FindControl("hplAttachSection") as HyperLink;
				LinkButton lbtDetachSection = e.Item.FindControl("lbtDetachSection") as LinkButton;
				lblPlaceholder.Text = placeholder;

				// Find an attached section
				Section section = this._activeTemplate.Sections[placeholder] as Section;
				if (section != null)
				{
					hplSection.Text = section.Title;
					hplSection.NavigateUrl = "~/Admin/SectionEdit.aspx?SectionId=" + section.Id;
					hplSection.Visible = true;
					hplAttachSection.Visible = false;
					lbtDetachSection.Visible = true;
					lbtDetachSection.Attributes.Add("onclick", "return confirm(\"Are you sure?\");");
					lbtDetachSection.CommandArgument = placeholder;
				}
				else
				{
					hplSection.Visible = false;
					hplAttachSection.Visible = true;
					hplAttachSection.NavigateUrl = String.Format("~/Admin/TemplateSection.aspx?TemplateId={0}&Placeholder={1}"
						, this._activeTemplate.Id, placeholder);
					lbtDetachSection.Visible = false;
				}				
			}
		}

		private void rptPlaceholders_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "detach")
			{
				string placeholder = e.CommandArgument.ToString();
				this._activeTemplate.Sections.Remove(placeholder);

				try
				{
					base.CoreRepository.UpdateObject(this._activeTemplate);
					ShowMessage(String.Format("Section in Placeholder {0} detached", placeholder));
					BindPlaceholders();
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
		}
	}
}
