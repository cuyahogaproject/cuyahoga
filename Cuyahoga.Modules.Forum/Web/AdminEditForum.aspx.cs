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
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.Forum;
using Cuyahoga.Modules.Forum.Domain;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	/// Summary description for Edit forum category.
	/// </summary>
	public class AdminEditForum : ModuleAdminBasePage
	{
		private ForumForum		_forum;
		private ForumModule		_module;

		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDateOnline;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTitle;
		protected System.Web.UI.WebControls.DropDownList lstCategories;
		protected System.Web.UI.WebControls.TextBox txtDescription;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDescription;
		protected System.Web.UI.WebControls.CheckBox ckbAllowGuestPost;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDateOffline;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as ForumModule;
			this.btnCancel.Attributes.Add("onClick", String.Format("document.location.href='AdminForum.aspx{0}'", base.GetBaseQueryString()));
			if (! this.IsPostBack)
			{
				this.lstCategories.Items.Add(new ListItem("none", "0"));
				IList categories = this._module.GetAllCategories();
				foreach (ForumCategory category in categories)
				{
					this.lstCategories.Items.Add(new ListItem(category.Name, category.Id.ToString()));
				}
			}

			if (Request.QueryString["ForumId"] != null)
			{
				int forumId = Int32.Parse(Request.QueryString["ForumId"]);
				if (forumId > 0)
				{
					this._forum = this._module.GetForumById(forumId);
					if (! this.IsPostBack)
					{
						BindForum();
					}
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?');");
				}
			}
		}

		private void BindForum()
		{
			if(this._forum != null)
			{
				this.txtName.Text			= this._forum.Name;
				this.txtDescription.Text	= this._forum.Description;

				if (this._forum.CategoryId	!= 0)
				{
					ListItem li = this.lstCategories.Items.FindByValue(this._forum.CategoryId.ToString());
					if (li != null)
					{
						li.Selected = true;
					}
				}

				if(this._forum.AllowGuestPost == 1)
				{
					this.ckbAllowGuestPost.Checked = true;
				}
			}
		}

		private void SaveForum()
		{
			try
			{
				this._forum.Name			= this.txtName.Text;
				this._forum.Description		= this.txtDescription.Text;
				this._forum.DateModified	= DateTime.Now;
				if (this.lstCategories.SelectedIndex > 0)
				{
					
					this._forum.CategoryId = Int32.Parse(this.lstCategories.SelectedValue);
				}
				if(this.ckbAllowGuestPost.Checked)
				{
					this._forum.AllowGuestPost = 1;
				}
				else
				{
					this._forum.AllowGuestPost = 0;
				}
				this._module.SaveForum(this._forum);
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
				if (this._forum == null)
				{
					this._forum = new ForumForum();
				}
				
				this.SaveForum();
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._forum != null)
			{
				try
				{
					this._module.DeleteForum(this._forum);
					Response.Redirect(String.Format("AdminForum.aspx{0}", base.GetBaseQueryString()));
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
			else
			{
				ShowError("No forum found to delete");
			}
		}
	}
}
