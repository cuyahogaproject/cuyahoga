using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Cuyahoga.Web.UI;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Modules.Categories
{
    public partial class CategoryControl : BaseModuleControl
    {
        private CategoryModule categoryModule;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.categoryModule = this.Module as CategoryModule;
            IList<Category> categories = ((CategoryModule)this.Module).GetCategories();
            this.BuildCategoryTree(categories);
        }


        private void BuildCategoryTree(IList<Category> sortedCategories)
        {
            PlaceHolder plh = this.FindControl("plhCategories") as PlaceHolder;
            if (plh != null)
            {

                HtmlGenericControl container = new HtmlGenericControl("div");
                container.Attributes.Add("class", "categorycontainer");

                if (sortedCategories.Count > 1)
                {
                    //handle the root category
                    container.Controls.Add(this.CreateRootCategoryDiv(sortedCategories[0]));
                    sortedCategories.RemoveAt(0);
                    //temporary controls for creating the div hierarchy
                    HtmlGenericControl parentDiv = container;
                    HtmlGenericControl lastDiv = container;
              
                    int level = 1;
                    foreach (Category c in sortedCategories)
                    {
                        //adding on same level as before
                        if (c.Level == level)
                        {
                           lastDiv = this.CreateCategoryDiv(c);
                           parentDiv.Controls.Add(lastDiv);                        
                        }
                        //moving down the hierarchy, so add container to (new) parent
                        else if (c.Level > level)
                        {
                            parentDiv = lastDiv;
                            lastDiv = this.CreateCategoryDiv(c);
                            parentDiv.Controls.Add(lastDiv);
                        }
                        //moving up, so start new topmost node
                        else if (c.Level < level)
                        {
                            parentDiv = container;
                            lastDiv = this.CreateCategoryDiv(c);
                            parentDiv.Controls.Add(lastDiv);
                        }
                        level = c.Level;
                    }//end foreach
                }
                else container.InnerText = base.GetText("NO_CATEGORIES_TO_DISPLAY");
                plh.Controls.Add(container);
            }
            
        }

        private HtmlGenericControl CreateRootCategoryDiv(Category category)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "rootcategorynode");

            HyperLink hpl = new HyperLink();
            hpl.Text = category.Name;
            hpl.NavigateUrl = this.categoryModule.CreateCategoryUrl(category);
            div.Controls.Add(hpl);

            return div;
        }


        private HtmlGenericControl CreateCategoryDiv(Category category)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("id", string.Concat("category", category.Path));
            div.Attributes.Add("class", "categorynode");
            div.Style.Add("padding-left", string.Format("{0}px", (category.Level * 2)));
            //div.Style.Add("padding-left", string.Format("{0}px", (category.Level * this.IndentationFactor)));

            HtmlGenericControl span = new HtmlGenericControl("span");
            span.InnerHtml = "<sup>L</sup>&nbsp;<sup>.</sup>&nbsp;";
            div.Controls.Add(span);

            CheckBox chkBox = new CheckBox();
            //chkBox.Visible = this.UseCheckBoxes;
            div.Controls.Add(chkBox);

            HyperLink hpl = new HyperLink();
            hpl.Text = category.Name;
            hpl.ToolTip = category.Description;
            hpl.NavigateUrl = this.categoryModule.CreateCategoryUrl(category);
            div.Controls.Add(hpl);

            return div;
        }


        private void WriteCategoryRoot(Category cat, HtmlGenericControl container)
        {

        }

  

        protected void rptCategories_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            Category category = e.Item.DataItem as Category;
            if (category != null)
            {
                CheckBox chkBox = (CheckBox)e.Item.FindControl("chkBoxCategory");
                chkBox.Visible = false;
                HyperLink hpl = (HyperLink)e.Item.FindControl("hplCategory");
                hpl.NavigateUrl = this.categoryModule.CreateCategoryUrl(category);

                Label lbl = (Label)e.Item.FindControl("lblCategory");
                StringBuilder sb = new StringBuilder();
                if (category.ParentCategory != null)
                {
                    for (int i = 0; i < (category.Level * 4); i++)
                    {
                        sb.Append("&nbsp;");
                    }
                    sb.Append("<sup>L</sup>&nbsp;<sup>.</sup>");
                }
                lbl.Text = sb.ToString();
            }
        }

     
        
    }
}