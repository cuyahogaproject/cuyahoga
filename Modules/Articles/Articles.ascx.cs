namespace Cuyahoga.Modules.Articles
{
	using System;
	// using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text.RegularExpressions;

	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Modules.Articles;

	/// <summary>
	///		Summary description for Articles.
	/// </summary>
	public class Articles : BaseModuleControl
	{
		private ArticleModule _module;
		private Article _activeArticle;

		protected System.Web.UI.WebControls.Panel pnlArticleDetails;
		protected System.Web.UI.WebControls.Repeater rptArticles;
		protected System.Web.UI.WebControls.Literal litContent;
		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.WebControls.HyperLink hplBack;
		protected System.Web.UI.WebControls.TextBox txtComment;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button btnSaveComment;
		protected System.Web.UI.WebControls.Panel pnlComment;
		protected System.Web.UI.WebControls.Repeater rptComments;
		protected System.Web.UI.WebControls.Panel pnlArticleList;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as ArticleModule;
			if (this._module != null && (! base.HasCachedOutput || this.Page.IsPostBack))
			{
				if (this._module.CurrentArticleId > 0)
				{
					// Article view
					this._activeArticle = this._module.GetArticleById(this._module.CurrentArticleId);
					this.litTitle.Text = this._activeArticle.Title;
					this.litContent.Text = this._activeArticle.Content;

					this.hplBack.NavigateUrl = UrlHelper.GetUrlFromSection(this._module.Section);
					this.hplBack.Text = base.GetText("BACK");
					this.btnSaveComment.Text = base.GetText("BTNSAVECOMMENT");

					this.pnlArticleDetails.Visible = true;
					if (this.Page.User.Identity.IsAuthenticated)
					{
						this.pnlComment.Visible = true;
					}
					else
					{
						this.pnlComment.Visible = false;
					}
					// Comments
					this.rptComments.DataSource = this._activeArticle.Comments;
					this.rptComments.DataBind();
				}
				else if (this._module.CurrentCategory != null)
				{
					// Category view
				}
				else
				{
					// Article list view
					int numberOfArticles = 10;
					if (this._module.Section.Settings["NUMBER_OF_ARTICLES_IN_LIST"] != null)
					{
						numberOfArticles = Int32.Parse(this._module.Section.Settings["NUMBER_OF_ARTICLES_IN_LIST"].ToString());
					}
					this.rptArticles.ItemDataBound += new RepeaterItemEventHandler(rptArticles_ItemDataBound);
					this.rptArticles.DataSource = this._module.GetDisplayArticles(numberOfArticles);
					this.rptArticles.DataBind();
					this.pnlArticleList.Visible = true;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnSaveComment.Click += new System.EventHandler(this.btnSaveComment_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptArticles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Article article = e.Item.DataItem as Article;
			HyperLink hpl = e.Item.FindControl("hplTitle") as HyperLink;
			hpl.NavigateUrl = UrlHelper.GetUrlFromSection(this._module.Section) + "/" + article.Id;

			Panel pnlSummary = e.Item.FindControl("pnlSummary") as Panel;
			Panel pnlContent = e.Item.FindControl("pnlContent") as Panel;
			DisplayType displayType = (DisplayType)Enum.Parse(typeof(DisplayType), this._module.Section.Settings["DISPLAY_TYPE"].ToString());
			pnlSummary.Visible = (displayType == DisplayType.FullContent || displayType == DisplayType.HeadersAndSummary);
			pnlContent.Visible = (displayType == DisplayType.FullContent);
		}

		private void btnSaveComment_Click(object sender, System.EventArgs e)
		{
			// Strip any html tags.
			string commentText = Regex.Replace(this.txtComment.Text, @"<(.|\n)*?>", string.Empty);
			// Replace carriage returns with <br>.
			commentText = commentText.Replace("\r", "<br>");
			// Save comment.
			Comment comment = new Comment();
			comment.Article = this._activeArticle;
			comment.CommentText = commentText;
			comment.User = this.Page.User.Identity as Cuyahoga.Core.Domain.User;
			try
			{
				this._module.SaveComment(comment);
				Context.Response.Redirect(UrlHelper.GetUrlFromSection(this._module.Section) + "/" + this._activeArticle.Id.ToString());
			}
			catch (Exception ex)
			{
				this.lblError.Text = ex.Message;
				this.lblError.Visible = true;
			}
		}
	}
}
