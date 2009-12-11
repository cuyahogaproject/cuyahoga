namespace Cuyahoga.Modules.Search
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;

	/// <summary>
	///		Summary description for SearchInput.
	/// </summary>
	public partial class SearchInput : BaseModuleControl
	{
		private SearchInputModule _module;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as SearchInputModule;

			if (! this.IsPostBack)
			{
				LocalizeControls();
			}

			// Register default button when enter key is pressed.
			DefaultButton.SetDefault(this.Page, this.txtSearchQuery, this.btnSearch);
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

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			if (this.txtSearchQuery.Text.Trim().Length > 0 && this._module.Section.Connections["Search"] != null)
			{
				Section sectionTo = this._module.Section.Connections["Search"] as Section;
				string query = Server.UrlEncode(this.txtSearchQuery.Text);
				Response.Redirect(UrlHelper.GetUrlFromSection(sectionTo) + "/Search?q=" + query);
			}
		}
	}
}
