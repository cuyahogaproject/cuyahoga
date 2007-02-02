using System;
using System.Collections;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using NHibernate;
using Cuyahoga.Modules.Flash.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Modules.Flash
{
	/// <summary>
	/// Summary description for FlashModule.
	/// </summary>
	public class FlashModule : ModuleBase, INHibernateModule, ISearchable
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public FlashModule()
		{
		}

		/// <summary>
		/// Get the content that belongs to the Section where the module is attached to.
		/// </summary>
		/// <returns></returns>
		public AlternateContent GetContent()
		{
			if (base.Section != null)
			{
				string hql = "from AlternateContent s where s.Section.Id = ? ";
				IList results = base.NHSession.Find(hql, this.Section.Id, NHibernateUtil.Int32);
				if (results.Count == 1)
				{
					return (AlternateContent)results[0];
				}
				else if (results.Count > 1)
				{
					throw new Exception("A request for AlternateContent should only return one item");
				}
				else
				{
					return null;
				}
			}
			else
			{
				throw new Exception("Unable to get the AlternateContent when no Section is available");
			}
		}

		/// <summary>
		/// Save the content.
		/// </summary>
		/// <param name="content"></param>
		public void SaveContent(AlternateContent content)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				if (content.Id == -1)
				{
					content.UpdateTimestamp = DateTime.Now;
					base.NHSession.Save(content);
					OnContentCreated(new IndexEventArgs(AlternateContentToSearchContent(content)));
				}
				else
				{
					base.NHSession.Update(content);
					OnContentUpdated(new IndexEventArgs(AlternateContentToSearchContent(content)));
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
		public void DeleteContent(AlternateContent content)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(content);
				tx.Commit();
				OnContentDeleted(new IndexEventArgs(AlternateContentToSearchContent(content)));
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete content: " + ex.Message, ex);
			}
		}

		public override void DeleteModuleContent()
		{
			// Delete the associated AlternateContent
			AlternateContent content = this.GetContent();
			if (content != null)
			{
				DeleteContent(content);
			}
		}

		private SearchContent AlternateContentToSearchContent(AlternateContent shc)
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
			AlternateContent shc = GetContent();
			if (shc != null)
			{
				SearchContent[] searchContents = new SearchContent[1];
				searchContents[0] = AlternateContentToSearchContent(shc);
				return searchContents;
			}
			else
			{
				return new SearchContent[0];
			}
		}

		public event IndexEventHandler ContentCreated;

		protected void OnContentCreated(IndexEventArgs e)
		{
			if (ContentCreated != null)
			{
				ContentCreated(this, e);
			}
		}

		public event IndexEventHandler ContentDeleted;

		protected void OnContentDeleted(IndexEventArgs e)
		{
			if (ContentDeleted != null)
			{
				ContentDeleted(this, e);
			}
		}

		public event IndexEventHandler ContentUpdated;

		protected void OnContentUpdated(IndexEventArgs e)
		{
			if (ContentUpdated != null)
			{
				ContentUpdated(this, e);
			}
		}

		#endregion
	}
    
    /// <summary>
	/// The MovieQuality of the Flash
	/// </summary>
	public enum MovieQuality
	{
		high,
		best,
		medium,
		low,
		autolow,
		autohigh
	}

	/// <summary>
	/// The MovieAlign of the Flash
	/// </summary>
	public enum MovieAlign
	{
		left,
		right,
		top,
		bottom,
		middle
	}

	/// <summary>
	/// The MovieScriptAccess of the Flash
	/// </summary>
	public enum MovieScriptAccess
	{
		sameDomain,
		always,
		never
	}   
}
