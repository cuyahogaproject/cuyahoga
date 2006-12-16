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

namespace Cuyahoga.Modules.Gallery.UI
{
	/// <summary>
	/// The base user control for the forum user controls
	/// </summary>
	public class BaseGalleryControl : BaseModuleControl
	{
		protected override void OnLoad(EventArgs e)
		{
			// Register stylesheet
			string cssfile = ResolveUrl("~/Modules/Gallery/Css/gallery.css");
			RegisterStylesheet("gallerycss", cssfile);
		}
	}
}
