using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Admin
{

    public partial class Categories : Cuyahoga.Web.Admin.UI.AdminBasePage
    {
        private ICategoryService categoryService;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.categoryService = this.Container.Resolve<ICategoryService>();

            if (!IsPostBack)
            {
                this.Title = "Categories";

                BindRootCategories();
                BindCategories();
            }
        }

        protected void BindRootCategories()
        {
            IList<Category> rootCategories = this.categoryService.GetAllRootCategories();
            this.rdioListRoot.DataSource = rootCategories;
            this.rdioListRoot.DataTextField = "Name";
            this.rdioListRoot.DataValueField = "Path";
            this.rdioListRoot.DataBind();

            if (rootCategories.Count == 0)
            {
                this.litNoRoot.Visible = false;
            }
            else
            {
                this.litNoRoot.Visible = true;
                this.rdioListRoot.SelectedIndex = 0;
            }


        }

        private void BindCategories()
        {
            IList<Category> categories = this.categoryService.GetByPathStartsWith(this.rdioListRoot.SelectedValue);
            this.rptCategories.DataSource = categories;
            this.rptCategories.DataBind();

            this.ddlMoveCategories.DataSource = categories;
            this.ddlMoveCategories.DataTextField = "Name";
            this.ddlMoveCategories.DataValueField = "Path";
            this.ddlMoveCategories.DataBind();
        }


        private string GetSelectedCategoryPath()
        {
            foreach (RepeaterItem itm in this.rptCategories.Items)
            {
                RadioButton rdio = itm.FindControl("rdioBtnCategory") as RadioButton;
                if (rdio != null)
                {
                    if (rdio.Checked)
                    {
                        return rdio.InputAttributes["path"];
                    }
                }
            }
            return null;
        }


        protected void rdioListRoot_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindCategories();
        }

        protected void rptCategories_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            Category cat = e.Item.DataItem as Category;
            if (cat != null)
            {
                RadioButton rdio = (RadioButton)e.Item.FindControl("rdioBtnCategory");
                rdio.InputAttributes.Add("path", cat.Path);
                rdio.InputAttributes.Add("name", cat.Name);

                Label lbl = (Label)e.Item.FindControl("lblCategory");
                lbl.Style.Add("padding-left", string.Format("{0}px", (cat.Level * 5)));
                lbl.Text = "<sup>L</sup>&nbsp;<sup>.</sup>";

                HyperLink hplAddChild = (HyperLink)e.Item.FindControl("hplAddChild");
                hplAddChild.NavigateUrl = String.Format("~/Admin/CategoryEdit.aspx?cid=0&pcid={0}", cat.Id);

                HyperLink hplEdit = (HyperLink)e.Item.FindControl("hplEdit");    
                hplEdit.NavigateUrl = String.Format("~/Admin/CategoryEdit.aspx?cid={0}&pcid={1}", cat.Id, (cat.ParentCategory != null ? cat.ParentCategory.Id : -1) );

                //FIX: ASP.NET radio button bug (SetUniqueRadioButton('[repeater id].*[rdio button group name]', this)
                string script =
                   "SetUniqueRadioButton('rptCategories.*Categories',this)";
                rdio.Attributes.Add("onclick", script);
            }
        }


        protected void btnNewRoot_Click(object sender, EventArgs e)
        {
            this.CreateNewCategory(true);
        }

        private void CreateNewCategory(bool isRoot)
        {
            string newPath = string.Empty;

            if (isRoot)
            {
                IList<Category> rootCats = this.categoryService.GetAllRootCategories();
                newPath = this.categoryService.GetPathFragmentFromPosition(rootCats.Count + 1);
                Context.Response.Redirect(string.Format("CategoryEdit.aspx?cid=0&pcid=-1&path={0}", newPath));
            }
            else
            {
                Category cat = this.categoryService.GetByExactPath(this.GetSelectedCategoryPath());
                if (cat != null)
                {
                    IList<Category> childCats = cat.ChildCategories;
                    newPath = string.Concat(cat.Path, this.categoryService.GetPathFragmentFromPosition(childCats.Count + 1));
                    Context.Response.Redirect(string.Format("CategoryEdit.aspx?cid=0&pcid={0}&path={1}", cat.Id, newPath));
                }
                else ShowMessage("Plase select a category first");
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string path = this.GetSelectedCategoryPath();
            //get all child categories
            IList<Category> catList = this.categoryService.GetByPathStartsWith(path);
            if (catList.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(catList.Count);
                foreach (Category cat in catList)
                {
                    this.categoryService.DeleteCategory(cat);
                    sb.Append(cat.Name);
                    sb.Append(", ");
                }
                this.ShowMessage(string.Format("{0} Categories deleted: {1}", catList.Count, sb.ToString().TrimEnd(',', ' ')));
            }
            else ShowMessage("Please select a category first");
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            string moveTo = this.GetSelectedCategoryPath();
            string moveFrom = this.ddlMoveCategories.SelectedValue;
            if (moveTo != null && moveFrom != null)
            {
                if (moveTo.Length == 5 || moveFrom.Length == 5) ShowError("Can not move from/to root");
                else
                {
                this.categoryService.MoveCategoryToNewPath(moveFrom, moveTo);
                this.Response.Redirect("~/Admin/Categories.aspx");
                }
            }
            else ShowError("Could not move categories");
        }

     


    }
}
