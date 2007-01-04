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
using Cuyahoga.Modules.Articles.Domain;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.Articles.Web
{
	/// <summary>
	/// Summary description for AdminComments.
	/// </summary>
	public partial class AdminComments : ModuleAdminBasePage
	{
		private Article _article;
		private ArticleModule _articleModule;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._articleModule = base.Module as ArticleModule;

			if (Request.QueryString["ArticleId"] != null)
			{
				int articleId = Int32.Parse(Request.QueryString["ArticleId"]);
				if (articleId > 0)
				{
					this._article = this._articleModule.GetArticleById(articleId);
					BindComments();
				}
			}
		}

		private void BindComments()
		{
			this.rptComments.ItemDataBound += new RepeaterItemEventHandler(rptComments_ItemDataBound);
			this.rptComments.DataSource = this._article.Comments;
			this.rptComments.DataBind();
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

		protected void btnBack_Click(object sender, System.EventArgs e)
		{
			Context.Response.Redirect("AdminArticles.aspx" + base.GetBaseQueryString());
		}

		private void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Comment comment = (Comment)e.Item.DataItem;
			Literal litFrom = e.Item.FindControl("litFrom") as Literal;
			if (litFrom != null)
			{
				if (comment.User != null)
				{
					litFrom.Text = comment.User.UserName;
				}
				else
				{
					litFrom.Text = comment.Name;
				}
			}
			Literal litUpdateTimestamp = e.Item.FindControl("litUpdateTimestamp") as Literal;
			if (litUpdateTimestamp != null)
			{
				litUpdateTimestamp.Text = TimeZoneUtil.AdjustDateToUserTimeZone(comment.UpdateTimestamp, this.User.Identity).ToString();
			}
			LinkButton lbtDelete = e.Item.FindControl("lbtDelete") as LinkButton;
			if (lbtDelete != null)
			{
				lbtDelete.Attributes.Add("onclick", "return confirm('Are you sure?')");
				lbtDelete.Command += new CommandEventHandler(lbtDelete_Command);
				lbtDelete.CommandName = "DeleteComment";
				lbtDelete.CommandArgument = comment.Id.ToString();
			}
		}

		private void lbtDelete_Command(object sender, CommandEventArgs e)
		{
			try
			{
				Comment commentToDelete = null;
				foreach (Comment comment in this._article.Comments)
				{
					if (comment.Id == Int32.Parse(e.CommandArgument.ToString()) && e.CommandName == "DeleteComment")
					{
						commentToDelete = comment;
					}
				}
				if (commentToDelete != null)
				{
					this._article.Comments.Remove(commentToDelete);
					this._articleModule.DeleteComment(commentToDelete);
					// rebind the comments
					this.rptComments.DataBind();
				}
				else
				{
					base.ShowError("Could not find comment.");
				}
			}
			catch (Exception ex)
			{
				base.ShowError(ex.Message);
			}
		}
	}
}
