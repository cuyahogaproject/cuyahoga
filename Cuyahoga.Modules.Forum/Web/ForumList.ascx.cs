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
	public class ForumList : BaseForumControl
	{
		protected System.Web.UI.WebControls.Repeater rptForumList;
		protected System.Web.UI.WebControls.Panel pnlTop;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Welcome;
		protected System.Web.UI.WebControls.Repeater rptCategoryList;
		protected System.Web.UI.WebControls.Repeater Repeater1;
		protected System.Web.UI.WebControls.Label lblHdrForum;
		protected System.Web.UI.WebControls.Label lblHdrTopics;
		protected System.Web.UI.WebControls.Label lblHdrNumPosts;
		protected System.Web.UI.WebControls.Label lblHdrLastPost;
		protected System.Web.UI.WebControls.PlaceHolder phForumTop;
		protected System.Web.UI.WebControls.PlaceHolder phForumFooter;
		protected System.Web.UI.WebControls.Panel dummy;

		private void Page_Load(object sender, System.EventArgs e)
		{
            this.BindTopFooter();
            this.LocalizeControls();
            this.BindCategories();
		}

		private void BindTopFooter()
		{
			ForumTop tForumTop;
			ForumFooter tForumFooter;

			tForumTop = (ForumTop)this.LoadControl("~/Modules/Forum/ForumTop.ascx");
			tForumTop.Module = this.Module as ForumModule;
			this.phForumTop.Controls.Add(tForumTop);

			tForumFooter = (ForumFooter)this.LoadControl("~/Modules/Forum/ForumFooter.ascx");
			tForumFooter.Module	= this.Module as ForumModule;
			this.phForumFooter.Controls.Add(tForumFooter);
		}

		private void BindCategories()
		{
			this.rptCategoryList.DataSource	= base.ForumModule.GetAllCategories();
			this.rptCategoryList.DataBind();
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
			this.rptCategoryList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptCategoryList_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptCategoryList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				ForumCategory fCategory = e.Item.DataItem as ForumCategory;
				HyperLink hpl = (HyperLink)e.Item.FindControl("hplForumCategory");
				if(hpl != null)
				{
					hpl.Text = fCategory.Name;
					hpl.NavigateUrl	= hpl.NavigateUrl	= String.Format("{0}/ForumCategoryList/{1}",UrlHelper.GetUrlFromSection(base.ForumModule.Section), fCategory.Id);
				}

				// Get the forums in this category
				this.rptForumList = (Repeater)e.Item.FindControl("rptForumList");
				if(this.rptForumList != null)
				{
					this.rptForumList.ItemDataBound += new RepeaterItemEventHandler(this.rptForumList_ItemDataBound);
					this.rptForumList.DataSource = base.ForumModule.GetForumsByCategoryId(fCategory.Id);
					this.rptForumList.DataBind();
				}
			}
		}

		private void rptForumList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				ForumForum forum = e.Item.DataItem as ForumForum;
				HyperLink hpl = (HyperLink)e.Item.FindControl("hplForumlink");
				if(hpl != null)
				{
					hpl.Text = forum.Name;
					hpl.NavigateUrl	= String.Format("{0}/ForumView/{1}",UrlHelper.GetUrlFromSection(base.ForumModule.Section), forum.Id);
					hpl.CssClass = "forum";
				}
			}
		}

		public string GetForumLink(object o)
		{
			ForumForum forum = o as ForumForum;
			string strReturn = String.Format("<a href=\"{0}/ForumView/{1}\" class=\"forum\">{2}</a>",UrlHelper.GetUrlFromSection(base.ForumModule.Section), forum.Id,forum.Name);
			return strReturn;
		}
	}
}
