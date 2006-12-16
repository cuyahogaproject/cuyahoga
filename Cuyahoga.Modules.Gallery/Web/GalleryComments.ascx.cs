#region Copyright and License
/*
Copyright 2006 Dominique Paris, xp-rience.net
Design work copyright Dominique Paris (http://www.xp-rience.net/)

You can use this Software for any commercial or noncommercial purpose, 
including distributing derivative works.

In return, we simply require that you agree:

1. Not to remove any copyright notices from the Software. 
2. That if you distribute the Software in source code form you do so only 
   under this License (i.e. you must include a complete copy of this License 
   with your distribution), and if you distribute the Software solely in 
   object form you only do so under a license that complies with this License. 
3. That the Software comes "as is", with no warranties. None whatsoever. This 
   means no express, implied or statutory warranty, including without 
   limitation, warranties of merchantability or fitness for a particular 
   purpose or any warranty of noninfringement. Also, you must pass this 
   disclaimer on whenever you distribute the Software.
4. That if you sue anyone over patents that you think may apply to the 
   Software for a person's use of the Software, your license to the Software 
   ends automatically. 
5. That the patent rights, if any, licensed hereunder only apply to the 
   Software, not to any derivative works you make. 
6. That your rights under this License end automatically if you breach it in 
   any way.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
namespace Cuyahoga.Modules.Gallery.Web
{
	using System;
	using System.Text.RegularExpressions;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Util;
	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Modules.Gallery.Domain;

	using xprience.GeoIP;
	using Cuyahoga.Modules.Gallery.UI;

	/// <summary>
	///		Summary description for xpGalleryComments.
	/// </summary>
	public class GalleryComments : BaseGalleryControl
	{
		protected System.Web.UI.WebControls.Image imgGallery;
		protected System.Web.UI.WebControls.Repeater rptComments;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.TextBox txtWebsite;
		protected System.Web.UI.WebControls.Panel pnlAnonymous;
		protected System.Web.UI.WebControls.TextBox txtComment;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvComment;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button btnSaveComment;
		protected System.Web.UI.WebControls.Panel pnlComment;
		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.WebControls.Literal litDescription;

		protected Cuyahoga.ServerControls.Pager pgrComments;
		protected System.Web.UI.WebControls.Panel pnlAll;
		
		private GalleryCommentsModule _module;
		private xpGeoManager _geoman;

		private PhotoGallery _gallery;
		private int _galleryid = -1;
		private PageEngine _page;
		private string _culture;
		private bool _allowcomments = true;
		private bool _allanonymous = false;


		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as GalleryCommentsModule;
			this._page = base.Page as PageEngine;
			this._culture = _page.ActiveNode.Culture;
			string file = this.Page.ResolveUrl("~/Modules/Gallery/Config/GeoIP.dat");
			file = Server.MapPath( file );
			this._geoman = new xpGeoManager( file );

			_allowcomments = _module.AllowComments;
			_allanonymous = _module.AllowAnonymousComments;
			
			
			if ( _module.CurrentGalleryId != -1 )
			{
				_gallery = _module.GetGallery( _module.CurrentGalleryId );
				_galleryid = _gallery.Id;
				this.pnlAll.Visible = true;

				this.btnSaveComment.Text = base.GetText("BTNSAVECOMMENT");
				this.rfvName.ErrorMessage = base.GetText("NAMEREQUIRED");
				this.rfvComment.ErrorMessage = base.GetText("COMMENTREQUIRED");


				if ( false == Page.IsPostBack )
				{
					BindGallery();
					BindComments();
				}
			}
			else
			{
				this.pnlAll.Visible = false;
			}
		}

		private void BindGallery()
		{
			this.litTitle.Text = _gallery.Title;
			this.imgGallery.ImageUrl = base.ResolveUrl(_gallery.ThumbImage );
			this.litDescription.Text = ((Descriptions)_module.GetObjectDescriptions( DescriptionType.Gallery, _gallery.Id, _culture )).Description;
		}

		private void BindComments()
		{
			this.pgrComments.PageSize = _module.NumberOfRows;
			this.rptComments.DataSource = _gallery.GalleryComments;
			this.rptComments.DataBind();
			if (this._allanonymous || (this.Page.User.Identity.IsAuthenticated && this._allowcomments))
			{
				this.pnlComment.Visible = true;
				this.pnlAnonymous.Visible = (! this.Page.User.Identity.IsAuthenticated);
			}
			else
			{
				this.pnlComment.Visible = false;
			}
		}

		private string GetProfileUrl(int userId)
		{
			Section sectionTo = this._module.Section.Connections["ViewProfile"] as Section;
			if (sectionTo != null)
			{
				return UrlHelper.GetUrlFromSection(sectionTo) + "/ViewProfile/" + userId.ToString();
			}
			else
			{
				return "";
			}
		}

		private void AddCountryName( PlaceHolder panel, User user )
		{
			if (user != null )
			{
				if ( user.LastIp != null && user.LastIp.Trim() != string.Empty )
				{
					try
					{
						string country = _geoman.GetCountryName( user.LastIp, _culture);
						Literal lit = new Literal();
						lit.Text = " (" + country + ")";
						panel.Controls.Add( lit );
					}
					catch {}
				}
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
			this.rptComments.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptComments_ItemDataBound);
			this.pgrComments.PageChanged += new Cuyahoga.ServerControls.PageChangedEventHandler(this.pgrComments_PageChanged);
			this.pgrComments.CacheEmpty += new System.EventHandler(this.pgrComments_CacheEmpty);
			this.btnSaveComment.Click += new System.EventHandler(this.btnSaveComment_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rptComments_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			PlaceHolder plhCommentBy = e.Item.FindControl("plhCommentBy") as PlaceHolder;
			if (plhCommentBy != null)
			{
				GalleryComment comment = (GalleryComment)e.Item.DataItem;
				Literal litUpdateTimestamp = e.Item.FindControl("litUpdateTimestamp") as Literal;
				litUpdateTimestamp.Text = TimeZoneUtil.AdjustDateToUserTimeZone(comment.DateUpdated, this.Page.User.Identity).ToString();

				if (comment.User != null)
				{
					// Comment by registered user.
					if (comment.User.Website != null && comment.User.Website != String.Empty)
					{
						string url = GetProfileUrl(comment.User.Id);
						if ( url != String.Empty )
						{
							HyperLink hpl = new HyperLink();
							hpl.NavigateUrl = GetProfileUrl(comment.User.Id);
							hpl.Text = comment.User.FullName;
							plhCommentBy.Controls.Add(hpl);
							AddCountryName( plhCommentBy, comment.User );
						}
						else
						{
							Literal lit = new Literal();
							lit.Text = comment.User.FullName;
							plhCommentBy.Controls.Add(lit);
							AddCountryName( plhCommentBy, comment.User );
						}
					}
					else
					{
						Literal lit = new Literal();
						lit.Text = comment.User.FullName;
						plhCommentBy.Controls.Add(lit);
						AddCountryName( plhCommentBy, comment.User );
					}
				}
				else
				{
					// Comment by unregistered user.
					if (comment.Website != null && comment.Website != String.Empty)
					{
						HyperLink hpl = new HyperLink();
						hpl.NavigateUrl = comment.Website;
						hpl.Target = "_blank";
						hpl.Text = comment.Name;
						plhCommentBy.Controls.Add(hpl);
						AddCountryName( plhCommentBy, comment.User );
					}
					else
					{
						Literal lit = new Literal();
						lit.Text = comment.Name;
						plhCommentBy.Controls.Add(lit);
						AddCountryName( plhCommentBy, comment.User );
					}
				}
			}		
		}

		private void pgrComments_CacheEmpty(object sender, System.EventArgs e)
		{
			this.rptComments.DataSource = _gallery.GalleryComments;
		}

		private void pgrComments_PageChanged(object sender, Cuyahoga.ServerControls.PageChangedEventArgs e)
		{
			this.rptComments.DataBind();
		}

		private void btnSaveComment_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid)
			{
				// Strip any html tags.
				string commentText = Regex.Replace(this.txtComment.Text, @"<(.|\n)*?>", string.Empty);
				// Replace carriage returns with <br>.
				commentText = commentText.Replace("\r", "<br>");
				// Save comment.
				GalleryComment comment = new GalleryComment();
				comment.Gallery = _gallery;
				comment.CommentText = commentText;
				comment.Culture = _culture;
				comment.UserIp = HttpContext.Current.Request.UserHostAddress;

				if (this.Page.User.Identity.IsAuthenticated)
				{
					comment.User = this.Page.User.Identity as Cuyahoga.Core.Domain.User;
				}
				else
				{
					comment.Name = this.txtName.Text;
					comment.Website = this.txtWebsite.Text;
				}
				try
				{
					this._module.SaveComment(comment);
					// Clear the cache, so the comment will appear immediately.
					base.InvalidateCache();
					Context.Response.Redirect( UrlHelper.GetUrlFromSection(this._module.Section) + "/ViewGalleryComments/" + _galleryid.ToString() + "#Comments" );
				}
				catch (Exception ex)
				{
					this.lblError.Text = ex.Message;
					this.lblError.Visible = true;
				}
			}
		}
	}
}
