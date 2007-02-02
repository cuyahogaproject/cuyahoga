using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.Articles.Domain;
using Cuyahoga.Web.Util;
using System.Text.RegularExpressions;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Modules.Articles.Web
{
	public partial class ArticleDetails : BaseModuleControl
	{
		private ArticleModule _module;
		private Article _activeArticle; 

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as ArticleModule;

			if (this._module.CurrentArticleId > 0 && (!base.HasCachedOutput || this.Page.IsPostBack) || this.Page.User.Identity.IsAuthenticated)
			{
				// Article view
				this._activeArticle = this._module.GetArticleById(this._module.CurrentArticleId);
				this.litTitle.Text = this._activeArticle.Title;
				this.litContent.Text = this._activeArticle.Content;

				if (this._module.AllowComments || this._module.ShowAuthor || this._module.ShowCategory || this._module.ShowDateTime)
				{
					this.pnlArticleInfo.Visible = true;

					this.lblDateOnline.Text = TimeZoneUtil.AdjustDateToUserTimeZone(this._activeArticle.DateOnline, this.Page.User.Identity).ToString();
					this.lblDateOnline.Visible = this._module.ShowDateTime;

					this.litAuthor.Text = base.GetText("PUBLISHED") + " " + base.GetText("BY");
					this.litAuthor.Visible = this._module.ShowAuthor;
					this.hplAuthor.NavigateUrl = this._module.GetProfileUrl(this._activeArticle.CreatedBy.Id);
					this.hplAuthor.Text = this._activeArticle.CreatedBy.FullName;
					this.hplAuthor.Visible = this._module.ShowAuthor;

					this.litCategory.Text = base.GetText("CATEGORY");
					this.litCategory.Visible = this._module.ShowCategory;
					if (this._activeArticle.Category != null)
					{
						this.hplCategory.NavigateUrl = UrlHelper.GetUrlFromSection(this.Module.Section) +
							String.Format("/category/{0}", this._activeArticle.Category.Id);
						this.hplCategory.Text = this._activeArticle.Category.Title;
					}
					else
					{
						this.hplCategory.Text = String.Empty;
					}
					this.hplCategory.Visible = this._module.ShowCategory;

					if (this._module.AllowComments)
					{
						this.hplComments.NavigateUrl = UrlHelper.GetUrlFromSection(this._module.Section)
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

				this.hplBack.NavigateUrl = UrlHelper.GetUrlFromSection(this._module.Section);
				this.hplBack.Text = base.GetText("BACK");
				this.btnSaveComment.Text = base.GetText("BTNSAVECOMMENT");
				this.rfvName.ErrorMessage = base.GetText("NAMEREQUIRED");
				this.rfvComment.ErrorMessage = base.GetText("COMMENTREQUIRED");

				this.pnlArticleDetails.Visible = true;
				this.pnlComments.Visible = this._module.AllowComments && this._activeArticle.Comments.Count > 0;
				if (this._module.AllowAnonymousComments || (this.Page.User.Identity.IsAuthenticated && this._module.AllowComments))
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
				comment.Article = this._activeArticle;
				comment.CommentText = commentText;
				comment.UserIp = HttpContext.Current.Request.UserHostAddress;
				if (this.Page.User.Identity.IsAuthenticated)
				{
					comment.User = this.Page.User.Identity as Cuyahoga.Core.Domain.User;
				}
				else
				{
					comment.Name = this.txtName.Text;
					comment.Website = this.txtWebsite.Text;
				}
				try
				{
					this._module.SaveComment(comment);
					// Clear the cache, so the comment will appear immediately.
					base.InvalidateCache();
					Context.Response.Redirect(UrlHelper.GetUrlFromSection(this._module.Section) + "/" + this._activeArticle.Id.ToString());
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
				Literal litUpdateTimestamp = e.Item.FindControl("litUpdateTimestamp") as Literal;
				litUpdateTimestamp.Text = TimeZoneUtil.AdjustDateToUserTimeZone(comment.UpdateTimestamp, this.Page.User.Identity).ToString();

				if (comment.User != null)
				{
					// Comment by registered user.
					if (comment.User.Website != null && comment.User.Website != String.Empty)
					{
						HyperLink hpl = new HyperLink();
						hpl.NavigateUrl = this._module.GetProfileUrl(comment.User.Id);
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
					if (comment.Website != null && comment.Website != String.Empty)
					{
						HyperLink hpl = new HyperLink();
						hpl.NavigateUrl = comment.Website;
						hpl.Target = "_blank";
						hpl.Text = comment.Name;
						plhCommentBy.Controls.Add(hpl);
					}
					else
					{
						Literal lit = new Literal();
						lit.Text = comment.Name;
						plhCommentBy.Controls.Add(lit);
					}
				}
			}
		}
	}
}