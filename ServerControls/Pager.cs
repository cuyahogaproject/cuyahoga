using System;
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
		public string[] CacheParams
		{
			get 
			{ 
				if (ViewState["CacheParams"] != null)
					return (string[])ViewState["CacheParams"];
				else
					return null;
			}
			set { ViewState["CacheParams"] = value; }
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
			this._pagedDataSource.AllowCustomPaging = false;
			this._pagedDataSource.AllowPaging = true;
			this._pagedDataSource.PageSize = 10;
			this._pagedDataSource.CurrentPageIndex = -1;
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
					this._pagedDataSource.DataSource = controlDataSource;
					if (this.CurrentPageIndex == -1)
						this.CurrentPageIndex = 0;
					this._pagedDataSource.CurrentPageIndex = this.CurrentPageIndex;
					pi.SetValue(this._controlToPage, this._pagedDataSource, null);
					TotalPages = this._pagedDataSource.PageCount;
					// Call databind again, but now with the pageddatasource attached.
					this._controlToPage.DataBind();
					// ChildControls have to be created again.
					this.ChildControlsCreated = false;
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
}
