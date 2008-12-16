using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cuyahoga.Web.UI;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Modules.Sitemap.Web
{
	public partial class SitemapControl : BaseModuleControl
	{
		protected PlaceHolder plhNodes;

		protected void Page_Load(object sender, EventArgs e)
		{
			GenerateSitemap();
		}

		private void GenerateSitemap()
		{
			HtmlGenericControl mainList = new HtmlGenericControl("ul");
			mainList.Controls.Add(BuildListItemFromNode(base.PageEngine.RootNode));
			foreach (Node node in base.PageEngine.RootNode.ChildNodes)
			{
				if (node.ViewAllowed(base.PageEngine.CuyahogaUser))
				{
					HtmlControl listItem = BuildListItemFromNode(node);
					if (node.ChildNodes.Count > 0)
					{
						listItem.Controls.Add(BuildListFromNodes(node.ChildNodes));
					}
					mainList.Controls.Add(listItem);
				}
			}
			this.plhNodes.Controls.Add(mainList);
		}

		private HtmlControl BuildListItemFromNode(Node node)
		{
			HtmlGenericControl listItem = new HtmlGenericControl("li");
			HyperLink hpl = new HyperLink();
			hpl.NavigateUrl = UrlHelper.GetUrlFromNode(node);
			UrlHelper.SetHyperLinkTarget(hpl, node);
			hpl.Text = node.Title;
			listItem.Controls.Add(hpl);
			return listItem;
		}

		private HtmlControl BuildListFromNodes(IList<Node> nodes)
		{
			HtmlGenericControl list = new HtmlGenericControl("ul");
			foreach (Node node in nodes)
			{
				if (node.ViewAllowed(base.PageEngine.CuyahogaUser))
				{
					HtmlControl listItem = BuildListItemFromNode(node);
					if (node.ChildNodes.Count > 0)
					{
						listItem.Controls.Add(BuildListFromNodes(node.ChildNodes));
					}
					list.Controls.Add(listItem);
				}
			}
			return list;
		}
	}
}