using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Util;
using Cuyahoga.Modules.RemoteContent.Domain;
using Cuyahoga.Modules.RemoteContent.Util;
using Cuyahoga.Web.Components;
using log4net;
using NHibernate;

namespace Cuyahoga.Modules.RemoteContent
{
	/// <summary>
	/// The RemoteContent module provides facilities to display content that is 
	/// syndictated from other sites.
	/// </summary>
	public class RemoteContentModule : ModuleBase, INHibernateModule
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(RemoteContentModule));
		private int _cacheDuration;
		private bool _backgroundRefresh;
		private bool _showContents;
		private bool _showDates;
		private bool _showSources;
		private bool _showAuthors;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public RemoteContentModule()
		{
		}

		#region properties

		/// <summary>
		/// Property ShowContents (bool)
		/// </summary>
		public bool ShowContents
		{
			get { return this._showContents; }
		}

		/// <summary>
		/// Property ShowDates (bool)
		/// </summary>
		public bool ShowDates
		{
			get { return this._showDates; }
		}

		/// <summary>
		/// Property ShowSources (bool)
		/// </summary>
		public bool ShowSources
		{
			get { return this._showSources; }
		}

		/// <summary>
		/// Property ShowAuthors (bool)
		/// </summary>
		public bool ShowAuthors
		{
			get { return this._showAuthors; }
		}

		#endregion

		public override void ReadSectionSettings()
		{
			base.ReadSectionSettings ();
			// Set dynamic module settings
			this._cacheDuration = Convert.ToInt32(base.Section.Settings["CACHE_DURATION"]);
			this._backgroundRefresh = Convert.ToBoolean(base.Section.Settings["BACKGROUND_REFRESH"]);
			this._showContents = Convert.ToBoolean(base.Section.Settings["SHOW_CONTENTS"]);
			this._showDates = Convert.ToBoolean(base.Section.Settings["SHOW_DATES"]);
			this._showSources = Convert.ToBoolean(base.Section.Settings["SHOW_SOURCES"]);
			this._showAuthors = Convert.ToBoolean(base.Section.Settings["SHOW_AUTHORS"]);
		}


		/// <summary>
		/// Retrieve all feeds that belong to this module.
		/// </summary>
		/// <returns></returns>
		public IList GetAllFeeds()
		{
			string hql = "from Feed f where f.Section.Id = :sectionId";
			IQuery q = base.NHSession.CreateQuery(hql);
			q.SetInt32("sectionId", base.Section.Id);

			try
			{
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Feeds for section: " + base.Section.Title, ex);
			}
		}

		/// <summary>
		/// Get a single feed based on its identifier.
		/// </summary>
		/// <param name="feedId"></param>
		/// <returns></returns>
		public Feed GetFeedById(int feedId)
		{
			try
			{
				return (Feed)base.NHSession.Load(typeof(Feed), feedId);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Feed with identifier: " + feedId.ToString(), ex);
			}
		}

		/// <summary>
		/// Retrieve all feed items from all feeds sorted by date, descending. 
		/// </summary>
		/// <returns></returns>
		public IList GetAllFeedItems()
		{
			IList items = null;

			string hql = "from FeedItem fi where fi.Feed.Section.Id = :sectionId order by fi.PubDate desc";
			IQuery q = base.NHSession.CreateQuery(hql);
			q.SetInt32("sectionId", base.Section.Id);

			try
			{
				// First refresh feeds when neccesary.
				RefreshFeeds();

				items = q.List();

				return items;
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get FeedItems for section: " + base.Section.Title, ex);
			}
		}

		/// <summary>
		/// Save a Feed.
		/// </summary>
		/// <param name="feed"></param>
		public void SaveFeed(Feed feed)
		{
			// uses the ISession from the HttpContext
			SaveFeed(feed, base.NHSession);
		}

		private void SaveFeed(Feed feed, ISession session)
		{
			// First check if the pubdate exists. If not, use DateTime.Now.
			if (feed.PubDate == DateTime.MinValue)
			{
				feed.PubDate = DateTime.Now;
			}
			ITransaction tx = session.BeginTransaction();
			try
			{
				session.SaveOrUpdate(feed);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Feed", ex);
			}
		}

		/// <summary>
		/// Delete a Feed.
		/// </summary>
		/// <param name="feed"></param>
		public void DeleteFeed(Feed feed)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(feed);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Feed", ex);
			}
		}

		/// <summary>
		/// Verifies if a feed is valid. A Feed object with the feed properties is returned if this
		/// is the case. Otherwise an exception is thrown.
		/// </summary>
		/// <param name="feedUrl"></param>
		/// <returns></returns>
		public Feed VerifyFeed(string feedUrl)
		{
			XmlDocument doc = GetFeedXml(feedUrl);

			return XmlContentToFeed(doc, feedUrl);
		}

		/// <summary>
		/// Refresh the items of a single feed. It doesn't save the new items!
		/// </summary>
		/// <param name="feed"></param>
		public void RefreshFeedContents(Feed feed)
		{
			feed.FeedItems.Clear();
			try
			{
				XmlDocument doc = GetFeedXml(feed.Url);
				// Create an XmlNamespaceManager for resolving namespaces.
				XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
				nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

				XmlNodeList xmlItems = doc.SelectNodes("//channel/item");
				foreach (XmlNode xmlItem in xmlItems)
				{
					FeedItem feedItem = new FeedItem();
					// Truncate title to 100 characters
					string title = xmlItem.SelectSingleNode("title").InnerText;
					if (title.Length > 100)
					{
						title = title.Substring(0, 100);
					}
					feedItem.Title = title;
					feedItem.Url = xmlItem.SelectSingleNode("link").InnerText;

					if (xmlItem.SelectSingleNode("pubDate") != null)
					{
						feedItem.PubDate = RFC2822Date.Parse(xmlItem.SelectSingleNode("pubDate").InnerText);
					}
					else
					{
						feedItem.PubDate = DateTime.Now;
					}
					if (this._showContents && xmlItem.SelectSingleNode("description") != null)
					{
						feedItem.Content = xmlItem.SelectSingleNode("description").InnerText;
					}
					if (xmlItem.SelectSingleNode("author", nsmgr) != null)
					{
						feedItem.Author = xmlItem.SelectSingleNode("author", nsmgr).InnerText;
					}
					else if (xmlItem.SelectSingleNode("dc:creator", nsmgr) != null)
					{
						feedItem.Author = xmlItem.SelectSingleNode("dc:creator", nsmgr).InnerText;
					}
					feedItem.Feed = feed;
					feed.FeedItems.Add(feedItem);
				}
				feed.SortAndFilter();
			}
			catch (Exception ex)
			{
				log.Error("Error refreshing feed contents for: " + feed.Title, ex);
				throw;
			}
		}

		/// <summary>
		/// Refreshes the content of all feeds that are related to this module.
		/// </summary>
		/// <remarks>Each feed is refreshed in a separate thread.</remarks>
		private void RefreshFeeds()
		{
			IList feeds = GetAllFeeds();
			foreach (Feed feed in feeds)
			{
				if (feed.UpdateTimestamp.AddMinutes(this._cacheDuration) < DateTime.Now)
				{
					// HACK: update the updatetimestamp before anything is refreshed. 
					// This will prevent that other threads also refresh the feed.
					feed.UpdateTimestamp = DateTime.Now;
					base.NHSession.Update(feed);
					base.NHSession.Flush();
					// The feed is kind of locked now (with the updatetimestamp). Plenty
					// of time now to refresh the feeds.
					base.NHSession.Evict(feed);
					
					if (this._backgroundRefresh)
					{
						FeedFetcher ff = new FeedFetcher(feed, null);
						// Fire a separate to refresh the feed so the visitor doesn't have to wait.
						Thread t = new Thread(new ThreadStart(ff.FetchFeed));
						t.Priority = ThreadPriority.BelowNormal; // No need to rush here
						t.Start();
					}
					else
					{
						FeedFetcher ff = new FeedFetcher(feed, base.NHSession);
						ff.FetchFeed();
					}
				}
			}
		}

		/// <summary>
		/// Create a Feed object from XML content.
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="feedUrl"></param>
		/// <returns></returns>
		private Feed XmlContentToFeed(XmlDocument doc, string feedUrl)
		{
			Feed feed = new Feed();
			feed.Url = feedUrl;
			feed.Title = doc.SelectSingleNode("//channel/title").InnerText;
			return feed;
		}

		private XmlDocument GetFeedXml(string feedUrl)
		{
			WebRequest wr = WebRequest.Create(feedUrl);
			WebResponse response = wr.GetResponse();
			Stream receiveStream = response.GetResponseStream();
			XmlTextReader reader  = new XmlTextReader(receiveStream);

			XmlDocument doc = new XmlDocument();
			doc.Load(reader);

			reader.Close();
			receiveStream.Close();
			response.Close();

			return doc;
		}
	}

	/// <summary>
	/// Class that can be used to fetch multiple feeds in separate threads.
	/// </summary>
	internal class FeedFetcher
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(FeedFetcher));
		private Feed _feed;
		private ISession _session;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="feed"></param>
		/// <param name="session"></param>
		public FeedFetcher(Feed feed, ISession session)
		{
			this._feed = feed;
			this._session = session;
		}

		/// <summary>
		/// Fetch the contents of the current feed and store the items.
		/// </summary>
		public void FetchFeed()
		{
			try
			{
				ModuleLoader moduleLoader = IoC.Resolve<ModuleLoader>();
				RemoteContentModule module = moduleLoader.GetModuleFromSection(this._feed.Section) as RemoteContentModule;

				if (this._session == null)
				{
					// Use in a different session, otherwise things go wrong because this method might
					// run in a different thread.
					ISessionFactory sessionFactory = IoC.Resolve<ISessionFactory>("nhibernate.factory");
					using (ISession session = sessionFactory.OpenSession())
					{
						UpdateFeed(session, module, this._feed);
						session.Close();
					}
				}
				else
				{
					UpdateFeed(this._session, module, this._feed);
				}
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
			}
		}

		private void UpdateFeed(ISession session, RemoteContentModule module, Feed feed)
		{
			using (ITransaction tx = session.BeginTransaction())
			{
				try
				{
					session.Lock(feed, LockMode.None);
					module.RefreshFeedContents(feed);
					session.Update(feed);
					tx.Commit();
				}
				catch
				{
					tx.Rollback();
					throw;
				}
			}
		}
	}
}
