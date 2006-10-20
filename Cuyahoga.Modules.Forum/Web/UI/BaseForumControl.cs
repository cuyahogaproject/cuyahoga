using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.Forum.Web.UI
{
	/// <summary>
	/// The base user control for the forum user controls
	/// </summary>
	public class BaseForumControl : BaseModuleControl
	{
		private ForumModule _forumModule;

		/// <summary>
		/// The module controller.
		/// </summary>
		protected ForumModule ForumModule
		{
			get { return this._forumModule; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			// Cast module controller.
			this._forumModule = base.Module as ForumModule;

			// Register stylesheet
			string cssfile = ForumModule.ThemePath + "forum.css";
			RegisterStylesheet("forumcss", cssfile);

			base.OnLoad(e);
		}
	}
}
