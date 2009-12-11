using System;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Service.SiteStructure;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;

namespace Cuyahoga.Modules.LanguageSwitcher
{
	/// <summary>
	/// The LanguageSwitcher module enables switching to other languages on the current site.
	/// This basically means that users can select a different root Node with a different 
	/// culture setting.
	/// </summary>
	public class LanguageSwitcherModule : ModuleBase
	{
		private INodeService _nodeService;
		private DisplayMode _displayMode = DisplayMode.Text;
		private bool _redirectToUserLanguage = false;

		/// <summary>
		/// The DisplayMode of the language switcher.
		/// </summary>
		public DisplayMode DisplayMode
		{
			get { return this._displayMode; }
		}

		/// <summary>
		/// Indicates if the module should check for the browser language and optionally redirect to it 
		/// when no specific page is requested.
		/// </summary>
		public bool RedirectToUserLanguage
		{
			get { return this._redirectToUserLanguage; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="nodeService">Injected INodeService instance</param>
		public LanguageSwitcherModule(INodeService nodeService)
		{
			this._nodeService = nodeService;
		}

		public override void ReadSectionSettings()
		{
			base.ReadSectionSettings();
			if (base.Section.Settings["DISPLAY_MODE"] != null)
			{
				this._displayMode = (DisplayMode)Enum.Parse(typeof(DisplayMode), base.Section.Settings["DISPLAY_MODE"].ToString());
			}
			if (base.Section.Settings["REDIRECT_TO_USER_LANGUAGE"] != null)
			{
				this._redirectToUserLanguage = Convert.ToBoolean(base.Section.Settings["REDIRECT_TO_USER_LANGUAGE"]);
			}
		}

		/// <summary>
		/// Get a dictionary of rootnodes for the current site with the culture as the key.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		public Dictionary<string, Node> GetCultureRootNodesBySite(Site site)
		{
			IList<Node> rootNodes = this._nodeService.GetRootNodes(site);
			Dictionary<string, Node> cultureNodes = new Dictionary<string, Node>(rootNodes.Count);
			foreach (Node node in rootNodes)
			{
				cultureNodes.Add(node.Culture, node);
			}
			return cultureNodes;
		}

		/// <summary>
		/// Get a root node for a give culture and site.
		/// </summary>
		/// <param name="culture"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		public Node GetRootNodeByCultureAndSite(string culture, Site site)
		{
			return this._nodeService.GetRootNodeByCultureAndSite(culture, site);
		}
	}

	/// <summary>
	/// The display mode of the module.
	/// </summary>
	public enum DisplayMode
	{
		Text,
		Flag,
		DropDown
	}
}
