using System;
using System.Collections;
using System.Web.UI.WebControls;

using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Forum;
using Cuyahoga.Modules.Forum.Domain;
using Cuyahoga.Core.Domain;
using Cuyahoga.Modules.Forum.Web.UI;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	///		Summary description for Links.
	/// </summary>
	public class ForumViewProfile : BaseForumControl
	{
		protected System.Web.UI.WebControls.PlaceHolder phForumTop;
		protected System.Web.UI.WebControls.PlaceHolder phForumFooter;
		protected System.Web.UI.WebControls.Literal ltrUserName;
		protected System.Web.UI.WebControls.Label lblRealName;
		protected System.Web.UI.WebControls.Literal ltrRealName;
		protected System.Web.UI.WebControls.Label lblLocation;
		protected System.Web.UI.WebControls.Literal ltrLocation;
		protected System.Web.UI.WebControls.Label lblOccupation;
		protected System.Web.UI.WebControls.Literal ltroccupation;
		protected System.Web.UI.WebControls.Label lblInterest;
		protected System.Web.UI.WebControls.Literal ltrInterest;
		protected System.Web.UI.WebControls.Label lblGender;
		protected System.Web.UI.WebControls.Literal ltrGender;
		protected System.Web.UI.WebControls.Label lblHomepage;
		protected System.Web.UI.WebControls.Literal ltrHomepage;
		protected System.Web.UI.WebControls.Label lblMSN;
		protected System.Web.UI.WebControls.Literal ltrMSN;
		protected System.Web.UI.WebControls.Label lblYahooMessenger;
		protected System.Web.UI.WebControls.Literal ltrYahooMessenger;
		protected System.Web.UI.WebControls.Label lblAIMName;
		protected System.Web.UI.WebControls.Literal ltrAIMName;
		protected System.Web.UI.WebControls.Label lblICQNumber;
		protected System.Web.UI.WebControls.Literal ltrICQNumber;
		protected System.Web.UI.WebControls.Label lblUserName;

		

		#region Private vars
		private ForumUser	_forumUser;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._forumUser	= base.ForumModule.GetForumUserByUserId(base.ForumModule.CurrentUserId);

			if(this._forumUser == null) // The user have NOT modified his profile, so we create an empty one
			{
				this._forumUser = new ForumUser();
				base.ForumModule.SaveForumUser(this._forumUser);
			}

            this.BindTopFooter();
            this.BindProfile();
            base.LocalizeControls();
        }

		private void BindTopFooter()
		{
			ForumTop	tForumTop;
			ForumFooter	tForumFooter;

			tForumTop			= (ForumTop)this.LoadControl("~/Modules/Forum/ForumTop.ascx");
			tForumTop.Module	= base.ForumModule;
			this.phForumTop.Controls.Add(tForumTop);

			tForumFooter		= (ForumFooter)this.LoadControl("~/Modules/Forum/ForumFooter.ascx");
			tForumFooter.Module	= base.ForumModule;
			this.phForumFooter.Controls.Add(tForumFooter);
		}

		private void BindProfile()
		{
			Cuyahoga.Core.Domain.User tUser;

			tUser = (Cuyahoga.Core.Domain.User)Context.User.Identity;
			this.ltrUserName.Text		= tUser.UserName;
			this.ltrRealName.Text		= tUser.FullName;
			this.ltrAIMName.Text		= this._forumUser.AIMName;

			if(this._forumUser.Gender == 0)
                this.ltrGender.Text		= GetText("female");
			else
				this.ltrGender.Text		= GetText("male");

			this.ltrHomepage.Text		= this._forumUser.Homepage;
			this.ltrICQNumber.Text		= this._forumUser.ICQNumber;
			this.ltrInterest.Text		= this._forumUser.Interest;
			this.ltrLocation.Text		= this._forumUser.Location;
			this.ltrMSN.Text			= this._forumUser.MSN;
			this.ltroccupation.Text		= this._forumUser.Occupation;
			this.ltrYahooMessenger.Text	= this._forumUser.YahooMessenger;
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
