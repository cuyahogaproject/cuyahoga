namespace Cuyahoga.Modules.YetAnotherForum
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Web.UI;
	using yaf;

	/// <summary>
	///		Summary description for CuyahogaYaf.
	/// </summary>
	public class CuyahogaYaf : BaseModuleControl
	{
		private CuyahogaYafModule _module;

		protected yaf.Forum Forum1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as CuyahogaYafModule;

			try 
			{
				Forum1.BoardID = Int32.Parse(this._module.Section.Settings["BOARDID"].ToString());

				if (this._module.Section.Settings["CATEGORYID"] != null
					|| this._module.Section.Settings["CATEGORYID"].ToString() != String.Empty)
				{
					int categoryID = Int32.Parse(this._module.Section.Settings["CATEGORYID"].ToString());
					Forum1.CategoryID = categoryID;
				}
			}
			catch(Exception)
			{
				Forum1.BoardID = 1;
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
