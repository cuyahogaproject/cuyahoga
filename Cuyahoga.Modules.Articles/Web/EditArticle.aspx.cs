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
using Cuyahoga.Core.Util;
using Cuyahoga.Modules.Articles;
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.Articles.Domain;

using ArticleCategory = Cuyahoga.Modules.Articles.Domain.Category;

namespace Cuyahoga.Modules.Articles.Web
{
	/// <summary>
	/// Summary description for EditArticle.
	/// </summary>
	public partial class EditArticle : ModuleAdminBasePage
	{
		private Article _article;
		private ArticleModule _articleModule;

		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDateOnline;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDateOffline;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.fckContent.BasePath = this.Page.ResolveUrl("~/Support/FCKeditor/");
			this._articleModule = base.Module as ArticleModule;
			this.btnCancel.Attributes.Add("onclick", String.Format("document.location.href='AdminArticles.aspx{0}'", base.GetBaseQueryString()));

			if (! this.IsPostBack)
			{
				BindCategories();
			}

			if (Request.QueryString["ArticleId"] != null)
			{
				int articleId = Int32.Parse(Request.QueryString["ArticleId"]);
				if (articleId > 0)
				{
					this._article = this._articleModule.GetArticleById(articleId);
					if (! this.IsPostBack)
					{
						BindArticle();
					}
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onclick", "return confirm('Are you sure?');");
				}
			}
		}

		private void BindCategories()
		{
			this.ddlCategory.Items.Add(new ListItem("none", "0"));
			IList categories = this._articleModule.GetAvailableCategories();
			foreach (ArticleCategory category in categories)
			{
				this.ddlCategory.Items.Add(new ListItem(category.Title, category.Id.ToString()));
			}
		}

		private void BindArticle()
		{
			this.txtTitle.Text = this._article.Title;
			this.txtSummary.Text = this._article.Summary;
			this.fckContent.Value = this._article.Content;
			this.chkSyndicate.Checked = this._article.Syndicate;
			this.calDateOnline.SelectedDate = TimeZoneUtil.AdjustDateToUserTimeZone(this._article.DateOnline, this.User.Identity);
			this.calDateOffline.SelectedDate = TimeZoneUtil.AdjustDateToUserTimeZone(this._article.DateOffline, this.User.Identity);
			if (this._article.Category != null)
			{
				ListItem li = this.ddlCategory.Items.FindByValue(this._article.Category.Id.ToString());
				if (li != null)
				{
					li.Selected = true;
				}
			}
		}

		private void SaveArticle()
		{
			try
			{
				this._article.Title = this.txtTitle.Text;
				if (this.txtSummary.Text.Length > 0)
				{
					this._article.Summary = this.txtSummary.Text;
				}
				else
				{
					this._article.Summary = null;
				}
				this._article.Content = this.fckContent.Value;
				if (this.ddlCategory.SelectedIndex > 0)
				{
					
					this._article.Category = this._articleModule.GetCategoryById(Int32.Parse(this.ddlCategory.SelectedValue));
				}
				else if (this.txtCategory.Text.Length > 0)
				{
					this._article.Category = new ArticleCategory();
					this._article.Category.Title = this.txtCategory.Text;
				}
				else
				{
					this._article.Category = null;
				}
				this._article.Syndicate = this.chkSyndicate.Checked;
				if (this.calDateOnline.SelectedDate != DateTime.MinValue)
				{
					this._article.DateOnline = TimeZoneUtil.AdjustDateToServerTimeZone(this.calDateOnline.SelectedDate, this.User.Identity);
				}
				else
				{
					this._article.DateOnline = DateTime.Now;
				}
				if (this.calDateOffline.SelectedDate != DateTime.MinValue)
				{
					this._article.DateOffline = TimeZoneUtil.AdjustDateToServerTimeZone(this.calDateOffline.SelectedDate, this.User.Identity);
				}
				else
				{
					this._article.DateOffline = new DateTime(2199, 1, 1);
				}
				this._article.ModifiedBy = (Cuyahoga.Core.Domain.User)this.User.Identity;
				this._articleModule.SaveArticle(this._article);
				Response.Redirect(String.Format("AdminArticles.aspx{0}", base.GetBaseQueryString()));
			}
			catch (Exception ex)
			{
				ShowException(ex);
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

		}
		#endregion

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				if (this._article == null)
				{
					this._article = new Article();
					this._article.Section = this._articleModule.Section;
					this._article.CreatedBy = (Cuyahoga.Core.Domain.User)this.User.Identity;
				}
				SaveArticle();
			}
		}

		protected void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._article != null)
			{
				try
				{
					this._articleModule.DeleteArticle(this._article);
					Response.Redirect(String.Format("AdminArticles.aspx{0}", base.GetBaseQueryString()));
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
			else
			{
				ShowError("No article found to delete");
			}
		}
	}
}
