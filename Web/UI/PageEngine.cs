using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Collections;
using System.Web.Security;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Web.Components;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Page engine. This class loads all content based on url parameters and merges
	/// the content with the template.
	/// </summary>
	public class PageEngine : CuyahogaPage
	{
		private Site _currentSite;
		private Node _rootNode;
		private Node _activeNode;
		private Section _activeSection;
		private BaseTemplate _templateControl;
		private bool _shouldLoadContent;
		private IDictionary _stylesheets;
		private IDictionary _metaTags;

		private ModuleLoader _moduleLoader;
		private INodeService _nodeService;
		private ISiteService _siteService;
		private ISectionService _sectionService;

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
			this._stylesheets = new Hashtable();
			this._metaTags = new Hashtable();

			// Get services from the container. Ideally, it should be possible to register the aspx page in the container
			// to automatically resolve dependencies but there were memory issues with registering pages in the container.
			this._moduleLoader = Container.Resolve<ModuleLoader>();
			this._nodeService = Container.Resolve<INodeService>();
			this._siteService = Container.Resolve<ISiteService>();
			this._sectionService = Container.Resolve<ISectionService>();
		}

		/// <summary>
		/// Register stylesheets.
		/// </summary>
		/// <param name="key">The unique key for the stylesheet. Note that Cuyahoga already uses 'maincss' as key.</param>
		/// <param name="absoluteCssPath">The path to the css file from the application root (starting with /).</param>
		public void RegisterStylesheet(string key, string absoluteCssPath)
		{
			if (this._stylesheets[key] == null)
			{
				this._stylesheets.Add(key, absoluteCssPath);
			}
		}

		/// <summary>
		/// Register a meta tag. The values can be overriden.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="content"></param>
		public void RegisterMetaTag(string name, string content)
		{
			if (content != null && content.Length > 0)
			{
				this._metaTags[name] = content;
			}
		}

		/// <summary>
		/// Load the content and the template as early as possible, so everything is in place before 
		/// modules handle their own ASP.NET lifecycle events.
		/// </summary>
		/// <param name="obj"></param>
		protected override void OnInit(EventArgs e)
		{		
			// Load the current site
			Node entryNode = null;
			string siteUrl = UrlHelper.GetSiteUrl();
			SiteAlias currentSiteAlias = this._siteService.GetSiteAliasByUrl(siteUrl);
			if (currentSiteAlias != null)
			{
				this._currentSite = currentSiteAlias.Site;
				entryNode = currentSiteAlias.EntryNode;
			}
			else
			{
				this._currentSite = this._siteService.GetSiteBySiteUrl(siteUrl);
			}
			if (this._currentSite == null)
			{
				throw new SiteNullException("No site found at " + siteUrl);
			}

			// Load the active node
			// Query the cache by SectionId, ShortDescription and NodeId.
			if (Context.Request.QueryString["SectionId"] != null)
			{
				try
				{
					this._activeSection = this._sectionService.GetSectionById(Int32.Parse(Context.Request.QueryString["SectionId"]));
					this._activeNode = this._activeSection.Node;
				}
				catch
				{
					throw new SectionNullException("Section not found: " + Context.Request.QueryString["SectionId"]);
				}
			}
			else if (Context.Request.QueryString["ShortDescription"] != null)
			{
				this._activeNode = this._nodeService.GetNodeByShortDescriptionAndSite(Context.Request.QueryString["ShortDescription"], this._currentSite);
			}
			else if (Context.Request.QueryString["NodeId"] != null)
			{
				this._activeNode = this._nodeService.GetNodeById(Int32.Parse(Context.Request.QueryString["NodeId"]));
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
				this._activeNode = this._nodeService.GetRootNodeByCultureAndSite(currentCulture, this._currentSite);
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
			InsertStylesheets();
			InsertMetaTags();

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
				// Register stylesheet that belongs to the template.
				RegisterStylesheet("maincss", appRoot + this._activeNode.Template.BasePath + "/Css/" + this._activeNode.Template.Css);
				//Register the metatags
				if (ActiveNode.MetaKeywords != null)
				{
					RegisterMetaTag("keywords", ActiveNode.MetaKeywords);
				}
				else
				{
					RegisterMetaTag("keywords", ActiveNode.Site.MetaKeywords);
				}
				if (ActiveNode.MetaDescription != null)
				{
					RegisterMetaTag("description", ActiveNode.MetaDescription);
				}
				else
				{
					RegisterMetaTag("description", ActiveNode.Site.MetaDescription);
				}
				// Load sections that are related to the template
				foreach (DictionaryEntry sectionEntry in this.ActiveNode.Template.Sections)
				{
					string placeholder = sectionEntry.Key.ToString();
					Section section = sectionEntry.Value as Section;
					if (section != null)
					{
						BaseModuleControl moduleControl = CreateModuleControlForSection(section);
						if (moduleControl != null)
						{
							((PlaceHolder)this._templateControl.Containers[placeholder]).Controls.Add(moduleControl);
						}
					}
				}
			}
			else
			{
				throw new Exception("No template associated with the current Node.");
			}

			// ===== Load sections and modules =====
			foreach (Section section in this._activeNode.Sections)
			{
				BaseModuleControl moduleControl = CreateModuleControlForSection(section);
				if (moduleControl != null)
				{
					((PlaceHolder)this._templateControl.Containers[section.PlaceholderId]).Controls.Add(moduleControl);
				}
			}

			this.Controls.AddAt(0, this._templateControl);
			// remove html that was in the original page (Default.aspx)
			for (int i = this.Controls.Count -1; i < 0; i --)
				this.Controls.RemoveAt(i);
		}

		private BaseModuleControl CreateModuleControlForSection(Section section)
		{
			// Check view permissions before adding the section to the page.
			if (section.ViewAllowed(this.User.Identity))
			{
				// Create the module that is connected to the section.
				ModuleBase module = this._moduleLoader.GetModuleFromSection(section);
				//this._moduleLoader.NHibernateModuleAdded -= new EventHandler(ModuleLoader_ModuleAdded);

				if (module != null)
				{
					if (Context.Request.PathInfo.Length > 0 && section == this._activeSection)
					{
						// Parse the PathInfo of the request because they can be the parameters 
						// for the module that is connected to the active section.
						module.ModulePathInfo = Context.Request.PathInfo;
					}
					return LoadModuleControl(module);
				}
			}
			return null;
		}

		private BaseModuleControl LoadModuleControl(ModuleBase module)
		{
			BaseModuleControl ctrl = (BaseModuleControl)this.LoadControl(UrlHelper.GetApplicationPath() + module.CurrentViewControlPath);
			ctrl.Module = module;
			return ctrl;
		}

		private void LoadMenus()
		{
			IList menus = this._nodeService.GetMenusByRootNode(this._rootNode);
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

		private void InsertStylesheets()
		{
			string[] stylesheetLinks = new string[this._stylesheets.Count];
			int i = 0;
			foreach (string stylesheet in this._stylesheets.Values)
			{
				stylesheetLinks[i] = stylesheet;
				i++;
			}
			this.TemplateControl.RenderCssLinks(stylesheetLinks);
		}

		private void InsertMetaTags()
		{
			this.TemplateControl.RenderMetaTags(this._metaTags);
		}
	}
}
