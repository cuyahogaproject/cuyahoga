using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Specialized;

namespace Cuyahoga.Web.Util
{
	/// <summary>
	/// The default url handler.
	/// </summary>
	public class UrlHandlerModule : IHttpModule
	{
		public UrlHandlerModule()
		{
		}

		#region IHttpModule Members

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
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
						// Store the original url in the Context.Items collection. We need to save this for setting
						// the action of the form.
						string rewritePath = regEx.Replace(urlToRewrite, UrlHelper.GetApplicationPath() + mappings[i]);
						context.RewritePath(rewritePath);
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
			string url = HttpContext.Current.Request.RawUrl;
			RewriteUrl(url, app.Context);
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
