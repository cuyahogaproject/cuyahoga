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
			//this.pgrResults.AllowCustomPaging = true;
		}

		private void BindSearchResults(int pageIndex)
		{
			SearchResultCollection results = this._module.GetSearchResults(this.txtSearchText.Text, this._indexDir);
			SearchResultCollection filteredResults = FilterResults(results);
			if (filteredResults.Count > 0)
			{
				int start = pageIndex * RESULTS_PER_PAGE;
				int end = start + RESULTS_PER_PAGE;
				if (end > filteredResults.Count)
				{
					end = filteredResults.Count;
				}
				this.pnlResults.Visible = true;
				this.pnlNotFound.Visible = false;

				this.lblFrom.Text = (start + 1).ToString();
				this.lblTo.Text = end.ToString();
				this.lblTotal.Text = filteredResults.Count.ToString();
				this.lblQueryText.Text = this.txtSearchText.Text;
				float duration = results.ExecutionTime * 0.0000001F;
				this.lblDuration.Text = duration.ToString();
				//this.pgrResults.VirtualItemCount = results.TotalCount;
				this.rptResults.DataSource = filteredResults;
				this.rptResults.DataBind();
			}
			else
			{
				this.pnlResults.Visible = false;
				this.pnlNotFound.Visible = true;
			}
		}

		/// <summary>
		/// A searchresult contains a SectionId propery that indicates to which section the 
		/// result belongs. We need to get a real Section to determine if the current user 
		/// has view access to that Section.
		/// </summary>
		/// <param name="nonFilteredResults"></param>
		/// <returns></returns>
		private SearchResultCollection FilterResults(SearchResultCollection nonFilteredResults)
		{
			SearchResultCollection filteredResults = new SearchResultCollection();

			CoreRepository cr = HttpContext.Current.Items["CoreRepository"] as CoreRepository;
			if (cr != null)
			{
				foreach (SearchResult result in nonFilteredResults)
				{
					Section section = (Section)cr.GetObjectById(typeof(Section), result.SectionId);
					if (section.ViewAllowed(this.Page.User.Identity))
					{
						filteredResults.Add(result);
					}
				}
			}

			return filteredResults;
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
			this.rptResults.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptResults_ItemDataBound);
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

		private void rptResults_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			SearchResult sr = e.Item.DataItem as SearchResult;
			Literal litDateCreated = e.Item.FindControl("litDateCreated") as Literal;
			litDateCreated.Text = TimeZoneUtil.AdjustDateToUserTimeZone(sr.DateCreated, this.Page.User.Identity).ToString();
		}
	}
}
