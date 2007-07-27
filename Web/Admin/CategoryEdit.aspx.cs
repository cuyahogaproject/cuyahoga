using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Admin
{
    public partial class CategoryEdit : Cuyahoga.Web.Admin.UI.AdminBasePage
    {
        private ICategoryService categoryService;
        private Category category;

        private int CategoryId
        {
            get { return (int)ViewState["cid"]; }
            set { this.ViewState["cid"] = value; }
        }

        private int ParentCategoryId
        {
            get { return (int)ViewState["pcid"]; }
            set { this.ViewState["pcid"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
               this.CategoryId = int.Parse(Request.QueryString["cid"]);
               this.ParentCategoryId = int.Parse(Request.QueryString["pcid"]);

               this.categoryService = this.Container.Resolve<ICategoryService>();

               if (!IsPostBack)
               {
                   //edit modes:
                   if (this.CategoryId < 1)
                   {
                       this.Title = "Add Category";
                       this.btnDelete.Enabled = false;
                       AddNewCategory();
                   }
                   else
                   {
                       this.Title = "Edit Category";
                       this.btnDelete.Enabled = true;
                       EditCategory();
                   }
               }
        }

        protected void AddNewCategory()
        {
            if (this.ParentCategoryId == -1)
            {
                IList<Category> rootCats = this.categoryService.GetAllRootCategories();
                this.txtPath.Text = this.categoryService.GetPathFragmentFromPosition(rootCats.Count + 1);
            }
            else
            {
                Category parentCat = this.categoryService.GetById(this.ParentCategoryId);
                this.txtPath.Text = string.Concat(parentCat.Path, this.categoryService.GetPathFragmentFromPosition(parentCat.ChildCategories.Count + 1));
            } 
        }

        protected void EditCategory()
        {
            this.category = this.categoryService.GetById(this.CategoryId);
            this.BindCategory();
        }

        protected void BindCategory()
        {
            this.txtDescription.Text = this.category.Description;
            this.txtKey.Text = this.category.Key;
            this.txtName.Text = this.category.Name;
            this.txtPath.Text = this.category.Path;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Exception exception = null;
            try
            {
                string path = this.txtPath.Text;

                if (this.CategoryId < 1) this.category = new Category();
                else this.category = this.categoryService.GetById(this.CategoryId);

                if (this.ParentCategoryId != -1) //not is root category
                {
                    this.category.ParentCategory = this.categoryService.GetById(this.ParentCategoryId);
                }
                this.category.Description = this.txtDescription.Text;
                this.category.Key = this.txtKey.Text;
                this.category.Name = this.txtName.Text;
                this.category.Path = path;
                this.categoryService.SaveOrUpdateCategory(this.category);
            }
            catch(Exception ex)
            {
                exception = ex;
                this.ShowError("Could not save category. Details:<br />" + ex.ToString());
            }
            if(exception == null) Response.Redirect("~/Admin/Categories.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Categories.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Exception exception = null;
            try
            {
                this.categoryService.DeleteCategory(this.categoryService.GetById(this.CategoryId));
            }
            catch (Exception ex)
            {
                exception = ex;
                this.ShowError("Could not delete category. Details:<br />" + ex.ToString());
            }
            if (exception == null) Response.Redirect("~/Admin/Categories.aspx");
        }



    
    }
}
