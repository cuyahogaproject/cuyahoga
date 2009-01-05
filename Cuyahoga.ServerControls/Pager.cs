using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Cuyahoga.ServerControls
{
	/// <summary>
	/// The Pager control enables paging WebControls that can bind to a DataSource.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:Pager runat=server></{0}:Pager>")]
	public class Pager : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private const int _defaultPageSize = 10;
		private const int _defaultMaxDisplayPages = 10;
		private const int _defaultCacheDuration = 30;

		private PagedDataSource _pagedDataSource;
		private Control _controlToPage;
		private int _cacheDuration;

		#region properties

		/// <summary>
		/// Property ControlToPage (String)
		/// </summary>
		public string ControlToPage
		{
			get 
			{
				if (ViewState["ControlToPage"] != null)
					return ViewState["ControlToPage"].ToString();
				else
					return String.Empty;
			}
			set { ViewState["ControlToPage"] = value; }
		}

		/// <summary>
		/// Property CacheDataSource (bool)
		/// </summary>
		[DefaultValue(false)]
		public bool CacheDataSource
		{
			get 
			{
				if (ViewState["CacheDataSource"] != null)
					return (bool)ViewState["CacheDataSource"];
				else
					return false;
			}
			set { ViewState["CacheDataSource"] = value; }
		}

		/// <summary>
		/// Property CacheParams (string)
		/// </summary>
		[TypeConverterAttribute(typeof(ParamsConverter))]
		public string[] CacheVaryByParams
		{
			get 
			{ 
				if (ViewState["CacheVaryByParams"] != null)
					return (string[])ViewState["CacheVaryByParams"];
				else
					return null;
			}
			set { ViewState["CacheVaryByParams"] = value; }
		}
		
		/// <summary>
		/// Property CacheDuration (int)
		/// </summary>
		[DefaultValue(_defaultCacheDuration)]
		public int CacheDuration
		{
			get 
			{
				if (ViewState["CacheDuration"] != null)
					return (int)ViewState["CacheDuration"];
				else
					return this._cacheDuration;
			}
			set 
			{ 
				ViewState["CacheDuration"] = value; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public int PageCount
		{
			get { return TotalPages; }
		}

		protected int TotalPages
		{
			get
			{
				if (ViewState["TotalPages"] != null)
					return (int)ViewState["TotalPages"];
				else
					return this._pagedDataSource.PageCount; 
			}
			set { ViewState["TotalPages"] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public int Count
		{
			get { return this._pagedDataSource.Count; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public int CurrentPageIndex
		{
			get 
			{
				if (ViewState["CurrentPageIndex"] != null)
					return (int)ViewState["CurrentPageIndex"];
				else
					return -1;
			}
			set { ViewState["CurrentPageIndex"] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(_defaultPageSize)]
		public int PageSize
		{
			get 
			{ 
				if (ViewState["PageSize"] != null)
					return (int)ViewState["PageSize"];
				else
					return _defaultPageSize;
			}
			set 
			{ 
				ViewState["PageSize"] = value;
				if (this._pagedDataSource != null)
				{
					this._pagedDataSource.PageSize = value;
				}
				this.ChildControlsCreated = false;
			}
		}

		/// <summary>
		/// Allow custom paging (for example, when limiting items with a query)?
		/// </summary>
		[DefaultValue(false)]
		public bool AllowCustomPaging
		{
			get
			{
				if (ViewState["AllowCustomPaging"] != null)
					return (bool)ViewState["AllowCustomPaging"];
				else
					return this._pagedDataSource.AllowCustomPaging; 
			}
			set 
			{ 
				this._pagedDataSource.AllowCustomPaging = value;
				ViewState["AllowCustomPaging"] = value; 
			}
		}

		/// <summary>
		/// Virtual number of items.
		/// </summary>
		[Browsable(false)]
		public int VirtualItemCount
		{
			get
			{
				if (ViewState["VirtualItemCount"] != null)
					return (int)ViewState["VirtualItemCount"];
				else
					return this._pagedDataSource.VirtualCount; 
			}
			set 
			{ 
				this._pagedDataSource.VirtualCount = value;
				ViewState["VirtualItemCount"] = value; 
			}
		}

		/// <summary>
		/// The maximal number of pages that are clickable in in the pager.
		/// If the number of pages exceeds this limit, an option is presented
		/// to navigate to the next (or previous) set of pages.
		/// </summary>
		[DefaultValue(_defaultMaxDisplayPages)]
		public int MaxDisplayPages
		{
			get
			{
				if (ViewState["MaxDisplayPages"] != null)
					return (int)ViewState["MaxDisplayPages"];
				else
					return _defaultMaxDisplayPages;
			}
			set
			{
				ViewState["MaxDisplayPages"] = value;
			}
		}

		/// <summary>
		/// Hide the pager when there is only one page?
		/// </summary>
		[DefaultValue(false)]
		public bool HideWhenOnePage
		{
			get
			{
				if (ViewState["HideWhenOnePage"] != null)
					return (bool)ViewState["HideWhenOnePage"];
				else
					return false;
			}
			set 
			{ 
				ViewState["HideWhenOnePage"] = value; 
			}
		}

		/// <summary>
		/// The validator causes the form to validate?
		/// </summary>
		[DefaultValue(false)]
		public bool CausesValidation
		{
			get
			{
				if (ViewState["CausesValidation"] != null)
					return (bool)ViewState["CausesValidation"];
				else
					return false;
			}
			set 
			{ 
				ViewState["CausesValidation"] = value; 
			}
		}

		/// <summary>
		/// The link behavior for the pager controls.
		/// </summary>
		[DefaultValue(PagerLinkMode.LinkButton)]
		public PagerLinkMode PagerLinkMode
		{
			get
			{
				if (ViewState["PagerLinkMode"] != null)
				{
					return (PagerLinkMode)ViewState["PagerLinkMode"];
				}
				else
				{
					return PagerLinkMode.LinkButton;
				}
			}
			set
			{
				ViewState["PagerLinkMode"] = value;
			}
		}

		/// <summary>
		/// The base url of the page where the pager is put on.
		/// </summary>
		[Description("Set this if you want a specific url for the the pages that are being paged.")]
		public string PageUrl
		{
			get
            {
            	if (ViewState["PageUrl"] != null)
				{
					return (string)ViewState["PageUrl"];
				}
				else
				{
					if (HttpContext.Current != null)
					{
						return HttpContext.Current.Request.RawUrl;
					}
					else
					{
						return null;
					}
				}
            }
			set
            {
				if (HttpContext.Current != null)
				{
					// Only set the PageUrl when it is not already in the current url.
					if (HttpContext.Current.Request.RawUrl.Contains(value))
					{
						ViewState["PageUrl"] = HttpContext.Current.Request.RawUrl;
					}
					else
					{
						ViewState["PageUrl"] = value;
					}
				}
            }
		}

		#endregion

		#region events

		public event PageChangedEventHandler PageChanged;
            
		protected virtual void OnPageChanged(PageChangedEventArgs e)
		{
			this.CurrentPageIndex = e.CurrentPage;
			if (PageChanged != null)
				PageChanged(this, e);
		}

		public event EventHandler CacheEmpty;

		protected virtual void OnCacheEmpty(EventArgs e)
		{
			if (CacheEmpty != null)
				CacheEmpty(this, e);
		}

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Pager()
		{
			this._cacheDuration = _defaultCacheDuration;
		}
		
		#region methods

		protected override void OnInit(EventArgs e)
		{
			if (this.ControlToPage != String.Empty && this.ControlToPage != null)
			{
				this._controlToPage = this.Parent.FindControl(this.ControlToPage);
				if (this._controlToPage != null)
					this._controlToPage.DataBinding += new EventHandler(ControlToPage_DataBinding);
				else
					throw new NullReferenceException("The ControlToPage was not found on the page.");
			}
			else
				throw new NullReferenceException("The ControlToPage property has to be set to the ID of another control on the page.");

			InitPagedDataSource();	

			base.OnInit (e);
		}

		protected override void OnLoad(EventArgs e)
		{
			// Check if we have pathinfo or querystring parameters that set a specific page number.
			if (HttpContext.Current.Request.QueryString.HasKeys() || ! String.IsNullOrEmpty(HttpContext.Current.Request.PathInfo))
			{
				string pathInfoPattern = @"\/page\/(\d+)$";
				string queryStringPattern = @"(\?|\&)page=(\d+)$";

				if (Regex.IsMatch(this.PageUrl, pathInfoPattern))
				{
					int pageNumber = Int32.Parse(Regex.Match(this.PageUrl, pathInfoPattern).Groups[1].Value);
					OnPageChanged(new PageChangedEventArgs(-1, pageNumber -1));
				}
				else if (Regex.IsMatch(this.PageUrl, queryStringPattern))
				{
					int pageNumber = Int32.Parse(Regex.Match(this.PageUrl, queryStringPattern).Groups[2].Value);
					OnPageChanged(new PageChangedEventArgs(-1, pageNumber -1));
				}
			}
			base.OnLoad(e);
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			BuildNavigationControls();
			base.CreateChildControls();
		}


		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Site != null && this.Site.DesignMode)
			{
				writer.Write("1 2 3 ...");
			}
			base.Render (writer);
		}

		private void InitPagedDataSource()
		{
			this._pagedDataSource = new PagedDataSource();
			this._pagedDataSource.AllowCustomPaging = this.AllowCustomPaging;
			this._pagedDataSource.AllowPaging = true;
			this._pagedDataSource.PageSize = this.PageSize;
			this._pagedDataSource.CurrentPageIndex = this.CurrentPageIndex;
		}

		private void BuildNavigationControls()
		{
			if (this._controlToPage != null && this.CurrentPageIndex != -1)
			{
				int currentPageGroupIndex = GetCurrentPageGroupIndex();
				int totalPageGroups = GetTotalPageGroups();

				// First
				Control firstPageControl = CreateLinkControl(ButtonAction.First, "<<", 0);
				firstPageControl.Visible = (! this._pagedDataSource.IsFirstPage);
				this.Controls.Add(firstPageControl);

				// Prev
				Control prevPageControl = CreateLinkControl(ButtonAction.Prev, "<", this.CurrentPageIndex - 1);
				prevPageControl.Visible = (! this._pagedDataSource.IsFirstPage);
				this.Controls.Add(prevPageControl);

				// Previous page group
				int prevPageGroupPageIndex = (currentPageGroupIndex - 1) * this.MaxDisplayPages;
				Control prevGroupControl = CreateLinkControl(ButtonAction.PrevGroup, "...", prevPageGroupPageIndex);
				prevGroupControl.Visible = currentPageGroupIndex > 0;
				this.Controls.Add(prevGroupControl);

				// Numbers
				int beginPageNumberIndex = currentPageGroupIndex * this.MaxDisplayPages;
				int endPageNumberIndex = beginPageNumberIndex + this.MaxDisplayPages - 1;
				if (this.TotalPages <= endPageNumberIndex)
				{
					endPageNumberIndex = this.TotalPages - 1;
					if (endPageNumberIndex - this.MaxDisplayPages >= 0)
					{
						beginPageNumberIndex = endPageNumberIndex - this.MaxDisplayPages;
					}
				}
				for (int i = 0; i < this.TotalPages; i++)
				{
					string pageNumberString = Convert.ToString(i + 1);
					Control numberControl = CreateLinkControl(ButtonAction.Page, pageNumberString, i);
					if (i >= beginPageNumberIndex && i <= endPageNumberIndex)
					{
						if (this._pagedDataSource.CurrentPageIndex == i)
						{
							if (! (this.TotalPages == 1 && this.HideWhenOnePage))
							{
								Label currentPageLabel = new Label();
                                currentPageLabel.Text = pageNumberString;
								currentPageLabel.Font.Bold = true;
								numberControl = currentPageLabel;
							}
							else
							{
								numberControl.Visible = false;
							}
						}
					}
					else
					{
						numberControl.Visible = false;
					}
					this.Controls.Add(numberControl);
				}

				// Next page group
				int nextPageGroupPageIndex = (currentPageGroupIndex + 1) * this.MaxDisplayPages;
				Control nextGroupControl = CreateLinkControl(ButtonAction.NextGroup, "...", nextPageGroupPageIndex);
				nextGroupControl.Visible = currentPageGroupIndex < totalPageGroups - 1;
				this.Controls.Add(nextGroupControl);

				// Next
				Control nextPageControl = CreateLinkControl(ButtonAction.Next, ">", this.CurrentPageIndex + 1);
				nextPageControl.Visible = (this._pagedDataSource.DataSource == null || ! this._pagedDataSource.IsLastPage);
				this.Controls.Add(nextPageControl);

				// Last
				Control lastPageControl = CreateLinkControl(ButtonAction.Last, ">>", this.TotalPages - 1);
				lastPageControl.Visible = (this._pagedDataSource.DataSource == null || ! this._pagedDataSource.IsLastPage);
				this.Controls.Add(lastPageControl);
			}
		}

		private string GetCacheKey()
		{
			string cacheKey = "Pager_" + this._controlToPage.ID;
			if (this.CacheVaryByParams != null && this.CacheVaryByParams.Length > 0)
			{
				// Add the values of the individual cache parameters to the cache key
				foreach (string param in this.CacheVaryByParams)
				{
					cacheKey += "_" + HttpContext.Current.Request.Params[param];
				}
			}
			return cacheKey;
		}

		private LinkButton CreateLinkButton()
		{
			LinkButton lbt = new LinkButton();
			lbt.CausesValidation = this.CausesValidation;
			return lbt;
		}

		private Control CreateLinkControl(ButtonAction buttonAction, string buttonText, int pageIndex)
		{
			Control control = null;
			if (this.PagerLinkMode == PagerLinkMode.LinkButton)
			{
				LinkButton lbt = new LinkButton();
				lbt.CausesValidation = false;
				lbt.ID = buttonAction.ToString();
				lbt.Text = buttonText;

				switch (buttonAction)
				{
					case ButtonAction.First:
						lbt.Click += new EventHandler(First_Click);
						break;
					case ButtonAction.Prev:
						lbt.Click += new EventHandler(Prev_Click);
						break;
					case ButtonAction.PrevGroup:
						lbt.Click += new EventHandler(PrevGroup_Click);
						break;
					case ButtonAction.Page:
						// Override ID with the page index. 
						lbt.ID = pageIndex.ToString();
						lbt.Click += new EventHandler(Number_Click);
						break;
					case ButtonAction.NextGroup:
						lbt.Click += new EventHandler(NextGroup_Click);
						break;
					case ButtonAction.Next:
						lbt.Click += new EventHandler(Next_Click);
						break;
					case ButtonAction.Last:
						lbt.Click += new EventHandler(Last_Click);
						break;
				}

				control = lbt;
			}
			else
			{
				HyperLink hpl = new HyperLink();
				hpl.EnableViewState = false;
				hpl.Text = buttonText;

				if (this.PagerLinkMode == PagerLinkMode.HyperLinkPathInfo)
				{
					hpl.NavigateUrl = GetPageUrlWithPageNumberToPathInfo(pageIndex);
				}
				else
				{
					hpl.NavigateUrl = GetPageUrlWithPageNumberToQueryString(pageIndex);
				}

				control = hpl;
			}

			return control;
		}


		private string GetPageUrlWithPageNumberToPathInfo(int pageIndex)
		{
			string pathInfoPagePattern = @"\/page\/\d+$";
			string urlWithoutPageInfo = Regex.Replace(this.PageUrl, pathInfoPagePattern, String.Empty);
			return urlWithoutPageInfo + String.Format("/page/{0}", pageIndex + 1);
		}

		private string GetPageUrlWithPageNumberToQueryString(int pageIndex)
		{
			string queryStringPagePattern = @"(\?|\&)page=\d+$";
			string urlWithoutPageInfo = Regex.Replace(this.PageUrl, queryStringPagePattern, String.Empty);
			if (urlWithoutPageInfo.Contains("?"))
			{
				return urlWithoutPageInfo + String.Format("&page={0}", pageIndex + 1);
			}
			else
			{
				return urlWithoutPageInfo + String.Format("?page={0}", pageIndex + 1);
			}
		}

		private int GetCurrentPageGroupIndex()
		{
			return (int)Math.Floor((this.CurrentPageIndex / (double)this.MaxDisplayPages));
		}

		private int GetTotalPageGroups()
		{
			double temp = (this.TotalPages / (double)this.MaxDisplayPages);
			return (int)Math.Ceiling(temp);
		}

		#endregion

		#region event handlers

		private void ControlToPage_DataBinding(object sender, EventArgs e)
		{
			// Take the datasource and hand it over to the internal PagedDataSource.
			// We need a little reflection here.
			PropertyInfo pi = this._controlToPage.GetType().GetProperty("DataSource");
			if (pi != null)
			{
				IEnumerable controlDataSource = (IEnumerable)pi.GetValue(this._controlToPage, null);
				// We don't have to do anything special when the datasource of the controlToPage already is 
				// a PagedDataSource.
				if (! (controlDataSource is PagedDataSource))
				{
					// Maybe we have a cached datasource
					string cacheKey = GetCacheKey();
					if (this.CacheDataSource && controlDataSource == null)
					{
						this._pagedDataSource = (PagedDataSource)HttpContext.Current.Cache[cacheKey];
						// The cache can be empty. If so, raise the CacheEmpty event so that the DataSource 
						// of the controlToPage can be set (again).
						if (this._pagedDataSource == null)
						{
							InitPagedDataSource();
							OnCacheEmpty(EventArgs.Empty);
							// Re-fetch the DataSource property value
							controlDataSource = (IEnumerable)pi.GetValue(this._controlToPage, null);
							this._pagedDataSource.DataSource = controlDataSource;							
						}
					}
					else
					{
						this._pagedDataSource.DataSource = controlDataSource;
					}
					if (this.CurrentPageIndex == -1)
						this.CurrentPageIndex = 0;
					this._pagedDataSource.CurrentPageIndex = this.CurrentPageIndex;
					// Don't swap the datasource when using custom paging
					if (! this.AllowCustomPaging)
					{
						pi.SetValue(this._controlToPage, this._pagedDataSource, null);
						// Call databind again, but now with the pageddatasource attached.
						this._controlToPage.DataBind();
					}
					TotalPages = this._pagedDataSource.PageCount;
					// ChildControls have to be created again.
					this.ChildControlsCreated = false;
					// Cache the datasource when required
					if (this.CacheDataSource)
					{
						HttpContext.Current.Cache.Insert(cacheKey, this._pagedDataSource, null, DateTime.Now.AddSeconds(this.CacheDuration), TimeSpan.Zero);
					}
				}
			}
			else
				throw new InvalidOperationException("The ControlToPage doesn't have a DataSource property.");
		}

		private void First_Click(object sender, EventArgs e)
		{
			int nextPage = 0;
			PageChangedEventArgs args = new PageChangedEventArgs(this.CurrentPageIndex, nextPage);
			OnPageChanged(args);			
		}

		private void Prev_Click(object sender, EventArgs e)
		{
			int nextPage = this.CurrentPageIndex - 1;
			PageChangedEventArgs args = new PageChangedEventArgs(this.CurrentPageIndex, nextPage);
			OnPageChanged(args);
		}

		private void Number_Click(object sender, EventArgs e)
		{
			int nextPage = Int32.Parse(((LinkButton)sender).ID);
			PageChangedEventArgs args = new PageChangedEventArgs(this.CurrentPageIndex, nextPage);
			OnPageChanged(args);
		}

		private void Next_Click(object sender, EventArgs e)
		{
			int nextPage = this.CurrentPageIndex + 1;
			PageChangedEventArgs args = new PageChangedEventArgs(this.CurrentPageIndex, nextPage);
			OnPageChanged(args);
		}

		private void Last_Click(object sender, EventArgs e)
		{
			int nextPage = this.TotalPages -1;
			PageChangedEventArgs args = new PageChangedEventArgs(this.CurrentPageIndex, nextPage);
			OnPageChanged(args);
		}

		private void PrevGroup_Click(object sender, EventArgs e)
		{
			int nextPage = (GetCurrentPageGroupIndex() - 1) * this.MaxDisplayPages;
			PageChangedEventArgs args = new PageChangedEventArgs(this.CurrentPageIndex, nextPage);
			OnPageChanged(args);
		}

		private void NextGroup_Click(object sender, EventArgs e)
		{
			int nextPage = (GetCurrentPageGroupIndex() + 1) * this.MaxDisplayPages;
			PageChangedEventArgs args = new PageChangedEventArgs(this.CurrentPageIndex, nextPage);
			OnPageChanged(args);
		}

		#endregion

		private enum ButtonAction
		{
			First,
			Prev,
			PrevGroup,
			Page,
			NextGroup,
			Next,
			Last
		}
	}

	#region PageChangedEvent

	/// <summary>
	/// EventArgs class for Pager events.
	/// </summary>
	public class PageChangedEventArgs
	{
		private int _prevPage;
		private int _currentPage;

		/// <summary>
		/// The page index of the previous page.
		/// </summary>
		public int PrevPage
		{
			get { return this._prevPage; }
		}

		/// <summary>
		/// The page index of the current page (after clicking next).
		/// </summary>
		public int CurrentPage
		{
			get { return this._currentPage; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="prevPage"></param>
		/// <param name="currentPage"></param>
		public PageChangedEventArgs(int prevPage, int currentPage)
		{
			this._prevPage = prevPage;
			this._currentPage = currentPage;
		}
	}

	/// <summary>
	/// Delegate for pager events.
	/// </summary>
	public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);

	#endregion

	#region Type Converters

	public class ParamsConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertFrom (context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertTo (context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null) 
			{
				return new string[0];
			}

			if (value is string) 
			{
				string s = (string)value;
				if (s.Length == 0) 
				{
					return new string[0];
				}
				string[] parts = s.Split(culture.TextInfo.ListSeparator[0]);
				return parts;
			}

			return base.ConvertFrom (context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value != null) 
			{
				if (!(value is string[])) 
				{
					throw new ArgumentException("Invalid string array", "value");
				}
			}

			if (destinationType == typeof(string)) 
			{
				if (value == null) 
				{
					return String.Empty;
				}

				string[] prms = (string[])value;
				if (prms.Length == 0) 
				{
					return String.Empty;
				}
				return String.Join(culture.TextInfo.ListSeparator, prms);
			}

			return base.ConvertTo (context, culture, value, destinationType);
		}
	}
	#endregion

	/// <summary>
	/// The way pager links are generated.
	/// </summary>
	public enum PagerLinkMode
	{
		/// <summary>
		/// Pager links are linkbuttons and cause a postback
		/// </summary>
		LinkButton,
		/// <summary>
		/// Pager links are plain hyperlinks and add '/page/pagenumber' to the end of the pathinfo
		/// </summary>
		HyperLinkPathInfo,
		/// <summary>
		/// Pager links are plain hyperlink and '&page=pagenumber' to the querystring.
		/// </summary>
		HyperLinkQueryString
	}
}
