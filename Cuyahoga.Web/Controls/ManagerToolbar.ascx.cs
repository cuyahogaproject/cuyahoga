using System;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Web.Controls
{
	public partial class ManagerToolbar : System.Web.UI.UserControl
	{
		private PageEngine _pageEngine;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!(this.Page is PageEngine))
			{
				throw new Exception("The Manager toolbar control needs to be on a Page of type PageEngine to work properly.");
			}
			this._pageEngine = (PageEngine)this.Page;
			this._pageEngine.RegisterStylesheet("managertoolbar", UrlUtil.GetApplicationPath() + "Manager/Content/Css/ManagerToolbar.css");
			if (!IsPostBack)
			{
				EnableLinks();
			}
		}

		private void EnableLinks()
		{
			User user = this._pageEngine.CuyahogaUser;
			Site site = this._pageEngine.CurrentSite;

			if (user != null && user.IsAuthenticated)
			{
				this.hplManagePages.Visible = user.HasRight(Rights.ManagePages, site);
				this.hplManageFiles.Visible = user.HasRight(Rights.ManageFiles, site);
				this.hplManageUsers.Visible = user.HasRight(Rights.ManageUsers, site);
				if (this._pageEngine.ActiveNode != null)
				{
					int activeNodeId = this._pageEngine.ActiveNode.Id;
					this.hplManagePages.NavigateUrl += "/Index/" + activeNodeId;
					this.hplPageContent.Visible = user.HasRight(Rights.ManagePages, site) && user.CanEdit(this._pageEngine.ActiveNode);
					this.hplPageContent.NavigateUrl = "~/Manager/Pages/Content/" + activeNodeId;
					this.hplPageLayout.Visible = user.HasRight(Rights.ManagePages, site);
					this.hplPageLayout.NavigateUrl = "~/Manager/Pages/Design/" + activeNodeId;
				}
			}
		}
	}
}