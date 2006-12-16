using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.IO;

using Cuyahoga.Core.Util;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Forum;
using Cuyahoga.Modules.Forum.Domain;
using Cuyahoga.Modules.Forum.Utils;
using Cuyahoga.Modules.Forum.Web.UI;

namespace Cuyahoga.Modules.Forum
{
	/// <summary>
	///		Summary description for Links.
	/// </summary>
	public class ForumViewPost : BaseForumControl
	{
		private const int BUFFER_SIZE = 8192;
		#region Private vars
		private ForumPost	_forumPost;
		private ForumForum	_forumForum;
		private ForumModule	_module;
		#endregion

		protected System.Web.UI.WebControls.Repeater rptForumPostRepliesList;
		protected System.Web.UI.WebControls.Literal lblPostedDate;
		protected System.Web.UI.WebControls.Label lblUserInfo;
		protected System.Web.UI.WebControls.Literal lblMessages;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.HyperLink hplReply;
		protected System.Web.UI.WebControls.HyperLink hplNewTopic;
		protected System.Web.UI.WebControls.PlaceHolder phForumTop;
		protected System.Web.UI.WebControls.PlaceHolder phForumFooter;
		protected System.Web.UI.WebControls.Label lblTopic;
		protected System.Web.UI.WebControls.HyperLink hplAuthor;
		protected System.Web.UI.WebControls.HyperLink hplQuotePost;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Literal ltlFileinfo;
		protected System.Web.UI.WebControls.Panel pnlAttachment;
		protected System.Web.UI.WebControls.Label lblAttachment;
		protected System.Web.UI.WebControls.HyperLink hplPostAttachment;
        protected System.Web.UI.WebControls.LinkButton lbtnRemove;



		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as ForumModule;

			this._forumForum	= this._module.GetForumById(this._module.CurrentForumId);
			this._module.CurrentForumCategoryId = this._forumForum.CategoryId;
			this.BindTopFooter();
			if(this._forumForum.AllowGuestPost == 1 || this.Page.User.Identity.IsAuthenticated)
			{
				this.hplNewTopic.Visible = true;
				this.hplReply.Visible = true;
				this.hplQuotePost.Visible = true;
			}
			else
			{
				this.hplNewTopic.Visible = false;
				this.hplReply.Visible = false;
				this.hplQuotePost.Visible = false;
			}



			if(!this.IsPostBack)
			{
				if(this._module.DownloadId != 0)
				{
					this.DownloadCurrentFile();
				}
                this.BindLinks();
				this.BindForumPost();
                this.BindForumPostReplies();
                this.LocalizeControls();
            }
		}

		private void BindTopFooter()
		{
			ForumTop tForumTop;
			ForumFooter tForumFooter;

			this._module = this.Module as ForumModule;
			tForumTop = (ForumTop)this.LoadControl("~/Modules/Forum/ForumTop.ascx");
			tForumTop.Module = this._module;
			this.phForumTop.Controls.Add(tForumTop);

			tForumFooter = (ForumFooter)this.LoadControl("~/Modules/Forum/ForumFooter.ascx");
			tForumFooter.Module	= this._module;
			this.phForumFooter.Controls.Add(tForumFooter);
		}

		private void Translate()
		{
			string uname = "";
			if(this.Page.User.Identity.IsAuthenticated)
			{
				Cuyahoga.Core.Domain.User currentUser = Context.User.Identity as Cuyahoga.Core.Domain.User;
				uname = currentUser.FullName;
			}
			else
			{
				uname = base.GetText("GUEST");
			}
		}

		private void BindLinks()
		{
			this.hplReply.NavigateUrl = String.Format("{0}/ForumReplyPost/{1}/post/{2}",UrlHelper.GetUrlFromSection(this._module.Section), this._module.CurrentForumId,this._module.CurrentForumPostId);
			this.hplNewTopic.NavigateUrl = String.Format("{0}/ForumNewPost/{1}",UrlHelper.GetUrlFromSection(this._module.Section), this._module.CurrentForumId);
			this.hplQuotePost.NavigateUrl = String.Format("{0}/ForumReplyPostQuote/{1}/post/{2}/orig/{3}",UrlHelper.GetUrlFromSection(this._module.Section), this._module.CurrentForumId,this._module.CurrentForumPostId,this._module.CurrentForumPostId);
			this.hplQuotePost.CssClass = "forum";
		}

		private void BindForumPost()
		{
			this._forumPost	= this._module.GetForumPostById(this._module.CurrentForumPostId);
			this._forumPost.Views++;
			this._module.SaveForumPost(this._forumPost);
			this.lblTopic.Text	= this._forumPost.Topic;

			if(this._forumPost.UserId == 0)
			{
				this.hplAuthor.Text = "Guest";
				this.hplAuthor.CssClass = "forum";
				this.lblUserInfo.Text	= "&nbsp;";
			}
			else
			{
				this.hplAuthor.Text			= this._forumPost.UserName;
				this.hplAuthor.NavigateUrl	= String.Format("{0}/ForumViewProfile/{1}",UrlHelper.GetUrlFromSection(this._module.Section), this._forumPost.UserId);
				this.hplAuthor.CssClass		= "forum";
				this.lblUserInfo.Text		= "&nbsp;";
			}
			string msg				= this._forumPost.Message;
			this.lblMessages.Text	= this._forumPost.Message;
			this.lblPostedDate.Text = TimeZoneUtil.AdjustDateToUserTimeZone(this._forumPost.DateCreated, this.Page.User.Identity).ToLongDateString() + " " +TimeZoneUtil.AdjustDateToUserTimeZone(this._forumPost.DateCreated, this.Page.User.Identity).ToLongTimeString();
			if(this._forumPost.AttachmentId != 0)
			{
				ForumFile fFile = this._module.GetForumFileById(this._forumPost.AttachmentId);
				this.pnlAttachment.Visible = true;
				this.hplPostAttachment.NavigateUrl = String.Format("{0}/ForumViewPost/{1}/PostId/{2}/Download/{3}",UrlHelper.GetUrlFromSection(this._module.Section), this._forumPost.ForumId,this._forumPost.Id,this._forumPost.AttachmentId);
				this.hplPostAttachment.Text = fFile.OrigFileName;
				this.hplPostAttachment.ToolTip = String.Format("{0} - {1} bytes",fFile.OrigFileName,fFile.FileSize);

				this.ltlFileinfo.Text = String.Format(GetText("attachinfo"),fFile.FileSize,fFile.DlCount);
			}
            User cuyahogaUser = this.Page.User.Identity as User;

            if (cuyahogaUser != null)
            {
                if (cuyahogaUser.CanEdit(this._module.Section))
                {
                    this.lbtnRemove.Visible = true;
                }
            }
		}

		private void BindForumPostReplies()
		{
			// Bind the link
			HyperLink hpl = (HyperLink)this.FindControl("hplNewTopic");
			if(hpl != null)
			{
				hpl.Text = "New topic";
				hpl.NavigateUrl	= String.Format("{0}/ForumNewPost/{1}",UrlHelper.GetUrlFromSection(this._module.Section), this._module.CurrentForumId);
				hpl.CssClass	= "forum";
			}
			this.rptForumPostRepliesList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptForumPostRepliesList_ItemDataBound);
			this.rptForumPostRepliesList.DataSource	= this._module.GetAllForumPostReplies(this._module.CurrentForumPostId);
			this.rptForumPostRepliesList.DataBind();
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


		private void rptForumPostRepliesList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				ForumPost fPost = e.Item.DataItem as ForumPost;
				// Attachement ??
				if(fPost.AttachmentId != 0)
				{
					Panel replyPanel = (Panel)e.Item.FindControl("pnlReplyAttachment");
					
					if(replyPanel != null)
					{
						replyPanel.Visible = true;
						ForumFile fFile = this._module.GetForumFileById(fPost.AttachmentId);
						HyperLink hplReplyAttachment = (HyperLink)e.Item.FindControl("hplReplyttachment");
						hplReplyAttachment.NavigateUrl = String.Format("{0}/ForumViewPost/{1}/PostId/{2}/Download/{3}",UrlHelper.GetUrlFromSection(this._module.Section), fPost.ForumId,fPost.Id,fPost.AttachmentId);
						hplReplyAttachment.Text = fFile.OrigFileName;
						hplReplyAttachment.ToolTip = String.Format("{0} - {1} bytes",fFile.OrigFileName,fFile.FileSize);
						Literal ltlReplyFileinfo = (Literal)e.Item.FindControl("ltlReplyFileinfo");
						ltlReplyFileinfo.Text = String.Format(GetText("attachinfo"),fFile.FileSize,fFile.DlCount);
					}
				}
                User cuyahogaUser = this.Page.User.Identity as User;

                if (cuyahogaUser != null)
                {
                    if (cuyahogaUser.CanEdit(this._module.Section))
                    {
                        e.Item.FindControl("lbtnRemove").Visible = true;
                    }
                }
			}
		}

		private void rptForumPostRepliesList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
		
		}

		public string GetPostedDate(object o)
		{
			ForumPost tPost = o as ForumPost;
			return TimeZoneUtil.AdjustDateToUserTimeZone(tPost.DateCreated, this.Page.User.Identity).ToLongDateString() + " " +TimeZoneUtil.AdjustDateToUserTimeZone(tPost.DateCreated, this.Page.User.Identity).ToLongTimeString();
		}
		public string GetMessage(object o)
		{
			ForumPost tPost = o as ForumPost;
			return tPost.Message;
		}

		public string GetUserProfileLink(object o)
		{
			ForumPost tPost = o as ForumPost;
			if(tPost.UserId != 0)
			{
				return String.Format("<a href=\"{0}/ForumViewProfile/{1}\" class=\"forum\">{2}</a>",UrlHelper.GetUrlFromSection(this._module.Section),tPost.UserId,tPost.UserName);
			}
			else
			{
				return String.Format("<a href=\"#\" class=\"forum\">{0}</a>",tPost.UserName);
			}
		}
		public string GetQuoteLink(object o)
		{
			ForumPost tPost = o as ForumPost;
			if(this._forumForum.AllowGuestPost == 1 || this.Page.User.Identity.IsAuthenticated)
			{
				return String.Format("<a class=\"forum\" href=\"{0}/ForumReplyPostQuote/{1}/post/{2}/orig/{3}\">" + base.GetText("hplQuotePost") + "</a>",UrlHelper.GetUrlFromSection(this._module.Section), this._module.CurrentForumId,tPost.Id,this._module.CurrentForumPostId);
			}
			else
			{
				return "&nbsp;";
			}
		}

        public string GetForumPostId(object o)
		{
			ForumPost tPost = o as ForumPost;
			return tPost.Id.ToString();
		}

        
		private void DownloadCurrentFile()
		{
			//Response.End();
			
			ForumFile file = this._module.GetForumFileById(this._module.DownloadId);
			//if (file.IsDownloadAllowed(this.Page.User.Identity))
			{
				string physicalFilePath = file.ForumFileName;
				if (System.IO.File.Exists(physicalFilePath))
				{
					Stream fileStream = null;
					try
					{
						byte[] buffer = new byte[BUFFER_SIZE];
						// Open the file.
						fileStream = new System.IO.FileStream(physicalFilePath, System.IO.FileMode.Open,
							System.IO.FileAccess.Read, System.IO.FileShare.Read);

						// Total bytes to read:
						long dataToRead = fileStream.Length;

						// Support for resuming downloads
						long position = 0;
						if (Request.Headers["Range"] != null)
						{
							Response.StatusCode = 206;
							Response.StatusDescription = "Partial Content";
							position = long.Parse(Request.Headers["Range"].Replace("bytes=", "").Replace("-", ""));
						}
						if (position != 0)
						{
							Response.AddHeader("Content-Range", "bytes " + position.ToString() + "-" + ((long)(dataToRead - 1)).ToString() + "/" + dataToRead.ToString());
						}
						Response.ContentType = file.ContentType;
						Response.AppendHeader("Content-Disposition", "attachment; filename=" + file.OrigFileName);
						// The content length depends on the amount that is already transfered in an earlier request.
						Response.AppendHeader("Content-Length", (fileStream.Length - position).ToString());
					
						// Stream the actual content
						bool isInterrupted = false;
						while (dataToRead > 0 && ! isInterrupted)
						{
							// Verify that the client is connected.
							if (Response.IsClientConnected)
							{
								// Read the data in buffer.
								int length = fileStream.Read(buffer, 0, BUFFER_SIZE);

								// Write the data to the current output stream.
								Response.OutputStream.Write(buffer, 0, length);

								// Flush the data to the HTML output.
								Response.Flush();

								buffer = new byte[BUFFER_SIZE];
								dataToRead = dataToRead - length;
							}
							else
							{
								//prevent infinite loop if user disconnects
								isInterrupted = true;
							}
						}

						// Only update download statistics if the download is succeeded.
						if (! isInterrupted)
						{
							file.DlCount++;
							this._module.SaveForumFile(file);
						}
					}
					finally
					{
						if (fileStream != null)
						{
							fileStream.Close();
						}
						Response.End();
					}
				}
				else
				{
					throw new Exception("The physical file was not found on the server.");
				}
			}
			//else
			//{
			//	throw new Exception("You are not allowed to download the file.");
			//}
			
		}

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            LinkButton lbtnRemoveTemp = (LinkButton)sender;
            if (!lbtnRemoveTemp.Parent.GetType().Equals(typeof(RepeaterItem)) )
            {
                foreach (RepeaterItem item in this.rptForumPostRepliesList.Items)
                {
                    LinkButton lbtnRemoveTemp2 = (LinkButton)item.FindControl("lbtnRemove");
                    ForumPost postTemp = this._module.GetForumPostById(int.Parse(lbtnRemoveTemp2.CommandArgument));

                    if (postTemp.AttachmentId != 0)
                    {
                        ForumFile fFile = this._module.GetForumFileById(postTemp.AttachmentId);
                        this._module.DeleteForumFile(fFile);
                    }
                    
                    this._module.DeleteForumPost(postTemp);
                }

                this._forumPost = this._module.GetForumPostById(this._module.CurrentForumPostId);
                if (this._forumPost.AttachmentId != 0)
                {
                    ForumFile fFile = this._module.GetForumFileById(this._forumPost.AttachmentId);
                    this._module.DeleteForumFile(fFile);
                }
                this._module.DeleteForumPost(this._forumPost);
                Response.Redirect(String.Format("{0}/ForumView/{1}", UrlHelper.GetUrlFromSection(base.ForumModule.Section), base.ForumModule.CurrentForumId));
            }
            else
            {
                ForumPost post = this._module.GetForumPostById(int.Parse(lbtnRemoveTemp.CommandArgument));
                if (post.AttachmentId != 0)
                {
                    ForumFile fFile = this._module.GetForumFileById(post.AttachmentId);
                    this._module.DeleteForumFile(fFile);
                }
                this._module.DeleteForumPost(post);
                Response.Redirect(String.Format("{0}/ForumViewPost/{1}/PostId/{2}", UrlHelper.GetUrlFromSection(base.ForumModule.Section), base.ForumModule.CurrentForumId, base.ForumModule.CurrentForumPostId));
            }
            
        }
	
	
	}
}
