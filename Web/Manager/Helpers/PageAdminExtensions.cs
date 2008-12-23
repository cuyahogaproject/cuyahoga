using System;
using System.Web;
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

			string imageTag = "<img src=\"{0}\" class=\"handle\" alt=\"{1}\" />";
			if (node.IsRootNode) 
			{
				imageTag = String.Format(imageTag, urlHelper.Content("~/manager/Content/Images/house.png"), "home");
			}
			else if (node.IsExternalLink)
			{
				imageTag = String.Format(imageTag, urlHelper.Content("~/manager/Content/Images/page_link.png"), "page-link");
			}
			else if (! node.ShowInNavigation)
			{
				imageTag = String.Format(imageTag, urlHelper.Content("~/manager/Content/Images/page_white.png"), "page-hidden");
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

			
			if (node.ChildNodes.Count > 0)
			{
				TagBuilder expanderImage = new TagBuilder("img");

				if (node.Level < 1 || (node.IsInPath(activeNode)))
				{
					expanderImage.AddCssClass("children-visible");
					expanderImage.Attributes.Add("src", urlHelper.Content("~/manager/Content/Images/collapse.png"));
				}
				else
				{
					expanderImage.AddCssClass("children-hidden");
					expanderImage.Attributes.Add("src", urlHelper.Content("~/manager/Content/Images/expand.png"));					
				}
				expanderImage.Attributes.Add("alt", "toggle");
				return expanderImage.ToString();
			}
			else
			{
				TagBuilder expanderSpan = new TagBuilder("span");
				string className = "no-children";
				expanderSpan.AddCssClass(className);
				return expanderSpan.ToString();
			}
		}

		public static ContainerElement PageRow(this HtmlHelper htmlHelper, Node node, Node activeNode)
		{
			TagBuilder tagBuilder = new TagBuilder("tr");

			tagBuilder.Attributes["id"] = "page-" + node.Id;
			if (node.ParentNode != null)
			{
				tagBuilder.Attributes["class"] = "parent-" + node.ParentNode.Id;
			}
			if (activeNode != null && node.Id == activeNode.Id)
			{
				tagBuilder.Attributes["class"] += " selected";
			}
			HttpResponseBase httpResponse = htmlHelper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			return new ContainerElement(httpResponse, "tr");
		}

		public static ContainerElement PageElement(this HtmlHelper htmlHelper, Node node, Node activeNode)
		{
			TagBuilder tagBuilder = new TagBuilder("li");

			tagBuilder.Attributes["id"] = "page-" + node.Id;
			tagBuilder.Attributes["class"] = String.Empty;
			if (node.ParentNode != null)
			{
				tagBuilder.Attributes["class"] += "parent-" + node.ParentNode.Id;
			}
			if (activeNode != null && node.Id == activeNode.Id)
			{
				tagBuilder.Attributes["class"] += " selected";
			}
			HttpResponseBase httpResponse = htmlHelper.ViewContext.HttpContext.Response;
			httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
			return new ContainerElement(httpResponse, "li");
		}
	}
}