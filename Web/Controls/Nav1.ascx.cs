namespace Cuyahoga.Web.Controls
{
	using System;
	// using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;

	/// <summary>
	///		Summary description for Nav1.
	/// </summary>
	public class Nav1 : System.Web.UI.UserControl
	{
		private Cuyahoga.Web.UI.PageEngine _page;
		protected System.Web.UI.WebControls.HyperLink hplHome;
		protected System.Web.UI.WebControls.HyperLink hplAdmin;
		protected System.Web.UI.WebControls.Repeater rptNav1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (this.Page is PageEngine)
				{
					this._page = (PageEngine)this.Page;	
					// Bind home hyperlink
					this.hplHome.NavigateUrl = UrlHelper.GetUrlFromNode(this._page.RootNode);
					this.hplHome.Text = this._page.RootNode.Title;
					// Bind level 1 nodes
					this.rptNav1.ItemDataBound += new RepeaterItemEventHandler(rptNav1_ItemDataBound);
					this.rptNav1.DataSource = this._page.RootNode.ChildNodes;
					this.rptNav1.DataBind();
					
					if (this._page.CuyahogaUser != null)
					{
						this.hplAdmin.Visible = this._page.CuyahogaUser.HasPermission(AccessLevel.Administrator);
					}
				}
			}
			catch (InvalidCastException ex)
			{
				throw new Exception("This control requires a Page of the type Cuyahoga.Web.UI.Page.", ex);
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptNav1_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Node node = (Node)e.Item.DataItem;
			if (node.ViewAllowed(this._page.CuyahogaUser))
			{
				HyperLink hpl = (HyperLink)e.Item.FindControl("hplNav1");
				hpl.NavigateUrl = UrlHelper.GetUrlFromNode(node);
				hpl.Text = node.Title;
				if (node.Level <= this._page.ActiveNode.Level && node.Id == this._page.ActiveNode.Trail[node.Level])
				{
					hpl.CssClass = "selected";
				}
			}
		}
	}
}
