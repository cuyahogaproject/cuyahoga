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
using System.Configuration;

using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Admin
{
	/// <summary>
	/// Summary description for TemplatePreview.
	/// </summary>
	public class TemplatePreview : System.Web.UI.Page
	{
		protected override void AddParsedSubObject(object obj)
		{
			string template = Context.Request.QueryString["Template"];
			
			BaseTemplate templateControl = (BaseTemplate)this.LoadControl(template);
			// TODO: insert final css (configurationfile?)
			templateControl.Css = UrlHelper.GetApplicationPath() + Config.GetConfiguration()["Css"];
			templateControl.InsertContainerButtons();
			this.Controls.Add(templateControl);
			
			base.AddParsedSubObject (obj);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
