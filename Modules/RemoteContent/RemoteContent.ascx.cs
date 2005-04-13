namespace Cuyahoga.Modules.RemoteContent
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Web.UI;

	/// <summary>
	///		Summary description for RemoteContent.
	/// </summary>
	public class RemoteContent : BaseModuleControl
	{	
		private RemoteContentModule _module;

		protected System.Web.UI.WebControls.Repeater rptFeedItems;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (! this.IsPostBack && ! base.HasCachedOutput)
			{
				this._module = base.Module as RemoteContentModule;
				this.rptFeedItems.DataSource = this._module.GetAllFeedItems();
				this.rptFeedItems.DataBind();
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
			this.rptFeedItems.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptFeedItems_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptFeedItems_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			FeedItem fi = e.Item.DataItem as FeedItem;
			if (fi != null)
			{
				if (this._module.ShowContents)
				{
					Panel pnlContents = e.Item.FindControl("pnlContents") as Panel;
					pnlContents.Visible = true;
				}
				
				Label lblPubdate = e.Item.FindControl("lblPubdate") as Label;
				lblPubdate.Visible = this._module.ShowDates;
				Label lblAuthor = e.Item.FindControl("lblAuthor") as Label;
				lblAuthor.Visible = this._module.ShowAuthors;
				Label lblSource = e.Item.FindControl("lblSource") as Label;
				lblSource.Visible = this._module.ShowSources;
			}
		}
	}
}
