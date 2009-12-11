using System;
using Cuyahoga.Web.UI;
using yaf;

namespace Cuyahoga.Modules.YetAnotherForum
{
	/// <summary>
	///		Summary description for CuyahogaYaf.
	/// </summary>
	public class CuyahogaYaf : BaseModuleControl
	{
		private CuyahogaYafModule _module;

		protected Forum Forum1;

		private void Page_Load(object sender, EventArgs e)
		{
			this._module = this.Module as CuyahogaYafModule;

			try 
			{
				Forum1.BoardID = this._module.BoardId;

				if (this._module.CategoryId > -1)
				{
					Forum1.CategoryID = this._module.CategoryId;
				}
			}
			catch (Exception)
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
