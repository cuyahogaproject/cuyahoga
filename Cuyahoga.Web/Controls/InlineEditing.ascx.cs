using System;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Controls
{
	public partial class InlineEditing : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			// Register jQuery UI scripts for edit dialog
			if (this.Page is PageEngine)
			{
				var cuyahogaPage = (PageEngine) this.Page;
				cuyahogaPage.RegisterJavaScript("jquery", ResolveUrl("~/Support/jquery/jquery-1.3.2.min.js"));
				cuyahogaPage.RegisterJavaScript("ui.core", ResolveUrl("~/manager/Scripts/ui.core.js"));
				cuyahogaPage.RegisterJavaScript("ui.dialog", ResolveUrl("~/manager/Scripts/ui.dialog.js"));
				cuyahogaPage.RegisterStylesheet("ui.css", ResolveUrl("~/Manager/Content/Css/jquery-ui/smoothness/jquery-ui-1.7.2.cuyahoga.css"));
			}
		}
	}
}