using System;
using System.Collections;
using System.Web.UI.WebControls;

using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Forum;
using Cuyahoga.Modules.Forum.Domain;
using Cuyahoga.Modules.Forum.Utils;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	///		Summary description for Links.
	/// </summary>
	public class ForumTop : LocalizedUserControl
	{
		protected System.Web.UI.WebControls.Panel pnlTop;
		protected System.Web.UI.WebControls.HyperLink hplForumBreadCrumb;
		protected System.Web.UI.WebControls.Label lblWelcomeText;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Welcome;
		protected System.Web.UI.WebControls.HyperLink hplPostlink;
		protected System.Web.UI.WebControls.HyperLink hplCategoryLink;
		protected System.Web.UI.WebControls.HyperLink hplForumLink;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.HyperLink Hyperlink1;
		protected System.Web.UI.WebControls.HyperLink Hyperlink2;
		protected System.Web.UI.WebControls.HyperLink Hyperlink3;
		protected System.Web.UI.WebControls.HyperLink Hyperlink4;
		protected System.Web.UI.WebControls.HyperLink hplSearch;
		protected System.Web.UI.WebControls.HyperLink hplForumProfile;
		protected System.Web.UI.WebControls.Label lblForward_1;
		protected System.Web.UI.WebControls.Label lblForward_2;
		protected System.Web.UI.WebControls.Label lblForward_3;

		
		
		#region Private vars		
		private ForumModule		_module;
		private ForumForum		_forumForum;
		private ForumCategory	_forumCategory;
		#endregion

		#region Properties
		
		public ForumModule Module
		{
			get { return this._module; }
			set { this._module = value; }
		}
		
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as ForumModule;
			this.ForumBreadCrumb();
			this.Translate();
		}

		private void Translate()
		{
			string uname = "";
			if(this.Page.User.Identity.IsAuthenticated)
			{
				Cuyahoga.Core.Domain.User currentUser = Context.User.Identity as Cuyahoga.Core.Domain.User;
				uname = currentUser.FullName;

				// Add links
				this.hplForumProfile.Visible		= true;
				this.hplForumProfile.NavigateUrl	= String.Format("{0}/ForumProfile/{1}",UrlHelper.GetUrlFromSection(this._module.Section),currentUser.Id);
			}
			else
			{
				uname = base.GetText("GUEST");
			}
			this.hplSearch.NavigateUrl	= String.Format("{0}/ForumSearch",UrlHelper.GetUrlFromSection(this._module.Section));
			
            this.lblWelcomeText.Text	= String.Format(base.GetText("lblWelcomeText"),uname);
		}

		private void ForumBreadCrumb()
		{
			if(this._module.CurrentForumId != 0)
			{
				this._forumForum	= this._module.GetForumById(this._module.CurrentForumId);
				this._forumCategory	= this._module.GetForumCategoryById(this._forumForum.CategoryId);

				this.hplForumLink.NavigateUrl	= String.Format("{0}/ForumView/{1}",UrlHelper.GetUrlFromSection(this._module.Section), this._module.CurrentForumId);
				this.hplForumLink.Text			= this._forumForum.Name;
				this.hplForumLink.Visible		= true;
				this.hplForumLink.CssClass		= "forum";

				this.lblForward_1.Visible = true;
				this.lblForward_2.Visible = true;

				this.hplCategoryLink.NavigateUrl	= String.Format("{0}/ForumCategoryList/{1}",UrlHelper.GetUrlFromSection(this._module.Section), this._module.CurrentForumCategoryId);
				this.hplCategoryLink.Text			= this._forumCategory.Name;
				this.hplCategoryLink.Visible		= true;
				this.hplCategoryLink.CssClass		= "forum";
			}

			if(this._module.CurrentForumPostId != 0)
			{
				this.hplPostlink.NavigateUrl	= String.Format("{0}/ForumViewPost/{1}/post/{2}",UrlHelper.GetUrlFromSection(this._module.Section), this._module.CurrentForumId,this._module.CurrentForumPostId);
				this.hplPostlink.Visible		= true;
				this.hplPostlink.Text			= this._module.GetForumPostById(this._module.CurrentForumPostId).Topic;
				this.hplPostlink.CssClass		= "forum";
				this.lblForward_3.Visible		= true;
			}

			HyperLink hplBreadCrumb	= (HyperLink)this.FindControl("hplForumBreadCrumb");
			if(hplBreadCrumb != null)
			{
				hplBreadCrumb.Text			= base.GetText("FORUMHOME");
				hplBreadCrumb.NavigateUrl	= UrlHelper.GetUrlFromSection(this._module.Section);
                hplBreadCrumb.ToolTip = base.GetText("FORUMHOME");
				hplBreadCrumb.CssClass		= "forum";
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
