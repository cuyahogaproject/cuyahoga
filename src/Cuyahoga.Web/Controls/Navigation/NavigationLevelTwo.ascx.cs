namespace Cuyahoga.Web.Templates.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;

	/// <summary>
	///		Summary description for NavigationLevelTwo.
	/// </summary>
	public class NavigationLevelTwo : System.Web.UI.UserControl
	{
		private Cuyahoga.Web.UI.PageEngine _page;
		protected System.Web.UI.WebControls.Repeater rptNav2;

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (this.Page is PageEngine)
				{
					this._page = (PageEngine)this.Page;	
					// Bind level 2 nodes
					if (this._page.ActiveNode.Level > 0)
					{
						this.rptNav2.ItemDataBound += new RepeaterItemEventHandler(rptNav2_ItemDataBound);
						this.rptNav2.DataSource = this._page.ActiveNode.NodePath[1].ChildNodes;
						this.rptNav2.DataBind();
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
			this.rptNav2.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptNav2_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptNav2_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			Node node = (Node)e.Item.DataItem;
			if (node.ShowInNavigation && node.ViewAllowed(this._page.CuyahogaUser))
			{
				HyperLink hpl = (HyperLink)e.Item.FindControl("hplNav2");
				hpl.NavigateUrl = UrlHelper.GetUrlFromNode(node);
				hpl.Text = node.Title;
				UrlHelper.SetHyperLinkTarget(hpl, node);
				if (node.Level <= this._page.ActiveNode.Level && node.Id == this._page.ActiveNode.Trail[node.Level])
				{
					hpl.CssClass = "subselected";
				}
			}
			else
			{
				e.Item.Visible = false;
			}
		}
	}
}
