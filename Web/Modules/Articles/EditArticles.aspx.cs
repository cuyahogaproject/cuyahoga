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

using Cuyahoga.Core;
using Cuyahoga.Modules.Articles;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Modules.Articles
{
	/// <summary>
	/// Summary description for EditArticles.
	/// </summary>
	public class EditArticles : ModuleAdminBasePage
	{
		protected System.Web.UI.WebControls.Repeater rptArticles;
		protected Cuyahoga.ServerControls.Pager pgrArticles;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnNew;
		private ArticleModule _module;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Section.Module as ArticleModule;
			this.btnNew.Attributes.Add("onClick", String.Format("document.location.href=\"EditArticle.aspx?NodeId={0}&SectionId={1}&ArticleId=-1\"", base.Node.Id, base.Section.Id));

			if (! this.IsPostBack)
			{
				this.rptArticles.DataSource = this._module.Articles;
				this.rptArticles.DataBind();
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
