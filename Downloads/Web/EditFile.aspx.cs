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
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.Downloads.Web
{
	/// <summary>
	/// Summary description for EditFile.
	/// </summary>
	public class EditFile : ModuleAdminBasePage
	{
		private DownloadsModule _downloadsModule;

		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// The base page has already created the module, we only have to cast it here to the right type.
			this._downloadsModule = base.Module as DownloadsModule;

			if (! this.IsPostBack)
			{
				// Check existence of physical dir.
				try
				{
					string test = this._downloadsModule.FileDir;
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
