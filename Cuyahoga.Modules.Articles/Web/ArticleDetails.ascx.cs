using System;
using System.Web;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.Articles.Domain;
using Cuyahoga.Web.Util;
using System.Text.RegularExpressions;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Modules.Articles.Web
{
	public partial class ArticleDetails : BaseModuleControl<ArticleModule>
	{
		private Article _activeArticle; 

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Module.CurrentArticleId > 0 && (!base.HasCachedOutput || this.Page.IsPostBack) || this.Page.User.Identity.IsAuthenticated)
			{
				// Article view
				this._activeArticle = this.Module.GetArticleById(this.Module.CurrentArticleId);
				this.litTitle.Text = this._activeArticle.Title;
				this.litContent.Text = this._activeArticle.Content;

				if (this.Module.AllowComments || this.Module.ShowAuthor || this.Module.ShowCategory || this.Module.ShowDateTime)
				{
					this.pnlArticleInfo.Visible = true;

					this.lblDateOnline.Text = TimeZoneUtil.AdjustDateToUserTimeZone(this._activeArticle.PublishedAt.Value, this.Page.User.Identity).ToString();
					this.lblDateOnline.Visible = this.Module.ShowDateTime;

					this.litAuthor.Text = base.GetText("PUBLISHED") + " " + base.GetText("BY");
					this.litAuthor.Visible = this.Module.ShowAuthor;
					this.hplAuthor.NavigateUrl = this.Module.GetProfileUrl(this._activeArticle.CreatedBy.Id);
					this.hplAuthor.Text = this._activeArticle.CreatedBy.FullName;
					this.hplAuthor.Visible = this.Module.ShowAuthor;

					this.litCategory.Text = base.GetText("CATEGORY");
					this.litCategory.Visible = this.Module.ShowCategory;
					
					// TODO: categories
					this.hplCategory.Text = String.Empty;
					this.hplCategory.Visible = this.Module.ShowCategory;

					if (this.Module.AllowComments)
					{
						this.hplComments.NavigateUrl = UrlHelper.GetUrlFromSection(this.Module.Section)
							+ String.Format("/{0}#comments", this._activeArticle.Id);
						this.hplComments.Text = base.GetText("COMMENTS") + " " + this._activeArticle.Comments.Count.ToString();
					}
					else
					{
						this.hplComments.Visible = false;
					}
				}
				else
				{
					this.pnlArticleInfo.Visible = false;
				}

				this.hplBack.NavigateUrl = UrlUtil.GetUrlFromSection(this.Module.Section);
				this.hplBack.Text = base.GetText("BACK");
				this.btnSaveComment.Text = base.GetText("BTNSAVECOMMENT");
				this.rfvName.ErrorMessage = base.GetText("NAMEREQUIRED");
				this.rfvComment.ErrorMessage = base.GetText("COMMENTREQUIRED");

				this.pnlArticleDetails.Visible = true;
				this.pnlComments.Visible = this.Module.AllowComments && this._activeArticle.Comments.Count > 0;
				if (this.Module.AllowAnonymousComments || (this.Page.User.Identity.IsAuthenticated && this.Module.AllowComments))
				{
					this.pnlComment.Visible = true;
					this.pnlAnonymous.Visible = (!this.Page.User.Identity.IsAuthenticated);
				}
				else
				{
					this.pnlComment.Visible = false;
				}
				// Comments
				this.rptComments.DataSource = this._activeArticle.Comments;
				this.rptComments.ItemDataBound += new RepeaterItemEventHandler(rptComments_ItemDataBound);
				this.rptComments.DataBind();
			}
		}

		protected void btnSaveComment_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid)
			{
				// Strip any html tags.
				string commentText = Regex.Replace(this.txtComment.Text, @"<(.|\n)*?>", string.Empty);
				// Replace carriage returns with <br/>.
				commentText = commentText.Replace("\r", "<br/>");
				// Save comment.
				Comment comment = new Comment();
				comment.ContentItem = this._activeArticle;
				comment.CommentText = commentText;
				comment.UserIp = HttpContext.Current.Request.UserHostAddress;
				comment.ContentItem = this._activeArticle;
				comment.CommentDateTime = DateTime.Now;
				if (this.Page.User.Identity.IsAuthenticated)
				{
					comment.User = this.Page.User.Identity as Cuyahoga.Core.Domain.User;
				}
				else
				{
					comment.Name = this.txtName.Text;
					comment.WebSite = this.txtWebsite.Text;
				}
				try
				{
					this.Module.SaveComment(comment);
					// Clear the cache, so the comment will appear immediately.
					base.InvalidateCache();
					Context.Response.Redirect(UrlUtil.GetUrlFromSection(this.Module.Section) + "/" + this._activeArticle.Id.ToString());
				}
				catch (Exception ex)
				{
					this.lblError.Text = ex.Message;
					this.lblError.Visible = true;
				}
			}
		}

		private void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			PlaceHolder plhCommentBy = e.Item.FindControl("plhCommentBy") as PlaceHolder;
			if (plhCommentBy != null)
			{
				Comment comment = (Comment)e.Item.DataItem;
				Literal litUpdateTimestamp = (Literal)e.Item.FindControl("litUpdateTimestamp");
				litUpdateTimestamp.Text = TimeZoneUtil.AdjustDateToUserTimeZone(comment.CommentDateTime, this.Page.User.Identity).ToString();

				if (comment.User != null)
				{
					// Comment by registered user.
					if (!string.IsNullOrEmpty(comment.User.Website))
					{
						HyperLink hpl = new HyperLink();
						hpl.NavigateUrl = this.Module.GetProfileUrl(comment.User.Id);
						hpl.Text = comment.User.FullName;
						plhCommentBy.Controls.Add(hpl);
					}
					else
					{
						Literal lit = new Literal();
						lit.Text = comment.User.FullName;
						plhCommentBy.Controls.Add(lit);
					}
				}
				else
				{
					// Comment by unregistered user.
					if (!String.IsNullOrEmpty(comment.WebSite))
					{
						HyperLink hpl = new HyperLink();
						hpl.NavigateUrl = comment.WebSite;
						hpl.Target = "_blank";
						hpl.Text = Server.HtmlEncode(comment.Name);
						plhCommentBy.Controls.Add(hpl);
					}
					else
					{
						Literal lit = new Literal();
						lit.Text = Server.HtmlEncode(comment.Name);
						plhCommentBy.Controls.Add(lit);
					}
				}
			}
		}
	}
}