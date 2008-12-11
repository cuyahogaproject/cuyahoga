using System;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Helpers
{
	public static class PageManagementExtensions
	{
		/// <summary>
		/// Renders an icon image tag for a Page.
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		public static string PageImage(this HtmlHelper htmlHelper, Node node)
		{
			UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext);

			string imageTag = "<img src=\"{0}\" alt=\"{1}\" />";
			if (node.IsRootNode) 
			{
				imageTag = String.Format(imageTag, urlHelper.Content("~/manager/Content/Images/house.png"), "home");
			}
			else if (node.IsExternalLink)
			{
				imageTag = String.Format(imageTag, urlHelper.Content("~/manager/Content/Images/page_link.png"), "page-link");
			}
			else
			{
				imageTag = String.Format(imageTag, urlHelper.Content("~/manager/Content/Images/page.png"), "page");
			}
			return imageTag;
		}

		public static string PageExpander(this HtmlHelper htmlHelper, Node node, Node activeNode)
		{
			UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext);

			TagBuilder expanderSpan = new TagBuilder("span");
			string className = "no-children";
			if (node.ChildNodes.Count > 0)
			{
				TagBuilder expanderImage = new TagBuilder("img");
				expanderImage.AddCssClass("expander");

				if (node.Level < 1 || (activeNode != null && activeNode.NodePath[node.Level] == node))
				{
					className = "children-visible";
					expanderImage.Attributes.Add("src", urlHelper.Content("~/manager/Content/Images/collapse.png"));
				}
				else
				{
					className = "children-hidden";
					expanderImage.Attributes.Add("src", urlHelper.Content("~/manager/Content/Images/expand.png"));					
				}
				expanderImage.Attributes.Add("alt", "toggle");
				expanderSpan.InnerHtml = expanderImage.ToString(TagRenderMode.SelfClosing);
			}
			expanderSpan.AddCssClass(className);
			return expanderSpan.ToString();
		}
	}
}