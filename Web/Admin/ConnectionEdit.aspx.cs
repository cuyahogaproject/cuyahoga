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
using Cuyahoga.Core.Communication;

using Cuyahoga.Web.Components;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for ConnectionEdit.
	/// </summary>
	public class ConnectionEdit : Cuyahoga.Web.Admin.UI.AdminBasePage
	{
		private Section _activeSection;
		private IActionProvider _activeActionProvider;

		protected System.Web.UI.WebControls.Panel pnlTo;
		protected System.Web.UI.WebControls.Label lblSectionFrom;
		protected System.Web.UI.WebControls.Label lblModuleType;
		protected System.Web.UI.WebControls.DropDownList ddlAction;
		protected System.Web.UI.WebControls.DropDownList ddlSectionTo;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnBack;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (Context.Request.QueryString["SectionId"] != null)
			{
				this.Title = "Add connection";

				// Get section data
				this._activeSection = (Section)base.CoreRepository.GetObjectById(typeof(Section), 
					Int32.Parse(Context.Request.QueryString["SectionId"]));

				ModuleBase moduleInstance = base.ModuleLoader.GetModuleFromSection(this._activeSection);
				if (moduleInstance is IActionProvider)
				{
					this._activeActionProvider = moduleInstance as IActionProvider;

					if (! this.IsPostBack)
					{
						BindSection();
						BindCompatibleSections();
					}
				}
				else
				{
					ShowError("The module that is connected to the section doesn't support outgoing connections.");
				}
			}
		}

		private void BindSection()
		{		
			this.lblSectionFrom.Text = this._activeSection.FullName;
			this.lblModuleType.Text = this._activeSection.ModuleType.Name;
			ActionCollection outboundActions = this._activeActionProvider.GetOutboundActions();
			foreach (Action action in outboundActions)
			{
				// Only add actions that are not assigned yet.
				if (this._activeSection.Connections[action.Name] == null)
				{
					this.ddlAction.Items.Add(action.Name);
				}
			}
		}

		private void BindCompatibleSections()
		{
			string selectedAction = String.Empty;

			if (this.ddlAction.SelectedIndex == -1 && this.ddlAction.Items.Count > 0)
			{
				selectedAction = this.ddlAction.Items[0].Value;
			}
			else
			{
				selectedAction = this.ddlAction.SelectedValue;
			}

			ArrayList compatibleModuleTypes = new ArrayList();
			// Get all ModuleTypes.
			IList moduleTypes = CoreRepository.GetAll(typeof(ModuleType));
			foreach (ModuleType mt in moduleTypes)
			{
				string assemblyQualifiedName = mt.ClassName + ", " + mt.AssemblyName;
				Type moduleTypeType = Type.GetType(assemblyQualifiedName);

                if (moduleTypeType != null) // throw exception when moduleTypeType == null?
                {
                    ModuleBase moduleInstance = base.ModuleLoader.GetModuleFromType(mt);
                    if (moduleInstance is IActionConsumer)
                    {
                        IActionConsumer actionConsumer = moduleInstance as IActionConsumer;
                        Action currentAction = this._activeActionProvider.GetOutboundActions().FindByName(selectedAction);
                        if (actionConsumer.GetInboundActions().Contains(currentAction))
                        {
                            compatibleModuleTypes.Add(mt);
                        }
                    }
                }
			}

			if (compatibleModuleTypes.Count > 0)
			{
				// Retrieve all sections that have the compatible ModuleTypes
				IList compatibleSections = CoreRepository.GetSectionsByModuleTypes(compatibleModuleTypes);
				if (compatibleSections.Count > 0)
				{
					this.pnlTo.Visible = true;
					this.btnSave.Enabled = true;
					this.ddlSectionTo.DataSource = compatibleSections;
					this.ddlSectionTo.DataValueField = "Id";
					this.ddlSectionTo.DataTextField = "FullName";
					this.ddlSectionTo.DataBind();
				}
				else
				{
					this.pnlTo.Visible = false;
					this.btnSave.Enabled = false;
				}
			}
		}

		private void RedirectToSectionEdit()
		{
			if (this.ActiveNode != null)
			{
				Context.Response.Redirect(String.Format("SectionEdit.aspx?NodeId={0}&SectionId={1}", this.ActiveNode.Id, this._activeSection.Id));
			}
			else
			{
				Context.Response.Redirect(String.Format("SectionEdit.aspx?SectionId={0}", this._activeSection.Id));
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
			this.ddlAction.SelectedIndexChanged += new System.EventHandler(this.ddlAction_SelectedIndexChanged);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			RedirectToSectionEdit();
		}

		private void ddlAction_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BindCompatibleSections();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			this._activeSection.Connections[this.ddlAction.SelectedValue] 
				= base.CoreRepository.GetObjectById(typeof(Section)
					, Int32.Parse(this.ddlSectionTo.SelectedValue));
			try
			{
				base.CoreRepository.UpdateObject(this._activeSection);

				RedirectToSectionEdit();
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}
	}
}
