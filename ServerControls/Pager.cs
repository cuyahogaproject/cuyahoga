using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Reflection;

namespace Cuyahoga.ServerControls
{
	/// <summary>
	/// The Pager control enables paging WebControls that can bind to a DataSource.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:Pager runat=server></{0}:Pager>")]
	public class Pager : System.Web.UI.WebControls.WebControl
	{
		private PagedDataSource _pagedDataSource;
		private Control _controlToPage;
		private bool _cacheDataSource;
		private string[] _cacheParams;
		private int _cacheDuration;

		#region properties

		/// <summary>
		/// Property ControlToPage (Control)
		/// </summary>
		public Control ControlToPage
		{
			get { return this._controlToPage; }
			set { this._controlToPage = value; }
		}

		/// <summary>
		/// Property CacheDataSource (bool)
		/// </summary>
		public bool CacheDataSource
		{
			get { return this._cacheDataSource; }
			set { this._cacheDataSource = value; }
		}

		/// <summary>
		/// Property CacheParams (string)
		/// </summary>
		public string[] CacheParams
		{
			get { return this._cacheParams; }
			set { this._cacheParams = value; }
		}
		
		/// <summary>
		/// Property CacheDuration (int)
		/// </summary>
		public int CacheDuration
		{
			get { return this._cacheDuration; }
			set { this._cacheDuration = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int PageCount
		{
			get { return this._pagedDataSource.PageCount; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get { return this._pagedDataSource.Count; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int CurrentPageIndex
		{
			get 
			{
				if (ViewState["CurrentPageIndex"] != null)
					return Int32.Parse(ViewState["CurrentPageIndex"].ToString());
				else
					return 0;
			}
			set 
			{
				ViewState["CurrentPageIndex"] = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int PageSize
		{
			get { return this._pagedDataSource.PageSize; }
			set { this._pagedDataSource.PageSize = value; }
		}

		#endregion

		public event PageChangedEventHandler PageChanged;
            
		protected virtual void OnPageChanged(PageChangedEventArgs e)
		{
			if (PageChanged != null)
				PageChanged(this, e);
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Pager()
		{
			this._pagedDataSource = new PagedDataSource();
			this._pagedDataSource.AllowCustomPaging = true;
			this._pagedDataSource.AllowPaging = true;
			this._pagedDataSource.PageSize = 10;
			this._pagedDataSource.CurrentPageIndex = -1;
			this._cacheDataSource = false;
			this._cacheParams = null;
			this._cacheDuration = 30;
		}
		
		#region methods

		protected override void OnInit(EventArgs e)
		{
			if (this._controlToPage != null)
				this._controlToPage.DataBinding += new EventHandler(ControlToPage_DataBinding);
			else
				throw new NullReferenceException("The Control property may not be null because the pager must know which control to page.");
			base.OnInit (e);
		}

		protected override void Render(HtmlTextWriter writer)
		{
//			if (this.DesignMode)
//			{
//				writer.Write("1 2 3 ...");
//			}
			base.Render (writer);
		}

		#endregion

		private void ControlToPage_DataBinding(object sender, EventArgs e)
		{
			// Take the datasource and hand it over to the internal PagedDataSource.
			// We need a little reflection here
			PropertyInfo pi = this._controlToPage.GetType().GetProperty("DataSource");
			if (pi != null)
			{
				IEnumerable controlDataSource = (IEnumerable)pi.GetValue(this._controlToPage, null);
				// We don't have to do anything special when the datasource of the controlToPage already is 
				// a PagedDataSource
				if (! (controlDataSource is PagedDataSource))
				{
					this._pagedDataSource.DataSource = controlDataSource;
					this._pagedDataSource.CurrentPageIndex = this.CurrentPageIndex;
					controlDataSource = this._pagedDataSource;
					// Call databind again, but now with the pageddatasource attached.
					this._controlToPage.DataBind();
				}
			}
			else
				throw new InvalidOperationException("The ControlToPage doesn't have a DataSource property.");
		}
	}

	#region PageChangedEvent

	public class PageChangedEventArgs
	{
		private int _prevPage;
		private int _currentPage;

		public int PrevPage
		{
			get { return this._prevPage; }
		}

		public int CurrentPage
		{
			get { return this._currentPage; }
		}

		public PageChangedEventArgs(int prevPage, int currentPage)
		{
			this._prevPage = prevPage;
			this._currentPage = currentPage;
		}
	}

	public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);

	#endregion
}
