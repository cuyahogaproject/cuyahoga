using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Collections;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Page engine. This class loads all content based on url parameters and merges
	/// the content with the template.
	/// </summary>
	public class PageEngine : System.Web.UI.Page
	{
		private Site _currentSite;
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

		/// <summary>
		/// 
		/// </summary>
		public Site CurrentSite
		{
			get { return this._currentSite; }
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
			this._coreRepository = (CoreRepository)HttpContext.Current.Items["CoreRepository"];

			// Load the current site
			Node entryNode = null;
			string siteUrl = UrlHelper.GetSiteUrl();
			SiteAlias currentSiteAlias = this._coreRepository.GetSiteAliasByUrl(siteUrl);
			if (currentSiteAlias != null)
			{
				this._currentSite = currentSiteAlias.Site;
				entryNode = currentSiteAlias.EntryNode;
			}
			else
			{
				this._currentSite = this._coreRepository.GetSiteBySiteUrl(siteUrl);
			}
			if (this._currentSite == null)
			{
				throw new SiteNullException("No site found at " + siteUrl);
			}

			// Load the active node
			// Query the cache by ShortDescription, then NodeId and last, SectionId.
			if (Context.Request.QueryString["ShortDescription"] != null)
			{
				this._activeNode = this._coreRepository.GetNodeByShortDescriptionAndSite(Context.Request.QueryString["ShortDescription"], this._currentSite);
			}
			else if (Context.Request.QueryString["NodeId"] != null)
			{
				this._activeNode = (Node)this._coreRepository.GetObjectById(typeof(Node)
					, Int32.Parse(Context.Request.QueryString["NodeId"])
					, true);
			}
			else if (Context.Request.QueryString["SectionId"] != null)
			{
				try
				{
					this._activeSection = (Section)this._coreRepository.GetObjectById(typeof(Section)
						, Int32.Parse(Context.Request.QueryString["SectionId"]));
					this._activeNode = this._activeSection.Node;
				}
				catch
				{
					throw new SectionNullException("Section not found: " + Context.Request.QueryString["SectionId"]);
				}
			}
			else if (entryNode != null)
			{
				this._activeNode = entryNode;
			}
			else
			{
				// Can't load a particular node, so the root node has to be the active node
				// Maybe we have culture information stored in a cookie, so we might need a different 
				// root Node.
				string currentCulture = this._currentSite.DefaultCulture;
				if (Context.Request.Cookies["CuyahogaCulture"] != null)
				{
					currentCulture = Context.Request.Cookies["CuyahogaCulture"].Value;
				}
				this._activeNode = this._coreRepository.GetRootNodeByCultureAndSite(currentCulture, this._currentSite);
			}
			// Raise an exception when there is no Node found. It will be handled by the global error handler
			// and translated into a proper 404.
			if (this._activeNode == null)
			{
				throw new NodeNullException(String.Format(@"No node found with the following parameters: 
					NodeId: {0},
					ShortDescription: {1},
					SectionId: {2}"
					, Context.Request.QueryString["NodeId"]
					, Context.Request.QueryString["ShortDescription"]
					, Context.Request.QueryString["SectionId"]));
			}
			this._rootNode = this._activeNode.NodePath[0];

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
				LoadContent();			
				LoadMenus();
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

		private void LoadContent()
		{
			// ===== Load templates  =====

			string appRoot = UrlHelper.GetApplicationPath();
			// We know the active node so the template can be loaded.
			if (this._activeNode.Template != null)
			{
				string templatePath = appRoot + this._activeNode.Template.Path;
				this._templateControl = (BaseTemplate)this.LoadControl(templatePath);
				// Explicitly set the id to 'p' to save some bytes (otherwise _ctl0 would be added).
				this._templateControl.ID = "p";
				this._templateControl.Title = this._activeNode.Site.Name + " - " + this._activeNode.Title;
				this._templateControl.Css = appRoot + this._activeNode.Template.BasePath + "/Css/" + this._activeNode.Template.Css;
				// Load sections that are related to the template
				foreach (DictionaryEntry sectionEntry in this.ActiveNode.Template
			}
			else
			{
				throw new Exception("No template associated with the current Node.");
			}

			// ===== Load sections and modules =====
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
					section.SessionFactoryRebuilt -= new EventHandler(Section_SessionFactoryRebuilt);

					if (module != null)
					{
						if (Context.Request.PathInfo.Length > 0 && section == this._activeSection)
						{
							// Parse the PathInfo of the request because they can be the parameters 
							// for the module that is connected to the active section.
							module.ModulePathInfo = Context.Request.PathInfo;
						}
						BaseModuleControl ctrl = LoadModuleControl(module);
						((PlaceHolder)this._templateControl.Containers[section.PlaceholderId]).Controls.Add(ctrl);
					}
				}
			}

			this.Controls.AddAt(0, this._templateControl);
			// remove html that was in the original page (Default.aspx)
			for (int i = this.Controls.Count -1; i < 0; i --)
				this.Controls.RemoveAt(i);
		}

		private BaseModuleControl LoadModuleControl(ModuleBase module)
		{
			BaseModuleControl ctrl = (BaseModuleControl)this.LoadControl(UrlHelper.GetApplicationPath() + module.CurrentViewControlPath);
			ctrl.Module = module;
			return ctrl;
		}

		private void LoadMenus()
		{
			IList menus = this._coreRepository.GetMenusByRootNode(this._rootNode);
			foreach (CustomMenu menu in menus)
			{
				PlaceHolder plc = this._templateControl.Containers[menu.Placeholder] as PlaceHolder;
				if (plc != null)
				{
					// rabol: [#CUY-57] fix.
					Control menuControlList = GetMenuControls(menu);
					if(menuControlList != null)
					{
						plc.Controls.Add(menuControlList);
					}
				}
			}
		}

		private Control GetMenuControls(CustomMenu menu)
		{
			if (menu.Nodes.Count > 0)
			{
				// The menu is just a simple <ul> list.
				HtmlGenericControl listControl = new HtmlGenericControl("ul");
				foreach (Node node in menu.Nodes)
				{
					if (node.ViewAllowed(this.CuyahogaUser))
					{
						HtmlGenericControl listItem = new HtmlGenericControl("li");
						HyperLink hpl = new HyperLink();
						hpl.NavigateUrl = UrlHelper.GetUrlFromNode(node);
						UrlHelper.SetHyperLinkTarget(hpl, node);
						hpl.Text = node.Title;
						listItem.Controls.Add(hpl);
						listControl.Controls.Add(listItem);
						if (node.Id == this.ActiveNode.Id)
						{
							hpl.CssClass = "selected";
						}
					}
				}
				return listControl;
			}
			else
			{
				return null;
			}
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
	}
}
