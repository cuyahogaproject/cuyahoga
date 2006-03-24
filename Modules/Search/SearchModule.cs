using System;
using System.Collections;
using System.Web;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Communication;

namespace Cuyahoga.Modules.Search
{
	/// <summary>
	/// The searchmodule provides search capabilities on the DotLucene search index.
	/// <seealso cref="Cuyahoga.Core.Search"/>
	/// </summary>
	public class SearchModule : ModuleBase, IActionConsumer
	{
		private int _resultsPerPage = 10;
		private bool _showInputPanel = true;
		private ActionCollection _inboundActions;
		private Action _currentAction;
		private string _searchQuery;

		/// <summary>
		/// The number of results to show at one page. Default is 10.
		/// </summary>
		public int ResultsPerPage
		{
			get { return this._resultsPerPage; }
		}

		/// <summary>
		/// Show the search input panel?
		/// </summary>
		public bool ShowInputPanel
		{
			get { return this._showInputPanel; }
		}

		/// <summary>
		/// 
		/// </summary>
		public Action CurrentAction
		{
			get { return this._currentAction; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string SearchQuery
		{
			get { return this._searchQuery; }
			set { this._searchQuery = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SearchModule()
		{
			// Init inbound actions
			this._inboundActions = new ActionCollection();
			this._inboundActions.Add(new Action("Search", new string[0]));
			this._currentAction = this._inboundActions[0];
		}

		public override void ReadSectionSettings()
		{
			base.ReadSectionSettings ();
			if (base.Section.Settings["RESULTS_PER_PAGE"] != null)
			{
				this._resultsPerPage = Convert.ToInt32(base.Section.Settings["RESULTS_PER_PAGE"]);
			}
			if (base.Section.Settings["SHOW_INPUT_PANEL"] != null)
			{
				this._showInputPanel = Convert.ToBoolean(base.Section.Settings["SHOW_INPUT_PANEL"]);
			}
		}


		/// <summary>
		/// Get paged search results.
		/// </summary>
		/// <param name="queryText">The query to search for.</param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="indexDir"></param>
		/// <returns></returns>
		public SearchResultCollection GetSearchResults(string queryText, int pageIndex, int pageSize, string indexDir)
		{
			IndexQuery query = new IndexQuery(indexDir);
			Hashtable keywordFilter = new Hashtable();
			keywordFilter.Add("site", this.Section.Node.Site.Name);
			SearchResultCollection nonFilteredResults = query.Find(queryText, keywordFilter, pageIndex, pageSize);
			// Filter results where the current user doesn't have access to.
			return FilterResults(nonFilteredResults);
		}

		/// <summary>
		/// Get all search results with a maximum of 200.
		/// </summary>
		/// <param name="queryText"></param>
		/// <param name="indexDir"></param>
		/// <returns></returns>
		public SearchResultCollection GetSearchResults(string queryText, string indexDir)
		{
			return GetSearchResults(queryText, 0, 200, indexDir);
		}

		protected override void ParsePathInfo()
		{
			base.ParsePathInfo ();
			if (base.ModuleParams != null)
			{
				if (base.ModuleParams.Length == 1)
				{
					// First argument is the module action
					this._currentAction = this._inboundActions.FindByName(base.ModuleParams[0]);
					if (this._currentAction != null)
					{
						if (this._currentAction.Name == "Search")
						{
							this._searchQuery = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["q"]);
						}
					}
					else
					{
						throw new Exception("Error when parsing module action: " + base.ModuleParams[0]);
					}
				}
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
					if (section.ViewAllowed(HttpContext.Current.User.Identity))
					{
						filteredResults.Add(result);
					}
				}
			}

			return filteredResults;
		}


		#region IActionConsumer Members

		public ActionCollection GetInboundActions()
		{
			return this._inboundActions;
		}

		#endregion
	}
}
