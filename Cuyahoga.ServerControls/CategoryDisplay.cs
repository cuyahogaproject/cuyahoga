using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.ServerControls
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:CategoryDisplay runat=server></{0}:CategoryDisplay>")]
	public class CategoryDisplay : WebControl
	{
		/// <summary>
		/// Gets or sets the section base url (e.g. UrlUtil.GetSectionUrl())
		/// </summary>
		public string SectionBaseUrl
		{
			get { return ViewState["SectionBaseUrl"].ToString(); }
			set { ViewState["SectionBaseUrl"] = value; }
		}

		/// <summary>
		/// Gets or sets the categories to display.
		/// </summary>
		public IList<Category> Categories
		{
			get { return ViewState["Categories"] as IList<Category>; }
			set { ViewState["Categories"] = value; }
		}

		protected override void CreateChildControls()
		{
			if (this.Categories != null)
			{
				for (int i = 0; i < this.Categories.Count; i++)
				{
					Category category = this.Categories[i];
					if (!String.IsNullOrEmpty(this.SectionBaseUrl))
					{
						string url = SectionBaseUrl + "/category/" + category.Id;
						Controls.Add(new HyperLink { NavigateUrl = url, Text = category.Name });
					}
					else
					{
						Controls.Add(new Literal { Text = category.Name });
					}
					if (i < this.Categories.Count - 1)
					{
						Controls.Add(new Literal { Text = ", " });
					}
				}
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Site != null && this.Site.DesignMode)
			{
				writer.Write("Category 1, Category 2");
			}
			base.Render(writer);
		}
	}
}
