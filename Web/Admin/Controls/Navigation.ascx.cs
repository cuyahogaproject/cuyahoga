namespace Cuyahoga.Web.Admin.Controls
{
	using System;
	using System.Collections;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core;
	using Cuyahoga.Core.Domain;
	using Cuyahoga.Core.Collections;
	using Cuyahoga.Web.Admin.UI;

	/// <summary>
	///		Summary description for Navigation.
	/// </summary>
	public class Navigation : System.Web.UI.UserControl
	{
        private AdminBasePage _page;

		protected System.Web.UI.WebControls.PlaceHolder plhNodes;
		protected System.Web.UI.WebControls.Image i1;
		protected System.Web.UI.WebControls.Image i2;
		protected System.Web.UI.WebControls.Image i3;
		protected System.Web.UI.WebControls.Image i4;		

		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				this._page = (AdminBasePage)this.Page;		
			}
			catch (InvalidCastException ex)
			{
				throw new Exception("This control requires a Page of the type AdminBasePage.", ex);
			}
			BuildNodeTree();
		}

		private void BuildNodeTree()
		{
			IList nodes = this._page.CoreRepository.GetRootNodes();
            DisplayNodes(nodes);			
		}

		private void DisplayNodes(IList nodes)
		{
			foreach (Node node in nodes)
			{				
				this.plhNodes.Controls.Add(CreateDisplayNode(node));
				if (this._page.ActiveNode != null 
					&& node.Level <= this._page.ActiveNode.Level 
					&& node.Id == this._page.ActiveNode.Trail[node.Level])
				{
					// The node is in the trail, expand.
					DisplayNodes(node.ChildNodes);
					if (this._page.ActiveNode.Id == node.Id)
					{
						// HACK: Replace the activenode with the one found while building the node tree to reduce future 
						// database calls.
						this._page.ActiveNode = node;
					}
				}
			}
		}

		private Control CreateDisplayNode(Node node)
		{
			int indent = node.Level * 20;
			HtmlGenericControl container = new HtmlGenericControl("div");
			container.Attributes.Add("class", "node");
			container.Attributes.Add("style", String.Format("padding-left: {0}px", indent.ToString()));
			Image img = new Image();
			img.ImageUrl ="../Images/doc2.gif";
			img.ImageAlign = ImageAlign.AbsMiddle;
			container.Controls.Add(img);
			if (this._page.ActiveNode != null && node.Id == this._page.ActiveNode.Id)
			{
				Label lbl = new Label();
				lbl.CssClass = "nodeActive";
				lbl.Text = node.Title;
				container.Controls.Add(lbl);
			}
			else
			{
				HyperLink hpl = new HyperLink();
				hpl.Text = node.Title;
				hpl.NavigateUrl = String.Format("../NodeEdit.aspx?NodeId={0}", node.Id.ToString());
				hpl.CssClass = "nodeLink";
				container.Controls.Add(hpl);
			}
			return container;
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
