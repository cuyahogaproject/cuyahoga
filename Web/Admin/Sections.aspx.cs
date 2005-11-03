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

using log4net;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Web.Admin.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for Sections.
	/// </summary>
	public class Sections : AdminBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Sections));

		protected System.Web.UI.WebControls.Repeater rptSections;
		protected System.Web.UI.WebControls.Button btnNew;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this.Title = "Sections";
			if (! this.IsPostBack)
			{
				BindSections();
			}
		}

		private void BindSections()
		{
			this.rptSections.DataSource = base.CoreRepository.GetUnconnectedSections();
			this.rptSections.DataBind();
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
			this.rptSections.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptSections_ItemDataBound);
			this.rptSections.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rptSections_ItemCommand);
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("SectionEdit.aspx?SectionId=-1");
		}

		private void rptSections_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			Section section = e.Item.DataItem as Section;
			if (section != null)
			{			
				HyperLink hplEdit = (HyperLink)e.Item.FindControl("hplEdit");
				hplEdit.NavigateUrl = String.Format("~/Admin/SectionEdit.aspx?SectionId={0}", section.Id);
				HyperLink hplAttach = (HyperLink)e.Item.FindControl("hplAttach");
				hplAttach.NavigateUrl = String.Format("~/Admin/SectionAttach.aspx?SectionId={0}", section.Id);
				LinkButton lbtDelete = (LinkButton)e.Item.FindControl("lbtDelete");
				lbtDelete.Attributes.Add("onClick", "return confirm('Are you sure?')");
			}
		}

		private void rptSections_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			int sectionId = Int32.Parse(e.CommandArgument.ToString());
			Section section = (Section)base.CoreRepository.GetObjectById(typeof(Section), sectionId);

			if (e.CommandName == "Delete")
			{
				try
				{
					// First tell the module to remove its content.
					ModuleBase module = section.CreateModule(UrlHelper.GetUrlFromSection(section));
					module.NHSessionRequired += new Cuyahoga.Core.Domain.ModuleBase.NHSessionEventHandler(module_NHSessionRequired);
					module.DeleteModuleContent();
					// Now delete the Section.
					base.CoreRepository.DeleteObject(section);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
					log.Error(String.Format("Error deleting section : {0}.", section.Id.ToString()), ex);
				}
			}
			BindSections();
		}

		private void module_NHSessionRequired(object sender, Cuyahoga.Core.Domain.ModuleBase.NHSessionEventArgs e)
		{
			e.Session = base.CoreRepository.ActiveSession;
		}
	}
}
