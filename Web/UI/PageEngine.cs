using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

using Cuyahoga.Core;
using Cuyahoga.Core.Collections;
using Cuyahoga.Core.DAL;
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

		#region properties

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

		public User CuyahogaUser
		{
			get	{ return this.User.Identity as User; }
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
		}

		/// <summary>
		/// Load the content and the template as early as possible, so everything is in place before 
		/// modules handle their own events. We could do this by overriding the OnInit method but 
		/// that gives unpredictable results with Mono (0.30).
		/// </summary>
		/// <param name="obj"></param>
		protected override void AddParsedSubObject(object obj)
		{
//			try
//			{
				// Get an instance of the CacheManager
				CacheManager cm = new CacheManager();
				// Add the CacheManager to the current context, so it can track all objects during the page lifecycle.
				Context.Items.Add("CacheManager", cm);
				// Get root node from the CacheManager.
				this._rootNode = cm.GetRootNode();

				// Load the active node
				if (Context.Request.QueryString["ShortDescription"] != null)
				{
					this._activeNode = cm.GetNodeByShortDescription(Context.Request.QueryString["ShortDescription"]);
				}
				else if (Context.Request.QueryString["NodeId"] != null)
				{
					this._activeNode = cm.GetNodeById(Int32.Parse(Context.Request.QueryString["NodeId"]));
				}
				else
				{
					// Can't load a particular node, so the root node has to be the active node
					this._activeNode = this._rootNode;
				}
				
				// Load template and usercontrols

				//string appRoot = this.ResolveUrl("~/");
				string appRoot = UrlHelper.GetApplicationPath();
				// We know the active node so the template can be loaded
				string templatePath = appRoot + this._activeNode.Template.Path;
				this._templateControl = (BaseTemplate)this.LoadControl(templatePath);
				// Explicitly set the id to 'p' to save some bytes (otherwise _ctl0 would be added).
				this._templateControl.ID = "p";
				this._templateControl.Title = this._activeNode.Title;
				this._templateControl.Css = appRoot + Config.GetConfiguration()["Css"];

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
						if (section.Id == sectionId)
						{
							this._activeSection = section;
						}
						if (section.Module != null)
						{
							BaseModuleControl ctrl = (BaseModuleControl)this.LoadControl(appRoot + section.Module.Path);
							ctrl.Module = section.Module;
							((PlaceHolder)this._templateControl.Containers[section.PlaceholderId]).Controls.Add(ctrl);

							if (Context.Request.PathInfo.Length > 0 && section == this._activeSection)
							{
								// Parse the PathInfo of the request because they are the parameters 
								// of the module that is connected to the active section.
								section.Module.ModuleParams = UrlHelper.GetModuleParamsFromPathInfo(Context.Request.PathInfo);
							}
						}
					}
				}
				this.Controls.AddAt(0, this._templateControl);
				// remove html that was in the original page (Default.aspx)
				for (int i = this.Controls.Count -1; i < 0; i --)
					this.Controls.RemoveAt(i);
//			}
//			catch (Exception ex)
//			{
//				Context.Response.Write("<h1>An error occured:</h1>" + ex.Message);
//			}
			base.AddParsedSubObject (obj);
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
				cm.SaveCache();

			// Ready, write the execution time to the debug output.
			TimeSpan ts = DateTime.Now - (DateTime)Context.Items["starttime"];
			Debug.WriteLine("Total execution time : " + ts.Milliseconds.ToString() + " milliseconds. \n");
			base.OnUnload (e);
		}
	}
}
