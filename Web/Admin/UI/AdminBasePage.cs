using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Configuration;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Admin.UI
{
	/// <summary>
	/// The base class for the Cuyahoga admin pages. Provides generic functionality.
	/// </summary>
	public class AdminBasePage : GenericBasePage
	{
		private Site _activeSite;
		private Node _activeNode;
		private Section _activeSection;

		/// <summary>
		/// The Site context of the admin page.
		/// </summary>
		public Site ActiveSite
		{
			get { return this._activeSite; }
		}

		/// <summary>
		/// The Node context of the admin page.
		/// </summary>
		public Node ActiveNode
		{
			get { return this._activeNode; }
			set { this._activeNode = value; }
		}

		/// <summary>
		/// The Node context of the admin page.
		/// </summary>
		public Section ActiveSection
		{
			get { return this._activeSection; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public AdminBasePage()
		{
			this._activeNode = null;
		}

		protected override void OnInit(EventArgs e)
		{
			// Set template dir, template filename and css.
			base.TemplateDir = ConfigurationSettings.AppSettings["TemplateDir"];
			base.TemplateFilename = ConfigurationSettings.AppSettings["DefaultTemplate"];
			base.Css = ConfigurationSettings.AppSettings["DefaultCss"];

			// Try to set active Site, Node and Section.
			if (Context.Request.QueryString["SectionId"] != null)
			{
				int sectionId = Int32.Parse(Context.Request.QueryString["SectionId"]);
				if (sectionId > 0)
				{
					this._activeSection = (Section)base.CoreRepository.GetObjectById(typeof(Section), sectionId);
					if (this._activeSection.Node != null)
					{
						this._activeNode = this._activeSection.Node;
						this._activeSite = this._activeNode.Site;
					}
				}
			}
			if (Context.Request.QueryString["NodeId"] != null && this._activeNode == null)
			{
				int nodeId = Int32.Parse(Context.Request.QueryString["NodeId"]);
				if (nodeId > 0)
				{
					this._activeNode = (Node)base.CoreRepository.GetObjectById(typeof(Node), nodeId);
					this._activeSite = this._activeNode.Site;
				}
			}
			if (Context.Request.QueryString["SiteId"] != null && this._activeSite == null)
			{
				int siteId = Int32.Parse(Context.Request.QueryString["SiteId"]);
				if (siteId > 0)
				{
					this._activeSite = (Site)base.CoreRepository.GetObjectById(typeof(Site), siteId);
				}
			}

			// Now, we're here we could check authorization as well.
			if (! ((User)this.User.Identity).HasPermission(AccessLevel.Administrator))
			{
				throw new ActionForbiddenException("The logged-in user has insufficient rights to access the site administration");
			}

			base.OnInit (e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			string message = Request.QueryString["message"];
			if (message != null)
			{
				ShowMessage(message);
			}
		}
	}
}
