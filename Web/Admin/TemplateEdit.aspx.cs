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
using System.IO;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for TemplateEdit.
	/// </summary>
	public class TemplateEdit : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		private Template _activeTemplate;

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.TextBox txtPath;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvPath;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.DropDownList ddlCss;
		protected System.Web.UI.WebControls.TextBox txtBasePath;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvBasePath;
		protected System.Web.UI.WebControls.DropDownList ddlTemplateControls;
		protected System.Web.UI.WebControls.Button btnBack;
		protected System.Web.UI.WebControls.Button btnVerifyBasePath;
		protected System.Web.UI.WebControls.Label lblTemplateControlWarning;
		protected System.Web.UI.WebControls.Label lblCssWarning;
		protected System.Web.UI.WebControls.Button btnDelete;
	
		private void Page_Load(object sender, System.EventArgs e)
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
				}

				if (! this.IsPostBack)
				{
					BindTemplateControls();
					BindTemplateUserControls();
					BindCss();
				}
			}	
		}

		private void BindTemplateControls()
		{
			this.txtName.Text = this._activeTemplate.Name;
			this.txtBasePath.Text = this._activeTemplate.BasePath;
			this.btnDelete.Visible = (this._activeTemplate.Id > 0 && base.CoreRepository.GetNodesByTemplate(this._activeTemplate).Count <= 0);
			this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?')");
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnBack.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnVerifyBasePath.Click += new System.EventHandler(this.btnVerifyBasePath_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				this._activeTemplate.Name = this.txtName.Text;
				this._activeTemplate.BasePath = this.txtBasePath.Text;
				this._activeTemplate.TemplateControl = this.ddlTemplateControls.SelectedValue;
				this._activeTemplate.Css = this.ddlCss.SelectedValue;
				SaveTemplate();
			}	
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
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

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("Templates.aspx");
		}

		private void btnVerifyBasePath_Click(object sender, System.EventArgs e)
		{
			this._activeTemplate.BasePath = this.txtBasePath.Text;
			CheckBasePath();
		}
	}
}
