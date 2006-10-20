using System;
using System.Collections;
using System.Web.UI.WebControls;

using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Forum;
using Cuyahoga.Modules.Forum.Domain;
using Cuyahoga.Modules.Forum.Utils;
using Cuyahoga.Modules.Forum.Web.UI;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	///		Summary description for Links.
	/// </summary>
	public class ForumView : BaseForumControl
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl Welcome;
		protected System.Web.UI.WebControls.Repeater rptForumPostList;
		protected System.Web.UI.WebControls.Label lblForumName;
		protected System.Web.UI.WebControls.HyperLink hplNewTopic;
		protected System.Web.UI.WebControls.PlaceHolder phForumTop;
		protected System.Web.UI.WebControls.PlaceHolder phForumFooter;
		protected System.Web.UI.WebControls.Panel Panel1;

		private ForumForum	_forumForum;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._forumForum	= base.ForumModule.GetForumById(base.ForumModule.CurrentForumId);
			base.ForumModule.CurrentForumCategoryId	= this._forumForum.CategoryId;

			if(this._forumForum.AllowGuestPost == 1 || this.Page.User.Identity.IsAuthenticated)
			{
				this.hplNewTopic.Visible = true;
			}
			else
			{
				this.hplNewTopic.Visible = false;
			}
            this.BindForumPosts();
			this.BindTopFooter();
            base.LocalizeControls();
            this.Translate();
		}

		private void BindTopFooter()
		{
			ForumTop tForumTop;
			ForumFooter tForumFooter;

			tForumTop = (ForumTop)this.LoadControl("~/Modules/Forum/ForumTop.ascx");
			tForumTop.Module = base.ForumModule;
			this.phForumTop.Controls.Add(tForumTop);

			tForumFooter = (ForumFooter)this.LoadControl("~/Modules/Forum/ForumFooter.ascx");
			tForumFooter.Module	= base.ForumModule;
			this.phForumFooter.Controls.Add(tForumFooter);
		}

		private void Translate()
		{
			this.lblForumName.Text = this._forumForum.Name;
		}

		private void BindForumPosts()
		{
			// Bind the link
			HyperLink hpl = (HyperLink)this.FindControl("hplNewTopic");
			if(hpl != null)
			{
				hpl.Text = base.GetText("lblNewTopic");
				hpl.NavigateUrl	= String.Format("{0}/ForumNewPost/{1}",UrlHelper.GetUrlFromSection(base.ForumModule.Section), base.ForumModule.CurrentForumId);
				hpl.CssClass = "forum";
			}
			this.rptForumPostList.DataSource	= base.ForumModule.GetAllForumPosts(base.ForumModule.CurrentForumId);
			this.rptForumPostList.DataBind();
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public string GetTopicLink(object o)
		{
			ForumPost post = o as ForumPost;
			return String.Format("<a href=\"{0}/ForumViewPost/{1}/PostId/{2}\" class=\"forum\">{3}</a>",UrlHelper.GetUrlFromSection(base.ForumModule.Section), post.ForumId,post.Id,post.Topic);
		}
	}
}
