using System;
using System.Collections.Generic;
using System.Web;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Search;
using Cuyahoga.Core.Communication;

namespace Cuyahoga.Modules.Search
{
	/// <summary>
	/// The searchmodule provides search capabilities on the Lucene.NET search index.
	/// <seealso cref="Cuyahoga.Core.Search"/>
	/// </summary>
	public class SearchModule : ModuleBase, IActionConsumer
	{
		private int _resultsPerPage = 10;
		private bool _showInputPanel = true;
		private readonly ModuleActionCollection _inboundModuleActions;
		private ModuleAction _currentModuleAction;
		private string _searchQuery;
        private readonly ISearchService _searchService;
       

        #region Section Settings

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

        #endregion


        public ModuleAction CurrentModuleAction
        {
            get { return this._currentModuleAction; }
        }

        public string SearchQuery
        {
            get { return this._searchQuery; }
            set { this._searchQuery = value; }
        }

        /// <summary>
        /// Categories have to be stored in session to be able to later
        /// combine categories with a query string for search
        /// </summary>
        public IList<string> CategoryNames
        {
            get { return (IList<string>)HttpContext.Current.Session["Categories:Section:"+this.Section.Id.ToString()]; }
            set { HttpContext.Current.Session["Categories:Section:"+this.Section.Id.ToString()] = value; }
        }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SearchModule(ISearchService searchService)
		{
			this._searchService = searchService;
			// Init inbound actions
			this._inboundModuleActions = new ModuleActionCollection();
			this._inboundModuleActions.Add(new ModuleAction("Search", new string[0]));
			this._inboundModuleActions.Add(new ModuleAction("Category", new string[0]));
			this._inboundModuleActions.Add(new ModuleAction("AlphabeticIndex", new string[0]));
			this._currentModuleAction = this._inboundModuleActions[0];
		}

        #region Overrides

        protected override void ParsePathInfo()
        {
            base.ParsePathInfo();
            if (base.ModuleParams != null)
            {
                if (base.ModuleParams.Length == 1)
                {
                    // First argument is the module action
                    this._currentModuleAction = this._inboundModuleActions.FindByName(base.ModuleParams[0]);
                    if (this._currentModuleAction != null)
                    {
                        if (this._currentModuleAction.Name == "Search")
                        {
                            this._searchQuery = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["q"]);
                        }
                        else if (this._currentModuleAction.Name == "Category")
                        {
                            string categoryNames = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["c"]);
                            this.CategoryNames = new List<string>(categoryNames.Split(','));
							//reset searchquery (following queries can still be combined with catgories)
							this._searchQuery = string.Empty;
                        }
                        else if (this._currentModuleAction.Name == "AlphabeticIndex")
                        {
							string letter = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["a"]);
							this._searchQuery = string.Format("title:{0}*", letter );
                        }

                    }
                    else
                    {
                        throw new Exception("Error when parsing module action: " + base.ModuleParams[0]);
                    }
                }
            }
        }

        public override void ReadSectionSettings()
        {
            base.ReadSectionSettings();
            if (base.Section.Settings["RESULTS_PER_PAGE"] != null)
            {
                this._resultsPerPage = Convert.ToInt32(base.Section.Settings["RESULTS_PER_PAGE"]);
            }
            if (base.Section.Settings["SHOW_INPUT_PANEL"] != null)
            {
                this._showInputPanel = Convert.ToBoolean(base.Section.Settings["SHOW_INPUT_PANEL"]);
            }
        }

        #endregion


        /// <summary>
        /// Get paged search results.
        /// </summary>
        /// <param name="queryText">The query to search for.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public SearchResultCollection GetSearchResults(string queryText, int pageIndex, int pageSize)
        {
            if (this.CategoryNames != null)
            {
                return this._searchService.FindContent(queryText, CategoryNames, pageIndex, pageSize);
            }
            else
            {
                return this._searchService.FindContent(queryText, pageIndex, pageSize);
            }
        }

        /// <summary>
        /// Get all search results with a maximum of 200.
        /// </summary>
        /// <param name="queryText"></param>
        public SearchResultCollection GetSearchResults(string queryText)
        {
            return GetSearchResults(queryText, 0, 200);
        }

		#region IActionConsumer Members

		public ModuleActionCollection GetInboundActions()
		{
			return this._inboundModuleActions;
		}

		#endregion
	}
}
