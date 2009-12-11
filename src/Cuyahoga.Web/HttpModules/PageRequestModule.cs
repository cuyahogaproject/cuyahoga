using System;
using System.Collections.Generic;
using System.Web;
using Castle.Windsor;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Util;
using log4net;

namespace Cuyahoga.Web.HttpModules
{
	public class PageRequestModule : IHttpModule
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(PageRequestModule));

		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application 
		///                 </param>
		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
			context.EndRequest += new EventHandler(context_EndRequest);
		}

		private void context_BeginRequest(object sender, EventArgs e)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			context.Items["RequestStart"] = DateTime.Now;

			string rawUrl = context.Request.RawUrl;
			log.Info("Starting request for " + rawUrl);
			DateTime startTime = DateTime.Now;

			if (rawUrl.Contains(".aspx"))
			{
				// Rewrite url
				UrlRewriter urlRewriter = new UrlRewriter(context);
				urlRewriter.RewriteUrl(rawUrl);
			}
		}

		private void context_EndRequest(object sender, EventArgs e)
		{
			ReleaseModules();
			
			// Log duration
			HttpContext context = ((HttpApplication)sender).Context;
			DateTime startTime = (DateTime) context.Items["RequestStart"];
			TimeSpan duration = DateTime.Now - startTime;
			log.Info(String.Format("Request finshed. Total duration: {0} ms.", duration.Milliseconds));
		}

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

		/// <summary>
		/// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
		/// </summary>
		public void Dispose()
		{
			// Nothing
		}
	}
}
