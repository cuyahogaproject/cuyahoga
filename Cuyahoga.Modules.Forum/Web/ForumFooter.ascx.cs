using System;
using System.Collections;
using System.Web.UI.WebControls;

using Cuyahoga.Web.UI;
using Cuyahoga.Modules.Forum;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	///		Summary description for Links.
	/// </summary>
	public class ForumFooter : LocalizedUserControl
	{
		protected System.Web.UI.WebControls.Panel pnlFooter;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label lblPoweredby;
		private ForumModule _module;

		#region Properties
		
		public ForumModule Module
		{
			get { return this._module; }
			set { this._module = value; }
		}
		
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
