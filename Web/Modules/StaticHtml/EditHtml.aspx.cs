using System;
using System.Collections;
using System.ComponentModel;
// using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Web.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.DAL;
using Cuyahoga.Modules.StaticHtml;

namespace Cuyahoga.Web.Modules.StaticHtml
{
	/// <summary>
	/// Summary description for EditHtml.
	/// </summary>
	public class EditHtml : ModuleAdminBasePage
	{
		private StaticHtmlModule _module;

		protected Cuyahoga.ServerControls.CuyahogaEditor cedStaticHtml;
		protected System.Web.UI.WebControls.Button btnSave;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Section.Module as StaticHtmlModule;

			if (! this.IsPostBack)
			{
				if (this._module.StaticHtmlContent != null)
				{
					this.cedStaticHtml.Text = this._module.StaticHtmlContent.Content;
				}
			}
		}

		private void SaveStaticHtml()
		{
			if (this._module.StaticHtmlContent == null)
			{
				this._module.StaticHtmlContent = new StaticHtmlContent();
			}
			this._module.StaticHtmlContent.Content = this.cedStaticHtml.Text;
			IModulesDataProvider dp = ModulesDataProvider.GetInstance();
			if (this._module.StaticHtmlContent.Id == -1)
			{
				dp.InsertStaticHtmlContent(this._module.Section.Id, Int32.Parse(Context.User.Identity.Name), this._module.StaticHtmlContent);
			}
			else
			{
				dp.UpdateStaticHtmlContent(this._module.Section.Id, Int32.Parse(Context.User.Identity.Name), this._module.StaticHtmlContent);
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveStaticHtml();
				ShowMessage("Content saved.");
			}
			catch (Exception ex)
			{
				ShowMessage(ex.Message);
			}
		}	
	}
}
