using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Utility class that handles the ASP.NET default button issue.
	/// Originally created by Janus Kamp Hansen - http://www.kamp-hansen.dk
	/// Extended by Darrell Norton - http://dotnetjunkies.com/weblog/darrell.norton/ 
	/// Tidied by Martijn Boland - http://www.cuyahoga-project.org
	/// </summary>
	public class DefaultButton
	{
		private DefaultButton()
		{
		}

		/// <summary>
		/// Sets the Button you want to submit when the Enter key is pressed within a TextBox.
		/// </summary>
		/// <param name="thisPage"></param>
		/// <param name="textControl"></param>
		/// <param name="defaultButton"></param>
		public static void SetDefault(Page thisPage, TextBox textControl, WebControl defaultButton)
		{
			textControl.Attributes.Add("onkeydown", "fnTrapKD('" + defaultButton.ClientID + "', event)");
			string scriptSrc = String.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", thisPage.ResolveUrl("~/js/DefaultButton.js"));
			thisPage.RegisterClientScriptBlock("DefaultButtonScript", scriptSrc);
		}
	}
}
