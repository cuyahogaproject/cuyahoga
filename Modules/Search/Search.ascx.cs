namespace Cuyahoga.Modules.Search
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Core.Search;
	using Cuyahoga.Core.Service;
	using Cuyahoga.Core.Util;
	using Cuyahoga.Web.UI;
	using Cuyahoga.ServerControls;

	/// <summary>
	///		Summary description for Search.
	/// </summary>
	public partial class Search : BaseModuleControl
	{
		private string _indexDir;
		private SearchModule _module;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as SearchModule;
			this._indexDir = Context.Server.MapPath(Config.GetConfiguration()["SearchIndexDir"]);
			this.pgrResults.PageSize = this._module.ResultsPerPage;
			this.pnlCriteria.Visible = this._module.ShowInputPanel;
			
			if (! this.IsPostBack)
			{
				if (this._module.SearchQuery != null)
				{
					BindSearchResults(this._module.SearchQuery, 0);
					this.txtSearchText.Text = this._module.SearchQuery;
				}
				LocalizeControls();
			}

			// Register default button when enter key is pressed.
			DefaultButton.SetDefault(this.Page, this.txtSearchText, this.btnSearch);
		}

		private void BindSearchResults(string queryString, int pageIndex)
		{
			SearchResultCollection results = this._module.GetSearchResults(queryString, this._indexDir);
			if (results.Count > 0)
			{
				int start = pageIndex * this._module.ResultsPerPage;
				int end = start + this._module.ResultsPerPage;
				if (end > results.Count)
				{
					end = results.Count;
				}
				this.pnlResults.Visible = true;
				this.pnlNotFound.Visible = false;

				this.lblFrom.Text = (start + 1).ToString();
				this.lblTo.Text = end.ToString();
				this.lblTotal.Text = results.Count.ToString();
				this.lblQueryText.Text = this._module.SearchQuery != null ? this._module.SearchQuery : this.txtSearchText.Text;
				float duration = results.ExecutionTime * 0.0000001F;
				this.lblDuration.Text = duration.ToString();
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
			this.rptResults.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptResults_ItemDataBound);
			this.pgrResults.PageChanged += new Cuyahoga.ServerControls.PageChangedEventHandler(this.pgrResults_PageChanged);

		}
		#endregion

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			if (this.txtSearchText.Text.Trim() != String.Empty)
			{
				BindSearchResults(this.txtSearchText.Text, 0);
			}
		}

		private void pgrResults_PageChanged(object sender, Cuyahoga.ServerControls.PageChangedEventArgs e)
		{
			this.pgrResults.CurrentPageIndex = e.CurrentPage;
			BindSearchResults(this.txtSearchText.Text, this.pgrResults.CurrentPageIndex);
		}

		private void rptResults_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			SearchResult sr = e.Item.DataItem as SearchResult;
			Literal litDateCreated = e.Item.FindControl("litDateCreated") as Literal;
			litDateCreated.Text = TimeZoneUtil.AdjustDateToUserTimeZone(sr.DateCreated, this.Page.User.Identity).ToString();
		}
	}
}
