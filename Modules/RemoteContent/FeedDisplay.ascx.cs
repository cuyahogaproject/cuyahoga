namespace Cuyahoga.Modules.RemoteContent
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Xml;
	using System.Net;
	using System.IO;

	using Cuyahoga.Web.UI;

	/// <summary>
	///	Display Remote Content.
	///	Code partially taken from the Rainbow Portal (http://www.rainbowportal.net) XmlFeed module.
	/// </summary>
	public class FeedDisplay : BaseModuleControl
	{
		protected System.Web.UI.WebControls.Xml xmlContent;

		private void Page_Load(object sender, System.EventArgs e)
		{
			RemoteContentModule module = this.Module as RemoteContentModule;
			if (module != null && ! base.HasCachedOutput)
			{
				try
				{
					FeedType feedType = (FeedType)Enum.Parse(typeof(FeedType), module.Section.Settings["FEED_TYPE"].ToString());
					string feedUrl = module.Section.Settings["FEED_URL"].ToString();
					string xsltFile = String.Empty;

					switch (feedType)
					{
						case FeedType.Rss_091:
						case FeedType.Atom_30:
							throw new NotImplementedException("Not implemented yet.");
						case FeedType.Rss_20:
							xsltFile = "RSS20.xslt";
							break;
					}
					string xsltPath = "~/Modules/RemoteContent/xslt/" + xsltFile;
					// handle on the remote resource
					HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(feedUrl);
					
					// set the HTTP properties
					wr.Timeout = 10000; // milliseconds to seconds
					// Read the response
					WebResponse resp = wr.GetResponse();
					// Stream read the response
					Stream stream = resp.GetResponseStream();
					// Read XML data from the stream
					XmlTextReader reader = new XmlTextReader(stream);
					// ignore the DTD
					reader.XmlResolver = null;
					// Create a new document object
					XmlDocument doc = new XmlDocument();
					// Create the content of the XML Document from the XML data stream
					doc.Load(reader);
					// the XML control to hold the generated XML document
					this.xmlContent.Document = doc;
                    this.xmlContent.TransformSource = xsltPath;
				}
				catch (Exception ex)
				{
					// TODO: logging
				}
			}
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
