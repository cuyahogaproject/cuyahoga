namespace Cuyahoga.Modules.Search
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Search;
	using Cuyahoga.Core.Util;
	using Cuyahoga.Web.UI;
	using Cuyahoga.ServerControls;

	/// <summary>
	///		Summary description for Search.
	/// </summary>
	public class Search : BaseModuleControl
	{
		private const int RESULTS_PER_PAGE = 10;
		private string _indexDir;
		private SearchModule _module;

		protected System.Web.UI.WebControls.Panel pnlCriteria;
		protected System.Web.UI.WebControls.Panel pnlResults;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.WebControls.Label lblFrom;
		protected System.Web.UI.WebControls.Label lblTo;
		protected System.Web.UI.WebControls.Label lblQueryText;
		protected System.Web.UI.WebControls.Label lblDuration;
		protected System.Web.UI.WebControls.Panel pnlNotFound;
		protected System.Web.UI.WebControls.Label lblTotal;
		protected System.Web.UI.WebControls.Repeater rptResults;
		protected Cuyahoga.ServerControls.Pager pgrResults;
		protected System.Web.UI.WebControls.Button btnSearch;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			this._module = this.Module as SearchModule;
			this._indexDir = Context.Server.MapPath(Config.GetConfiguration()["SearchIndexDir"]);
			this.pgrResults.PageSize = RESULTS_PER_PAGE;
			this.pgrResults.AllowCustomPaging = true;
		}

		private void BindSearchResults(int pageIndex)
		{
			SearchResultCollection results = this._module.GetSearchResults(this.txtSearchText.Text, pageIndex, RESULTS_PER_PAGE, this._indexDir);
			if (results.Count > 0)
			{
				int start = pageIndex * RESULTS_PER_PAGE;
				int end = start + RESULTS_PER_PAGE;
				if (end > results.TotalCount)
				{
					end = results.TotalCount;
				}
				this.pnlResults.Visible = true;
				this.pnlNotFound.Visible = false;

				this.lblFrom.Text = (start + 1).ToString();
				this.lblTo.Text = end.ToString();
				this.lblTotal.Text = results.TotalCount.ToString();
				this.lblQueryText.Text = this.txtSearchText.Text;
				float duration = results.ExecutionTime * 0.0000001F;
				this.lblDuration.Text = duration.ToString();
				this.pgrResults.VirtualItemCount = results.TotalCount;
				this.rptResults.DataSource = results;
				this.rptResults.DataBind();
			}
			else
			{
				this.pnlResults.Visible = false;
				this.pnlNotFound.Visible = true;
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
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.pgrResults.PageChanged += new Cuyahoga.ServerControls.PageChangedEventHandler(this.pgrResults_PageChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			if (this.txtSearchText.Text.Trim() != String.Empty)
			{
				BindSearchResults(0);
			}
		}

		private void pgrResults_PageChanged(object sender, Cuyahoga.ServerControls.PageChangedEventArgs e)
		{
			this.pgrResults.CurrentPageIndex = e.CurrentPage;
			BindSearchResults(this.pgrResults.CurrentPageIndex);
		}
	}
}
