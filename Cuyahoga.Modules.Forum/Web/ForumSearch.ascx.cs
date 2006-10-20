using System;
using System.Collections;
using System.Web.UI.WebControls;

using Cuyahoga.Web.UI;
using Cuyahoga.Modules.Forum;
using Cuyahoga.Modules.Forum.Domain;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Forum.Web.UI;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	///		Summary description for Links.
	/// </summary>
	public class ForumSearch : BaseForumControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.PlaceHolder phForumTop;
		protected System.Web.UI.WebControls.PlaceHolder phForumFooter;
		protected System.Web.UI.WebControls.Panel pnlSearch;
		protected System.Web.UI.WebControls.Label lblSearch;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.WebControls.Panel pnlSearchResult;
		protected System.Web.UI.WebControls.Repeater rptSearchresult;
		protected System.Web.UI.WebControls.Label lblSearchresult;
		protected System.Web.UI.WebControls.TextBox txtSearchfor;

		private void Page_Load(object sender, System.EventArgs e)
		{
            this.BindTopFooter();
            this.LocalizeControls();
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
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.pnlSearch.Visible	= false;
			this.pnlSearchResult.Visible = true;
			this.rptSearchresult.DataSource = base.ForumModule.SearchForumPosts(this.txtSearchfor.Text);
			this.rptSearchresult.DataBind();
		}

		public string GetForumPostLink(object o)
		{
			ForumPost post = o as ForumPost;
			return String.Format("<a href=\"{0}/ForumViewPost/{1}/PostId/{2}\" class=\"forum\">{3}</a>",UrlHelper.GetUrlFromSection(base.ForumModule.Section), post.ForumId,post.Id,post.Topic);
		}

	}
}
