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
using Cuyahoga.Modules.Articles;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Modules.Articles
{
	/// <summary>
	/// Summary description for EditArticle.
	/// </summary>
	public class EditArticle : ModuleAdminBasePage
	{
		private Article _article;

		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.TextBox txtSummary;
		protected Cuyahoga.ServerControls.CuyahogaEditor cedContent;
		protected System.Web.UI.WebControls.CheckBox chkSyndicate;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTitle;
		protected System.Web.UI.WebControls.TextBox txtCategory;
		protected System.Web.UI.WebControls.DropDownList ddlCategory;
		protected Cuyahoga.ServerControls.Calendar calDateOnline;
		protected Cuyahoga.ServerControls.Calendar calDateOffline;
		private ArticleModule _module;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Section.Module as ArticleModule;
			this.btnCancel.Attributes.Add("onClick", String.Format("document.location.href='AdminArticles.aspx{0}'", base.GetBaseQueryString()));

			if (! this.IsPostBack)
			{
				BindCategories();
			}

			if (Request.QueryString["ArticleId"] != null)
			{
				int articleId = Int32.Parse(Request.QueryString["ArticleId"]);
				if (articleId > 0)
				{
					this._article = this._module.GetArticleById(articleId);
					if (! this.IsPostBack)
					{
						BindArticle();
					}
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?');");
				}
			}
		}

		private void BindCategories()
		{
			this.ddlCategory.Items.Add(new ListItem("none", "0"));
			IList categories = this._module.GetAvailableCategories();
			foreach (Category category in categories)
			{
				this.ddlCategory.Items.Add(new ListItem(category.Title, category.Id.ToString()));
			}
		}

		private void BindArticle()
		{
			this.txtTitle.Text = this._article.Title;
			this.txtSummary.Text = this._article.Summary;
			this.cedContent.Text = this._article.Content;
			this.chkSyndicate.Checked = this._article.Syndicate;
			this.calDateOnline.SelectedDate = this._article.DateOnline;
			this.calDateOffline.SelectedDate	= this._article.DateOffline;
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
				this._article.Content = this.cedContent.Text;
				if (this.ddlCategory.SelectedIndex > 0)
				{
					this._article.Category = new Category();
					this._article.Category.Id = Int32.Parse(this.ddlCategory.SelectedValue);
				}
				else if (this.txtCategory.Text.Length > 0)
				{
					this._article.Category = new Category();
					this._article.Category.Title = this.txtCategory.Text;
				}
				else
				{
					this._article.Category = null;
				}
				this._article.Syndicate = this.chkSyndicate.Checked;
				this._article.DateOnline = this.calDateOnline.SelectedDate;
				this._article.DateOffline = this.calDateOffline.SelectedDate;
				this._article.ModifiedBy = (Cuyahoga.Core.Domain.User)this.User.Identity;
				this._article.DateModified = DateTime.Now;
				this._module.SaveArticle(this._article);
				Response.Redirect(String.Format("AdminArticles.aspx{0}", base.GetBaseQueryString()));
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
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
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				if (this._article == null)
				{
					this._article = new Article();
					this._article.Section = this._module.Section;
					this._article.CreatedBy = (Cuyahoga.Core.Domain.User)this.User.Identity;
				}
				SaveArticle();
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._article != null)
			{
				try
				{
					this._module.DeleteArticle(this._article);
					Response.Redirect(String.Format("AdminArticles.aspx{0}", base.GetBaseQueryString()));
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
			else
			{
				ShowError("No article found for deletion");
			}
		}
	}
}
