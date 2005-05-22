using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.Util;

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
		/// Messagebox control.
		/// </summary>
		public HtmlGenericControl MessageBox
		{
			get
			{
				return this.TemplateControl.FindControl("MessageBox") as HtmlGenericControl;
			}
		}

		/// <summary>
		/// Default constructor calls base constructor with parameters for templatecontrol, 
		/// templatepath and stylesheet.
		/// </summary>
		public ModuleAdminBasePage() : base("ModuleAdminTemplate.ascx",  "~/Controls/", "~/Admin/Css/Admin.css")
		{
			this._node = null;
			this._section = null;
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
				this._section.SessionFactoryRebuilt += new EventHandler(Section_SessionFactoryRebuilt);
				if (this._section.Node.Id == this._node.Id)
				{
					this._section.Node = this._node;
				}
				this._module = this._section.CreateModule(UrlHelper.GetUrlFromSection(this._section));
				this._module.NHSessionRequired += new ModuleBase.NHSessionEventHandler(Module_NHSessionRequired);
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

			base.OnInit (e);
		}

		/// <summary>
		/// Try to find the MessageBox control, insert the errortext and set visibility to true.
		/// TODO: reduce redundancy with the AdminBasePage
		/// </summary>
		/// <param name="errorText"></param>
		public void ShowError(string errorText)
		{
			if (this.MessageBox != null)
			{
				this.MessageBox.InnerText = "An error occured: " + errorText;
				this.MessageBox.Attributes["class"] = "errorbox";
				this.MessageBox.Visible = true;
			}
			else
			{
				// Throw an Exception that will be handled by the global exception handler.
				throw new Exception(errorText);
			}
		}

		/// <summary>
		/// Try to find the MessageBox control, insert the message and set visibility to true.
		/// TODO: reduce redundancy with the AdminBasePage
		/// </summary>
		/// <param name="message"></param>
		public void ShowMessage(string message)
		{
			if (this.MessageBox != null)
			{
				this.MessageBox.InnerText = message;
				this.MessageBox.Attributes["class"] = "messagebox";
				this.MessageBox.Visible = true;
			}
			// TODO: change the class attribute to make a difference with the error (nice background image?)
		}

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

		private void Module_NHSessionRequired(object sender, Cuyahoga.Core.Domain.ModuleBase.NHSessionEventArgs e)
		{
			e.Session = base.CoreRepository.ActiveSession;
		}

		private void Section_SessionFactoryRebuilt(object sender, EventArgs e)
		{
			// The SessionFactory was rebuilt, so the current NHibernate Session has become invalid.
			// This is handled by a simple reload of the page. 
			// TODO: handle more elegantly?
			Context.Response.Redirect(Context.Request.RawUrl);
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
