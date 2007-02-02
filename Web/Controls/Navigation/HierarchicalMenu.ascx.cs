namespace Cuyahoga.Web.Templates.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;

	/// <summary>
	///		Summary description for HierarchicalMenu.
	/// </summary>
	public class HierarchicalMenu : System.Web.UI.UserControl
	{
		private Cuyahoga.Web.UI.PageEngine _page;
		protected System.Web.UI.WebControls.PlaceHolder plhNodes;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Page is PageEngine)
			{
				this._page = (PageEngine)this.Page;	
				BuildNavigationTree();
			}
		}

		private void BuildNavigationTree()
		{
			HtmlGenericControl mainList = new HtmlGenericControl("ul");
			if (this._page.RootNode.ShowInNavigation && this._page.RootNode.ViewAllowed(this._page.CuyahogaUser))
			{
				mainList.Controls.Add(BuildListItemFromNode(this._page.RootNode));
			}
			foreach (Node node in this._page.RootNode.ChildNodes)
			{
				if (node.ShowInNavigation && node.ViewAllowed(this._page.CuyahogaUser))
				{
					HtmlControl listItem = BuildListItemFromNode(node);
					if (node.Level <= this._page.ActiveNode.Level 
						&& node.Id == this._page.ActiveNode.Trail[node.Level] 
						&& node.ChildNodes.Count > 0)
					{
						listItem.Controls.Add(BuildListFromNodes(node.ChildNodes));
					}
					mainList.Controls.Add(listItem);
				}
			}
			if (this._page.CuyahogaUser != null 
				&& this._page.CuyahogaUser.HasPermission(AccessLevel.Administrator))
			{
				HtmlGenericControl listItem = new HtmlGenericControl("li");
				HyperLink hpl = new HyperLink();
				hpl.NavigateUrl = this._page.ResolveUrl("~/Admin");
				hpl.Text = "Admin";
				listItem.Controls.Add(hpl);
				mainList.Controls.Add(listItem);
			}
			this.plhNodes.Controls.Add(mainList);
		}

		private HtmlControl BuildListItemFromNode(Node node)
		{
			HtmlGenericControl listItem = new HtmlGenericControl("li");
			HyperLink hpl = new HyperLink();
			hpl.NavigateUrl = UrlHelper.GetUrlFromNode(node);
			UrlHelper.SetHyperLinkTarget(hpl, node);
			hpl.Text = node.Title;
			// Little dirty trick to highlight the active item :)
			if (node.Id == this._page.ActiveNode.Id)
			{
				hpl.CssClass = "selected";
			}
			listItem.Controls.Add(hpl);
			return listItem;
		}

		private HtmlControl BuildListFromNodes(IList nodes)
		{
			HtmlGenericControl list = new HtmlGenericControl("ul");
			foreach (Node node in nodes)
			{
				if (node.ViewAllowed(this._page.CuyahogaUser) && node.ShowInNavigation)
				{
					HtmlControl listItem = BuildListItemFromNode(node);
					if (node.Level <= this._page.ActiveNode.Level 
						&& node.Id == this._page.ActiveNode.Trail[node.Level] 
						&& node.ChildNodes.Count > 0)
					{
						listItem.Controls.Add(BuildListFromNodes(node.ChildNodes));
					}
					list.Controls.Add(listItem);
				}
			}
			return list;
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
	}
}
