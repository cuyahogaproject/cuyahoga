using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Templates.Controls
{
	/// <summary>
	///		Summary description for NavigationLevelZeroOne.
	/// </summary>
	public class NavigationLevelZeroOne : UserControl
	{
		private PageEngine _page;
		protected HyperLink hplHome;
		protected HyperLink hplAdmin;
		protected Repeater rptNav1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liAdmin;

		private void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (this.Page is PageEngine)
				{
					this._page = (PageEngine)this.Page;	
					// Bind home hyperlink
					if (this._page.RootNode.ShowInNavigation && this._page.RootNode.ViewAllowed(this._page.CuyahogaUser))
					{
						this.hplHome.NavigateUrl = UrlHelper.GetUrlFromNode(this._page.RootNode);
						this.hplHome.Text = this._page.RootNode.Title;
					}
					else
					{
						this.hplHome.Visible = false;
					}
					// Bind level 1 nodes
					this.rptNav1.ItemDataBound += new RepeaterItemEventHandler(rptNav1_ItemDataBound);
					this.rptNav1.DataSource = this._page.RootNode.ChildNodes;
					this.rptNav1.DataBind();
					
					if (this._page.CuyahogaUser != null)
					{
						// show <li> tag for Admin link
						this.liAdmin.Visible = this._page.CuyahogaUser.HasPermission(AccessLevel.Administrator);
						this.hplAdmin.NavigateUrl = this._page.ResolveUrl("~/Admin");
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
			if (node.ShowInNavigation && node.ViewAllowed(this._page.CuyahogaUser))
			{
				HyperLink hpl = (HyperLink)e.Item.FindControl("hplNav1");
				hpl.NavigateUrl = UrlHelper.GetUrlFromNode(node);
				hpl.Text = node.Title;
				UrlHelper.SetHyperLinkTarget(hpl, node);
				if (node.Level <= this._page.ActiveNode.Level && node.Id == this._page.ActiveNode.Trail[node.Level])
				{
					hpl.CssClass = "selected";
				}
			}
			else
			{
				e.Item.Visible = false;
			}
		}
	}
}
