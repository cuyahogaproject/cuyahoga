using System;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;

using log4net;
using Castle.Windsor;
using Castle.MicroKernel;

using Cuyahoga.Web.Components;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Core.Domain;
using System.Collections.Generic;
using Castle.Core;

namespace Cuyahoga.Web.HttpHandlers
{
	/// <summary>
	/// This class handles all aspx page requests for Cuyahoga.
	/// </summary>
	public class PageHandler : IHttpHandler, IRequiresSessionState
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(PageHandler));
		//private IWindsorContainer _container;

		#region IHttpHandler Members

		/// <summary>
		/// Process the aspx request. This means (eventually) rewriting the url and registering the page 
		/// in the container.
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			string rawUrl = context.Request.RawUrl;
			log.Info("Starting request for " + rawUrl);
			DateTime startTime = DateTime.Now;

			// Rewrite url
			UrlRewriter urlRewriter = new UrlRewriter(context);
			string rewrittenUrl = urlRewriter.RewriteUrl(rawUrl);

			// Obtain the handler for the current page
			string aspxPagePath = rewrittenUrl.Substring(0, rewrittenUrl.IndexOf(".aspx") + 5);
			IHttpHandler handler = PageParser.GetCompiledPageInstance(aspxPagePath, context.Server.MapPath(aspxPagePath), context);

			// Process the page just like any other aspx page
			handler.ProcessRequest(context);

			// Release loaded modules. These modules are added to the HttpContext.Items collection by the ModuleLoader.
			ReleaseModules();

			// Log duration
			TimeSpan duration = DateTime.Now - startTime;
			log.Info(String.Format("Request finshed. Total duration: {0} ms.", duration.Milliseconds));
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReusable
		{
			get { return true; }
		}

		#endregion

		private void ReleaseModules()
		{
			List<ModuleBase> loadedModules = HttpContext.Current.Items["LoadedModules"] as List<ModuleBase>;
			if (loadedModules != null)
			{
				IWindsorContainer container = ContainerAccessorUtil.GetContainer();

				foreach (ModuleBase module in loadedModules)
				{
					container.Release(module);
				}
			}
		}
	}
}
