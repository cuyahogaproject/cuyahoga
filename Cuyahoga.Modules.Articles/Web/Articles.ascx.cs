namespace Cuyahoga.Modules.Articles.Web
{
	using System;
	using System.Collections;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text.RegularExpressions;

	using Cuyahoga.Core.Util;
	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Modules.Articles;
	using Cuyahoga.Modules.Articles.Domain;

	/// <summary>
	///		Summary description for Articles.
	/// </summary>
	public partial class Articles : BaseModuleControl
	{
		private ArticleModule _module;
		private IList _articleList;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as ArticleModule;
			this.pgrArticles.PageSize = this._module.NumberOfArticlesInList;
			// We need to set the url where the pager needs to send the user to because otherwise
			// The pager would add pathinfo parameters to the node url, which are not parsed 
			// (instead of the section url).
			this.pgrArticles.PageUrl = UrlHelper.GetUrlFromSection(this._module.Section);

			// Don't display the syndication icon on the article view
			base.DisplaySyndicationIcon = this._module.AllowSyndication && this._module.CurrentArticleId == -1;

			if (! IsPostBack && ((! base.HasCachedOutput) || this.Page.User.Identity.IsAuthenticated))
			{
				BindArticles();
			}

			SetDisplayTitle();

			BindArchiveLink();
		}

		private void BindArticles()
		{
			// Article list view
			if (this._articleList == null)
			{
				this._articleList = this._module.GetArticleList();
			}
			this.rptArticles.DataSource = this._articleList;
			this.rptArticles.DataBind();
		}

		private void BindArchiveLink()
		{
			if (this._module.ShowArchive)
			{
				if (this._module.IsArchive)
				{
					this.hplToggleArchive.NavigateUrl = UrlHelper.GetUrlFromSection(this._module.Section);
					this.hplToggleArchive.Text = base.GetText("CURRENT");
				}
				else
				{
					this.hplToggleArchive.NavigateUrl = UrlHelper.GetUrlFromSection(this._module.Section) + "/archive";
					this.hplToggleArchive.Text = base.GetText("ARCHIVE");
				}
				this.hplToggleArchive.Visible = true;
			}
			else
			{
				this.hplToggleArchive.Visible = false;
			}
		}

		private void SetDisplayTitle()
		{
			if (this._module.CurrentAction == ArticleModuleAction.Category)
			{
				this._module.DisplayTitle = String.Format("{0} {1}", GetText("CATEGORY"), this._module.GetCategoryById(this._module.CurrentCategoryId).Title);
			}

			if (this._module.IsArchive)
			{
				this._module.DisplayTitle += String.Format(" ({0}) ", GetText("ARCHIVE"));
			}
		}

		protected void pgrArticles_PageChanged(object sender, Cuyahoga.ServerControls.PageChangedEventArgs e)
		{
			if (this.IsPostBack || ((! base.HasCachedOutput) || this.Page.User.Identity.IsAuthenticated))
			{
				BindArticles();
			}
		}

		protected void rptArticles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Article article = e.Item.DataItem as Article;
				DisplayType displayType = (DisplayType)Enum.Parse(typeof(DisplayType), this._module.Section.Settings["DISPLAY_TYPE"].ToString());

				HyperLink hpl = e.Item.FindControl("hplTitle") as HyperLink;
				string articleUrl = UrlHelper.GetUrlFromSection(this._module.Section) + "/" + article.Id;
				hpl.NavigateUrl = articleUrl;

				Panel pnlSummary = e.Item.FindControl("pnlSummary") as Panel;
				pnlSummary.Visible = (displayType == DisplayType.HeadersAndSummary);
				HyperLink hplReadMore = e.Item.FindControl("hplReadMore") as HyperLink;
				hplReadMore.NavigateUrl = articleUrl;
				hplReadMore.Text = base.GetText("READMORE");

				Panel pnlContent = e.Item.FindControl("pnlContent") as Panel;
				pnlContent.Visible = (displayType == DisplayType.FullContent);

				Panel pnlArticleInfo = e.Item.FindControl("pnlArticleInfo") as Panel;

				if (this._module.AllowComments || this._module.ShowAuthor || this._module.ShowCategory || this._module.ShowDateTime)
				{
					pnlArticleInfo.Visible = true;

					Label lblDateOnline = e.Item.FindControl("lblDateOnline") as Label;
					lblDateOnline.Text = TimeZoneUtil.AdjustDateToUserTimeZone(article.DateOnline, this.Page.User.Identity).ToString();
					lblDateOnline.Visible = this._module.ShowDateTime;

					Literal litAuthor = e.Item.FindControl("litAuthor") as Literal;
					litAuthor.Text = base.GetText("PUBLISHED") + " " + base.GetText("BY");
					litAuthor.Visible = this._module.ShowAuthor;
					HyperLink hplAuthor = e.Item.FindControl("hplAuthor") as HyperLink;
					hplAuthor.NavigateUrl = this._module.GetProfileUrl(article.CreatedBy.Id);
					hplAuthor.Text = article.CreatedBy.FullName;
					hplAuthor.Visible = this._module.ShowAuthor;

					Literal litCategory = e.Item.FindControl("litCategory") as Literal;
					litCategory.Text = base.GetText("CATEGORY");
					litCategory.Visible = this._module.ShowCategory;
					HyperLink hplCategory = e.Item.FindControl("hplCategory") as HyperLink;
					if (article.Category != null)
					{
						hplCategory.NavigateUrl = UrlHelper.GetUrlFromSection(this.Module.Section) +
							String.Format("/category/{0}", article.Category.Id);
						hplCategory.Text = article.Category.Title;
					}
					else
					{
						hplCategory.Text = String.Empty;
					}
					hplCategory.Visible = this._module.ShowCategory;

					HyperLink hplComments = e.Item.FindControl("hplComments") as HyperLink;
					if (this._module.AllowComments)
					{
						hplComments.NavigateUrl = UrlHelper.GetUrlFromSection(this._module.Section)
							+ String.Format("/{0}#comments", article.Id);
						hplComments.Text = base.GetText("COMMENTS") + " " + article.Comments.Count.ToString();
					}
					else
					{
						hplComments.Visible = false;
					}
				}
				else
				{
					pnlArticleInfo.Visible = false;
				}
			}
		}
	}
}
