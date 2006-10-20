using System;
using System.Collections;
using System.Web.UI.WebControls;

using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Forum;
using Cuyahoga.Modules.Forum.Domain;
using Cuyahoga.Modules.Forum.Web.UI;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	///		Summary description for Links.
	/// </summary>
	public class ForumProfile : BaseForumControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.PlaceHolder phForumTop;
		protected System.Web.UI.WebControls.PlaceHolder phForumFooter;
		protected System.Web.UI.WebControls.Panel pnlProfile;
		protected System.Web.UI.WebControls.Label lblUserName;
		protected System.Web.UI.WebControls.Label lblRealName;
		protected System.Web.UI.WebControls.Label lblLocation;
		protected System.Web.UI.WebControls.Label lblMSN;
		protected System.Web.UI.WebControls.Label lblYahooMessenger;
		protected System.Web.UI.WebControls.Label lblAIMName;
		protected System.Web.UI.WebControls.Label lblICQNumber;
		protected System.Web.UI.WebControls.Label lblTimeZone;
		protected System.Web.UI.WebControls.Label lblOccupation;
		protected System.Web.UI.WebControls.Label lblInterest;
		protected System.Web.UI.WebControls.Label lblGender;
		protected System.Web.UI.WebControls.Label lblAvartar;
		protected System.Web.UI.WebControls.Label lblHomePage;
		protected System.Web.UI.WebControls.Label lblSignature;
		protected System.Web.UI.WebControls.Literal ltlUserName;
		protected System.Web.UI.WebControls.Literal ltlRealName;
		protected System.Web.UI.WebControls.RadioButton rbFemale;
		protected System.Web.UI.WebControls.DropDownList ddlTimeZone;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.TextBox txtSignature;
		protected System.Web.UI.WebControls.RadioButton rbMale;
		protected System.Web.UI.WebControls.TextBox txtLocation;
		protected System.Web.UI.WebControls.TextBox txtOccupation;
		protected System.Web.UI.WebControls.TextBox txtInterest;
		protected System.Web.UI.WebControls.TextBox txtHomepage;
		protected System.Web.UI.WebControls.TextBox txtMSN;
		protected System.Web.UI.WebControls.TextBox txtYahooMessenger;
		protected System.Web.UI.WebControls.TextBox txtAIMName;
		protected System.Web.UI.WebControls.TextBox txtICQNumber;
		protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.Button btnCancel;

		private ForumUser	_fUser;

		private void Page_Load(object sender, System.EventArgs e)
		{			
			if(this.Page.User.Identity.IsAuthenticated)
			{
				Cuyahoga.Core.Domain.User currentUser = Context.User.Identity as Cuyahoga.Core.Domain.User;
				this._fUser	= base.ForumModule.GetForumUserByUserId(currentUser.Id);
				if(this._fUser == null)
				{
					this._fUser = new ForumUser();
					base.ForumModule.SaveForumUser(this._fUser);
				}
			}


			if(!this.Page.IsPostBack)
			{
                this.BindTopFooter();
                this.LocalizeControls();
                this.ddlTimeZone.DataSource = Utils.Utils.TimeZones();
				this.ddlTimeZone.DataBind();
				this.BindUser();
				// Add text

			}
		}

		private void BindUser()
		{
			Cuyahoga.Core.Domain.User tUser = Context.User.Identity as Cuyahoga.Core.Domain.User;
			this.ltlUserName.Text	= tUser.UserName;
			this.ltlRealName.Text	= tUser.FullName;

			this.txtInterest.Text		= this._fUser.Interest;
			this.txtAIMName.Text		= this._fUser.AIMName;

			if(this._fUser.Gender == 0)
			{
				this.rbFemale.Checked = true;
			}
			else
			{
				this.rbMale.Checked = true;
			}

			//this._fUser.Avartar		= this.txt
			this.txtHomepage.Text		= this._fUser.Homepage;
			this.txtICQNumber.Text		= this._fUser.ICQNumber;
			this.txtLocation.Text		= this._fUser.Location;
			this.txtMSN.Text			= this._fUser.MSN;
			this.txtOccupation.Text		= this._fUser.Occupation;
			this.txtSignature.Text		= this._fUser.Signature;
			this.txtYahooMessenger.Text	= this._fUser.YahooMessenger;
			this.ddlTimeZone.Items.FindByValue(this._fUser.TimeZone.ToString()).Selected = true;
		}
		private void BindTopFooter()
		{
			ForumTop tForumTop;
			ForumFooter tForumFooter;

			tForumTop = (ForumTop)this.LoadControl("~/Modules/Forum/ForumTop.ascx");
			tForumTop.Module = base.ForumModule;
			this.phForumTop.Controls.Add(tForumTop);

			tForumFooter = (ForumFooter)this.LoadControl("~/Modules/Forum/ForumFooter.ascx");
			tForumFooter.Module	= base.ForumModule;
			this.phForumFooter.Controls.Add(tForumFooter);
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(String.Format("{0}", UrlHelper.GetUrlFromSection(base.ForumModule.Section)));
        }

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			Cuyahoga.Core.Domain.User currentUser = Context.User.Identity as Cuyahoga.Core.Domain.User;
			this._fUser.UserId	= currentUser.Id;
			this._fUser.Interest	= this.txtInterest.Text;
			this._fUser.AIMName		= this.txtAIMName.Text;
			//this._fUser.Avartar		= this.txt
			if(this.rbFemale.Checked)
			{
				this._fUser.Gender	= 0;
			}
			if(this.rbMale.Checked)
			{
				this._fUser.Gender	= 1;
			}
			
			this._fUser.Homepage	= this.txtHomepage.Text;
			this._fUser.ICQNumber	= this.txtICQNumber.Text;
			this._fUser.Location	= this.txtLocation.Text;
			this._fUser.MSN			= this.txtMSN.Text;
			this._fUser.Occupation	= this.txtOccupation.Text;
			this._fUser.Signature	= this.txtSignature.Text;
			this._fUser.TimeZone		= Int32.Parse(this.ddlTimeZone.SelectedItem.Value);
			this._fUser.YahooMessenger	= this.txtYahooMessenger.Text;
			base.ForumModule.SaveForumUser(this._fUser);

			Response.Redirect(String.Format("{0}/ForumList",UrlHelper.GetUrlFromSection(base.ForumModule.Section)));
		}

	}
}
