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

using Cuyahoga.Core.Util;
using Cuyahoga.Modules.Articles;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.Articles
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
			this.btnNew.Attributes.Add("onclick", String.Format("document.location.href='EditArticle.aspx{0}&ArticleId=-1'", base.GetBaseQueryString()));

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
			this.pgrArticles.PageChanged += new Cuyahoga.ServerControls.PageChangedEventHandler(this.pgrArticles_PageChanged);
			this.pgrArticles.CacheEmpty += new System.EventHandler(this.pgrArticles_CacheEmpty);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptArticles_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			Article article = e.Item.DataItem as Article;

			Literal litDateOnline = e.Item.FindControl("litDateOnline") as Literal;
			if (litDateOnline != null)
			{
				litDateOnline.Text = TimeZoneUtil.AdjustDateToUserTimeZone(article.DateOnline, this.User.Identity).ToString();
			}
			Literal litDateOffline = e.Item.FindControl("litDateOffline") as Literal;
			if (litDateOffline != null)
			{
				litDateOffline.Text = TimeZoneUtil.AdjustDateToUserTimeZone(article.DateOffline, this.User.Identity).ToString();
			}

			HyperLink hplEdit = e.Item.FindControl("hpledit") as HyperLink;
			if (hplEdit != null)
			{
				hplEdit.NavigateUrl = String.Format("~/Modules/Articles/EditArticle.aspx{0}&ArticleId={1}", base.GetBaseQueryString(), article.Id);
			}
			HyperLink hplComments = e.Item.FindControl("hplComments") as HyperLink;
			if (hplComments != null)
			{
				hplComments.NavigateUrl = String.Format("~/Modules/Articles/AdminComments.aspx{0}&ArticleId={1}", base.GetBaseQueryString(), article.Id);
			}
		}

		private void pgrArticles_PageChanged(object sender, Cuyahoga.ServerControls.PageChangedEventArgs e)
		{
			this.rptArticles.DataBind();
		}

		private void pgrArticles_CacheEmpty(object sender, System.EventArgs e)
		{
			this.rptArticles.DataSource = this._articleModule.GetAllArticles();
		}
	}
}
