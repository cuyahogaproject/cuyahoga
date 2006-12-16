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
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Threading;

using Cuyahoga.Modules.Gallery.Domain;

namespace Cuyahoga.Modules.Gallery.Web
{
	/// <summary>
	/// Summary description for ShowImage.
	/// </summary>
	public class ShowImage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Image image1;
	
		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.WebControls.Literal litDescription;
		protected System.Web.UI.WebControls.Literal litRating;
		protected System.Web.UI.WebControls.RadioButtonList rblRanking;
		protected System.Web.UI.WebControls.Literal litViews;
		protected System.Web.UI.WebControls.Literal litRanking;
		protected System.Web.UI.HtmlControls.HtmlAnchor lnkPrevious;
		protected System.Web.UI.HtmlControls.HtmlAnchor lnkNext;
		protected System.Web.UI.HtmlControls.HtmlImage imgPrevious;
		protected System.Web.UI.HtmlControls.HtmlImage imgNext;
		protected System.Web.UI.WebControls.Button btnVote;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdgalleryid;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdphotoid;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdrating;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdculture;
		protected System.Web.UI.WebControls.Literal litSerie;
		protected System.Web.UI.WebControls.Literal litCategory;
		protected System.Web.UI.WebControls.Literal litOriginalSize;
		protected System.Web.UI.WebControls.Panel pnlVote;
		protected System.Web.UI.WebControls.Panel pnlMessage;
		protected System.Web.UI.WebControls.Literal litMessage;
		protected System.Web.UI.HtmlControls.HtmlImage imgBack;
		protected System.Web.UI.WebControls.Literal litJavaScripts;
		protected System.Web.UI.WebControls.Literal liVoteInfo;

		private int _galleryid = -1;
		private int _photoid = -1;

		private PhotoGallery _gallery;
		private Photo _photo;
		private GalleryModuleBase _module;

		private ResourceManager _resMan;
		private CultureInfo _currentUICulture;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Base name of the resources consists of Namespace.Resources.Strings
			string baseName = this.GetType().BaseType.Namespace + ".Resources.Strings";
			this._resMan = new ResourceManager(baseName, this.GetType().BaseType.Assembly);
			_module = new GalleryModuleBase();
			
			// Cleanup message text
			this.litMessage.Text = "";

			if ( false == Page.IsPostBack )
			{
				if (Request.QueryString[ "GalleryId"] != null && Request.QueryString[ "PhotoId" ] != null )
				{
					_galleryid = Int32.Parse( Request.QueryString[ "GalleryId"] );
					_photoid = Int32.Parse( Request.QueryString[ "PhotoId" ] );

					_gallery = _module.GetGallery( _galleryid );
					_photo = _module.GetPhoto( _photoid );

					// check the complementary culture parameter
					if ( Request.QueryString[ "Culture"] != null )
					{
						try
						{
							this._currentUICulture = new CultureInfo( Request.QueryString[ "Culture"] );
						}
						catch
						{
							this._currentUICulture = new CultureInfo( "en-US" );
						}
					}
					else
					{
						this._currentUICulture = new CultureInfo( "en-US" );
					}

					// check if the photo has been viewed during the user session
					string views = "/";
					if ( Session[ "photoviewed" ] != null ) views = (string)Session[ "photoviewed" ];
					string s = String.Concat( "/", _photoid.ToString(), "/");
					if ( views.IndexOf( s ) < 0 )
					{
						_photo.Views += 1;
						_module.SavePhoto( _photo );
						Session[ "photoviewed" ] = String.Concat( views, _photoid.ToString(), "/" );
					}

					this.BindForm();

				}
				else
				{
					Response.End();
				}
			}
			else
			{
				if ( Request.Form[ "hdgalleryid" ] != null && Request.Form[ "hdphotoid" ] != null )
				{
					_galleryid = Int32.Parse( Request.Form[ "hdgalleryid" ] );
					_photoid = Int32.Parse( Request.Form[ "hdphotoid" ] );
					_currentUICulture = new CultureInfo( Request.Form[ "hdculture" ] );

					_gallery = _module.GetGallery( _galleryid );
					_photo = _module.GetPhoto( _photoid );

				}
				else
				{
					Response.End();
				}
			}
		}

		private void BindForm()
		{
			// set vote visible or not based on the fact that the user has already voted
			if ( false == this.HasVoted() )
			{
				this.pnlVote.Visible = true;
				this.btnVote.Text = this.GetText( "BTNVOTE" );
			}
			else
			{
				this.pnlVote.Visible = false;
				if (this.litMessage.Text == String.Empty )
					this.litMessage.Text = this.GetText( "ALREADYVOTED" );
			}

			// set photo information
			this.litTitle.Text = _photo.Title;
			this.litDescription.Text = _module.GetObjectDescriptionText( DescriptionType.Photo, _photoid, _currentUICulture.Name );

			if ( _photo.Serie != null && _photo.Serie != String.Empty )
				this.litSerie.Text = _photo.Serie;
			else
				this.litSerie.Text = this.GetText("NOTAVAILABLE");

			if ( _photo.Category != null && _photo.Category != String.Empty )
				this.litCategory.Text = _photo.Category;
			else
				this.litCategory.Text = this.GetText("NOTAVAILABLE");
			
			if ( _photo.OriginalSize != null && _photo.OriginalSize != String.Empty )
				this.litOriginalSize.Text = _photo.OriginalSize;
			else
				this.litOriginalSize.Text = this.GetText("NOTAVAILABLE");
					
			this.litViews.Text = _photo.Views.ToString();
			this.litRanking.Text =  _photo.TotalVotes.ToString();
			
			int width = (int)Math.Round( (_photo.GetRating() * 83 ) / 9 );
			this.imgBack.Attributes["width"] = width.ToString();
			this.imgBack.Attributes["src"] = base.ResolveUrl("~/Modules/Gallery/Images/rating_backg.gif");


			this.image1.ImageUrl = _photo.LargeImage;

			// process previous and next
			int i = 0;
			for ( i = 0; i < _gallery.Photos.Count; i ++ )
			{
				if ( _photoid == ((Photo)_gallery.Photos[ i ]).Id ) break;
			}
					
			string url = "";
			if ( i > 0 ) 
			{
				url = String.Format( "ShowImage.aspx?GalleryId={0}&PhotoId={1}&Culture={2}", _gallery.Id, ((Photo)_gallery.Photos[ i - 1 ]).Id,_currentUICulture.Name);
				lnkPrevious.HRef = url;
				imgPrevious.Attributes.Add( "OnClick", "document.location.href='" + url + "'");
				imgPrevious.Attributes.Add( "src", ResolveUrl("~/Modules/Gallery/images/picto_previous.gif") );

			}
			if ( i < _gallery.Photos.Count - 1 ) 
			{
				url = String.Format( "ShowImage.aspx?GalleryId={0}&PhotoId={1}&Culture={2}", _gallery.Id, ((Photo)_gallery.Photos[ i + 1 ]).Id,_currentUICulture.Name);
				lnkNext.HRef = url;
				imgNext.Attributes.Add( "OnClick", "document.location.href='" + url +"'");
				imgNext.Attributes.Add( "src", ResolveUrl("~/Modules/Gallery/images/picto_next.gif") );
			}
			this.hdgalleryid.Value = _galleryid.ToString();
			this.hdphotoid.Value = _photoid.ToString();
			this.hdculture.Value = _currentUICulture.Name;

			string lng = _currentUICulture.Name.Substring(0,2).ToUpper();
			string info = this.GetText("VOTEINFO");
			this.liVoteInfo.Text = info.Replace("\r\n", "<BR>");
		}

		public string GetText( string key )
		{
			return this._resMan.GetString(key, this._currentUICulture);		
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
			this.btnVote.Click += new System.EventHandler(this.btnVote_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnVote_Click(object sender, System.EventArgs e)
		{
			if ( false == this.HasVoted() )
			{
				if ( Request.Form["hdrating"] != null )
				{
					string svote = Request.Form["hdrating"];
					
					decimal vote = (decimal)Int32.Parse( svote );
					// round and select the correct rank
					int rank = (int)Math.Round( vote / 25M) + 5;
					
					switch ( rank )
					{
						case 1 : _photo.Rank1 += 1; break;
						case 2 : _photo.Rank2 += 1; break;
						case 3 : _photo.Rank3 += 1; break;
						case 4 : _photo.Rank4 += 1; break;
						case 5 : _photo.Rank5 += 1; break;
						case 6 : _photo.Rank6 += 1; break;
						case 7 : _photo.Rank7 += 1; break;
						case 8 : _photo.Rank8 += 1; break;
						case 9 : _photo.Rank9 += 1; break;
					}
					_module.SavePhoto( _photo );
					this.AddVote();
					this.litMessage.Text = this.GetText("THANKSVOTE");
				}
			}
			this.BindForm();
		}

		private bool HasVoted()
		{
			string rates = "/";
			if ( Session[ "photorated" ] != null ) rates = (string)Session[ "photorated" ];
			string s = String.Concat( "/", _photoid.ToString(), "/");
			return rates.IndexOf( s ) >= 0 ;
		}

		private void AddVote()
		{
			string rates = "/";
			if ( Session[ "photorated" ] != null ) rates = (string)Session[ "photorated" ];
			string s = String.Concat( "/", _photoid.ToString(), "/");
			if ( rates.IndexOf( s ) < 0 )
			{
				Session[ "photorated" ] = String.Concat( rates, _photoid.ToString(), "/" );
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			Literal ph = this.Page.FindControl( "litJavaScripts" ) as Literal;
			if (ph != null )
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				string jsfile = ResolveUrl("~/Modules/Gallery/js/yui/yahoo.js");
				sb.Append( PhotoActionHelper.GetJSScripTag( jsfile ) );
				jsfile = ResolveUrl("~/Modules/Gallery/js/yui/dom.js");
				sb.Append( PhotoActionHelper.GetJSScripTag( jsfile ) );
				jsfile = ResolveUrl("~/Modules/Gallery/js/yui/event.js");
				sb.Append( PhotoActionHelper.GetJSScripTag( jsfile ) );
				jsfile = ResolveUrl("~/Modules/Gallery/js/yui/dragdrop.js");
				sb.Append( PhotoActionHelper.GetJSScripTag( jsfile ) );
				jsfile = ResolveUrl("~/Modules/Gallery/js/yui/slider.js");
				sb.Append( PhotoActionHelper.GetJSScripTag( jsfile ) );

				ph.Text = sb.ToString();
			}
		}
	
	}

	
}

