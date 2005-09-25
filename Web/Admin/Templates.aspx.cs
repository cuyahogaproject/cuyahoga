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

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for Templates.
	/// </summary>
	public class Templates : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		protected System.Web.UI.WebControls.Repeater rptTemplates;
		protected System.Web.UI.WebControls.Button btnNew;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Templates";
			if (! this.IsPostBack)
			{
				BindTemplates();
			}
		}

		private void BindTemplates()
		{
			this.rptTemplates.DataSource = base.CoreRepository.GetAll(typeof(Template), "BasePath", "Name");
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
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptTemplates_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			Template template = e.Item.DataItem as Template;
			if (template != null)
			{			
				HyperLink hplEdit = (HyperLink)e.Item.FindControl("hplEdit");
				hplEdit.NavigateUrl = String.Format("~/Admin/TemplateEdit.aspx?TemplateId={0}", template.Id);
			}
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("TemplateEdit.aspx?TemplateId=-1");
		}
	}
}
