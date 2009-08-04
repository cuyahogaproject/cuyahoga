using System.Collections.Generic;
using System;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Util;
using Cuyahoga.ServerControls;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Articles.Domain;

namespace Cuyahoga.Modules.Articles.Web
{
	/// <summary>
	///		Summary description for Articles.
	/// </summary>
	public partial class Articles : BaseModuleControl<ArticleModule>
	{
		private IList<Article> _articleList;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.pgrArticles.PageSize = this.Module.NumberOfArticlesInList;
			// We need to set the url where the pager needs to send the user to because otherwise
			// The pager would add pathinfo parameters to the node url, which are not parsed 
			// (instead of the section url).
			this.pgrArticles.PageUrl = UrlUtil.GetUrlFromSection(this.Module.Section);

			// Don't display the syndication icon on the article view
			base.DisplaySyndicationIcon = this.Module.AllowSyndication && this.Module.CurrentArticleId == -1;

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
				this._articleList = this.Module.GetArticleList();
			}
			this.rptArticles.DataSource = this._articleList;
			this.rptArticles.DataBind();
		}

		private void BindArchiveLink()
		{
			if (this.Module.ShowArchive)
			{
				if (this.Module.IsArchive)
				{
					this.hplToggleArchive.NavigateUrl = UrlUtil.GetUrlFromSection(this.Module.Section);
					this.hplToggleArchive.Text = base.GetText("CURRENT");
				}
				else
				{
					this.hplToggleArchive.NavigateUrl = UrlUtil.GetUrlFromSection(this.Module.Section) + "/archive";
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
			if (this.Module.CurrentAction == ArticleModuleAction.Category)
			{
				this.Module.DisplayTitle = String.Format("{0} {1}", GetText("CATEGORY"), this.Module.GetCategoryById(this.Module.CurrentCategoryId).Name);
			}

			if (this.Module.IsArchive)
			{
				this.Module.DisplayTitle += String.Format(" ({0}) ", GetText("ARCHIVE"));
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
				DisplayType displayType = (DisplayType)Enum.Parse(typeof(DisplayType), this.Module.Section.Settings["DISPLAY_TYPE"].ToString());

				HyperLink hpl = e.Item.FindControl("hplTitle") as HyperLink;
				string articleUrl = UrlHelper.GetUrlFromSection(this.Module.Section) + "/" + article.Id;
				hpl.NavigateUrl = articleUrl;

				Panel pnlSummary = e.Item.FindControl("pnlSummary") as Panel;
				pnlSummary.Visible = (displayType == DisplayType.HeadersAndSummary);
				HyperLink hplReadMore = e.Item.FindControl("hplReadMore") as HyperLink;
				hplReadMore.NavigateUrl = articleUrl;
				hplReadMore.Text = base.GetText("READMORE");

				Panel pnlContent = e.Item.FindControl("pnlContent") as Panel;
				pnlContent.Visible = (displayType == DisplayType.FullContent);

				Panel pnlArticleInfo = e.Item.FindControl("pnlArticleInfo") as Panel;

				if (this.Module.AllowComments || this.Module.ShowAuthor || this.Module.ShowCategory || this.Module.ShowDateTime)
				{
					pnlArticleInfo.Visible = true;

					Label lblDateOnline = e.Item.FindControl("lblDateOnline") as Label;
					lblDateOnline.Text = TimeZoneUtil.AdjustDateToUserTimeZone(article.PublishedAt.Value, this.Page.User.Identity).ToString();
					lblDateOnline.Visible = this.Module.ShowDateTime;

					Literal litAuthor = e.Item.FindControl("litAuthor") as Literal;
					litAuthor.Text = base.GetText("PUBLISHED") + " " + base.GetText("BY");
					litAuthor.Visible = this.Module.ShowAuthor;
					HyperLink hplAuthor = e.Item.FindControl("hplAuthor") as HyperLink;
					hplAuthor.NavigateUrl = this.Module.GetProfileUrl(article.CreatedBy.Id);
					hplAuthor.Text = article.CreatedBy.FullName;
					hplAuthor.Visible = this.Module.ShowAuthor;

					Literal litCategory = e.Item.FindControl("litCategory") as Literal;
					litCategory.Text = base.GetText("CATEGORIES");
					litCategory.Visible = this.Module.ShowCategory;

					CategoryDisplay cadCategories = (CategoryDisplay) e.Item.FindControl("cadCategories");
					cadCategories.SectionBaseUrl = UrlUtil.GetUrlFromSection(Module.Section);
					cadCategories.Categories = article.Categories;
					cadCategories.Visible = this.Module.ShowCategory;

					HyperLink hplComments = e.Item.FindControl("hplComments") as HyperLink;
					if (this.Module.AllowComments)
					{
						hplComments.NavigateUrl = UrlHelper.GetUrlFromSection(this.Module.Section)
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
