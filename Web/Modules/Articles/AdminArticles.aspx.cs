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
	public class AdminArticles : ModuleAdminBasePage
	{
		protected System.Web.UI.WebControls.Repeater rptArticles;
		protected Cuyahoga.ServerControls.Pager pgrArticles;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnNew;
		private ArticleModule _articleModule;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// The base page has already created the module, we only have to cast it here to the right type.
			this._articleModule = base.Module as ArticleModule;
			this.btnNew.Attributes.Add("onClick", String.Format("document.location.href='EditArticle.aspx{0}&ArticleId=-1'", base.GetBaseQueryString()));

			if (! this.IsPostBack)
			{
				this.rptArticles.DataSource = this._articleModule.GetAllArticles();
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
			this.rptArticles.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptArticles_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptArticles_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			HyperLink hplEdit = e.Item.FindControl("hpledit") as HyperLink;
			if (hplEdit != null)
			{
				Article article = e.Item.DataItem as Article;
				hplEdit.NavigateUrl = String.Format("~/Modules/Articles/EditArticle.aspx{0}&ArticleId={1}", base.GetBaseQueryString(), article.Id);
			}
		}
	}
}
