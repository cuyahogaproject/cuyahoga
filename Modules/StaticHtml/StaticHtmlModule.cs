using System;
using System.Collections;

using NHibernate;
using NHibernate.Expression;

using Cuyahoga.Core.Service;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Modules.StaticHtml
{
	/// <summary>
	/// The StaticHtmlModule provides the content of simple static page. It needs at least its 
	/// Section to be set to do something with the content (load, update, delete).
	/// </summary>
	public class StaticHtmlModule : ModuleBase, ISearchable
	{
		public StaticHtmlModule()
		{	
			SessionFactory sf = SessionFactory.GetInstance();
			// Register classes that are used by the StaticHtmlModule.
			sf.RegisterPersistentClass(typeof(StaticHtmlContent));

			// Set a flag to indicate that the SessionFactory is rebuilt. It would be more elegant
			// to do this with an event but since this happens in the constructor, there can't
			// be any event handlers attached already. We leave it up to the Section to raise the 
			// event (see Section.CreateModule()).
			base.SessionFactoryRebuilt = sf.Rebuild();
		}

		/// <summary>
		/// Get the content that belongs to the Section where the module is attached to.
		/// </summary>
		/// <returns></returns>
		public StaticHtmlContent GetContent()
		{
			if (base.Section != null)
			{
				string hql = "from StaticHtmlContent s where s.Section.Id = ? ";
				IList results = base.NHSession.Find(hql, this.Section.Id, NHibernate.Type.TypeFactory.GetInt32Type());
				if (results.Count == 1)
				{
					return (StaticHtmlContent)results[0];
				}
				else if (results.Count > 1)
				{
					throw new Exception("A request for StaticHtmlContent should only return one item");
				}
				else
				{
					return null;
				}
			}
			else
			{
				throw new Exception("Unable to get the StaticHtmlContent when no Section is available");
			}
		}

		/// <summary>
		/// Save the content.
		/// </summary>
		/// <param name="content"></param>
		public void SaveContent(StaticHtmlContent content)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				if (content.Id == -1)
				{
					content.UpdateTimestamp = DateTime.Now;
					base.NHSession.Save(content);
					OnContentCreated(new IndexEventArgs(StaticHtmlContentToSearchContent(content)));
				}
				else
				{
					base.NHSession.Update(content);
					OnContentUpdated(new IndexEventArgs(StaticHtmlContentToSearchContent(content)));
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save content: " + ex.Message, ex);
			}
		}

		/// <summary>
		/// Delete the content.
		/// </summary>
		/// <param name="content"></param>
		public void DeleteContent(StaticHtmlContent content)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(content);
				tx.Commit();
				OnContentDeleted(new IndexEventArgs(StaticHtmlContentToSearchContent(content)));
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete content: " + ex.Message, ex);
			}
		}

		public override void DeleteModuleContent()
		{
			// Delete the associated StaticHtmlContent
			StaticHtmlContent content = this.GetContent();
			if (content != null)
			{
				DeleteContent(content);
			}
		}


		private SearchContent StaticHtmlContentToSearchContent(StaticHtmlContent shc)
		{
			SearchContent sc = new SearchContent();
			sc.Title = shc.Section.Title;
			sc.Summary = Text.TruncateText(shc.Content, 200); // trunctate the summary to 200 chars
			sc.Contents = shc.Content;
			sc.Author = shc.ModifiedBy.FullName;
			sc.ModuleType = shc.Section.ModuleType.Name;
			sc.Path = this.SectionUrl;
			sc.Category = String.Empty;
			sc.Site = shc.Section.Node.Site.Name;
			sc.DateCreated = shc.UpdateTimestamp;
			sc.DateModified = shc.UpdateTimestamp;
			sc.SectionId = shc.Section.Id;

			return sc;
		}

		#region ISearchable Members

		public SearchContent[] GetAllSearchableContent()
		{
			SearchContent[] searchContents = new SearchContent[1];
			StaticHtmlContent shc = GetContent();
			searchContents[0] = StaticHtmlContentToSearchContent(shc);
			return searchContents;
		}

		public event Cuyahoga.Core.Search.IndexEventHandler ContentCreated;

		protected void OnContentCreated(IndexEventArgs e)
		{
			if (ContentCreated != null)
			{
				ContentCreated(this, e);
			}
		}

		public event Cuyahoga.Core.Search.IndexEventHandler ContentDeleted;

		protected void OnContentDeleted(IndexEventArgs e)
		{
			if (ContentDeleted != null)
			{
				ContentDeleted(this, e);
			}
		}

		public event Cuyahoga.Core.Search.IndexEventHandler ContentUpdated;

		protected void OnContentUpdated(IndexEventArgs e)
		{
			if (ContentUpdated != null)
			{
				ContentUpdated(this, e);
			}
		}

		#endregion
	}
}
