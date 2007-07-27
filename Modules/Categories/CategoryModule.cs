using System;
using System.Web;
using System.Collections.Generic;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Communication;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Modules.Categories
{
    public class CategoryModule : ModuleBase, INHibernateModule, IActionProvider
    {
        private ICategoryService categoryService;
        private string key = string.Empty;

        public CategoryModule(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public string CreateCategoryUrl(Category c)
        {
            Section sectionTo = this.Section.Connections["SetCategory"] as Section;
            if (sectionTo != null)
            {
                return UrlHelper.GetUrlFromSection(sectionTo) + "/SetCategory?c=" + HttpContext.Current.Server.UrlEncode(c.Name);
            }
            else
            {
                return null;
            }
        }

        #region Data Services

        public IList<Category> GetCategories()
        {
            return this.categoryService.GetByKeyIncludingSubcategories(key);
        }

        #endregion

        #region Overrides

        public override void ReadSectionSettings()
        {
            this.key = Convert.ToString(base.Section.Settings["ROOT_CATEGORY_KEY"]);
            //this.useCheckBoxes = Convert.ToBoolean(base.Section.Settings["USE_CHECKBOXES"]);
            //this.indentationFactor = Convert.ToInt32(base.Section.Settings["INDENTATION_FACTOR"]);
            //this.indentationCssUnit = Convert.ToString(base.Section.Settings["INDENTATION_CSS_UNIT"]);
        }

        #endregion

        #region IActionProvider Members

        public ActionCollection GetOutboundActions()
        {
            ActionCollection actions = new ActionCollection();
            //has to be connected to a SearchModule
            actions.Add(new Action("SetCategory", new string[0]));
            return actions;
        }

        #endregion
    }
}
