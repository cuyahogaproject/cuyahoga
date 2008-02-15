using System;
using System.Web;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Configuration;

using log4net;

using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Components
{
	/// <summary>
	/// The UrlRewriter class is responsible for rewriting urls.
	/// </summary>
	public class UrlRewriter
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(UrlRewriter));
		private HttpContext _currentContext;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="context"></param>
		public UrlRewriter(HttpContext context)
		{
			this._currentContext = context;
		}

		/// <summary>
		/// Read the match patterns from the configuration and try to rewrite the url.
		/// TODO: caching? 
		/// </summary>
		/// <param name="urlToRewrite"></param>
		/// <returns></returns>
		public string RewriteUrl(string urlToRewrite)
		{
			string rewrittenUrl = urlToRewrite;

			NameValueCollection mappings = (NameValueCollection)ConfigurationManager.GetSection("UrlMappings");
			for (int i = 0; i < mappings.Count; i++)
			{
				string matchExpression = UrlHelper.GetApplicationPath() + mappings.GetKey(i);
				if (Regex.IsMatch(urlToRewrite, matchExpression, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant))
				{
					// Don't rewrite when the mapping is an empty string (used for Default, Admin etc.)
					if (mappings[i] != String.Empty)
					{
						// First check if the Application is upgrading. If so, redirect everything to the
						// maintenance page.
						if (Convert.ToBoolean(this._currentContext.Application["IsUpgrading"]))
						{
							this._currentContext.Response.Redirect("~/Install/Sorry.aspx", true);
							return null;
						}

						// Store the original url in the Context.Items collection. We need to save this for setting
						// the action of the form.
						string rewritePath = Regex.Replace(urlToRewrite, matchExpression, UrlHelper.GetApplicationPath() + mappings[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
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
						
						this._currentContext.RewritePath(path, pathInfo, querystring);
						rewrittenUrl = path + pathInfo;
						if (querystring.Length > 0)
						{
							rewrittenUrl += "?" + querystring;
						}
					}
					break;
				}
			}
			this._currentContext.Items["VirtualUrl"] = urlToRewrite;
			return rewrittenUrl;
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
