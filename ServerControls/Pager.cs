using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace Cuyahoga.ServerControls
{
	/// <summary>
	/// The Pager control enables paging WebControls that can bind to a DataSource.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:Pager runat=server></{0}:Pager>")]
	public class Pager : System.Web.UI.WebControls.WebControl, INamingContainer
	{
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
		public int CacheDuration
		{
			get 
			{
				if (ViewState["CacheDuration"] != null)
					return (int)ViewState["CacheDuration"];
				else
					return this._cacheDuration;
			}
			set { ViewState["CacheDuration"] = value; }
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
		public int PageSize
		{
			get { return this._pagedDataSource.PageSize; }
			set { this._pagedDataSource.PageSize = value; }
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
			InitPagedDataSource();
			this._cacheDuration = 30;
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
				
			base.OnInit (e);
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			BuildNavigationControls();
			base.CreateChildControls ();
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
			this._pagedDataSource.AllowCustomPaging = false;
			this._pagedDataSource.AllowPaging = true;
			this._pagedDataSource.PageSize = 10;
			this._pagedDataSource.CurrentPageIndex = this.CurrentPageIndex;
		}

		private void BuildNavigationControls()
		{
			if (this._controlToPage != null && this.CurrentPageIndex != -1)
			{
				LinkButton lbt = null;
				// First
				lbt = new LinkButton();
				lbt.ID = "First";
				lbt.Text = "<<";
				if (! this._pagedDataSource.IsFirstPage)
					lbt.Click += new EventHandler(First_Click);
				else
					lbt.Enabled = false;
				this.Controls.Add(lbt);

				// Prev
				lbt = new LinkButton();
				lbt.ID = "Prev";
				lbt.Text = "<";
				if (! this._pagedDataSource.IsFirstPage)
					lbt.Click += new EventHandler(Prev_Click);
				else
					lbt.Enabled = false;
				this.Controls.Add(lbt);

				// Numbers
				for (int i = 0; i < this.TotalPages; i++)
				{
					lbt = new LinkButton();
					lbt.ID = i.ToString();
					lbt.Text = Convert.ToString(i + 1);
					if (this._pagedDataSource.CurrentPageIndex != i)
						lbt.Click += new EventHandler(Number_Click);
					else
					{
						lbt.Font.Bold = true;
						lbt.Enabled = false;
					}
					this.Controls.Add(lbt);
				}

				// Next
				lbt = new LinkButton();
				lbt.ID = "Next";
				lbt.Text = ">";
				if (this._pagedDataSource.DataSource == null || ! this._pagedDataSource.IsLastPage)
					lbt.Click += new EventHandler(Next_Click);
				else
					lbt.Enabled = false;
				this.Controls.Add(lbt);

				// Last
				lbt = new LinkButton();
				lbt.ID = "Last";
				lbt.Text = ">>";
				if (this._pagedDataSource.DataSource == null || ! this._pagedDataSource.IsLastPage)
					lbt.Click += new EventHandler(Last_Click);
				else
					lbt.Enabled = false;
				this.Controls.Add(lbt);
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

		#endregion

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
					pi.SetValue(this._controlToPage, this._pagedDataSource, null);
					TotalPages = this._pagedDataSource.PageCount;
					// Call databind again, but now with the pageddatasource attached.
					this._controlToPage.DataBind();
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
}
