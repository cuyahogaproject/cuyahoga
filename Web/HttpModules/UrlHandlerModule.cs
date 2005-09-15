using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Specialized;

using log4net;

using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.HttpModules
{
	/// <summary>
	/// The default url handler.
	/// </summary>
	public class UrlHandlerModule : IHttpModule
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(UrlHandlerModule));

		public UrlHandlerModule()
		{
		}

		#region IHttpModule Members

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
			context.EndRequest += new EventHandler(context_EndRequest);
		}

		public void Dispose()
		{
		}

		#endregion

		/// <summary>
		/// Read the match patterns from the configuration and try to rewrite the url.
		/// TODO: caching? 
		/// </summary>
		/// <param name="urlToRewrite"></param>
		/// <param name="context"></param>
		private void RewriteUrl(string urlToRewrite, HttpContext context)
		{
			NameValueCollection mappings = (NameValueCollection)ConfigurationSettings.GetConfig("UrlMappings");
			for (int i = 0; i < mappings.Count; i++)
			{
				string matchExpression = UrlHelper.GetApplicationPath() + mappings.GetKey(i);
				Regex regEx = new Regex(matchExpression, RegexOptions.IgnoreCase|RegexOptions.Singleline|RegexOptions.CultureInvariant|RegexOptions.Compiled);
				if (regEx.IsMatch(urlToRewrite))
				{
					// Don't rewrite when the mapping is an empty string (used for Default, Admin etc.)
					if (mappings[i] != String.Empty)
					{
						// First check if the Application is upgrading. If so, redirect everything to the
						// maintenance page.
						if (Convert.ToBoolean(HttpContext.Current.Application["IsUpgrading"]))
						{
							HttpContext.Current.Response.Redirect("~/Install/Sorry.aspx", true);
							return;
						}

						// Store the original url in the Context.Items collection. We need to save this for setting
						// the action of the form.
						string rewritePath = regEx.Replace(urlToRewrite, UrlHelper.GetApplicationPath() + mappings[i]);
						// MONO_WORKAROUND: split the rewritten path in path, pathinfo and querystring
						// because MONO doesn't handle the pathinfo directly
						//context.RewritePath(rewritePath);
						string querystring = String.Empty;
						string pathInfo = String.Empty;
						string path = String.Empty;
						// 1. extract querystring
						int qmark = rewritePath.IndexOf("?");
						if (qmark != -1 || qmark + 1 < rewritePath.Length) 
						{
							querystring = rewritePath.Substring (qmark + 1);
							rewritePath = rewritePath.Substring (0, qmark);
						} 
						// 2. extract pathinfo
						int pathInfoSlashPos = rewritePath.IndexOf("aspx/") + 4;
						if (pathInfoSlashPos > 3)
						{
							pathInfo = rewritePath.Substring(pathInfoSlashPos);
							rewritePath = rewritePath.Substring(0, pathInfoSlashPos);
						}
						// 3. path
						path = rewritePath;
						log.Debug("urlToRewrite = " + urlToRewrite);
						log.Debug("path = " + path);
						log.Debug("pathInfo = " + pathInfo);
						log.Debug("querystring = " + querystring);
						
						context.RewritePath(path, pathInfo, querystring);
					}
					break;
				}
			}
			context.Items["VirtualUrl"] = urlToRewrite;
		}

		private void context_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			// register start time for performance measurements.
			app.Context.Items["starttime"] = DateTime.Now;
			log.Info("Starting request: " + app.Request.RawUrl);
			string url = HttpContext.Current.Request.RawUrl;
			RewriteUrl(url, app.Context);
		}

		private void context_EndRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			// Ready, write the execution time to the debug output.
			TimeSpan ts = DateTime.Now - (DateTime)HttpContext.Current.Items["starttime"];
			log.Info("Total execution time : " + ts.Milliseconds.ToString() + " ms.");
			log.Info("Ending request: " + app.Request.RawUrl);
		}
	}

	/// <summary>
	/// ConfigSection class
	/// </summary>
	public class UrlMappingsSectionHandler : NameValueSectionHandler
	{
		protected override string KeyAttributeName
		{
			get { return "match"; }
		}

		protected override string ValueAttributeName
		{
			get { return "replace"; }
		}
	}
}
