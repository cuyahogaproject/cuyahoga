using System;
using System.Collections;

using NHibernate;
using NHibernate.Expression;

using Cuyahoga.Core.Service;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.StaticHtml
{
	/// <summary>
	/// The StaticHtmlModule provides the content of simple static page. It needs at least its 
	/// Section to be set to do something with the content (load, update, delete).
	/// </summary>
	public class StaticHtmlModule : ModuleBase
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
				}
				else
				{
					base.NHSession.Update(content);
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
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete content: " + ex.Message, ex);
			}
		}
	}
}
