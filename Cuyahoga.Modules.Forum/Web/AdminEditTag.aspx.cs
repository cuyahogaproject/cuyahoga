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

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;
using Cuyahoga.Modules.Forum;
using Cuyahoga.Modules.Forum.Domain;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	/// Summary description for Edit forum category.
	/// </summary>
	public class AdminEditTag : ModuleAdminBasePage
	{
		private ForumTag		_forumtag;
		private ForumModule		_module;

		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDateOnline;
		protected System.Web.UI.WebControls.TextBox txtTextVersion;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTextVersion;
		protected System.Web.UI.WebControls.TextBox txtImageName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvImageName;
		protected System.Web.UI.WebControls.TextBox txtForumCodeStart;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvForumCodeStart;
		protected System.Web.UI.WebControls.TextBox txtForumCodeEnd;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvForumCodeEnd;
		protected System.Web.UI.WebControls.TextBox txtHtmlCodeStart;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvHtmlCodeStart;
		protected System.Web.UI.WebControls.TextBox txtHtmlCodeEnd;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvHtmlCodeEnd;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDateOffline;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as ForumModule;
			this.btnCancel.Attributes.Add("onClick", String.Format("document.location.href='AdminForum.aspx{0}'", base.GetBaseQueryString()));
			if (! this.IsPostBack)
			{
			}

			if (Request.QueryString["TagId"] != null)
			{
				int tagId = Int32.Parse(Request.QueryString["TagId"]);
				if (tagId > 0)
				{
					this._forumtag = this._module.GetTagById(tagId);
					if (! this.IsPostBack)
					{
						BindTag();
					}
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?');");
				}
			}
		}

		private void BindTag()
		{
			this.txtForumCodeStart.Text		= this._forumtag.ForumCodeStart;
			this.txtForumCodeEnd.Text		= this._forumtag.ForumCodeEnd;
			this.txtHtmlCodeStart.Text		= this._forumtag.HtmlCodeStart;
			this.txtHtmlCodeEnd.Text		= this._forumtag.HtmlCodeEnd;
		}

		private void SaveTag()
		{
			try
			{
				this._forumtag.ForumCodeStart		= this.txtForumCodeStart.Text;
				this._forumtag.ForumCodeEnd			= this.txtForumCodeEnd.Text;
				this._forumtag.HtmlCodeStart		= this.txtHtmlCodeStart.Text;
				this._forumtag.HtmlCodeEnd			= this.txtHtmlCodeEnd.Text;

				this._forumtag.DateModified	= DateTime.Now;
				this._module.SaveTag(this._forumtag);
				Response.Redirect(String.Format("AdminForum.aspx{0}", base.GetBaseQueryString()));
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				if (this._forumtag == null)
				{
					this._forumtag = new ForumTag();
				}
				this.SaveTag();
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._forumtag != null)
			{
				try
				{
					this._module.DeleteTag(this._forumtag);
					Response.Redirect(String.Format("AdminForum.aspx{0}", base.GetBaseQueryString()));
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
			else
			{
				ShowError("No tag found to delete");
			}
		}
	}
}
