namespace Cuyahoga.Web.Modules.Articles
{
	using System;
	// using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Web.UI;
	using Cuyahoga.Modules.Articles;

	/// <summary>
	///		Summary description for Articles.
	/// </summary>
	public class Articles : BaseModuleControl
	{
		protected System.Web.UI.WebControls.Panel pnlArticleDetails;
		protected System.Web.UI.WebControls.Repeater rptArticles;
		protected System.Web.UI.WebControls.Panel pnlArticleList;

		private void Page_Load(object sender, System.EventArgs e)
		{
			ArticleModule module = this.Module as ArticleModule;
			if (module != null && ! base.HasCachedOutput)
			{
				// Default to max 10 articles in the list.
				int numberOfArticles = 10;
				if (module.Section.Settings["NUMBER_OF_ARTICLES_IN_LIST"] != null)
				{
					numberOfArticles = Int32.Parse(module.Section.Settings["NUMBER_OF_ARTICLES_IN_LIST"].ToString());
				}
				this.rptArticles.ItemDataBound += new RepeaterItemEventHandler(rptArticles_ItemDataBound);
				this.rptArticles.DataSource = module.GetDisplayArticles(numberOfArticles);
				this.rptArticles.DataBind();
				this.pnlArticleList.Visible = true;
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

		private void rptArticles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{

		}
	}
}
