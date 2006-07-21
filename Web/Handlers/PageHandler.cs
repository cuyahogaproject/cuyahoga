using System;
using System.Web;
using System.Web.UI;

using log4net;
using Castle.Windsor;
using Castle.MicroKernel;

using Cuyahoga.Web.Components;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Handlers
{
	/// <summary>
	/// This class handles all aspx page requests for Cuyahoga.
	/// </summary>
	public class PageHandler : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(PageHandler));

		#region IHttpHandler Members

		/// <summary>
		/// Process the aspx request. This means (eventually) rewriting the url and registering the page 
		/// in the container.
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			// Rewrite url
			UrlRewriter urlRewriter = new UrlRewriter(context);
			string rewrittenUrl = urlRewriter.RewriteUrl(context.Request.RawUrl);

			// Obtain the handler for the current page
			string aspxPagePath = rewrittenUrl.Substring(0, rewrittenUrl.IndexOf(".aspx") + 5);
			IHttpHandler handler = PageParser.GetCompiledPageInstance(aspxPagePath, null, context);

			// Register the page in the container
			handler = RegisterAspxPage(handler, context);

			// Process the page just like any other aspx page
			handler.ProcessRequest(context);

			// Remove the page from the container
			RemoveAspxPage(handler, context);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReusable
		{
			get { return true; }
		}

		#endregion

		private IHttpHandler RegisterAspxPage(IHttpHandler handler, HttpContext context)
		{
			if (handler is ICuyahogaPage)
			{
				string pageKey = Guid.NewGuid().ToString();
				IWindsorContainer container = ContainerAccessorUtil.GetContainer();
				container.Kernel.AddComponent(pageKey, handler.GetType());

				if (container.Kernel.HasComponent(handler.GetType()))
				{
					IHttpHandler newHandler = (IHttpHandler)container[handler.GetType()];
					handler = newHandler;
				}
			}
			return handler;
		}

		private void RemoveAspxPage(IHttpHandler handler, HttpContext context)
		{
			if (handler is ICuyahogaPage)
			{
				IWindsorContainer container = (context.ApplicationInstance as IContainerAccessor).Container;
				container.Release(handler);
			}
		}
	}
}
