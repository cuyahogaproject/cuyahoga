using System;
using System.Collections;
using System.Web;
using System.Text;
using System.IO;
using System.Xml;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Web
{
	/// <summary>
	/// Summary description for Rss.
	/// </summary>
	public class Rss : System.Web.UI.Page
	{
		private CoreRepository _coreRepository;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Context.Response.Clear();
			Context.Response.ContentType = "text/xml";
			if (Context.Request.QueryString["SectionId"] != null)
			{
				int sectionId = Int32.Parse(Context.Request.QueryString["SectionId"]);
				string pathInfo = Context.Request.PathInfo;
				string cacheKey = String.Format("RSS_{0}_{1}", sectionId, pathInfo);
				if (Context.Cache[cacheKey] == null)
				{
					// Get the data for the RSS feed because it's not in the cache yet.
					// Use the same cache duration for the RSS feed as the Section.
					this._coreRepository = (CoreRepository)HttpContext.Current.Items["CoreRepository"];
					Section section = (Section)this._coreRepository.GetObjectById(typeof(Section), sectionId);
					ModuleBase module = section.CreateModule();
					// Create event handlers for NHibernate-related events that can occur in the module.
					module.NHSessionRequired += new ModuleBase.NHSessionEventHandler(Module_NHSessionRequired);

					module.ModulePathInfo = Context.Request.PathInfo;
					ISyndicatable syndicatableModule = module as ISyndicatable;
					if (syndicatableModule != null)
					{
						RssChannel channel = syndicatableModule.GetRssFeed();
						// Rss feed writer code from http://aspnet.4guysfromrolla.com/articles/021804-1.aspx
						// Use an XmlTextWriter to write the XML data to a string...
						StringWriter sw = new StringWriter();
						XmlTextWriter writer = new XmlTextWriter(sw);
						writer.Formatting = Formatting.Indented;

						// write out 
						writer.WriteStartElement("rss");
						writer.WriteAttributeString("version", "2.0");

						// write out 
						writer.WriteStartElement("channel");

						// write out -level elements
						writer.WriteElementString("title", channel.Title);
						writer.WriteElementString("link", Util.UrlHelper.GetFullUrlFromSection(section));
						writer.WriteElementString("description", channel.Description);
						writer.WriteElementString("language", channel.Language);
						writer.WriteElementString("pubDate", channel.PubDate.ToString("r"));
						writer.WriteElementString("lastBuildDate", channel.LastBuildDate.ToString("r"));
						writer.WriteElementString("generator", channel.Generator);
						writer.WriteElementString("ttl", channel.Ttl.ToString());

						foreach (RssItem item in channel.RssItems)
						{
							// write out 
							writer.WriteStartElement("item");

							// write out -level information
							writer.WriteElementString("title", item.Title);
							// TODO: Only supports ID's in the pathinfo now...
							writer.WriteElementString("link", Util.UrlHelper.GetFullUrlFromSection(section) + "/" + item.ItemId);
							writer.WriteElementString("description", item.Description);
							writer.WriteElementString("author", item.Author);
							writer.WriteElementString("pubDate", item.PubDate.ToString("r"));
							writer.WriteElementString("category", item.Category);

							// write out 
							writer.WriteEndElement();
						}

						// write out 
						writer.WriteEndElement();

						// write out 
						writer.WriteEndElement();

						// save the string in the cache
						Cache.Insert(cacheKey, sw.ToString(), null, DateTime.Now.AddSeconds(section.CacheDuration), TimeSpan.Zero);

						writer.Close();
					}
					else
					{
						throw new Exception(String.Format("The module {0} doesn't implement ISyndicatable", module.GetType().FullName));
					}
				}
				Context.Response.Write(Context.Cache[cacheKey].ToString());
			}

			Context.Response.End();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

		private void Module_NHSessionRequired(object sender, ModuleBase.NHSessionEventArgs e)
		{
			e.Session = this._coreRepository.ActiveSession;
		}
	}
}
