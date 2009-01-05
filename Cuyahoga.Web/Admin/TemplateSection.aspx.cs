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
using Cuyahoga.Web.Admin.UI;


namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for TemplateSection.
	/// </summary>
	public class TemplateSection : AdminBasePage
	{
		private Template _activeTemplate;
		private string _activePlaceholder;

		protected System.Web.UI.WebControls.Button btnAttach;
		protected System.Web.UI.WebControls.Label lblTemplate;
		protected System.Web.UI.WebControls.Label lblPlaceholder;
		protected System.Web.UI.WebControls.DropDownList ddlSections;
		protected System.Web.UI.WebControls.Button btnBack;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Attach Section to Template Placeholder";
			if (Context.Request.QueryString["TemplateId"] != null 
				&& Context.Request.QueryString["Placeholder"] != null)
			{
				this._activeTemplate = (Template)base.CoreRepository.GetObjectById(typeof(Template)
					, Int32.Parse(Context.Request.QueryString["TemplateId"]));
				this._activePlaceholder = Context.Request.QueryString["Placeholder"];

				if (! this.IsPostBack)
				{
					BindTemplateControls();
					BindSections();
				}
			}
			else
			{
				ShowError("Passed invalid template or placeholder");
			}
		}

		private void BindTemplateControls()
		{
			this.lblTemplate.Text = this._activeTemplate.Name;
			this.lblPlaceholder.Text = this._activePlaceholder;
		}

		private void BindSections()
		{
			IList unconnectedSections = base.CoreRepository.GetUnconnectedSections();
			if (unconnectedSections.Count > 0)
			{
				this.ddlSections.DataSource = unconnectedSections;
				this.ddlSections.DataValueField = "Id";
				this.ddlSections.DataTextField = "Title";
				this.ddlSections.DataBind();
			}
			else
			{
				this.btnAttach.Enabled = false;
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
			this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnAttach_Click(object sender, System.EventArgs e)
		{
			int selectedSectionId = Int32.Parse(this.ddlSections.SelectedValue);
			Section section = (Section)base.CoreRepository.GetObjectById(typeof(Section), selectedSectionId);
			this._activeTemplate.Sections[this._activePlaceholder] = section;
			try
			{
				base.CoreRepository.UpdateObject(this._activeTemplate);
				Context.Response.Redirect("~/Admin/TemplateEdit.aspx?TemplateId=" + this._activeTemplate.Id);
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("~/Admin/TemplateEdit.aspx?TemplateId=" + this._activeTemplate.Id);
		}
	}
}
