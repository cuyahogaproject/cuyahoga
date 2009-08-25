using System;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Service.Search;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.Search
{
    public partial class Search : BaseModuleControl<SearchModule>
    {

		protected void Page_Load(object sender, EventArgs e)
		{
			this.pgrResults.PageSize = this.Module.ResultsPerPage;
			this.pnlCriteria.Visible = this.Module.ShowInputPanel;

			if (! this.IsPostBack)
			{
				if (this.Module.SearchQuery != null)
   				{
   					BindSearchResults(this.Module.SearchQuery, 0);
   					this.txtSearchText.Text = this.Module.SearchQuery;
   				}
			}

			// Register default button when enter key is pressed.
			DefaultButton.SetDefault(this.Page, this.txtSearchText, this.btnSearch);
		}

		private void BindSearchResults(string queryString, int pageIndex)
		{
            SearchResultCollection results = this.Module.GetSearchResults(queryString);
            if (results.Count > 0)
            {
                int start = pageIndex * this.Module.ResultsPerPage;
                int end = start + this.Module.ResultsPerPage;
                if (end > results.Count)
                {
                    end = results.Count;
                }
                this.pnlResults.Visible = true;
                this.pnlNotFound.Visible = false;

                this.lblFrom.Text = (start + 1).ToString();
                this.lblTo.Text = end.ToString();
                this.lblTotal.Text = results.Count.ToString();
				if (!String.IsNullOrEmpty(this.Module.SearchQuery))
				{
					this.litFor.Text = base.GetText("FOR");
					this.lblQueryText.Text = this.Module.SearchQuery; // != null ? this._module.SearchQuery : this.txtSearchText.Text;
				}
				if (this.Module.CategoryNames != null && this.Module.CategoryNames.Count > 0)
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					foreach (string s in this.Module.CategoryNames)
					{
						sb.Append(s); sb.Append(", ");
					}
					this.lblFilter.Text = sb.ToString().TrimEnd(',', ' ');
				}
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
            else if (this.Module.CategoryNames != null)
            {
                BindSearchResults(string.Empty, 0);
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

		protected void lnkBtnRemoveFilter_Click(object sender, EventArgs e)
		{
			if (this.Module.CategoryNames != null)
			{
				this.Module.CategoryNames.Clear();
				this.Module.CategoryNames = null;
			}
			this.lblFilter.Text = "";
			this.lnkBtnRemoveFilter.Visible = false;
		}
	

    }
}