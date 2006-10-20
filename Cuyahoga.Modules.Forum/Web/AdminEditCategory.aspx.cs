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
	public class AdminEditCategory : ModuleAdminBasePage
	{
		private ForumCategory	_forumcategory;
		private ForumModule		_module;

		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDateOnline;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTitle;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvDateOffline;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as ForumModule;
			this.btnCancel.Attributes.Add("onClick", String.Format("document.location.href='AdminForum.aspx{0}'", base.GetBaseQueryString()));
			if (! this.IsPostBack)
			{
			}

			if (Request.QueryString["CategoryId"] != null)
			{
				int categoryId = Int32.Parse(Request.QueryString["CategoryId"]);
				if (categoryId > 0)
				{
					this._forumcategory = this._module.GetForumCategoryById(categoryId);
					if (! this.IsPostBack)
					{
						BindCategory();
					}
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?');");
				}
			}
		}

		private void BindCategory()
		{
			this.txtName.Text = this._forumcategory.Name;
		}

		private void SaveCategory()
		{
			try
			{
				this._forumcategory.Name			= this.txtName.Text;
				this._forumcategory.DateModified	= DateTime.Now;
				this._module.SaveForumCategory(this._forumcategory);
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
				if (this._forumcategory == null)
				{
					this._forumcategory = new ForumCategory();
				}
				this._forumcategory.Name = this.txtName.Text;
				this.SaveCategory();
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._forumcategory != null)
			{
				try
				{
					this._module.DeleteForumCategory(this._forumcategory);
					Response.Redirect(String.Format("AdminForum.aspx{0}", base.GetBaseQueryString()));
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
			else
			{
				ShowError("No forum category found to delete");
			}
		}
	}
}
