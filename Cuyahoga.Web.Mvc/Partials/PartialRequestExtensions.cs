using System.Web.Mvc;

namespace Cuyahoga.Web.Mvc.Partials
{
	public static class PartialRequestsExtensions
	{
		public static void RenderPartialRequest(this HtmlHelper html, string viewDataKey)
		{
			PartialRequest partial = html.ViewContext.ViewData.Eval(viewDataKey) as PartialRequest;
			if (partial != null)
			{
				partial.Invoke(html.ViewContext);
			}
		}
	}
}
