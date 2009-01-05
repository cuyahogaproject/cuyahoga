using System;
using System.Collections;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Admin.UI;
using log4net;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for Sections.
	/// </summary>
	public class Sections : AdminBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Sections));

		protected Repeater rptSections;
		protected Button btnNew;
	
		private void Page_Load(object sender, EventArgs e)
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

		private void btnNew_Click(object sender, EventArgs e)
		{
			Context.Response.Redirect("SectionEdit.aspx?SectionId=-1");
		}

		private void rptSections_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Section section = e.Item.DataItem as Section;
			if (section != null)
			{	
				IList templates = this.CoreRepository.GetTemplatesBySection(section);
				HyperLink hplEdit = (HyperLink)e.Item.FindControl("hplEdit");
				hplEdit.NavigateUrl = String.Format("~/Admin/SectionEdit.aspx?SectionId={0}", section.Id);
				LinkButton lbtDelete = (LinkButton)e.Item.FindControl("lbtDelete");
				HyperLink hplAttachTemplate = (HyperLink)e.Item.FindControl("hplAttachTemplate");
				hplAttachTemplate.NavigateUrl = String.Format("~/Admin/SectionAttachTemplate.aspx?SectionId={0}", section.Id);
				HyperLink hplAttachNode = (HyperLink)e.Item.FindControl("hplAttachNode");
				if (templates.Count > 0)
				{
					Literal litTemplates = (Literal)e.Item.FindControl("litTemplates");
					for (int i = 0; i < templates.Count; i++)
					{
						litTemplates.Text += ((Template)templates[i]).Name;
						if (i < templates.Count - 1)
						{
							litTemplates.Text += ", ";
						}
					}
					hplAttachNode.Visible = false;
					lbtDelete.Visible = false;
				}
				else
				{
					hplAttachNode.NavigateUrl = String.Format("~/Admin/SectionAttachNode.aspx?SectionId={0}", section.Id);
				}
				lbtDelete.Attributes.Add("onclick", "return confirm('Are you sure?')");
			}
		}

		private void rptSections_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			int sectionId = Int32.Parse(e.CommandArgument.ToString());
			Section section = (Section)base.CoreRepository.GetObjectById(typeof(Section), sectionId);

			if (e.CommandName == "Delete")
			{
				try
				{
					// First tell the module to remove its content.
					ModuleBase module = this.ModuleLoader.GetModuleFromSection(section);
					module.DeleteModuleContent();

					// Remove from all template sections
					IList templates = this.CoreRepository.GetTemplatesBySection(section);
					foreach (Template template in templates)
					{
						string attachedPlaceholderId = null;
						foreach (DictionaryEntry entry in template.Sections)
						{
							if (entry.Value == section)
							{
								attachedPlaceholderId = entry.Key.ToString();
								break;
							}
						}
						template.Sections.Remove(attachedPlaceholderId);
						base.CoreRepository.UpdateObject(template);
					} 

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
	}
}
