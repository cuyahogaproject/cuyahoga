using System;
using System.Web.UI.WebControls;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.StaticHtml
{
	/// <summary>
	///		Summary description for StaticHtml.
	/// </summary>
	public partial class StaticHtml : BaseModuleControl<StaticHtmlModule>
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Module != null && ! base.HasCachedOutput)
			{
				Literal htmlControl = new Literal();
				StaticHtmlContent shc = Module.GetContent();
				if (shc != null)
				{
					htmlControl.Text = shc.Content;
				}
				else
				{
					htmlControl.Text = String.Empty;
				}
				this.plcContent.Controls.Add(htmlControl);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
