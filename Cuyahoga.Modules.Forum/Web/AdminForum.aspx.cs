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
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.Forum.Domain;
using Cuyahoga.Modules.Forum.Utils;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	/// Summary description for Edit Forums.
	/// </summary>
	public class AdminForum : ModuleAdminBasePage
	{
		protected System.Web.UI.WebControls.Repeater rptForumCategories;
		protected Cuyahoga.ServerControls.Pager pgrForumCategories;
		protected System.Web.UI.WebControls.Button btnNewCategory;
		protected System.Web.UI.WebControls.Repeater rptForumlist;
		protected Cuyahoga.ServerControls.Pager Pager1;
		protected System.Web.UI.WebControls.Button btnNewForum;
		protected System.Web.UI.WebControls.Repeater rptEmoticons;
		protected Cuyahoga.ServerControls.Pager pgEmoticons;
		protected System.Web.UI.WebControls.Button btnNewEmoticon;
		protected Cuyahoga.ServerControls.Pager pgrForumTags;
		protected System.Web.UI.WebControls.Repeater rptTags;
		protected System.Web.UI.WebControls.Button btnNewTag;
		private ForumModule _module;

		private void Page_Load(object sender, System.EventArgs e)
		{
			base.Title = "Forum administration";
			this._module = base.Module as ForumModule;
			if (! this.IsPostBack)
			{
				this.rptForumCategories.DataSource = this._module.GetAllCategories();
				this.rptForumCategories.DataBind();

				this.rptForumlist.DataSource = this._module.GetAllForums();
				this.rptForumlist.DataBind();

				this.rptEmoticons.DataSource	= this._module.GetAllEmoticons();
				this.rptEmoticons.DataBind();

				this.rptTags.DataSource	= this._module.GetAllTags();
				this.rptTags.DataBind();
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.rptTags.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptTags_ItemDataBound);
			this.rptForumCategories.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptForumCategories_ItemDataBound);
			this.btnNewCategory.Click += new System.EventHandler(this.btnNewCategory_Click);
			this.rptForumlist.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptForumlist_ItemDataBound);
			this.btnNewForum.Click += new System.EventHandler(this.btnNewForum_Click);
			this.rptEmoticons.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptEmoticons_ItemDataBound);
			this.btnNewEmoticon.Click += new System.EventHandler(this.btnNewEmoticon_Click);
			this.btnNewTag.Click += new System.EventHandler(this.Button1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptForumCategories_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			ForumCategory forumcategory = e.Item.DataItem as ForumCategory;

			HyperLink hplEdit = e.Item.FindControl("hpledit") as HyperLink;
			if (hplEdit != null)
			{
				hplEdit.NavigateUrl = String.Format("~/Modules/Forum/AdminEditCategory.aspx{0}&CategoryId={1}", base.GetBaseQueryString(), forumcategory.Id);
			}
		}

		private void btnNewCategory_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(String.Format("~/Modules/Forum/AdminEditCategory.aspx?NodeId={0}&SectionId={1}&CategoryId=-1",this.Node.Id, this.Section.Id));
		}

		private void btnNewForum_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(String.Format("~/Modules/Forum/AdminEditForum.aspx?NodeId={0}&SectionId={1}&CategoryId=-1",this.Node.Id, this.Section.Id));
		
		}

		private void rptForumlist_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			ForumForum forum = e.Item.DataItem as ForumForum;

			HyperLink hplEdit = e.Item.FindControl("hplForumEdit") as HyperLink;
			if (hplEdit != null)
			{
				hplEdit.NavigateUrl = String.Format("~/Modules/Forum/AdminEditForum.aspx{0}&ForumId={1}", base.GetBaseQueryString(), forum.Id);
			}
		
		}

		private void btnNewEmoticon_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(String.Format("~/Modules/Forum/AdminEditEmoticon.aspx?NodeId={0}&SectionId={1}&EmoticonId=-1",this.Node.Id, this.Section.Id));
		}

		private void rptEmoticons_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			ForumEmoticon emoticon = e.Item.DataItem as ForumEmoticon;

			HyperLink hplEdit = e.Item.FindControl("hplEditEmoticon") as HyperLink;
			if (hplEdit != null)
			{
				hplEdit.NavigateUrl = String.Format("~/Modules/Forum/AdminEditEmoticon.aspx{0}&EmoticonId={1}", base.GetBaseQueryString(), emoticon.Id);
			}
		}

		private void Button1_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(String.Format("~/Modules/Forum/AdminEditTag.aspx?NodeId={0}&SectionId={1}&TagId=-1",this.Node.Id, this.Section.Id));		
		}

		private void rptTags_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			ForumTag tag = e.Item.DataItem as ForumTag;

			HyperLink hplEdit = e.Item.FindControl("hplTagEdit") as HyperLink;
			if (hplEdit != null)
			{
				hplEdit.NavigateUrl = String.Format("~/Modules/Forum/AdminEditTag.aspx{0}&TagId={1}", base.GetBaseQueryString(), tag.Id);
			}
		}
	
	}
}
