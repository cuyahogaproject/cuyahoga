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
			this._module = base.Module as StaticHtmlModule;

			if (! this.IsPostBack)
			{
				StaticHtmlContent shc = this._module.GetContent();
				if (shc != null)
				{
					this.cedStaticHtml.Text = shc.Content;
				}
				else
				{
					this.cedStaticHtml.Text = String.Empty;
				}
			}
		}

		private void SaveStaticHtml()
		{
			Cuyahoga.Core.Domain.User currentUser = (Cuyahoga.Core.Domain.User)Context.User.Identity;
			StaticHtmlContent content = this._module.GetContent();
			if (content == null)
			{
				// New
				content = new StaticHtmlContent();
				content.Section = this._module.Section;
				content.CreatedBy = currentUser;
			}
			else
			{
				// Exisiting
				content.ModifiedBy = currentUser;
			}
			content.Content = this.cedStaticHtml.Text;
			this._module.SaveContent(content);	
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
