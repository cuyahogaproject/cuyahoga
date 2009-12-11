using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core;
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.RemoteContent.Domain;

namespace Cuyahoga.Modules.RemoteContent.Web
{
	/// <summary>
	/// Summary description for AdminRemoteContent.
	/// </summary>
	public partial class AdminRemoteContent : ModuleAdminBasePage
	{
		private RemoteContentModule _remoteContentModule;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// The base page has already created the module, we only have to cast it here to the right type.
			this._remoteContentModule = base.Module as RemoteContentModule;
			this.btnNew.Attributes.Add("onclick", String.Format("document.location.href='EditFeed.aspx{0}&FeedId=-1'", base.GetBaseQueryString()));

			if (! this.IsPostBack)
			{
				BindFeeds();
			}
		}

		private void BindFeeds()
		{
			this.rptFeeds.DataSource = this._remoteContentModule.GetAllFeeds();
			this.rptFeeds.DataBind();
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.rptFeeds.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptFeeds_ItemDataBound);
			this.rptFeeds.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rptFeeds_ItemCommand);

		}
		#endregion

		private void rptFeeds_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			HyperLink hplEdit = e.Item.FindControl("hpledit") as HyperLink;
			if (hplEdit != null)
			{
				Feed feed = e.Item.DataItem as Feed;
				hplEdit.NavigateUrl = String.Format("~/Modules/RemoteContent/EditFeed.aspx{0}&FeedId={1}", base.GetBaseQueryString(), feed.Id);
			}
		}

		private void rptFeeds_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Refresh")
			{
				int feedId = Int32.Parse(e.CommandArgument.ToString());
				Feed feed = this._remoteContentModule.GetFeedById(feedId);
				try
				{
					this._remoteContentModule.RefreshFeedContents(feed);
					this._remoteContentModule.SaveFeed(feed);
					BindFeeds();
				}
				catch (Exception ex)
				{
					ShowError(ex.ToString());
				}
			}
		}
	}
}
