using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Web.Cache;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Page engine. This class loads all content based on url parameters and merges
	/// the content with the template.
	/// </summary>
	public class PageEngine : System.Web.UI.Page
	{
		private Node _rootNode;
		private Node _activeNode;
		private Section _activeSection;
		private BaseTemplate _templateControl;
		private CoreRepository _coreRepository;
		private bool _shouldLoadContent;

		#region properties

		/// <summary>
		/// Flag to indicate if the engine should load content (Templates, Nodes and Sections).
		/// </summary>
		protected bool ShouldLoadContent
		{
			set { this._shouldLoadContent = value; }
		}

		/// <summary>
		/// Property RootNode (Node)
		/// </summary>
		public Node RootNode
		{
			get { return this._rootNode; }
		}

		/// <summary>
		/// Property ActiveNode (Node)
		/// </summary>
		public Node ActiveNode
		{
			get { return this._activeNode; }
		}

		/// <summary>
		/// Property ActiveSection (Section)
		/// </summary>
		public Section ActiveSection
		{
			get { return this._activeSection; }
		}

		/// <summary>
		/// Property TemplateControl (BaseTemplate)
		/// </summary>
		public BaseTemplate TemplateControl
		{
			get { return this._templateControl; }
			set { this._templateControl = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public User CuyahogaUser
		{
			get	{ return this.User.Identity as User; }
		}

		/// <summary>
		/// 
		/// </summary>
		public CoreRepository CoreRepository
		{
			get { return this._coreRepository; }
		}

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PageEngine()
		{
			this._activeNode = null;
			this._activeSection = null;
			this._templateControl = null;
			this._shouldLoadContent = true;
		}

		/// <summary>
		/// Load the content and the template as early as possible, so everything is in place before 
		/// modules handle their own ASP.NET lifecycle events.
		/// </summary>
		/// <param name="obj"></param>
		protected override void OnInit(EventArgs e)
		{
			try
			{
				this._coreRepository = (CoreRepository)HttpContext.Current.Items["CoreRepository"];
				// Get the cachemanager for the current site.
				CacheManager cm = new CacheManager(this._coreRepository, Util.UrlHelper.GetSiteUrl());
				// Add the CacheManager to the current context, so it can track all objects during the page lifecycle.
				Context.Items.Add("CacheManager", cm);
				// Get initial root node from the CacheManager.
				this._rootNode = cm.GetRootNode();

				// Load the active node
				// Query the cache by ShortDescription, then NodeId and last, SectionId.
				if (Context.Request.QueryString["ShortDescription"] != null)
				{
					this._activeNode = cm.GetNodeByShortDescription(Context.Request.QueryString["ShortDescription"]);
				}
				else if (Context.Request.QueryString["NodeId"] != null)
				{
					this._activeNode = cm.GetNodeById(Int32.Parse(Context.Request.QueryString["NodeId"]));
				}
				else if (Context.Request.QueryString["SectionId"] != null)
				{
					this._activeSection = cm.GetSectionById(Int32.Parse(Context.Request.QueryString["SectionId"]));
					this._activeNode = this._activeSection.Node;
				}
				else
				{
					// Can't load a particular node, so the root node has to be the active node
					// Maybe we have culture information stored in a cookie, so we might need a different 
					// root Node.
					if (Context.Request.Cookies["CuyahogaCulture"] != null)
					{
						string currentCulture = Context.Request.Cookies["CuyahogaCulture"].Value;
						this._activeNode = cm.GetRootNodeByCulture(currentCulture);
					}
					else
					{
						this._activeNode = this._rootNode;
					}
				}
				// Set the root node again, but now based on the active node.
				if (this._activeNode.NodePath != null)
				{
					this._rootNode = this._activeNode.NodePath[0];
				}

				// Set culture
				// TODO: fix this because ASP.NET pages are not guaranteed to run in 1 thread (how?).
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(this._activeNode.Culture);	
				Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(this._activeNode.Culture);
				
				// Check node-level security
				if (! this._activeNode.ViewAllowed(this.User.Identity))
				{
					throw new AccessForbiddenException("You are not allowed to view this page.");
				}

				if (this._shouldLoadContent)
				{
					 LoadContent(cm);				
				}
			}
			// TODO: we could handle some exceptions here, but for now rethrow them, so it will be handled
			// by the general error page (and logged!).
			catch (Exception ex)
			{
				throw ex;
			}
			base.OnInit(e);
		}

		/// <summary>
		/// Use a custom HtmlTextWriter to render the page if the url is rewritten, to correct the form action.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (Context.Items["VirtualUrl"] != null)
			{
				writer = new FormFixerHtmlTextWriter(writer.InnerWriter, "", Context.Items["VirtualUrl"].ToString());
			}
			base.Render (writer);
		}

		protected override void OnUnload(EventArgs e)
		{
			// If the CacheManager contains any changes it should be stored again.
			CacheManager cm = Context.Items["CacheManager"] as CacheManager;
			if (cm != null && cm.HasChanges)
			{
				cm.SaveCache();
			}

			// Ready, write the execution time to the debug output.
			TimeSpan ts = DateTime.Now - (DateTime)Context.Items["starttime"];
			Debug.WriteLine("Total execution time : " + ts.Milliseconds.ToString() + " milliseconds. \n");
			base.OnUnload (e);
		}

		private void LoadContent(CacheManager cm)
		{
			// ===== Load template and usercontrols =====

			string appRoot = UrlHelper.GetApplicationPath();
			// We know the active node so the template can be loaded.
			if (this._activeNode.Template != null)
			{
				string templatePath = appRoot + this._activeNode.Template.Path;
				this._templateControl = (BaseTemplate)this.LoadControl(templatePath);
				// Explicitly set the id to 'p' to save some bytes (otherwise _ctl0 would be added).
				this._templateControl.ID = "p";
				this._templateControl.Title = this._activeNode.Site.Name + " - " + this._activeNode.Title;
				this._templateControl.Css = appRoot + Config.GetConfiguration()["CssDir"] + this._activeNode.Template.Css;
			}
			else
			{
				throw new Exception("No template associated with the current Node.");
			}

			// Load sections and modules
			int sectionId = -1;
			if (Context.Request.QueryString["SectionId"] != null)
			{
				sectionId = Int32.Parse(Context.Request.QueryString["SectionId"]);
			}
			foreach (Section section in this._activeNode.Sections)
			{
				// Check view permissions before adding the section to the page.
				if (section.ViewAllowed(this.User.Identity))
				{
					if (section.Id == sectionId && this._activeSection == null)
					{
						this._activeSection = section;
					}
					// Create the module that is connected to the section.
					// Create event handlers for NHibernate-related events that can occur in the module.
					section.SessionFactoryRebuilt += new EventHandler(Section_SessionFactoryRebuilt);
					ModuleBase module = section.CreateModule(UrlHelper.GetUrlFromSection(section));
					module.NHSessionRequired += new ModuleBase.NHSessionEventHandler(Module_NHSessionRequired);

					if (module != null)
					{
						BaseModuleControl ctrl = (BaseModuleControl)this.LoadControl(appRoot + section.ModuleType.Path);
						ctrl.Module = module;
						((PlaceHolder)this._templateControl.Containers[section.PlaceholderId]).Controls.Add(ctrl);

						if (Context.Request.PathInfo.Length > 0 && section == this._activeSection)
						{
							// Parse the PathInfo of the request because they are the parameters 
							// for the module that is connected to the active section.
							module.ModulePathInfo = Context.Request.PathInfo;
						}
					}
				}
			}
			this.Controls.AddAt(0, this._templateControl);
			// remove html that was in the original page (Default.aspx)
			for (int i = this.Controls.Count -1; i < 0; i --)
				this.Controls.RemoveAt(i);
		}

		private void Section_SessionFactoryRebuilt(object sender, EventArgs e)
		{
			// The SessionFactory was rebuilt, so the current NHibernate Session has become invalid.
			// This is handled by a simple reload of the page. 
			// TODO: handle more elegantly?
			if (Context.Items["VirtualUrl"] != null)
			{
				Context.Response.Redirect(Context.Items["VirtualUrl"].ToString());
			}
			else
			{
				Context.Response.Redirect(Context.Request.RawUrl);
			}
		}

		private void Module_NHSessionRequired(object sender, ModuleBase.NHSessionEventArgs e)
		{
			e.Session = this._coreRepository.ActiveSession;
		}
	}
}
