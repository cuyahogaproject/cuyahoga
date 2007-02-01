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
		}

		private void BindArticles()
		{
			// Article list view
			this.rptArticles.DataSource = this._module.GetArticleList();
			this.rptArticles.DataBind();

			if (this._module.CurrentAction == ArticleModuleAction.Category)
			{
				if (this.rptArticles.Items.Count > 0)
				{
					// HACK: Get the name of the category from the first article in the list.
					Article firstArticle = ((IList)this.rptArticles.DataSource)[0] as Article;
					base.Module.DisplayTitle = base.GetText("CATEGORY") + " " + firstArticle.Category.Title;
				}
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

				HyperLink hpl = e.Item.FindControl("hplTitle") as HyperLink;
				hpl.NavigateUrl = UrlHelper.GetUrlFromSection(this._module.Section) + "/" + article.Id;

				Literal litDateOnline = e.Item.FindControl("litDateOnline") as Literal;
				litDateOnline.Text =
					TimeZoneUtil.AdjustDateToUserTimeZone(article.DateOnline, this.Page.User.Identity).ToLongDateString();
				HyperLink hplAuthor = e.Item.FindControl("hplAuthor") as HyperLink;
				hplAuthor.NavigateUrl = this._module.GetProfileUrl(article.CreatedBy.Id);
				hplAuthor.Text = article.CreatedBy.FullName;

				HyperLink hplCategory = e.Item.FindControl("hplCategory") as HyperLink;
				if (article.Category != null)
				{
					hplCategory.NavigateUrl = UrlHelper.GetUrlFromSection(this.Module.Section) +
						String.Format("/category/{0}", article.Category.Id);
					hplCategory.Text = article.Category.Title;
				}
				else
				{
					hplCategory.Text = "-";
				}

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
				Panel pnlSummary = e.Item.FindControl("pnlSummary") as Panel;
				Panel pnlContent = e.Item.FindControl("pnlContent") as Panel;
				DisplayType displayType = (DisplayType)Enum.Parse(typeof(DisplayType), this._module.Section.Settings["DISPLAY_TYPE"].ToString());
				pnlSummary.Visible = (displayType == DisplayType.HeadersAndSummary);
				pnlContent.Visible = (displayType == DisplayType.FullContent);
			}
		}
	}
}
