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
	using System.IO;
	using System.Data;
	using System.Drawing;
	using System.Text;

	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Util;
	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Modules.Gallery.Domain;
	using Cuyahoga.Modules.Gallery.UI;

	/// <summary>
	///		Summary description for PhotoShow.
	/// </summary>
	public class PhotoShow : BaseGalleryControl
	{

		protected System.Web.UI.WebControls.PlaceHolder phlContent;

		private PhotoGallery _gallery;
		private Photo _photo;
		private PhotoShowModule _module;
		private PageEngine _page;
		private string _culture;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as PhotoShowModule;
			this._page = base.Page as PageEngine;
			this._module.ModulePathInfo = this.Context.Request.PathInfo;

			_culture = _page.ActiveNode.Culture;
			
			if ( _module.AreaSize <= 0 )
			{
				ShowError( "AREASIZENEGATIVE", "" );
				return;
			}
			int galleryid = _module.CurrentGalleryId;

			if ( galleryid < 0 )
			{
				// try to get from GallerySection object
				GallerySection gs = _module.GetGallerySection( _module.Section.Id );
				if ( gs != null ) galleryid = gs.GalleryId;
			}
			
			if ( galleryid > 0 )
			{
				_gallery = _module.GetGallery( galleryid );

				if (_gallery != null )
				{
					switch ( _module.ShowType )
					{
						case PhotoShowType.RandomImage  :
							ProcessRandom();
							break;

						case PhotoShowType.FadeIn :
							string script = String.Format( "<script type=\"text/javascript\">new fadeshow(fadeimages{0}, {1}, {2}, 0, {3}, 1)</script>",_gallery.Id,  _module.AreaSize, _module.AreaSize, _module.FadeTiming);
							Literal lit = new Literal();
							lit.Text = script;
							this.phlContent.Controls.Add( lit );
							break;
					}
				}
			}
		}

		private void ShowError( string errorcode, string param )
		{
			string error = "<p class=\"xperror\">" + base.GetText( errorcode ) + " " + param + "</p>";
			Literal lit = new Literal();
			lit.Text = error;
			this.phlContent.Controls.Add( lit );
		}

		private void ProcessRandom()
		{
			Random rnd = new Random();
			int i = rnd.Next( _gallery.Photos.Count );
			_photo = _gallery.Photos[ i ] as Photo;

			string src = (_module.UseThumbnails ) ? _photo.ThumbImage : _photo.LargeImage;
			string file = Server.MapPath( src );
			FileInfo info = new FileInfo( file );
			if ( false == info.Exists )
			{
				ShowError( "FILENOTEXIST", src );
				return;
			}
			System.Drawing.Bitmap img = new Bitmap( file );
				
			HtmlImage himg = new HtmlImage();
			himg.Alt = _photo.Title;
			himg.Attributes.Add("title", _photo.Title ); //Firefox
			himg.Border = 0;
			himg.Src = src;

			// Calculate the real width and height
			int sq = _module.AreaSize;

			decimal rz = (decimal)sq / Math.Max( (decimal)img.Width, (decimal)img.Height );
			himg.Width = (int)Math.Round( (decimal)img.Width * rz );
			himg.Height = (int)Math.Round( (decimal)img.Height * rz );
			
			ProcessAction( 	himg );
		}

		private void ProcessAction( HtmlImage img )
		{
			switch( _module.ShowAction )
			{
				case PhotoShowAction.RevealImage :

					HyperLink hpl = new HyperLink();
					hpl.NavigateUrl = _photo.LargeImage;
					hpl.Attributes.Add("rel", "lightbox");
					hpl.Controls.Add( img );
					this.phlContent.Controls.Add( hpl );
					break;

				case PhotoShowAction.PopupWindow :
					string url = base.ResolveUrl("~/Modules/Gallery/ShowImage.aspx");
					url = String.Format("Popup('" + url + "?GalleryId={0}&PhotoId={1}&Culture={2}','{3}','{4}','{5}','{6}'); return false;",_gallery.Id, _photo.Id, _culture, _module.PopupWidth, _module.PopupHeight, _module.PopupTop, _module.PopupLeft);
					img.Attributes.Add("OnClick", url );
					this.phlContent.Controls.Add( img );
					break;

				default :
					this.phlContent.Controls.Add( img );
					break;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			BaseTemplate template = _page.TemplateControl;
			Literal ph = template.FindControl("JavaScripts") as Literal;
			if (ph != null && _gallery != null )
			{
				if ( _module.ShowType == PhotoShowType.FadeIn )
				{
					string jsfile = base.ResolveUrl("~/Modules/Gallery/js/fadeshow.js");
					ph.Text += PhotoActionHelper.GetJSScripTag( jsfile );
					ph.Text += PhotoActionHelper.GetJSFadeIn( _gallery, _module.UseThumbnails );
				}
				else if ( _module.ShowAction == PhotoShowAction.RevealImage )
				{
					string src = base.ResolveUrl("~/Modules/Gallery/js/lightbox/lightbox.js");
					ph.Text += PhotoActionHelper.GetJSScripTag( src );
					src = base.ResolveUrl("~/Modules/xpgallery/js/lightbox/lightbox.css" );
					_page.RegisterStylesheet("lightbox", src );
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
