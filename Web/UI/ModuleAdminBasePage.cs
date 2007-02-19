using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Util;
using Cuyahoga.Web.Components;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Summary description for ModuleAdminBasePage.
	/// </summary>
	public class ModuleAdminBasePage : GenericBasePage
	{
		private Node _node;
		private Section _section;
		private ModuleBase _module;
		private ModuleLoader _moduleLoader;

		/// <summary>
		/// Property Node (Node)
		/// </summary>
		public Node Node
		{
			get { return this._node; }
		}

		/// <summary>
		/// Property Section (Section)
		/// </summary>
		public Section Section
		{
			get { return this._section; }
		}

		/// <summary>
		/// Property Module (ModuleBase)
		/// </summary>
		protected ModuleBase Module
		{
			get { return this._module; }
		}

		/// <summary>
		/// Default constructor calls base constructor with parameters for templatecontrol, 
		/// templatepath and stylesheet.
		/// </summary>
		public ModuleAdminBasePage() : base("ModuleAdminTemplate.ascx",  "~/Controls/", "~/Admin/Css/Admin.css")
		{
			this._node = null;
			this._section = null;

			this._moduleLoader = Container.Resolve<ModuleLoader>();
		}

		/// <summary>
		/// In the OnInit method the Node and Section of every ModuleAdminPage is set. 
		/// An exception is thrown when one of them cannot be set.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			try
			{
				int nodeId = Int32.Parse(Context.Request.QueryString["NodeId"]);
				this._node = (Node)base.CoreRepository.GetObjectById(typeof(Node), nodeId);
				int sectionId = Int32.Parse(Context.Request.QueryString["SectionId"]);
				this._section = (Section)base.CoreRepository.GetObjectById(typeof(Section), sectionId);
				this._module = this._moduleLoader.GetModuleFromSection(this._section);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to initialize the Module Admin page because a Node or a Section could not be created.", ex);
			}
			// Check permissions
			if (! Context.User.Identity.IsAuthenticated)
			{
				throw new AccessForbiddenException("You are not logged in.");
			}
			else
			{
				User user = Context.User.Identity as User;
				if (! user.CanEdit(this._section))
				{
					throw new ActionForbiddenException("You are not allowed to edit the section.");
				}
			}

			// Optional indexing event handlers
			if (this._module is ISearchable 
				&& Boolean.Parse(Config.GetConfiguration()["InstantIndexing"]))
			{
				ISearchable searchableModule = (ISearchable)this._module;
				searchableModule.ContentCreated += new IndexEventHandler(searchableModule_ContentCreated);
				searchableModule.ContentUpdated += new IndexEventHandler(searchableModule_ContentUpdated);
				searchableModule.ContentDeleted += new IndexEventHandler(searchableModule_ContentDeleted);
			}

			// Set FCKEditor context (used by some module admin pages)
			// It would be nicer if we could do this in the Global.asax, but there the 
			// ultra-convenient ~/Path (ResolveUrl) isn't available :).
			string userFilesPath = Config.GetConfiguration()["FCKeditor:UserFilesPath"];
			if (userFilesPath != null && HttpContext.Current.Application["FCKeditor:UserFilesPath"] == null)
			{	
				HttpContext.Current.Application.Lock();
				HttpContext.Current.Application["FCKeditor:UserFilesPath"] = ResolveUrl(userFilesPath);
				HttpContext.Current.Application.UnLock();
			}
			
			base.OnInit (e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetBaseQueryString()
		{
			return String.Format("?NodeId={0}&SectionId={1}", this.Node.Id, this.Section.Id);
		}

		private void IndexContent(SearchContent searchContent, IndexAction action)
		{
			// Index
			string indexDir = Context.Server.MapPath(Config.GetConfiguration()["SearchIndexDir"]);
			IndexBuilder ib = new IndexBuilder(indexDir, false);
			switch (action)
			{
				case IndexAction.Create:
					ib.AddContent(searchContent);
					break;
				case IndexAction.Update:
					ib.UpdateContent(searchContent);
					break;
				case IndexAction.Delete:
					ib.DeleteContent(searchContent);
					break;
			}
			ib.Close();
		}

		private void searchableModule_ContentCreated(object sender, IndexEventArgs e)
		{
			IndexContent(e.SearchContent, IndexAction.Create);	
		}

		private void searchableModule_ContentUpdated(object sender, IndexEventArgs e)
		{
			IndexContent(e.SearchContent, IndexAction.Update);	
		}

		private void searchableModule_ContentDeleted(object sender, IndexEventArgs e)
		{
			IndexContent(e.SearchContent, IndexAction.Delete);	
		}

		private enum IndexAction
		{
			Create,
			Update,
			Delete
		}
	}
}
